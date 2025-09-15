using System.Collections.Generic;

namespace SampleCommandSet.Models
{
    public class ScheduleFieldInfo
    {
        public string Name { get; set; }
        public string ParameterName { get; set; }
    }

    public class ScheduleFilterInfo
    {
        public string FieldName { get; set; }
        public string FilterType { get; set; }
        public string Value { get; set; }
    }

    public class TableViewInfo
    {
        public long Id { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public List<ScheduleFieldInfo> Fields { get; set; }
        public List<ScheduleFilterInfo> Filters { get; set; }
    }
}
