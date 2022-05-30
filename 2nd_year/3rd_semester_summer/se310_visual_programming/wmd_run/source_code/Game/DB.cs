using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
    
namespace Game
{
    public class Profile
    {
        public string Name { set; get; }
        public string Faction { set; get; }
        public bool Exist { set; get; }
        public Profile(string Name)
        {
            //Checks if the entered username already exists
            this.Name = Name;
            Exist = false;
            List<string> file = File.ReadAllLines("DB/Profiles.txt").ToList();
            foreach (string line in file)
            {
                string[] partition = line.Split(',');
                if (partition[0]==Name)
                    Exist = true;
            }
        }
        public void Insert()
        {
            //Flush the new profile along with the read profiles into Profile.txt
            List<string> file = File.ReadAllLines("DB/Profiles.txt").ToList();
            file.Add(Name+","+Faction);
            File.WriteAllLines("DB/Profiles.txt", file);
        }

        public void Editor()
        {
            //Handles profile editing
            List<string> file = File.ReadAllLines("DB/Profiles.txt").ToList();
            //Removes the old profile
            if(file.Contains(Name+",Rising Sun"))
            file.Remove(Name + ",Rising Sun");
            else if (file.Contains(Name + ",Soviets"))
                file.Remove(Name + ",Soviets");
            else if (file.Contains(Name + ",Allies"))
                file.Remove(Name + ",Allies");
            else if (file.Contains(Name + ",NOD"))
                file.Remove(Name + ",NOD");
            //Inserts the new profile
            file.Add(Name + "," + Faction);
            File.WriteAllLines("DB/Profiles.txt", file);
        }

        public string FindFaction()
        {
            //Returns the faction name
            List<string> file = File.ReadAllLines("DB/Profiles.txt").ToList();
            foreach (string line in file)
            {
                string[] partition = line.Split(',');
                if (partition[0] == Name)
                    return partition[1];
            }
            //Default faction name 'COM'
            return "Allies";
        }
    }

    public class Stat
    {
        public int Games { set; get; }
        public int Profiles { set; get; }
        public string HD { set; get; }
        public string LD { set; get; }
        public string AD { set; get; }
        public bool IsEmpty { set; get; }
        public Stat()
        {
            List<string> file = File.ReadAllLines("DB/History.txt").ToList();
            if (file.Any<string>())
            {
                List<string> time = new List<string>();
                List<int> sec = new List<int>();
                Games = File.ReadLines("DB/History.txt").Count();
                Profiles = File.ReadLines("DB/Profiles.txt").Count();
                foreach (string line in file)
                {
                    string[] partition = line.Split(',');
                    //The time of the games
                    time.Add(partition[2]);
                }
                foreach (string line in time)
                {
                    string[] partition = line.Split(':');
                    //The duration of the games
                    sec.Add(60 * Convert.ToInt32(partition[0]) + Convert.ToInt32(partition[1]));
                }
                int max = sec.Max();
                int min = sec.Min();
                int all = sec.Sum();
                HD = (max / 60) + " : " + (max % 60);
                LD = (min / 60) + " : " + (min % 60);
                AD = (all / 60) + " : " + (all % 60);
            }
            else
                IsEmpty = true;
        }
    }

    public class History
    {
        public int GNo { set; get; }
        public string Date { set; get; }
        public string Time { set; get; }
        public int Steps { set; get; }
        public string Winner { set; get; }
        public int MaxS { set; get; }
        public int MinS { set; get; }
        public string[] Players { set; get; }

        public History(string t,int s,int MaxS,int MinS,string[] players)
        {
            GNo = File.ReadLines("DB/History.txt").Count()+1;
            Date = DateTime.Now.ToLongTimeString();
            Time = t;
            Steps = s;
            this.MaxS = MaxS;
            this.MinS = MinS;
            Players = players;
        }
        public void Setwinner(string w)
        {
            Winner = w;
        }
        public void Apply()
        {
            //Flushes the whole game stats
            List<string> file = File.ReadAllLines("DB/History.txt").ToList();
            file.Add(GNo + "," + Date + "," + Time + "," + Steps + "," + Winner+","+MaxS + "," + MinS + "," + Players[0] + "," + Players[1] + "," + Players[2] + "," + Players[3]);
            File.WriteAllLines("DB/History.txt", file);
        }
    }

    public class InGame
    {
        public int GameNo { set; get; }
        public string Player { set; get; }
        public string Procedure { set; get; }
        public string Value { set; get; }
        public List<string> Actions { set; get; }
        public int Prey { set; get; }

        public InGame()
        {
            GameNo = File.ReadLines("DB/History.txt").Count()+1;
            Actions = new List<string>();
        }

        public void Flush(string a,string b,string c,int d)
        {
            //Record every activity of each game
            Actions.Add(GameNo+","+a+","+b+","+c+","+d);
        }
        public void Apply()
        {
            //Flushes to InGame.txt at the end of the game
            List<string> file = File.ReadAllLines("DB/InGame.txt").ToList();
            Actions.AddRange(file);
            File.WriteAllLines("DB/InGame.txt", Actions);
        }
    }
}
