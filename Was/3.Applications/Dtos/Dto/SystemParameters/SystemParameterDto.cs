namespace Kengic.Was.Application.WasModel.Dto.SystemParameters
{
    public class SystemParameterDto : EntityForTimeDto<string>
    {
        public string Value { get; set; }
        public string Template { get; set; }
    }
}