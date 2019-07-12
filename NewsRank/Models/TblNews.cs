using System;
using System.Collections.Generic;

namespace NewsRank.Models
{
    public partial class TblNews
    {
        public int Id { get; set; }
        public int NewsRank { get; set; }
        public string NewsTitle { get; set; }
        public string NewsUrl { get; set; }
        public DateTime? SubmitTime { get; set; }
        public string NewsType { get; set; }
        public string NewsContent { get; set; }
    }
}
