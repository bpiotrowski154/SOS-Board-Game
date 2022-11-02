
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SOS
{
    public class Player
    {
        public Player(string color, Brush colorValue)
        {
            playerColor = color;
            this.colorValue = colorValue;
            this.placementType = "S";
            totalPoints = 0;
        }
        public string playerColor { get; set; }
        public Brush colorValue { get; set; }
        public string placementType {get; set; }
        public int totalPoints { get; set; }
    }
}
