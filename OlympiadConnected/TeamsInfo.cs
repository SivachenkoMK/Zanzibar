using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.OlympiadConnected
{
    public static class TeamsInfo
    {
        public static string[] TeamNames = new string[20];

        public static string[] TeamPasswords = new string[20];

        public static int GetNumberByTeamName(string teamName)
        {
            for (int i = 0; i < TeamNames.Length; i++)
            {
                if (TeamNames[i] == teamName)
                {
                    return i + 1;
                }
            }
            throw new Exception("No team found when searching by name");
        }

        public static int GetNumberByTeamPassword(string teamPassword)
        {
            for (int i = 0; i < TeamPasswords.Length; i++)
            {
                if (TeamPasswords[i] == teamPassword)
                {
                    return i + 1;
                }
            }
            throw new Exception("No team found when searching by password");
        }

        public static string GetTeamNameByPassword(string teamPassword)
        {
            int number = GetNumberByTeamPassword(teamPassword) - 1;
            return TeamNames[number];
        }

        public static bool IsOneOfPasswordsForCaptain(string supposedPassword)
        {
            foreach (string password in TeamPasswords)
            {
                if (password == supposedPassword)
                {
                    return true;
                }
            }
            return false;
        }

        public static void InitializeTeams()
        {
            TeamNames[0] = "Team1";
            TeamPasswords[0] = "Password1";

            TeamNames[1] = "Team2";
            TeamPasswords[1] = "Password2";

            TeamNames[2] = "Team3";
            TeamPasswords[2] = "Password3";

            TeamNames[3] = "Team4";
            TeamPasswords[3] = "Password4";

            TeamNames[4] = "Team5";
            TeamPasswords[4] = "Password5";

            TeamNames[5] = "Team6";
            TeamPasswords[5] = "Password6";

            TeamNames[6] = "Team7";
            TeamPasswords[6] = "Password7";

            TeamNames[7] = "Team8";
            TeamPasswords[7] = "Password8";

            TeamNames[8] = "Team9";
            TeamPasswords[8] = "Password9";

            TeamNames[9] = "Team10";
            TeamPasswords[9] = "Password10";

            TeamNames[10] = "Team11";
            TeamPasswords[10] = "Password11";

            TeamNames[11] = "Team12";
            TeamPasswords[11] = "Password12";

            TeamNames[12] = "Team13";
            TeamPasswords[12] = "Password13";

            TeamNames[13] = "Team14";
            TeamPasswords[13] = "Password14";

            TeamNames[14] = "Team15";
            TeamPasswords[14] = "Password15";

            TeamNames[15] = "Team16";
            TeamPasswords[15] = "Password16";

            TeamNames[16] = "Team17";
            TeamPasswords[16] = "Password17";

            TeamNames[17] = "Team18";
            TeamPasswords[17] = "Password18";

            TeamNames[18] = "Team19";
            TeamPasswords[18] = "Password19";

            TeamNames[19] = "Team20";
            TeamPasswords[19] = "Password20";
        }
    }
}
