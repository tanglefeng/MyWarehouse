//====================================================================
// Function:
// this class expands the data of an OPCItemStateMgt-object with
// specific data to provide a maximum flexibility and performance
//====================================================================

namespace Kengic.Was.CrossCutting.Opc.OpcItemManages
{
    /// <summary>
    ///     this class expands the data of an OPCItemExtender-object with specific
    ///     data to provide a maximum flexibility and performance
    /// </summary>
    public class OpcItemExtenderNumber
    {
        private object _itemValue;

        /// <summary>
        ///     creates a instance of this class
        /// </summary>
        /// <param name="itemIsActive">
        ///     determines whether the item is active
        /// </param>
        /// <param name="itemId">the item id of the item</param>
        /// <param name="itemServerHandle">
        ///     the server handle (provided from the opc server)
        /// </param>
        /// <param name="itemClientHandle">the client handle of the item</param>
        /// <param name="itemName">the item Name of the item</param>
        /// <param name="groupName">the groupName of the item</param>
        /// <param name="opcItemControl">
        ///     the corresponding object of this opc item;
        /// </param>
        public OpcItemExtenderNumber(bool itemIsActive, string itemId, int itemServerHandle, int itemClientHandle,
            string itemName, string groupName,
            ref IOpcItemCorrespond opcItemControl)
        {
            ItemId = itemId;
            IsActive = itemIsActive;
            ServerHandle = itemServerHandle;
            ClientHandle = itemClientHandle;
            ItemName = itemName;
            GourpName = groupName;
            OpcItemCorrespond = opcItemControl;
        }

        public string GourpName { get; set; }
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public int ServerHandle { get; set; }
        public int ClientHandle { get; set; }
        public bool IsActive { get; set; }

        public object ActiveValue
        {
            // only returns the last written value! (not of the value of the control, which might be changed)
            // this is necessary to have the chance to have consistent data
            get { return _itemValue; }
            set
            {
                _itemValue = value;
                OpcItemCorrespond?.SetOpcParameter(value);
            }
        }

        public IOpcItemCorrespond OpcItemCorrespond { get; set; }
    }
}