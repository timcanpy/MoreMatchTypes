using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreMatchTypes.Data_Classes.Storage
{
    class SurvivalSaveData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Promotion { get; set; }
        public string Date { get; set; }
        public string OwnerID { get; set; }
        public string Record { get; set; }
        public int MaxWinStreak { get; set; }
        public float MaxRating { get; set; }
        public List<string> Details { get; set; }
    }
}
