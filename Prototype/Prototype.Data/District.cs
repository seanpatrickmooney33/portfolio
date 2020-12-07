﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Data
{
    public class District
    {
        public static Dictionary<RaceTypeEnum, int> NumberOfDistricts { get; } = new Dictionary<RaceTypeEnum, int>() {
            { RaceTypeEnum.BoardOfEqualization, 4 },
            { RaceTypeEnum.Usrep, 53 },
            { RaceTypeEnum.StateSenate, 40 },
            { RaceTypeEnum.StateAssembly, 80 },
            { RaceTypeEnum.CourtsOfAppeal, 6 },
        };

        public static Dictionary<RaceTypeEnum, Dictionary<int, List<CountyTypeEnum>>> DistrictInfo { get; } = new Dictionary<RaceTypeEnum, Dictionary<int, List<CountyTypeEnum>>>()
        {
            { RaceTypeEnum.Usrep,
                new Dictionary<int, List<CountyTypeEnum>>()  {
                    { 1, new List<CountyTypeEnum>() {  CountyTypeEnum.Butte, CountyTypeEnum.Glenn, CountyTypeEnum.Lassen, CountyTypeEnum.Modoc, CountyTypeEnum.Nevada, CountyTypeEnum.Placer, CountyTypeEnum.Plumas, CountyTypeEnum.Shasta, CountyTypeEnum.Sierra, CountyTypeEnum.Siskiyou, CountyTypeEnum.Tehama } },
                    { 2, new List<CountyTypeEnum>() {  CountyTypeEnum.DelNorte, CountyTypeEnum.Humboldt, CountyTypeEnum.Marin, CountyTypeEnum.Mendocino, CountyTypeEnum.Sonoma, CountyTypeEnum.Trinity } },
                    { 3, new List<CountyTypeEnum>() {  CountyTypeEnum.Colusa, CountyTypeEnum.Glenn, CountyTypeEnum.Lake, CountyTypeEnum.Sacramento, CountyTypeEnum.Solano, CountyTypeEnum.Sutter, CountyTypeEnum.Yolo, CountyTypeEnum.Yuba } },
                    { 4, new List<CountyTypeEnum>() {  CountyTypeEnum.Alpine, CountyTypeEnum.Amador, CountyTypeEnum.Calaveras, CountyTypeEnum.ElDorado, CountyTypeEnum.Fresno, CountyTypeEnum.Madera, CountyTypeEnum.Mariposa, CountyTypeEnum.Nevada, CountyTypeEnum.Placer, CountyTypeEnum.Tuolumne } },
                    { 5, new List<CountyTypeEnum>() {  CountyTypeEnum.ContraCosta, CountyTypeEnum.Lake, CountyTypeEnum.Napa, CountyTypeEnum.Solano, CountyTypeEnum.Sonoma } },
                    { 6, new List<CountyTypeEnum>() {  CountyTypeEnum.Sacramento, CountyTypeEnum.Yolo } },
                    { 7, new List<CountyTypeEnum>() {  CountyTypeEnum.Sacramento } },
                    { 8, new List<CountyTypeEnum>() {  CountyTypeEnum.Inyo, CountyTypeEnum.Mono, CountyTypeEnum.SanBernardino } },
                    { 9, new List<CountyTypeEnum>() {  CountyTypeEnum.ContraCosta, CountyTypeEnum.Sacramento, CountyTypeEnum.SanJoaquin } },
                    { 10, new List<CountyTypeEnum>() {  CountyTypeEnum.SanJoaquin, CountyTypeEnum.Stanislaus } },
                    { 11, new List<CountyTypeEnum>() {  CountyTypeEnum.ContraCosta} },
                    { 12, new List<CountyTypeEnum>() { CountyTypeEnum.SanFrancisco} },
                    { 13, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.SanFrancisco} },
                    { 14, new List<CountyTypeEnum>() { CountyTypeEnum.SanFrancisco, CountyTypeEnum.SanMateo } },
                    { 15, new List<CountyTypeEnum>() {  CountyTypeEnum.Alameda, CountyTypeEnum.ContraCosta } },
                    { 16, new List<CountyTypeEnum>() {  CountyTypeEnum.Fresno, CountyTypeEnum.Madera, CountyTypeEnum.Merced } },
                    { 17, new List<CountyTypeEnum>() {  CountyTypeEnum.Alameda, CountyTypeEnum.SantaClara } },
                    { 18, new List<CountyTypeEnum>() {  CountyTypeEnum.SanMateo, CountyTypeEnum.SantaClara, CountyTypeEnum.SantaCruz } },
                    { 19, new List<CountyTypeEnum>() {  CountyTypeEnum.SantaClara } },
                    { 20, new List<CountyTypeEnum>() {  CountyTypeEnum.Monterey, CountyTypeEnum.SanBenito, CountyTypeEnum.SantaClara, CountyTypeEnum.SantaCruz } },
                    { 21, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno, CountyTypeEnum.Kern, CountyTypeEnum.Kings, CountyTypeEnum.Tulare} },
                    { 22, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno, CountyTypeEnum.Tulare } },
                    { 23, new List<CountyTypeEnum>() { CountyTypeEnum.Kern, CountyTypeEnum.LosAngeles, CountyTypeEnum.Tulare } },
                    { 24, new List<CountyTypeEnum>() {  CountyTypeEnum.SanLuisObispo, CountyTypeEnum.SantaBarbara, CountyTypeEnum.Ventura } },
                    { 25, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 26, new List<CountyTypeEnum>() {  CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 27, new List<CountyTypeEnum>() {  CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 28, new List<CountyTypeEnum>() {  CountyTypeEnum.LosAngeles } },
                    { 29, new List<CountyTypeEnum>() {  CountyTypeEnum.LosAngeles } },
                    { 30, new List<CountyTypeEnum>() {  CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 31, new List<CountyTypeEnum>() { CountyTypeEnum.SanBernardino } },
                    { 32, new List<CountyTypeEnum>() {  CountyTypeEnum.LosAngeles } },
                    { 33, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 34, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 35, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 36, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside} },
                    { 37, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 38, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange } },
                    { 39, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange, CountyTypeEnum.SanBernardino } },
                    { 40, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 41, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 42, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 43, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 44, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 45, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 46, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 47, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange } },
                    { 48, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 49, new List<CountyTypeEnum>() { CountyTypeEnum.Orange, CountyTypeEnum.SanDiego } },
                    { 50, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside, CountyTypeEnum.SanDiego } },
                    { 51, new List<CountyTypeEnum>() { CountyTypeEnum.Imperial, CountyTypeEnum.SanDiego } },
                    { 52, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 53, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } }
                }
            },
            { RaceTypeEnum.StateSenate,
                new Dictionary<int, List<CountyTypeEnum>>()
                {
                    { 1, new List<CountyTypeEnum>() { CountyTypeEnum.Alpine, CountyTypeEnum.ElDorado, CountyTypeEnum.Lassen, CountyTypeEnum.Modoc, CountyTypeEnum.Nevada, CountyTypeEnum.Placer, CountyTypeEnum.Plumas, CountyTypeEnum.Sacramento, CountyTypeEnum.Shasta, CountyTypeEnum.Sierra, CountyTypeEnum.Siskiyou} },
                    { 2, new List<CountyTypeEnum>() { CountyTypeEnum.DelNorte, CountyTypeEnum.Humboldt, CountyTypeEnum.Lake, CountyTypeEnum.Marin, CountyTypeEnum.Mendocino, CountyTypeEnum.Sonoma, CountyTypeEnum.Trinity } },
                    { 3, new List<CountyTypeEnum>() { CountyTypeEnum.ContraCosta, CountyTypeEnum.Napa, CountyTypeEnum.Sacramento, CountyTypeEnum.Solano, CountyTypeEnum.Sonoma, CountyTypeEnum.Yolo } },
                    { 4, new List<CountyTypeEnum>() { CountyTypeEnum.Butte, CountyTypeEnum.Colusa, CountyTypeEnum.Glenn, CountyTypeEnum.Placer, CountyTypeEnum.Sacramento, CountyTypeEnum.Sutter, CountyTypeEnum.Tehama, CountyTypeEnum.Yuba } },
                    { 5, new List<CountyTypeEnum>() { CountyTypeEnum.Sacramento, CountyTypeEnum.SanJoaquin, CountyTypeEnum.Stanislaus } },
                    { 6, new List<CountyTypeEnum>() { CountyTypeEnum.Sacramento, CountyTypeEnum.Yolo } },
                    { 7, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.ContraCosta } },
                    { 8, new List<CountyTypeEnum>() { CountyTypeEnum.Amador, CountyTypeEnum.Calaveras, CountyTypeEnum.Fresno, CountyTypeEnum.Inyo, CountyTypeEnum.Madera, CountyTypeEnum.Mariposa, CountyTypeEnum.Mono, CountyTypeEnum.Sacramento, CountyTypeEnum.Stanislaus, CountyTypeEnum.Tulare, CountyTypeEnum.Tuolumne } },
                    { 9, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.ContraCosta } },
                    { 10, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.SantaClara } },
                    { 11, new List<CountyTypeEnum>() { CountyTypeEnum.SanFrancisco, CountyTypeEnum.SanMateo } },
                    { 12, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno, CountyTypeEnum.Madera, CountyTypeEnum.Merced, CountyTypeEnum.Monterey, CountyTypeEnum.SanBenito, CountyTypeEnum.Stanislaus } },
                    { 13, new List<CountyTypeEnum>() { CountyTypeEnum.SanMateo, CountyTypeEnum.SantaClara } },
                    { 14, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno, CountyTypeEnum.Kern, CountyTypeEnum.Kings, CountyTypeEnum.Tulare } },
                    { 15, new List<CountyTypeEnum>() { CountyTypeEnum.SantaClara } },
                    { 16, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno, CountyTypeEnum.Kern, CountyTypeEnum.Kings, CountyTypeEnum.Tulare } },
                    { 17, new List<CountyTypeEnum>() { CountyTypeEnum.Monterey, CountyTypeEnum.SanLuisObispo, CountyTypeEnum.SantaClara, CountyTypeEnum.SantaCruz } },
                    { 18, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 19, new List<CountyTypeEnum>() { CountyTypeEnum.SantaBarbara, CountyTypeEnum.Ventura } },
                    { 20, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 21, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 22, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 23, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Riverside, CountyTypeEnum.SanBernardino }},
                    { 24, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 25, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 26, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 27, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 28, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 29, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange, CountyTypeEnum.SanBernardino } },
                    { 30, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 31, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 32, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange } },
                    { 33, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 34, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange } },
                    { 35, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 36, new List<CountyTypeEnum>() { CountyTypeEnum.Orange, CountyTypeEnum.SanDiego } },
                    { 37, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 38, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 39, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 40, new List<CountyTypeEnum>() { CountyTypeEnum.Imperial, CountyTypeEnum.SanDiego } },
                }
            },
            { RaceTypeEnum.StateAssembly,
                new Dictionary<int, List<CountyTypeEnum>>()
                { 
                    { 1, new List<CountyTypeEnum>() { CountyTypeEnum.Butte, CountyTypeEnum.Lassen, CountyTypeEnum.Modoc, CountyTypeEnum.Nevada, CountyTypeEnum.Placer, CountyTypeEnum.Plumas, CountyTypeEnum.Shasta, CountyTypeEnum.Sierra, CountyTypeEnum.Siskiyou } },
                    { 2, new List<CountyTypeEnum>(){ CountyTypeEnum.DelNorte, CountyTypeEnum.Humboldt, CountyTypeEnum.Mendocino, CountyTypeEnum.Sonoma, CountyTypeEnum.Trinity } },
                    { 3, new List<CountyTypeEnum>(){ CountyTypeEnum.Butte, CountyTypeEnum.Colusa, CountyTypeEnum.Glenn, CountyTypeEnum.Sutter, CountyTypeEnum.Tehama, CountyTypeEnum.Yuba } },
                    { 4, new List<CountyTypeEnum>() { CountyTypeEnum.Colusa, CountyTypeEnum.Lake, CountyTypeEnum.Napa, CountyTypeEnum.Solano, CountyTypeEnum.Sonoma, CountyTypeEnum.Yolo } },
                    { 5, new List<CountyTypeEnum>() { CountyTypeEnum.Alpine, CountyTypeEnum.Amador, CountyTypeEnum.Calaveras, CountyTypeEnum.ElDorado, CountyTypeEnum.Madera, CountyTypeEnum.Mariposa, CountyTypeEnum.Mono, CountyTypeEnum.Placer, CountyTypeEnum.Tuolumne } },
                    { 6, new List<CountyTypeEnum>() { CountyTypeEnum.ElDorado, CountyTypeEnum.Placer, CountyTypeEnum.Sacramento } },
                    { 7, new List<CountyTypeEnum>() { CountyTypeEnum.Sacramento, CountyTypeEnum.Yolo } },
                    { 8, new List<CountyTypeEnum>() { CountyTypeEnum.Sacramento } },
                    { 9, new List<CountyTypeEnum>() { CountyTypeEnum.Sacramento, CountyTypeEnum.SanJoaquin } },
                    { 10, new List<CountyTypeEnum>() { CountyTypeEnum.Marin, CountyTypeEnum.Sonoma } },
                    { 11, new List<CountyTypeEnum>() { CountyTypeEnum.ContraCosta, CountyTypeEnum.Sacramento, CountyTypeEnum.Solano } },
                    { 12, new List<CountyTypeEnum>() { CountyTypeEnum.SanJoaquin, CountyTypeEnum.Stanislaus } },
                    { 13, new List<CountyTypeEnum>() { CountyTypeEnum.SanJoaquin } },
                    { 14, new List<CountyTypeEnum>() { CountyTypeEnum.ContraCosta, CountyTypeEnum.Solano } },
                    { 15, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.ContraCosta } },
                    { 16, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.ContraCosta } },
                    { 17, new List<CountyTypeEnum>() { CountyTypeEnum.SanFrancisco } },
                    { 18, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda } },
                    { 19, new List<CountyTypeEnum>() { CountyTypeEnum.SanFrancisco, CountyTypeEnum.SanMateo } },
                    { 20, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda } },
                    { 21, new List<CountyTypeEnum>() { CountyTypeEnum.Merced, CountyTypeEnum.Stanislaus } },
                    { 22, new List<CountyTypeEnum>() { CountyTypeEnum.SanMateo } },
                    { 23, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno, CountyTypeEnum.Tulare } },
                    { 24, new List<CountyTypeEnum>() { CountyTypeEnum.SanMateo, CountyTypeEnum.SantaClara } },
                    { 25, new List<CountyTypeEnum>() { CountyTypeEnum.Alameda, CountyTypeEnum.SantaClara } },
                    { 26, new List<CountyTypeEnum>() { CountyTypeEnum.Inyo, CountyTypeEnum.Kern, CountyTypeEnum.Tulare } },
                    { 27, new List<CountyTypeEnum>() { CountyTypeEnum.SantaClara } },
                    { 28, new List<CountyTypeEnum>() { CountyTypeEnum.SantaClara } },
                    { 29, new List<CountyTypeEnum>() { CountyTypeEnum.Monterey, CountyTypeEnum.SantaClara, CountyTypeEnum.SantaCruz } },
                    { 30, new List<CountyTypeEnum>() { CountyTypeEnum.Monterey, CountyTypeEnum.SanBenito, CountyTypeEnum.SantaClara, CountyTypeEnum.SantaCruz } },
                    { 31, new List<CountyTypeEnum>() { CountyTypeEnum.Fresno } },
                    { 32, new List<CountyTypeEnum>() { CountyTypeEnum.Kern, CountyTypeEnum.Kings } },
                    { 33, new List<CountyTypeEnum>() { CountyTypeEnum.SanBernardino } },
                    { 34, new List<CountyTypeEnum>() { CountyTypeEnum.Kern } },
                    { 35, new List<CountyTypeEnum>() { CountyTypeEnum.SanLuisObispo, CountyTypeEnum.SantaBarbara } },
                    { 36, new List<CountyTypeEnum>() { CountyTypeEnum.Kern, CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 37, new List<CountyTypeEnum>() { CountyTypeEnum.SanLuisObispo, CountyTypeEnum.SantaBarbara, CountyTypeEnum.Ventura } },
                    { 38, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 39, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 40, new List<CountyTypeEnum>() { CountyTypeEnum.SanBernardino } },
                    { 41, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 42, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside, CountyTypeEnum.SanBernardino } },
                    { 43, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 44, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 45, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Ventura } },
                    { 46, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 47, new List<CountyTypeEnum>() { CountyTypeEnum.SanBernardino } },
                    { 48, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 49, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 50, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 51, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 52, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.SanBernardino } },
                    { 53, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 54, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 55, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles, CountyTypeEnum.Orange, CountyTypeEnum.SanBernardino } },
                    { 56, new List<CountyTypeEnum>() { CountyTypeEnum.Imperial, CountyTypeEnum.Riverside } },
                    { 57, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 58, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 59, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 60, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 61, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 62, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 63, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 64, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 65, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 66, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 67, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside } },
                    { 68, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 69, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 70, new List<CountyTypeEnum>() { CountyTypeEnum.LosAngeles } },
                    { 71, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside, CountyTypeEnum.SanDiego } },
                    { 72, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 73, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 74, new List<CountyTypeEnum>() { CountyTypeEnum.Orange } },
                    { 75, new List<CountyTypeEnum>() { CountyTypeEnum.Riverside, CountyTypeEnum.SanDiego } },
                    { 76, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 77, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 78, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 79, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } },
                    { 80, new List<CountyTypeEnum>() { CountyTypeEnum.SanDiego } }
                }
            }
        };
    }
}
