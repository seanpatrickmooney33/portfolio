using System;
using System.Collections.Generic;
using System.Text;
using Prototype.Data;

namespace Prototype.Test.Utility
{
    public static class TestData
    {
        #region Election
        
        public const string ElectionName = "myDefaultElection";
        public static DateTime ElectionDate = DateTime.UtcNow;
        public const Boolean ElectionIsActive = true;
        
        #endregion Election

        #region Race
        
        public const String Rmsg = "[B|CAS|1|20200225|1704030]\n" +
            "[R|CAS|2|110200250000|U.S. House of Representatives District 25|SE]\n" +
            "[R|CAS|3|120200280000|State Senate District 28|SE]\n" +
            "[E|CAS|4|20200225|1704030]\n";



        public const int Race1Id = 1;
        public const String Race1Name = "State Senate District 28";
        public const RaceTypeEnum Race1Type = RaceTypeEnum.StateSenate;
        public const int Race1District = 28;
        public const String Race1Description = "mbikfjtnrie9b8 ouhensner ose irs987vb  www";
        public const Boolean Race1Locked = false;
        public const String Race1RedisKey = "02120000280000";

        public const int Race2Id = 2;
        public const String Race2Name = "U.S. House of Representatives District 25";
        public const RaceTypeEnum Race2Type = RaceTypeEnum.Usrep;
        public const int Race2District = 25;
        public const String Race2Description = "He is a unique description of asdkfja jbipoajwipoenrepre";
        public const Boolean Race2Locked = false;
        public const String Race2RedisKey = "02110000250000";



        #endregion Race


        #region Candidate

        #region 214
        public const int Candidate214Id = 214;
        public const String Candidate214LastName = "Nevenic";
        public const String Candidate214FirstName = "Anna ";
        public const String Candidate214MiddleName = "";
        public const String Candidate214DisplayName = "Anna Nevenic";
        public const int Candidate214Displayorder = 1;
        public const PartyTypeEnum Candidate214Party = PartyTypeEnum.Democratic;
        #endregion 214
        
        #region 215
        public const int Candidate215Id = 215;
        public const String Candidate215LastName = "Romero";
        public const String Candidate215FirstName = "Elizabeth";
        public const String Candidate215MiddleName = "";
        public const String Candidate215DisplayName = "Elizabeth Romero";
        public const int Candidate215Displayorder = 2;
        public const PartyTypeEnum Candidate215Party = PartyTypeEnum.Democratic;
        #endregion 215

        #region 216
        public const int Candidate216Id = 216;
        public const String Candidate216LastName = "Silver";
        public const String Candidate216FirstName = "Joy";
        public const String Candidate216MiddleName = "";
        public const String Candidate216DisplayName = "Joy Silver";
        public const int Candidate216Displayorder = 3;
        public const PartyTypeEnum Candidate216Party = PartyTypeEnum.Democratic;
        #endregion 216

        #region 217
        public const int Candidate217Id = 217;
        public const String Candidate217LastName = "Melendez";
        public const String Candidate217FirstName = "Melissa";
        public const String Candidate217MiddleName = "";
        public const String Candidate217DisplayName = "Melissa Melendez";
        public const int Candidate217Displayorder = 4;
        public const PartyTypeEnum Candidate217Party = PartyTypeEnum.Republican;
        #endregion 217

        #region 218
        public const int Candidate218Id = 218;
        public const String Candidate218LastName = "Schwab";
        public const String Candidate218FirstName = "John";
        public const String Candidate218MiddleName = "";
        public const String Candidate218DisplayName = "John Schwab";
        public const int Candidate218Displayorder = 5;
        public const PartyTypeEnum Candidate218Party = PartyTypeEnum.Republican;
        #endregion 218

        #region 231
        public const int Candidate231Id = 231;
        public const String Candidate231LastName = "Cooper III";
        public const String Candidate231FirstName = "Robert";
        public const String Candidate231MiddleName = "";
        public const String Candidate231DisplayName = "Robert Cooper III";
        public const int Candidate231Displayorder = 1;
        public const PartyTypeEnum Candidate231Party = PartyTypeEnum.Democratic;
        #endregion 231

        #region 232
        public const int Candidate232Id = 232;
        public const String Candidate232LastName = "Elize";
        public const String Candidate232FirstName = "Getro ";
        public const String Candidate232MiddleName = "F. ";
        public const String Candidate232DisplayName = "Getro F. Elize";
        public const int Candidate232Displayorder = 2;
        public const PartyTypeEnum Candidate232Party = PartyTypeEnum.Democratic;
        #endregion 232

        #region 233
        public const int Candidate233Id = 233;
        public const String Candidate233LastName = "Rudnick";
        public const String Candidate233FirstName = "F.";
        public const String Candidate233MiddleName = "David";
        public const String Candidate233DisplayName = "F. David Rudnick";
        public const int Candidate233Displayorder = 3;
        public const PartyTypeEnum Candidate233Party = PartyTypeEnum.Democratic;
        #endregion 233

        #region 234
        public const int Candidate234Id = 234;
        public const String Candidate234LastName = "Smith";
        public const String Candidate234FirstName = "Christy";
        public const String Candidate234MiddleName = "";
        public const String Candidate234DisplayName = "Christy Smith";
        public const int Candidate234Displayorder = 4;
        public const PartyTypeEnum Candidate234Party = PartyTypeEnum.Democratic;
        #endregion 234

        #region 235
        public const int Candidate235Id = 235;
        public const String Candidate235LastName = "Uygur";
        public const String Candidate235FirstName = "Cenk";
        public const String Candidate235MiddleName = "";
        public const String Candidate235DisplayName = "Cenk Uygur";
        public const int Candidate235Displayorder = 5;
        public const PartyTypeEnum Candidate235Party = PartyTypeEnum.Democratic;
        #endregion 235

        #region 236
        public const int Candidate236Id = 236;
        public const String Candidate236LastName = "Valdéz-Ortega";
        public const String Candidate236FirstName = "Aníbal";
        public const String Candidate236MiddleName = "";
        public const String Candidate236DisplayName = "Aníbal Valdéz-Ortega";
        public const int Candidate236Displayorder = 6;
        public const PartyTypeEnum Candidate236Party = PartyTypeEnum.Democratic;
        #endregion 236

        #region 237
        public const int Candidate237Id = 237;
        public const String Candidate237LastName = "Garcia";
        public const String Candidate237FirstName = "Mike";
        public const String Candidate237MiddleName = "";
        public const String Candidate237DisplayName = "Mike Garcia";
        public const int Candidate237Displayorder = 7;
        public const PartyTypeEnum Candidate237Party = PartyTypeEnum.Republican;
        #endregion 237

        #region 238
        public const int Candidate238Id = 238;
        public const String Candidate238LastName = "Jenks";
        public const String Candidate238FirstName = "Kenneth";
        public const String Candidate238MiddleName = "";
        public const String Candidate238DisplayName = "Kenneth Jenks";
        public const int Candidate238Displayorder = 8;
        public const PartyTypeEnum Candidate238Party = PartyTypeEnum.Republican;
        #endregion 238

        #region 239
        public const int Candidate239Id = 239;
        public const String Candidate239LastName = "Knight";
        public const String Candidate239FirstName = "Steve";
        public const String Candidate239MiddleName = "";
        public const String Candidate239DisplayName = "Steve Knight";
        public const int Candidate239Displayorder = 9;
        public const PartyTypeEnum Candidate239Party = PartyTypeEnum.Republican;
        #endregion 239

        #region 240
        public const int Candidate240Id = 240;
        public const String Candidate240LastName = "Lackey";
        public const String Candidate240FirstName = "Courtney";
        public const String Candidate240MiddleName = "";
        public const String Candidate240DisplayName = "Courtney Lackey";
        public const int Candidate240Displayorder = 10;
        public const PartyTypeEnum Candidate240Party = PartyTypeEnum.Republican;
        #endregion 240

        #region 241
        public const int Candidate241Id = 241;
        public const String Candidate241LastName = "Lozano";
        public const String Candidate241FirstName = "David";
        public const String Candidate241MiddleName = "";
        public const String Candidate241DisplayName = "David Lozano";
        public const int Candidate241Displayorder = 11;
        public const PartyTypeEnum Candidate241Party = PartyTypeEnum.Republican;
        #endregion 241

        #region 242
        public const int Candidate242Id = 242;
        public const String Candidate242LastName = "Mercuri";
        public const String Candidate242FirstName = "Daniel";
        public const String Candidate242MiddleName = "";
        public const String Candidate242DisplayName = "Daniel Mercuri";
        public const int Candidate242Displayorder = 12;
        public const PartyTypeEnum Candidate242Party = PartyTypeEnum.Republican;
        #endregion 242

        public const String Cmsg = "[B|CAS|1|20200229|1423190]\n" +
            "[C|CAS|2|120200280000|214||Nevenic|Anna ||01|Anna Nevenic]\n" +
            "[C|CAS|3|120200280000|215||Romero|Elizabeth||01|Elizabeth Romero]\n" +
            "[C|CAS|4|120200280000|216||Silver|Joy||01|Joy Silver]\n" +
            "[C|CAS|5|120200280000|217||Melendez|Melissa||02|Melissa Melendez]\n" +
            "[C|CAS|6|120200280000|218||Schwab|John||02|John Schwab]\n" +
            "[C|CAS|7|110200250000|231||Cooper III|Robert||01|Robert Cooper III]\n" +
            "[C|CAS|8|110200250000|232||Elize|Getro |F. |01|Getro F. Elize]\n" +
            "[C|CAS|9|110200250000|233||Rudnick|F.|David|01|F. David Rudnick]\n" +
            "[C|CAS|10|110200250000|234||Smith|Christy||01|Christy Smith]\n" +
            "[C|CAS|11|110200250000|235||Uygur|Cenk||01|Cenk Uygur]\n" +
            "[C|CAS|12|110200250000|236||Valdéz-Ortega|Aníbal||01|Aníbal Valdéz-Ortega]\n" +
            "[C|CAS|13|110200250000|237||Garcia|Mike||02|Mike Garcia]\n" +
            "[C|CAS|14|110200250000|238||Jenks|Kenneth||02|Kenneth Jenks]\n" +
            "[C|CAS|15|110200250000|239||Knight|Steve||02|Steve Knight]\n" +
            "[C|CAS|16|110200250000|240||Lackey|Courtney||02|Courtney Lackey]\n" +
            "[C|CAS|17|110200250000|241||Lozano|David||02|David Lozano]\n" +
            "[C|CAS|18|110200250000|242||Mercuri|Daniel||02|Daniel Mercuri]\n" +
            "[E|CAS|19|20200229|1423190]\n";

        #endregion Candidate

        #region Values
        public const string Vmsg = "[B|CAS|1|20200317|1641320]\n"+
                            "[A|CAS|2|19||||180|180|||||202003171641|202003171641]\n"+
                            "[A|CAS|3|33||||402|402|||||202003171641|202003171641]\n"+
                            "[A|CAS|4|56||||72|72|||||202003171641|202003171641]\n"+
                            "[A|CAS|5|59||||654|654|||||202003171641|202003171641]\n"+
                            "[E|CAS|6|20200317|1641320]\n"+
                            "[B|CAS|1|20200317|1641320]\n"+
                            "[V|CAS|2|120200280000|59|402|402||214|5912|2.9|2.9|215|47516|23.5|23.5|216|42222|20.9|20.9|217|81918|40.5|40.5|218|24536|12.1|12.1]\n"+
                            "[V|CAS|3|120200280000|33|402|402||214|5912|2.90|2.90|215|47516|23.50|23.50|216|42222|20.90|20.90|217|81918|40.50|40.50|218|24536|12.10|12.10]\n"+
                            "[V|CAS|4|110200250000|59|252|252||231|2908|1.8|1.8|232|1389|0.9|0.9|233|1062|0.7|0.7|234|57423|36.1|36.1|235|10391|6.5|6.5|236|7214|4.5|4.5|237|40311|25.4|25.4|238|2498|1.6|1.6|239|27372|17.2|17.2|240|3030|1.9|1.9|241|2726|1.7|1.7|242|2525|1.6|1.6]\n"+
                            "[V|CAS|5|110200250000|19|180|180||231|2510|2.00|2.00|232|1272|1.00|1.00|233|919|0.70|0.70|234|45011|36.00|36.00|235|8725|7.00|7.00|236|6387|5.10|5.10|237|29121|23.30|23.30|238|2122|1.70|1.70|239|21897|17.50|17.50|240|2554|2.00|2.00|241|2357|1.90|1.90|242|2231|1.80|1.80]\n"+
                            "[V|CAS|6|110200250000|56|72|72||231|398|1.20|1.20|232|117|0.30|0.30|233|143|0.40|0.40|234|12412|36.80|36.80|235|1666|4.90|4.90|236|827|2.50|2.50|237|11190|33.20|33.20|238|376|1.10|1.10|239|5475|16.20|16.20|240|476|1.40|1.40|241|369|1.10|1.10|242|294|0.90|0.90]\n"+
                            "[E|CAS|7|20200317|1641320]\n";

        #endregion Values
    }
}
