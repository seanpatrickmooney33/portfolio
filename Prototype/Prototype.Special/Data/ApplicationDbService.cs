using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prototype.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Data
{
    public class ApplicationDbService
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationDbService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddActivityLog(ActivityLogType ActivityLogType, String UserName, String Description)
        {
            _dbContext.ActivityLogs.Add(new ActivityLog() {
                ActivityLogType = ActivityLogType,
                Description = Description,
                ActiveUserName = UserName,
                CreateDate = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
        }

        #region Election
        public IQueryable<Election> GetElection() { return _dbContext.Elections; }

        public async Task CreateElection(Election e) { await CreateElection(new List<Election>() { e }); }
        public async Task CreateElection(List<Election> e) {
            e.ForEach(e =>
            {
                e.CreatedBy = "Fixme";// need to set these to the logged in user info
                e.ModifyBy = "Fixme"; // need to set these to the logged in user info});
            });
            _dbContext.AddRange(e);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteElection(int Id)
        {
            await DeleteElection(new List<int>() { Id });
        }
        public async Task DeleteElection(IEnumerable<int> Ids) {
            List<Election> ElectionsToDelete = _dbContext.Elections.Where(e => Ids.Contains(e.Id))
                                                                .Include(e => e.Races)
                                                                .ToList();
            List<int> x = new List<int>();
            ElectionsToDelete.Select(e => x.Concat(e.Races.Select(r => r.Id)));
            await DeleteRace(x.Distinct().ToList());

            _dbContext.Elections.RemoveRange(ElectionsToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditElection(Election e) 
        {
            Election electionToUpdate = await _dbContext.Elections.FindAsync(e.Id);
            if(electionToUpdate != null)
            {
                electionToUpdate.ModifyDate = DateTime.UtcNow;
                electionToUpdate.ModifyBy = "Fixme"; // this should be the logged in user info
                electionToUpdate.Name = e.Name;
                electionToUpdate.ElectionDate = e.ElectionDate;
                electionToUpdate.IsActive = e.IsActive;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ActivateElection(int Id)
        {
            if(_dbContext.Elections.Any(x => x.Id.Equals(Id))) 
            { 
                List<Election> elections = _dbContext.Elections.ToList();
                elections.ForEach(x => {
                    if (x.Id.Equals(Id)){ x.IsActive = !x.IsActive; }
                    else { x.IsActive = false; }
                });

                _dbContext.Elections.UpdateRange(elections);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion Election

        #region Races
        public IQueryable<Race> GetRaces() { return _dbContext.Races; }

        public async Task CreateRace(Race race) { await CreateRace(new List<Race>() { race }); ; }
        public async Task CreateRace(IEnumerable<Race> races)
        {
            foreach (Race race in races)
            {
                Election e = await _dbContext.Elections.FirstOrDefaultAsync(x => x.Id.Equals(race.ElectionId));

                if (e != null) 
                { 
                    List<CountyTypeEnum> counties = District.DistrictInfo[race.Type][race.District];
                    race.RaceCountyDataList = counties.Select(x => new RaceCountyData() { RaceId = race.Id, County = x }).ToList();

                    _dbContext.Races.Add(race);
                    _dbContext.RaceCountyData.AddRange(race.RaceCountyDataList);
                }

            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditRace(Race race) { await EditRace(new List<Race>() { race }); }
        public async Task EditRace(IEnumerable<Race> races)
        {
            List<int> Ids = races.Select(x => x.Id).ToList();
            List<Race> r = GetRaces().Where(x => Ids.Contains(x.Id)).ToList();

            foreach (Race race in races)
            {
                Race updatedRace = r.FirstOrDefault(x => x.Id.Equals(race.Id));
                if (updatedRace != null)
                {
                    updatedRace.Name = race.Name;
                    updatedRace.Locked = race.Locked;
                    updatedRace.Type = race.Type;
                    updatedRace.District = race.District;
                    updatedRace.Description = race.Description;

                    _dbContext.Races.Update(updatedRace);

                    _dbContext.RaceCountyData.RemoveRange(
                        _dbContext.RaceCountyData.Where(x => x.RaceId.Equals(updatedRace.Id))
                    );

                    List<CountyTypeEnum> counties = District.DistrictInfo[updatedRace.Type][updatedRace.District];
                    updatedRace.RaceCountyDataList = counties.Select(x => new RaceCountyData() { RaceId = updatedRace.Id, County = x }).ToList();
                    _dbContext.RaceCountyData.AddRange(updatedRace.RaceCountyDataList);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRace(int Id)
        {
            await DeleteRace(new List<int>() { Id });
        }
        public async Task DeleteRace(IEnumerable<int> Ids) {
            List<Race> RacesToDelete = _dbContext.Races.Where(r => Ids.Contains(r.Id))
                                                    .Include(r => r.Candidates).ThenInclude(z => z.CandidateResults)
                                                    .ToList();

            RacesToDelete.ForEach(x => {
                x.Candidates.ToList().ForEach(z => {
                    _dbContext.CandidateResults.RemoveRange(z.CandidateResults);
                });
                _dbContext.Candidates.RemoveRange(x.Candidates);
            });

            _dbContext.Races.RemoveRange(RacesToDelete);
            await _dbContext.SaveChangesAsync();
        }
        #endregion Race

        #region Candidate
        public async Task EditCandidate(Candidate candidate) { await EditCandidate(new List<Candidate>() { candidate }); }
        public async Task EditCandidate(IEnumerable<Candidate> candidate) 
        {
            List<int> Ids = candidate.Select(x => x.Id).ToList();
            List<Candidate> uu = GetCandidate().Where(x => Ids.Contains(x.Id)).ToList();

            foreach(Candidate c in candidate)
            {
                Candidate u = uu.FirstOrDefault(x => x.Id.Equals(c.Id));
                if(u != null) { 
                    u.DisplayName = c.DisplayName;
                    u.DisplayOrder = c.DisplayOrder;
                    u.FirstName = c.FirstName;
                    u.LastName = c.LastName;
                    u.MiddleName = c.MiddleName;
                    u.Party = c.Party;
                    u.RaceId = c.RaceId;
                    this._dbContext.Candidates.Update(u);
                }
            }
            await this._dbContext.SaveChangesAsync();
        }

        public IQueryable<Candidate> GetCandidate() { return _dbContext.Candidates.OrderBy(x => x.DisplayOrder); }

        public async Task DeleteCandidate(int Id) {
            await DeleteCandidate(new List<int>() { Id });
        }
        public async Task DeleteCandidate(IEnumerable<int> Ids) {
            List<Candidate> CandidateToDelete = _dbContext.Candidates.Where(c => Ids.Contains(c.Id))
                                                            .Include(x => x.CandidateResults)
                                                            .ToList();

            CandidateToDelete.ForEach(c => {
                _dbContext.CandidateResults.RemoveRange(c.CandidateResults);
            });
            _dbContext.Candidates.RemoveRange(CandidateToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateCandidate(Candidate candidate) { await CreateCandidate(new List<Candidate>() { candidate }); }
        public async Task CreateCandidate(IEnumerable<Candidate> candidates)
        {
            candidates.ToList().ForEach(c => 
            {
                List<RaceCountyData> raceCountyDataList = _dbContext.RaceCountyData.Where(x => x.RaceId.Equals(c.RaceId)).ToList();

                // post 0 candidateResult foreach Candidate in the Race
                List<CandidateResult> candidateResultList = new List<CandidateResult>();
                foreach (RaceCountyData raceCountyData in raceCountyDataList)
                {
                    candidateResultList.Add(new CandidateResult()
                    {
                        RaceCountyDataId = raceCountyData.Id,
                        RaceCountyData = raceCountyData,
                        CandidateId = c.Id,
                        Candidate = c
                    });
                }

                _dbContext.CandidateResults.AddRange(candidateResultList);

            });

            await _dbContext.Candidates.AddRangeAsync(candidates);
            await _dbContext.SaveChangesAsync();

        }
        #endregion Candidate

        #region CandidateResults

        private void PopulateWithRandomData(IEnumerable<CandidateResult> candidateResults)
        {
            double TotalResult = 0;
            Random random = new Random();
            int minStep = 10;
            int maxStep = 1000;

            foreach (CandidateResult candidateResult in candidateResults)
            {
                int value = random.Next(minStep *= 2, maxStep *= 2);
                TotalResult += value;
                candidateResult.Votes = value;
            }

            foreach (CandidateResult candidateResult in candidateResults)
            {
                candidateResult.Percent = Math.Floor((candidateResult.Votes / TotalResult) * 100);
            }
        }

        public async Task AddRandomResults()
        {
            Election election = await GetElection().Where(x => x.IsActive)
                                                   .Include(x => x.Races).ThenInclude(x => x.RaceCountyDataList).ThenInclude(x => x.CandidateResults)
                                                   .Include(x => x.Races).ThenInclude(x => x.Candidates)
                                                   .FirstOrDefaultAsync();

            foreach (Race race in election.Races)
            {
                foreach (RaceCountyData raceCountyData in race.RaceCountyDataList)
                {
                    // add to archive then remove?
                    _dbContext.CandidateResults.RemoveRange(raceCountyData.CandidateResults);

                    List<CandidateResult> candidateResults = new List<CandidateResult>();
                    foreach (Candidate candidate in race.Candidates)
                    {
                        candidateResults.Add(new CandidateResult()
                        {
                            CandidateId = candidate.Id,
                            RaceCountyDataId = raceCountyData.Id,
                            Votes = 0,
                            Percent = 0.0
                        });
                    }

                    PopulateWithRandomData(candidateResults);

                    raceCountyData.CandidateResults = candidateResults;
                    //raceCountyData.NumberOfPrecinct = 0;
                    raceCountyData.PrecinctsReporting = new Random().Next(100, 2000);

                    _dbContext.RaceCountyData.Update(raceCountyData);

                    await _dbContext.CandidateResults.AddRangeAsync(candidateResults);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddZeroResult()
        {
            Election election = await _dbContext.Elections
                                               .Where(x => x.IsActive)
                                               .Include(x => x.Races).ThenInclude(x => x.RaceCountyDataList).ThenInclude(x => x.CandidateResults)
                                               .Include(x => x.Races).ThenInclude(x => x.Candidates)
                                               .FirstOrDefaultAsync();

            foreach (Race race in election.Races)
            {
                foreach (RaceCountyData raceCountyData in race.RaceCountyDataList)
                {
                    // add to archive then remove?
                    _dbContext.CandidateResults.RemoveRange(raceCountyData.CandidateResults);

                    List<CandidateResult> candidateResults = new List<CandidateResult>();
                    foreach (Candidate candidate in race.Candidates)
                    {
                        candidateResults.Add(new CandidateResult()
                        {
                            CandidateId = candidate.Id,
                            RaceCountyDataId = raceCountyData.Id,
                            Votes = 0,
                            Percent = 0.0
                        });
                    }

                    raceCountyData.CandidateResults = candidateResults;
                    raceCountyData.PrecinctsReporting = 0;

                    _dbContext.RaceCountyData.Update(raceCountyData);

                    await _dbContext.CandidateResults.AddRangeAsync(candidateResults);
                }
            }
            await _dbContext.SaveChangesAsync();

        }

        public IQueryable<CandidateResult> GetCandidateResults() { return _dbContext.CandidateResults; }

        public async Task DeleteCandidateResults(CandidateResult candidateResult) { await DeleteCandidateResults(new List<CandidateResult>() { candidateResult }); }
        public async Task DeleteCandidateResults(IEnumerable<CandidateResult> candidateResult)
        {
            _dbContext.RemoveRange(candidateResult);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditCandidateResults(CandidateResult candidateResult) { await EditCandidateResults(new List<CandidateResult>() { candidateResult }); }
        public async Task EditCandidateResults(IEnumerable<CandidateResult> candidateResult) {
            _dbContext.CandidateResults.UpdateRange(candidateResult);
            await _dbContext.SaveChangesAsync();
        }

        #endregion CandidateResults

        #region RaceCountyData
        public IQueryable<RaceCountyData> GetRaceCountyData() { return _dbContext.RaceCountyData; }

        public async Task EditRaceCountyData(RaceCountyData raceCountyData) { await EditRaceCountyData(new List<RaceCountyData>() { raceCountyData }); }
        public async Task EditRaceCountyData(IEnumerable<RaceCountyData> raceCountyData)
        {
            List<int> Ids = raceCountyData.Select(x => x.Id).ToList();
            List<RaceCountyData> gg = GetRaceCountyData().Where(x => Ids.Contains(x.Id)).ToList();
            foreach (RaceCountyData xx in raceCountyData)
            {
                RaceCountyData ff = gg.FirstOrDefault(x => x.Id.Equals(xx.Id));
                if(ff != null)
                {
                    ff.County = xx.County;
                    ff.NumberOfPrecinct = xx.NumberOfPrecinct;
                    ff.PrecinctsReporting = xx.PrecinctsReporting;
                    ff.RaceId = xx.RaceId;
                    ff.UpdatedDateTime = xx.UpdatedDateTime;
                    _dbContext.RaceCountyData.Update(ff);
                }
            }

            await _dbContext.SaveChangesAsync();

        }

        #endregion RaceCoutyData
    }
}
