using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Parameters
{
    public class SorterParameter : EntityForTime<string>
    {
        public string ConnectionName { get; set; }
        public int StorageDb { get; set; }
        public int StartAddress { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
    }
}