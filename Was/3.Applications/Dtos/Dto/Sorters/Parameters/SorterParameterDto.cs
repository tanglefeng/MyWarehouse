namespace Kengic.Was.Application.WasModel.Dto.Sorters.Parameters
{
    public class SorterParameterDto : EntityForTimeDto<string>
    {
        public string ConnectionName { get; set; }
        public int StorageDb { get; set; }
        public int StartAddress { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
    }
}