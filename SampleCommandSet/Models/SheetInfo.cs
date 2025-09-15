using System;
using System.Collections.Generic;

namespace SampleCommandSet.Models
{
    public class SheetInfo
    {
        public long Id { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string SheetNumber { get; set; }
        public List<long> ViewIds { get; set; }
    }
}
