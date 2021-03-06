﻿using System.Collections.Generic;
using System.ComponentModel;

namespace Models
{
    public class Tire : Entity
    {
        public Producer Producer {get; set;} 
        public TireSeason Season {get; set;} 
        public int Width {get; set;} 
        public int Profile {get; set;} 
        public int Diameter {get; set;}

        public IEnumerable<Order> Orders {get; set;}
    }

    public enum TireSeason
    {
        Summer = 0,
        Winter = 1,
        Allseason = 2
    }
}