using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS
{
    public struct CellData
    {
        public CellData(string value, string colorOfPlayer)
        {
            this.value = value;
            this.colorOfPlayer = colorOfPlayer;
        }
        public string value { get; set; }
        public string colorOfPlayer { get; set; }

    }
}
