namespace Kengic.Was.Application.WasModel.Dto
{
    public class DynamicQueryDto
    {
        public string Method { get; set; }
        public string MethodTypeName { get; set; }
        public object[] Parameters { get; set; }
        public string TypeName { get; set; }
    }
}