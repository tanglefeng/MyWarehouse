namespace Kengic.Was.Application.Services.Prodave
{
    public interface IProdaveApplicationService
    {
        bool ReadWord(string datablock, int startAddress, int dataLength, out object[] buffer);
        bool WriteWord(string datablock, int startAddress, object[] buffer);
    }
}