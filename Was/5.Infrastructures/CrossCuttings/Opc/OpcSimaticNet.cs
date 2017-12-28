//====================================================================
// Function:
// This class provides some function which rebuild 
// the function include connection,read,write,formatData and so on
// must be running in X86
//====================================================================

using System;
using System.Collections.Generic;
using System.Resources;
using Kengic.Was.CrossCutting.Opc.OpcGroupManages;
using Kengic.Was.CrossCutting.Opc.OpcItemManages;
using Kengic.Was.CrossCutting.Opc.OpcSeverManages;
using OpcRcw.Da;

namespace Kengic.Was.CrossCutting.Opc
{
    /// <summary>
    ///     This class provides some function which rebuild the function include
    ///     connection,read,write,formatData and so on
    /// </summary>
    public class OpcSimaticNet
    {
        // Connect to the server with "OPC.SimaticNet"
        private const string OpcId = "OPC.SimaticNet";

        private OpcServerManagement _opcManagementServer;

        // Internal used variables for OPC
        private int _transIdSwitchPos;
        //accepts all OPCGroupManagement-objects which is using
        public OpcGroupManagementList OpcGroupManagerList;

        //accepts all OPCItemExtender-objects which is using
        public OpcItemExtenderList OpcItemExtenderList;
        // ResourceManager m_ResMan - language Resource
        public ResourceManager ResourceManager { get; set; }

        /// <summary>
        ///     Connect OPC server Connect the client with the server Connect to the
        ///     server with "OPC.SimaticNet"
        /// </summary>
        public void ConnectServer(out string outMessage)
        {
            try
            {
                OpcItemExtenderList = new OpcItemExtenderList(ResourceManager);
                OpcGroupManagerList = new OpcGroupManagementList(ResourceManager);

                //Connect to the Server with "OPC.SimaticNet"
                _opcManagementServer = new OpcServerManagement(ResourceManager);
                _opcManagementServer.ConnectOpcServer(OpcId);
                outMessage = ResourceManager.GetString("EXT_OPC_SERVER_CONNECTED");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"ConnectServer£ºHave some Error when connecting OPC.SimaticNet_{ResourceManager.GetString("EXT_OPC_SERVER_CONNECTED")},ERROR_{ex.Message}");
            }
        }


        public bool InitializeGroupAndItem(string itemofGroupName, string groupReadWriteType, string groupSyncType,
            int groupClientHandle,
            bool[] itemsActive, string[] itemsId, string[] itemName, int[] itmsClientHandle, int itemNum,
            int updateRateSettings,
            out string message, out bool groupActiveSatus)
        {
            //ResourceManager.GetString("EXT_OPC_ADDING_GROUP");
            OpcGroupManagement opcGroupManager = null;
            groupActiveSatus = false;
            //default UpdateRate is 1000ms
            //int updateRateSettings=1000;

            try
            {
                // Now build the opc items for the Item settings
                int[] itemsServerHandle;
                if (groupSyncType == "SYNCHRONOUS")
                {
                    CreateGroupsAndItems(itemofGroupName, false, groupClientHandle, updateRateSettings, null, true,
                        false,
                        itemsActive, itemsId, itmsClientHandle, groupReadWriteType, out itemsServerHandle,
                        out message, ref opcGroupManager, out groupActiveSatus);
                }
                else
                {
                    CreateGroupsAndItems(itemofGroupName, false, groupClientHandle, updateRateSettings, this, false,
                        true,
                        itemsActive, itemsId, itmsClientHandle, groupReadWriteType, out itemsServerHandle,
                        out message, ref opcGroupManager, out groupActiveSatus);
                }


                // Save the Opc-item Information into OpcItem Arraylist Structure 
                var opcItemCorrespond = new IOpcItemCorrespond[itemNum];
                if (message != ResourceManager.GetString("EXT_OPC_GROUP_MANAGERLIST"))
                {
                    var itmExtender = new OpcItemExtender(ResourceManager, itemofGroupName);

                    for (var j = 0; j < itemNum; j++)
                    {
                        var itmExNumber = new OpcItemExtenderNumber(itemsActive[j], itemsId[j], itemsServerHandle[j],
                            itmsClientHandle[j], itemName[j], itemofGroupName,
                            ref opcItemCorrespond[j]);
                        itmExtender.Add(itmExNumber);
                    }

                    OpcItemExtenderList.Add(itmExtender);
                }
                //Create DataChange Event if the Group is SYNCHRONOUS
                if (groupSyncType == "SYNCHRONOUS")
                {
                    return true;
                }

                if (opcGroupManager != null)
                {
                    opcGroupManager.OnOpcDataChanged += Opc_OnDataChange;
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return false;
            }
        }

        /// <summary>
        ///     creates the opcitems , creates OPCManagement-objects (they manage
        ///     the opc-handling of the OPCGroupStateMgt and OPCItemStateMgt)
        /// </summary>
        /// <param name="grpName">the group name</param>
        /// <param name="grpActive">
        ///     determines whether the group is active
        /// </param>
        /// <param name="grpClientHandle">
        ///     the client handles of the group
        /// </param>
        /// <param name="reqUpdateRate">
        ///     the requested update rate of the group
        /// </param>
        /// <param name="callBackReceiver">
        ///     the object which wants to get the callback messages (e.g.
        ///     OnDataChange)
        /// </param>
        /// <param name="setSync">
        ///     determines whether a sync-interface is needed (for sync read/write)
        /// </param>
        /// <param name="setAsync">
        ///     determines whether a async-interface is needed (for async
        ///     read/write, datachange)
        /// </param>
        /// <param name="itemActiveStates">
        ///     an array of booleans, to determine which opc-item should be set to
        ///     active
        /// </param>
        /// <param name="itemIDs">an array of item-ids</param>
        /// <param name="readWriteType"></param>
        /// <param name="itemServerHandles">
        ///     these are the delivered item serverhandles of the opc server
        /// </param>
        /// <param name="itemClientHandles">
        ///     an array of clienthandles for the items
        /// </param>
        /// <param name="message"></param>
        /// <param name="groupManagement"></param>
        /// <param name="outGroupActiceSatus"></param>
        /// <exception cref="Exception">
        ///     forwards any exception (with short error description)
        /// </exception>
        private void CreateGroupsAndItems(string grpName, bool grpActive, int grpClientHandle, int reqUpdateRate,
            object callBackReceiver, bool setSync, bool setAsync, bool[] itemActiveStates, string[] itemIDs,
            int[] itemClientHandles,
            string readWriteType, out int[] itemServerHandles, out string message,
            ref OpcGroupManagement groupManagement,
            out bool outGroupActiceSatus)
        {
            itemServerHandles = null;
            outGroupActiceSatus = false;

            try
            {
                message = ResourceManager.GetString("EXT_OPC_ADDING_GROUP");

                //Is there the Group in the GroupManagement List
                if (OpcGroupManagerList[grpName] != null)
                {
                    message = ResourceManager.GetString("EXT_OPC_GROUP_MANAGERLIST");
                    return;
                }

                // let the manager create a opc-group            
                int revisedUpdateRate;
                groupManagement = _opcManagementServer.AddGroup(grpName, grpActive, grpClientHandle, reqUpdateRate,
                    callBackReceiver, out revisedUpdateRate);

                // add this group to the list in order to dispose all groups on finalization
                OpcGroupManagerList.Add(groupManagement);


                // check the revised update rate
                if (revisedUpdateRate > reqUpdateRate)
                {
                    message = ResourceManager.GetString("EXT_OPC_REV_UPDATERATE") + revisedUpdateRate +
                              ResourceManager.GetString("EXT_OPC_REV_UPDATERATE2") + reqUpdateRate +
                              ResourceManager.GetString("EXT_OPC_REV_UPDATERATE3");
                }

                // Set Sync/Sync interfaces
                if (setSync)
                {
                    groupManagement.SetSyncInterface();
                }
                if (setAsync)
                {
                    groupManagement.ConnectCallback();
                    groupManagement.SetASyncInterface();
                }


                message = ResourceManager.GetString("EXT_OPC_ADD_GROUP");

                // let the manager add opc-items
                message = ResourceManager.GetString("EXT_OPC_ADDING_ITEMS");
                groupManagement.AddOpcItems(itemActiveStates, itemIDs, itemClientHandles, out itemServerHandles);
                message = ResourceManager.GetString("EXT_OPC_ADD_ITEMS");


                if (readWriteType != "Read")
                {
                    return;
                }

                if (ChangeGroupState(grpName, true))
                {
                    outGroupActiceSatus = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Have some error when initiliztion opc's groups and items!_{ex.Message}");
            }
        }

        /// <summary>
        ///     reads the item
        /// </summary>
        /// <param name="groupName">the name of group which will read</param>
        /// <param name="readType">readType: Synchronous or Asynchronous</param>
        /// <param name="message">out: message</param>
        public void ReadOpcGroup(string groupName, string readType, out string message)
        {
            OpcItemExtender opcReadItemExender = null;

            message = string.Empty;

            int[] errors = null;
            OPCITEMSTATE[] result = null;

            try
            {
                var opcReadGroupManagement = OpcGroupManagerList[groupName];
                if (opcReadGroupManagement == null)
                {
                    message = groupName + "no OPCGroupManagement Exit!";
                    return;
                }
                opcReadItemExender = OpcItemExtenderList[groupName];
                if (opcReadItemExender == null)
                {
                    message = groupName + "no OPCItemExtender Exit!";
                    return;
                }

                if (readType != "SYNCHRONOUS")
                {
                    return;
                }
                int revisedUpdateRate;
                opcReadGroupManagement.ChangeGroupState(opcReadGroupManagement.RevisedUpdateRate, true,
                    out revisedUpdateRate);
                message = ResourceManager.GetString("EXT_OPC_READINGSYNC");
                result = opcReadGroupManagement.ReadSync(OPCDATASOURCE.OPC_DS_CACHE,
                    opcReadItemExender.GetAllServerHandles(), out errors);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ReadOPCGroup ERROR£º{ex.Message}");
            }
            finally
            {
                //Save the informatin into OpcItem Arraylist 
                if ((errors != null) && (result != null))
                {
                    for (var dwCount = 0; dwCount < result.Length; dwCount++)
                    {
                        if ((errors[dwCount] == 0) && (GetQualityText(result[dwCount].wQuality) == "GOOD"))
                        {
                            // the clienthandle is the index of the meant timer in the timers array
                            //itm = ((OPCItemExtenderNumber)(opcReadItemExender[result[dwCount].hClient]));
                            // now update the opc-item-control
                            //itm.actValue = result[dwCount].vDataValue;
                            //(OPCItemExtender)(m_itmExtItemTest1Settings[result[dwCount].hClient]);
                            ((OpcItemExtenderNumber) opcReadItemExender[result[dwCount].hClient]).ActiveValue =
                                result[dwCount].vDataValue;
                        }
                        else
                        {
                            ((OpcItemExtenderNumber) opcReadItemExender[result[dwCount].hClient]).ActiveValue = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     changes the error code given from the OPC server into a string
        /// </summary>
        /// <param name="qualityText">error number</param>
        /// <returns>
        ///     error string
        /// </returns>
        internal string GetQualityText(int qualityText)
        {
            const int opcQualityBad = 0x0;
            const int opcQualityUncertain = 0x40;
            const int opcQualityGood = 0xC0;
            const int opcQualityConfigError = 0x4;
            const int opcQualityNotConnected = 0x8;
            const int opcQualityDeviceFailure = 0xC;
            const int opcQualitySensorFailure = 0x10;
            const int opcQualityLastKnown = 0x14;
            const int opcQualityCommFailure = 0x18;
            const int opcQualityOutOfService = 0x1C;
            const int opcQualityLastUsable = 0x44;
            const int opcQualitySensorCal = 0x50;
            const int opcQualityEguExceeded = 0x54;
            const int opcQualitySubNormal = 0x58;
            const int opcQualityLocalOverride = 0xD8;

            string returnQualityText;

            switch (qualityText)
            {
                case opcQualityBad:
                    returnQualityText = "BAD";
                    break;
                case opcQualityUncertain:
                    returnQualityText = "UNCERTAIN";
                    break;
                case opcQualityGood:
                    returnQualityText = "GOOD";
                    break;
                case opcQualityConfigError:
                    returnQualityText = "OPC_QUALITY_CONFIG_ERROR";
                    break;
                case opcQualityNotConnected:
                    returnQualityText = "NOT_CONNECTED";
                    break;
                case opcQualityDeviceFailure:
                    returnQualityText = "DEVICE_FAILURE";
                    break;
                case opcQualitySensorFailure:
                    returnQualityText = "SENSOR_FAILURE";
                    break;
                case opcQualityLastKnown:
                    returnQualityText = "LAST_KNOWN";
                    break;
                case opcQualityCommFailure:
                    returnQualityText = "COMM_FAILURE";
                    break;
                case opcQualityOutOfService:
                    returnQualityText = "OUT_OF_SERVICE";
                    break;
                case opcQualityLastUsable:
                    returnQualityText = "LAST_USABLE";
                    break;
                case opcQualitySensorCal:
                    returnQualityText = "SENSOR_CAL";
                    break;
                case opcQualityEguExceeded:
                    returnQualityText = "EGU_EXCEEDED";
                    break;
                case opcQualitySubNormal:
                    returnQualityText = "SUB_NORMAL";
                    break;
                case opcQualityLocalOverride:
                    returnQualityText = "LOCAL_OVERRIDE";
                    break;
                default:
                    returnQualityText = "UNKNOWN ERROR";
                    break;
            }
            return returnQualityText;
        }

        /// <summary>
        ///     Wirte Opc Item by group
        /// </summary>
        /// <param name="groupName">the name of group which will write</param>
        /// <param name="itemValueDictonary">
        ///     there are item's value in this hashtable
        /// </param>
        /// <param name="readWriteType"></param>
        /// <param name="mesage">out: message</param>
        /// <param name="returnErrors"></param>
        /// <param name="returnValue"></param>
        public void WriteOpcGroup(string groupName, Dictionary<string, string> itemValueDictonary, string readWriteType,
            out string mesage,
            out int[] returnErrors, out bool returnValue)
        {
            returnErrors = null;
            mesage = string.Empty;
            returnValue = false;

            // Write the settings into the plc
            var opcGrpManagement = OpcGroupManagerList[groupName];
            if (opcGrpManagement == null)
            {
                mesage = groupName + "no Exit!";
                return;
            }

            var opcItmExender = OpcItemExtenderList[groupName];
            if (opcItmExender == null)
            {
                mesage = groupName + "no Exit!";
                return;
            }

            var tempItemNum = int.Parse(opcItmExender.Count.ToString().Trim());

            if (tempItemNum != int.Parse(itemValueDictonary.Count.ToString().Trim()))
            {
                mesage = ResourceManager.GetString("EXT_EXC_ASGN_ASYNCWRITE");
                return;
            }

            var serverTimer = opcItmExender.GetAllServerHandles();

            var pItemValues = new object[tempItemNum];

            for (var i = 0; i < opcItmExender.Count; i++)
            {
                //pItemValues[i] = dtItmofGrp[((OPCItemExtenderNumber)opcItmExender[i]).itemName.ToString().Trim()].ToString();

                var itm = ((OpcItemExtenderNumber) opcItmExender[i]).ItemName.Trim();
                pItemValues[i] = itemValueDictonary[itm];
            }

            try
            {
                // write it down
                mesage = ResourceManager.GetString("EXT_OPC_WRITINGSYNC");

                if (readWriteType == "SYNCHRONOUS")
                {
                    opcGrpManagement.WriteSync(serverTimer, pItemValues, out returnErrors);
                }
                else
                {
                    opcGrpManagement.WriteAsync(serverTimer, pItemValues, ref _transIdSwitchPos);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"WriteOPCGroup ERROR£º{ex.Message}");
            }
            finally
            {
                // now update the opc-item-extender objects
                if (returnErrors != null)
                {
                    for (var i = 0; i < returnErrors.Length; i++)
                    {
                        if (returnErrors[i] == 0)
                        {
                            ((OpcItemExtenderNumber) opcItmExender[i]).ActiveValue = pItemValues[i];
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     <para>
        ///         the functions below are our OPC callback function How to implement
        ///         this - see class declaration <see langword="public" /> class ... :
        ///         System.Windows.Forms.Form , <see cref="IOPCDataCallback" /> After
        ///         that:
        ///     </para>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>to ClassView</description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 opc dotnet client --> {} OpcDotNetClient --> Bases and Interfaces
        ///                 --> --> right mouse click --> Add --> Implement Interface... .NET
        ///                 will generate the interfaces below - already with the correct
        ///                 parameters
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        public void Opc_OnReadComplete(object sender, OpcGroupManagement.OpcDataCallbackEventArgs e)
        {
            //Get groupName through h_Group and GroupServerHandle
            var groupName = OpcGroupManagerList.GetGroupName(e.HGroup);
            if (groupName == null)
            {
                return;
            }

            var opcReadItemExender = OpcItemExtenderList[groupName];
            if (opcReadItemExender == null)
            {
                return;
            }

            try
            {
                for (var i = 0; i < e.DwCount; i++)
                {
                    // save the value into the OPCItemExtender object
                    // NOTE: consequently, setting the value, will also fill the data into the database!
                    // --> this may take several seconds!
                    // --> when filling data into database it is recommended to call this method via a AsyncCallback (as delegate)

                    ((OpcItemExtenderNumber) opcReadItemExender[e.PhClientItems[i]]).ActiveValue = e.PvValues[i];
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"OPC_OnReadComplete£º¡º{ResourceManager.GetString("EXT_EXC_MSG_ONREADCOMPLETE")}¡» ERROR£º{ex.Message}");
            }
        }

        internal void Opc_OnDataChange(object sender, OpcGroupManagement.OpcDataCallbackEventArgs e)
        {
            //Get groupName through h_Group and GroupServerHandle
            var groupName = OpcGroupManagerList.GetGroupName(e.HGroup);
            if (groupName == null)
            {
                return;
            }

            var opcReadItemExender = OpcItemExtenderList[groupName];
            if (opcReadItemExender == null)
            {
                return;
            }

            try
            {
                for (var i = 0; i < e.DwCount; i++)
                {
                    // save the value into the OPCItemExtender object
                    // NOTE: consequently, setting the value, will also fill the data into the database!
                    // --> this may take several seconds!
                    // --> when filling data into database it is recommended to call this method via a AsyncCallback (as delegate)

                    ((OpcItemExtenderNumber) opcReadItemExender[e.PhClientItems[i]]).ActiveValue = e.PvValues[i];
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"OPC_OnDataChange£º¡º{ResourceManager.GetString("EXT_EXC_MSG_ONDATACHANGE")}¡» ERROR£º{ex.Message}");
            }
        }

        public void OnWriteComplete(object sender, OpcGroupManagement.OpcWriteCompletedEventArgs e)
        {
            //Get groupName through h_Group and GroupServerHandle
            var groupName = OpcGroupManagerList.GetGroupName(e.HGroup);
            if (groupName == null)
            {
                return;
            }

            var opcReadItemExender = OpcItemExtenderList[groupName];
            if (opcReadItemExender == null)
            {
                return;
            }

            try
            {
                for (var xCount = 0; xCount < e.DwCount; xCount++)
                {
                    if (e.Transid == _transIdSwitchPos)
                    {
                        // the switch position in the plc is not delivered via a parameter;
                        // so the opc-client has to manage it;
                        // here this is done with m_SentSwitchPos;
                        // the switch position should only be adjusted, if the writing was
                        // successful.     
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"OnWriteComplete£º¡º{ResourceManager.GetString("EXT_EXC_MSG_ONWRITECOMPLETE")}¡» ERROR£º{ex.Message}");
            }
        }

        /// <summary>
        ///     change group Status
        /// </summary>
        /// <param name="groupName">Group name</param>
        /// <param name="groupActive">Active Status</param>
        public bool ChangeGroupState(string groupName, bool groupActive)
        {
            try
            {
                var opcChangeGroupManagement = OpcGroupManagerList[groupName];
                if (opcChangeGroupManagement == null)
                {
                    return false;
                }

                int revisedUpdateRate;
                opcChangeGroupManagement.ChangeGroupState(opcChangeGroupManagement.RevisedUpdateRate, groupActive,
                    out revisedUpdateRate);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ChangeGroupState ERROR£º{ex.Message}");
            }
        }

        /// <summary>
        ///     Disconnect OPC Server
        /// </summary>
        public void Disconnection() => _opcManagementServer?.Dispose();

        /// <summary>
        ///     Dispose the information of the group
        /// </summary>
        /// <param name="groupName">Group Name</param>
        public bool ReleaseOpcNet(string groupName)
        {
            try
            {
                if (ChangeGroupState(groupName, false) == false)
                {
                    throw new ApplicationException($"have some error when disactiving group_{groupName}");
                }

                var opcItmExtender = OpcItemExtenderList[groupName];
                opcItmExtender?.Clear();

                var opcGrpManagement = OpcGroupManagerList[groupName];
                opcGrpManagement?.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Have some error when disposing the group_{groupName}_{ex.Message}");
            }
        }
    }
}