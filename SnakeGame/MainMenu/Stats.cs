using System;
using System.Collections.Generic;
using System.Drawing;
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
            return new Stats() { BestScore = 0, CurrentSkin = "green" };
        }

        public static void SaveStats(Stats stats)
        {
            string json = JsonSerializer.Serialize(stats);
            File.WriteAllText(Application.StartupPath + "/data.json", json);
        }
    }

    public class SkinsManager
    {
        public static Color GetSkinColor(string skin)
        {
            switch (skin.ToLower())
            {
                case "green":
                    return Color.Green;
                case "yellow":
                    return Color.Yellow;
                case "blue":
                    return Color.Blue;
                case "purple":
                    return Color.Purple;
                case "pink":
                    return Color.Pink;
                case "lightgreen":
                    return Color.LawnGreen;
                default:
                    return Color.White;
            }
        }
    }
}
