namespace Kengic.Was.Application.Services.Prodave
{
    public class ProdaveApplicationService : IProdaveApplicationService
    {
        public bool ReadWord(string datablock, int startAddress, int dataLength, out object[] buffer)
        {
            buffer = new object[] {};
            return true;
        }

        public bool WriteWord(string datablock, int startAddress, object[] buffer) => true;
    }
}