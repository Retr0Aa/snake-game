using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame.MainMenu
{
    public class Stats
    {
        public int BestScore { get; set; }
        public string CurrentSkin { get; set; }
    }

    public class StatsLoader
    {
        public static Stats LoadStats()
        {
            string path = Application.StartupPath + "/data.json";
            if (File.Exists(path))
            {
                return JsonSerializer.Deserialize<Stats>(File.ReadAllText(path));
            }
            return new Stats() { BestScore = 0, CurrentSkin = "Green" };
        }

        public static void SaveStats(Stats stats)
        {
            string json = JsonSerializer.Serialize(stats);
            File.WriteAllText(Application.StartupPath + "/data.json", json);
        }
    }
}
