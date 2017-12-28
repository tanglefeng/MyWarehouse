namespace Kengic.Was.CrossCutting.Opc.OpcItemManages
{
    /// <summary>
    ///     <see langword="interface" /> to connect ui-objects with the values
    ///     delivered from opc-values
    /// </summary>
    public interface IOpcItemCorrespond
    {
        void SetOpcParameter(object theValue);
    }
}