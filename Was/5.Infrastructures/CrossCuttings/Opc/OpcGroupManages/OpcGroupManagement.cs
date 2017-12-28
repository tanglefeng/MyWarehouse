//====================================================================
// Function:
// this class encapsulates all necessary operations for this automatoin task
// to read and write opc-items via the OPC DA interface
//====================================================================

using System;
using System.Diagnostics;
using System.Resources;
using System.Runtime.InteropServices;
using OpcRcw.Comn;
using OpcRcw.Da;
using FILETIME = OpcRcw.Da.FILETIME;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Kengic.Was.CrossCutting.Opc.OpcGroupManages
{
    /// <summary>
    ///     this class encapsulates all necessary operations for this automatoin task
    ///     to read and write opc-items via the OPC DA interface
    /// </summary>
    public class OpcGroupManagement : IDisposable, IOPCDataCallback
    {
        public delegate void DataChangedEventHandler(object source, OpcDataCallbackEventArgs e);

        public delegate void ReadCompleteEventHandler(object source, OpcDataCallbackEventArgs e);

        public delegate void WriteCompleteEventHandler(object source, OpcWriteCompletedEventArgs e);

        private IConnectionPoint _connectionPoint;
        private IConnectionPointContainer _connectionPointContainer;


        private int _cookieCp;

        //to control, whether this object is disposed </summary>
        private bool _disposed;

        private int[] _itemServerHandles;

        /// <summary> necessary for messages from the opc-server </summary>
        private int _localeId;

        private IOPCAsyncIO2 _opcAsyncIo2;

        private IOPCGroupStateMgt _opcGroupStateManagement;

        //just the OPCItemMgt-object </summary>
        private IOPCItemMgt _opcItemManagement;

        /// <summary> These interfaces are necessary to have read/write access to process variables </summary>
        private IOPCSyncIO _opcSyncIo;

        /// <summary> creates an instance of this class </summary>
        /// <param name="opcServer"> the OPCServer; must not be null! </param>
        /// <param name="groupName"> the group name</param>
        /// <param name="isActive"> determines whether the group is active </param>
        /// <param name="groupClientHandle"> the group client handle </param>
        /// <param name="requestUpdateRate"> the requested update rate </param>
        /// <param name="callbackReceiver"> the instance which wants to receive the callback messages</param>
        /// <param name="resourceManager"> the resourcemanager instance of the userinterface-instance</param>
        /// <exception cref="ArgumentNullException">
        ///     if "theServer" or "resMan" is null, then an exception
        ///     will be thrown
        /// </exception>
        public OpcGroupManagement(IOPCServer opcServer, string groupName, bool isActive, int groupClientHandle,
            int requestUpdateRate,
            object callbackReceiver, ResourceManager resourceManager)
        {
            if (opcServer == null)
            {
                throw new ArgumentNullException(nameof(opcServer));
            }
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }

            CallbackReceiver = callbackReceiver;
            ResourceManager = resourceManager;
            GroupName = groupName;
            GroupClientHandle = groupClientHandle;
            RequestUpdateRate = requestUpdateRate;
            IsActive = isActive;
            OpcServer = opcServer;
        }

        public int RevisedUpdateRate { get; set; }

        public int RequestUpdateRate { get; }

        public string GroupName { set; get; }

        public int GroupClientHandle { get; }

        public int GroupServerHandle { get; set; }

        public bool IsActive { get; }

        //necessary to support internationalisation </summary>
        public ResourceManager ResourceManager { get; }

        //the OPC server-object, which has to be passed by, is 
        // necessary to get error-texts or similar from the opc-server. </summary>
        public IOPCServer OpcServer { get; }

        //this object will receive all(!) callback messages given from the opc-server </summary>
        public object CallbackReceiver { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        public void OnCancelComplete(int dwTransid, int hGroup)
        {
            // will not be implemented
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount,
            int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
            => OnOpcReadCompleted?.Invoke(this,
                new OpcDataCallbackEventArgs(dwTransid, hGroup, hrMasterquality, hrMastererror, dwCount,
                    phClientItems,
                    pvValues, pwQualities, pftTimeStamps, pErrors));

        public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount,
            int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
            => OnOpcDataChanged?.Invoke(this,
                new OpcDataCallbackEventArgs(dwTransid, hGroup, hrMasterquality, hrMastererror, dwCount,
                    phClientItems,
                    pvValues, pwQualities, pftTimeStamps, pErrors));

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles,
            int[] pErrors) => OnOpcWriteCompleted?.Invoke(this,
                new OpcWriteCompletedEventArgs(dwTransid, hGroup, hrMastererr, dwCount, pClienthandles, pErrors));

        /// <summary>This function provides the switching of the active state of the given group.</summary>
        /// <param name="reqUpdateRate">requested update rate</param>
        /// <param name="activateGroup">de/activate the group</param>
        /// <param name="revUpdateRate">revised update rate</param>
        /// <exception cref="Exception">forwards any exception (with short error description)</exception>
        /// <remarks>
        ///     In order to minimize the traffic on the net and the demands on the opc server it
        ///     is recommended to switch off the active groups if they are not necessary.
        /// </remarks>
        public void ChangeGroupState(int reqUpdateRate, bool activateGroup, out int revUpdateRate)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            const int active = 0;
            const int loc = 0x409;
            revUpdateRate = 0;

            var pTimeBias = IntPtr.Zero;
            var pDeadband = IntPtr.Zero;

            // Access unmanaged memory
            var hRequestedUpdateRate = GCHandle.Alloc(reqUpdateRate, GCHandleType.Pinned);
            var hLoc = GCHandle.Alloc(loc, GCHandleType.Pinned);
            var hActive = GCHandle.Alloc(active, GCHandleType.Pinned);
            var hServerGroup = GCHandle.Alloc(GroupServerHandle, GCHandleType.Pinned);

            try
            {
                // Set state
                hActive.Target = activateGroup ? 1 : 0;

                // Because we have to allocate unmanaged memory, we have to pinn them. Otherwise .Net's
                // garbage collector might free the memory and the opc server will do something randomly.
                _opcGroupStateManagement.SetState(hRequestedUpdateRate.AddrOfPinnedObject(),
                    out revUpdateRate,
                    hActive.AddrOfPinnedObject(),
                    // if the parameter is used, it must be pinned!!!
                    pTimeBias,
                    // if the parameter is used, it must be pinned!!!
                    pDeadband,
                    hLoc.AddrOfPinnedObject(),
                    hServerGroup.AddrOfPinnedObject());
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"changeGroupState ERROR£º{ex.Message}");
            }
            finally
            {
                // now free the unmanaged memory
                if (hActive.IsAllocated)
                {
                    hActive.Free();
                }
                if (hLoc.IsAllocated)
                {
                    hLoc.Free();
                }
                if (hRequestedUpdateRate.IsAllocated)
                {
                    hRequestedUpdateRate.Free();
                }
                if (hServerGroup.IsAllocated)
                {
                    hServerGroup.Free();
                }
            }
        }

        /// <summary>
        ///     Use C# destructor syntax for finalization code.
        ///     This destructor will run only if the Dispose method
        ///     does not get called.
        ///     It gives your base class the opportunity to finalize.
        ///     Do not provide destructors in types derived from this class.
        /// </summary>
        ~OpcGroupManagement()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        /// <summary>
        ///     Dispose(bool disposing) executes in two distinct scenarios.
        ///     If disposing equals true, the method has been called directly
        ///     or indirectly by a user's code.
        ///     If disposing equals false, the method has been called by the
        ///     runtime from inside the finalizer and you should not reference
        ///     other objects. ///
        /// </summary>
        /// <param name="disposing">
        ///     determines whether it is called from the finalizer or
        ///     from the user.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                // Don't dispose managed ressources in here; GC will take care
                try
                {
                    // Release unmanaged resources.
                    RemoveAllItemsAndGroup();
                }
                catch (Exception ex)
                {
                    // user called dispose -> a try-catch is still there
                    if (disposing)
                    {
                        throw;
                    }

                    // if not disposing (so, the gc has called dispose!), nothing is there to catch the exception
                    // -> show a message box
                    //MessageBox.Show(ex.Message, pr_ResMan.GetString("EXT_EXC_MSG_ALL_CAPTION"),
                    //    MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    throw new ApplicationException(
                        $"{ResourceManager.GetString("EXT_EXC_MSG_ALL_CAPTION")}£ºError={ex.Message}");
                }
                finally
                {
                    _disposed = true;
                    FreeGroup();
                }
                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.
            }
        }

        /// <summary>event will be fired, if OPC calls datachange</summary>
        public event DataChangedEventHandler OnOpcDataChanged;

        /// <summary>eventwill be fired, if OPC calls onreadcomplete</summary>
        public event ReadCompleteEventHandler OnOpcReadCompleted;

        /// <summary>eventwill be fired, if OPC calls onwritecomplete</summary>
        public event WriteCompleteEventHandler OnOpcWriteCompleted;

        /// <summary>adds a IOPCGroupStateMgt-object to the OPC-server</summary>
        /// <param name="opcCommonInterface">the common interface</param>
        /// <returns> the revisedUpdateRate for further use </returns>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public int AddOpcGroup(IOPCCommon opcCommonInterface)
        {
            // initialize arguments.
            var pTimeBias = IntPtr.Zero;
            var pDeadband = IntPtr.Zero;

            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            RevisedUpdateRate = 0;

            try
            {
                // get the default locale for the server.
                opcCommonInterface.GetLocaleID(out _localeId);
                var guid = typeof (IOPCGroupStateMgt).GUID;
                string message;
                if (guid == Guid.Empty)
                {
                    message = ResourceManager.GetString("EXT_OPC_IF_GROUP");
                    throw new Exception(message);
                }

                // Add a group object "m_OPCGroup" and query for interface IOPCItemMgt
                // Parameter as following:
                // [in] active, so do OnDataChange callback
                // [in] Request this Update Rate from Server
                // [in] Client Handle, not necessary in this sample
                // [in] No time interval to system UTC time
                // [in] No Deadband, so all data changes are reported
                // [in] Server uses english language to for text values
                // [out] Server handle to identify this group in later calls
                // [out] The answer from Server to the requested Update Rate
                // [in] requested interface type of the group object
                // [out] pointer to the requested interface
                int groupServerHandle;
                int revisedUpdateRate;
                object objectGroup;
                OpcServer.AddGroup(GroupName,
                    Convert.ToInt32(IsActive),
                    RequestUpdateRate,
                    GroupClientHandle,
                    pTimeBias,
                    pDeadband,
                    _localeId,
                    out groupServerHandle,
                    out revisedUpdateRate,
                    ref guid,
                    out objectGroup);

                GroupServerHandle = groupServerHandle;
                RevisedUpdateRate = revisedUpdateRate;

                if (objectGroup == null)
                {
                    message = ResourceManager.GetString("EXT_OPC_IF_GROUP2") + GroupName +
                              ResourceManager.GetString("EXT_OPC_IF_GROUP3");
                    throw new Exception(message);
                }

                // Get our reference from the created group
                _opcGroupStateManagement = (IOPCGroupStateMgt) objectGroup;
                if (_opcGroupStateManagement == null)
                {
                    message = ResourceManager.GetString("EXT_OPC_IF_GROUP4");
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"addOPCGroup ERROR£º{ex.Message}");
            }
            return RevisedUpdateRate;
        }


        /// <summary> adds an IOPCItemStateMgt-object to given group </summary>
        /// <param name="itemsActive"> determines which item is active </param>
        /// <param name="itemIDs"> determines the itemIDs </param>
        /// <param name="itemClientHandles"> the clientHandles of the items </param>
        /// <param name="itmServerHandles"> the serverHandles given from the opc-server </param>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public void AddOpcItems(bool[] itemsActive, string[] itemIDs, int[] itemClientHandles,
            out int[] itmServerHandles)
        {
            // initialize arguments.
            //number of elements we can add
            var numberOfItems = itemClientHandles.Length;
            var itemdefs = new OPCITEMDEF[numberOfItems];
            var ppResults = IntPtr.Zero;
            var ppErrors = IntPtr.Zero;

            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            // Init item server handles
            itmServerHandles = new int[numberOfItems];

            try
            {
                // Get Item interface
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (_opcGroupStateManagement != null)
                {
                    _opcItemManagement = _opcGroupStateManagement as IOPCItemMgt;
                }
                string message;
                if (_opcItemManagement == null)
                {
                    message = ResourceManager.GetString("EXT_OPC_IF_ITEM");
                    throw new Exception(message);
                }

                // Now add the items
                for (var i = 0; i < numberOfItems; i++)
                {
                    // Accesspath not needed
                    itemdefs[i].szAccessPath = string.Empty;
                    // AddItem Active, so OnDataChange will come in an active group for this item 
                    itemdefs[i].bActive = Convert.ToInt32(itemsActive[i]);
                    // We want to get the items as string
                    itemdefs[i].vtRequestedDataType = Convert.ToInt16(VarEnum.VT_BSTR);
                    // "BinaryLargeOBject" not needed by SimaticNet OPC Server
                    itemdefs[i].dwBlobSize = 0;
                    // no blob
                    itemdefs[i].pBlob = IntPtr.Zero;

                    itemdefs[i].hClient = itemClientHandles[i];
                    itemdefs[i].szItemID = itemIDs[i];
                }

                // Adding items to the Group
                _opcItemManagement.AddItems(numberOfItems, itemdefs, out ppResults, out ppErrors);

                if (ppResults == IntPtr.Zero)
                {
                    message = ResourceManager.GetString("EXT_OPC_RESULT_FAILED");
                    throw new Exception(message);
                }
                if (ppErrors == IntPtr.Zero)
                {
                    message = ResourceManager.GetString("EXT_OPC_ERROR_FAILED");
                    throw new Exception(message);
                }

                //Evaluate return ErrorCodes to exclude possible Errors
                var errors = new int[numberOfItems];
                Marshal.Copy(ppErrors, errors, 0, numberOfItems);

                var result = new OPCITEMRESULT[numberOfItems];
                var pos = ppResults;

                for (var dwCount = 0; dwCount < numberOfItems; dwCount++)
                {
                    if (errors[dwCount] != 0)
                    {
                        message = itemIDs[dwCount] + ResourceManager.GetString("EXT_OPC_ERROR_ITEM_ADD");
                        throw new Exception(message);
                    }
                    // Item was added succesfully
                    result[dwCount] = (OPCITEMRESULT) Marshal.PtrToStructure(pos, typeof (OPCITEMRESULT));
                    itmServerHandles[dwCount] = result[dwCount].hServer;

                    pos = (IntPtr) (pos.ToInt32() + Marshal.SizeOf(typeof (OPCITEMRESULT)));
                }

                _itemServerHandles = new int[numberOfItems];
                _itemServerHandles = itmServerHandles;

                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(ppResults);
                Marshal.FreeCoTaskMem(ppErrors);
            }
            catch (Exception ex)
            {
                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(ppResults);
                Marshal.FreeCoTaskMem(ppErrors);

                throw new ApplicationException($"addOPCItems ERROR£º{ex.Message}");
            }
        }

        /// <summary> activates the sync-interface </summary>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public void SetSyncInterface()
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            try
            {
                if (_opcGroupStateManagement == null)
                {
                    return;
                }
                // Query interface for Sync calls on group object
                // Take care:	IOPCSyncIO is for DA 1.x AND DA 2.x
                //				IOPCSyncIO2 is for DA 3.x
                // ReSharper disable once SuspiciousTypeConversion.Global
                _opcSyncIo = _opcGroupStateManagement as IOPCSyncIO;
                if (_opcSyncIo == null)
                {
                    throw new Exception(ResourceManager.GetString("EXT_OPC_SYNCIO_FAILED"));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"setSyncInterface ERROR£º{ex.Message}");
            }
        }

        /// <summary> activates the async-interface </summary>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public void SetASyncInterface()
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            try
            {
                if (_disposed)
                {
                    throw new NullReferenceException("This object has been disposed!");
                }

                if (_opcGroupStateManagement == null)
                {
                    return;
                }
                // Query interface for Async calls on group object
                // Take care:	IOPCAsyncIO is only for DA 1.x
                //				IOPCAsyncIO2 is for DA 2.x
                //				IOPCAsyncIO3 is for DA 3.x
                _opcAsyncIo2 = _opcGroupStateManagement as IOPCAsyncIO2;
                if (_opcAsyncIo2 == null)
                {
                    throw new Exception("Could not get the interface 'IOPCAsyncIO2'!");
                }

                if (_cookieCp == 0)
                {
                    ConnectCallback();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"setASyncInterface ERROR£º{ex.Message}");
            }
        }

        /// <summary> Connects the group to the connectionpointcontainer of IOPCGroupStateMgt </summary>
        public void ConnectCallback()
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            try
            {
                // Allow connection only one time!
                if ((_opcGroupStateManagement == null) || (_cookieCp != 0))
                {
                    return;
                }
                // Query interface for callbacks on group object
                _connectionPointContainer = (IConnectionPointContainer) _opcGroupStateManagement;
                if (_connectionPointContainer == null)
                {
                    throw new Exception("Could not get the interface 'IConnectionPointCointainer'!");
                }

                // Establish Callback for all async operations
                var iid = typeof (IOPCDataCallback).GUID;
                _connectionPointContainer.FindConnectionPoint(ref iid, out _connectionPoint);

                if (_connectionPoint == null)
                {
                    throw new Exception("Could not get a connection point!");
                }
                // Creates a connection between the OPC servers's connection point and 
                // this client's sink (the callback object). 
                _connectionPoint.Advise(this, out _cookieCp);

                if (_cookieCp != 0)
                {
                    return;
                }

                throw new Exception("'Advise' for the sink interface failed!");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"connectCallback ERROR£º{ex.Message}");
            }
        }

        /// <summary>Reads values via an 'asynchronous read'</summary>
        /// <param name="itemServerHandles">the corresponding item server handles</param>
        /// <param name="transactionId">transaction id for tracing</param>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public void ReadAsync(int[] itemServerHandles, ref int transactionId)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            var pErrors = IntPtr.Zero;
            var numItems = itemServerHandles.Length;
            var errors = new int[numItems];

            try
            {
                // We don't use the CancelID in this programm
                int cancelId;

                transactionId++;

                _opcAsyncIo2.Read(numItems,
                    itemServerHandles,
                    transactionId,
                    out cancelId,
                    out pErrors);

                //check if an error occured
                Marshal.Copy(pErrors, errors, 0, numItems);

                for (var xCount = 0; xCount < numItems; xCount++)
                {
                    // Data has been received
                    if (errors[xCount] == 0)
                    {
                        continue;
                    }
                    string message;
                    OpcServer.GetErrorString(errors[xCount], _localeId, out message);
                    Debug.WriteLine(message);
                    throw new Exception(message);
                }
                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pErrors);
            }
            catch (Exception ex)
            {
                Marshal.FreeCoTaskMem(pErrors);
                throw new ApplicationException($"readAsync ERROR£º{ex.Message}");
            }
        }


        /// <summary>Reads opc-items via an synchronous read</summary>
        /// <param name="opcDataSrc">determines the source, the data have to be read (cache or device)</param>
        /// <param name="itemServerHandles">the serverhandles of the opc-items to be read</param>
        /// <param name="errors"></param>
        /// <returns> itemstates, returned form the opc-server </returns>
        public OPCITEMSTATE[] ReadSync(OPCDATASOURCE opcDataSrc,
            int[] itemServerHandles, out int[] errors)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            var pItemStates = IntPtr.Zero;
            var pErrors = IntPtr.Zero;
            var numItems = itemServerHandles.Length;
            var result = new OPCITEMSTATE[numItems];

            try
            {
                // Usually reading from CACHE is more efficient
                // AND the underlying device has to do less communication work.
                // BUT, you must have an active ITEM and GROUP, so that the opc server
                // will update the data by itself; otherwise you'll get quality OUT_OF_SERVICE
                _opcSyncIo.Read(opcDataSrc, //OPCDATASOURCE.OPC_DS_DEVICE or OPCDATASOURCE.OPC_DS_CACHE,
                    numItems,
                    itemServerHandles,
                    out pItemStates,
                    out pErrors);

                if (pItemStates == IntPtr.Zero)
                {
                    throw new ArgumentNullException(ResourceManager.GetString("EXT_OPC_RDSYNC_ITEMSTATES"));
                }
                if (pErrors == IntPtr.Zero)
                {
                    throw new ArgumentNullException(ResourceManager.GetString("EXT_OPC_RDSYNC_ERRORS"));
                }

                //Evaluate return ErrorCodes to exclude possible Errors
                errors = new int[numItems];
                Marshal.Copy(pErrors, errors, 0, numItems);

                var pos = pItemStates;

                // Now get the read values and check errors
                string errorStr = null;
                for (var dwCount = 0; dwCount < numItems; dwCount++)
                {
                    result[dwCount] = (OPCITEMSTATE) Marshal.PtrToStructure(pos, typeof (OPCITEMSTATE));

                    if (errors[dwCount] != 0)
                    {
                        OpcServer.GetErrorString(errors[dwCount], _localeId, out errorStr);
                        Debug.WriteLine(errorStr);
                    }

                    pos = (IntPtr) (pos.ToInt32() + Marshal.SizeOf(typeof (OPCITEMSTATE)));
                }

                if (!string.IsNullOrEmpty(errorStr))
                {
                    throw new Exception(errorStr);
                }

                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pItemStates);
                Marshal.FreeCoTaskMem(pErrors);
            }
            catch (Exception ex)
            {
                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pItemStates);
                Marshal.FreeCoTaskMem(pErrors);

                // just forward it
                throw new ApplicationException($"readSync ERROR£º{ex.Message}");
            }
            return result;
        }

        /// <summary>Writes values via an 'sychronous write'</summary>
        /// <param name="itemServerHandles">the corresponding item server handles</param>
        /// <param name="pItemValues">values to write</param>
        /// <param name="errors"></param>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public void WriteSync(int[] itemServerHandles, object[] pItemValues, out int[] errors)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            var pErrors = IntPtr.Zero;
            var numItems = itemServerHandles.Length;

            try
            {
                // sync write will block the user interface! so the ui is not responsive.
                // therefore it should only be used if the user is not allowed to control the ui.
                _opcSyncIo.Write(numItems,
                    itemServerHandles,
                    pItemValues,
                    out pErrors);

                errors = new int[numItems];
                //check if an error occured
                Marshal.Copy(pErrors, errors, 0, numItems);

                string errorStr = null;
                for (var xCount = 0; xCount < numItems; xCount++)
                {
                    // Data has been received
                    if (errors[xCount] == 0)
                    {
                        continue;
                    }
                    // Errors occured - raise Exception
                    OpcServer.GetErrorString(errors[xCount], _localeId, out errorStr);
                    Debug.WriteLine(errorStr);
                }

                if (!string.IsNullOrEmpty(errorStr))
                {
                    throw new Exception(errorStr);
                }

                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pErrors);
            }
            catch (Exception ex)
            {
                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pErrors);
                throw new ApplicationException($"writeSync ERROR£º{ex.Message}");
            }
        }

        /// <summary>Writes the opc-items via an 'asynchronous write'</summary>
        /// <param name="itemServerHandles">the corresponding item server handles</param>
        /// <param name="itemValues">the values to write</param>
        /// <param name="transactionId">transaction id for tracing</param>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        public void WriteAsync(int[] itemServerHandles, object[] itemValues,
            ref int transactionId)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            var pErrors = IntPtr.Zero;
            var numItems = itemServerHandles.Length;
            var errors = new int[numItems];

            try
            {
                // We never use dwCancelID anymore
                int dwCancelId;

                transactionId++;

                _opcAsyncIo2.Write(numItems,
                    itemServerHandles,
                    itemValues,
                    transactionId,
                    out dwCancelId,
                    out pErrors);

                //check if an error occured
                Marshal.Copy(pErrors, errors, 0, numItems);

                for (var xCount = 0; xCount < numItems; xCount++)
                {
                    // Data has been received
                    if (errors[xCount] == 0)
                    {
                        continue;
                    }
                    // Errors occured - raise Exception
                    string message;
                    OpcServer.GetErrorString(errors[xCount], _localeId, out message);
                    Debug.WriteLine(message);
                    throw new Exception(message);
                }
                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pErrors);
            }
            catch (Exception ex)
            {
                // Free allocated COM-ressouces
                Marshal.FreeCoTaskMem(pErrors);

                throw new ApplicationException($"writeAsync ERROR£º{ex.Message}");
            }
        }

        /// <summary> frees the IOPCItemStateMgt-interfaces and then the IOPCGroupStateMgt-interface </summary>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        private static void RemoveAllItemsAndGroup()
        {
            try
            {
                //DisconnectCallback();
                //RemoveItems();
                //RemoveGroup();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"removeAllItemsAndGroup ERROR£º{ex.Message}");
            }
        }

        /// <summary> releases the IOPCGroupStateMgt-object </summary>
        private void FreeGroup() => _opcGroupStateManagement = null;

        /// <summary>  </summary>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        private void RemoveGroup()
        {
            if (GroupServerHandle == 0)
            {
                return;
            }
            if (OpcServer == null)
            {
                return;
            }
            try
            {
                OpcServer.RemoveGroup(GroupServerHandle, Convert.ToInt32(true));
                GroupServerHandle = 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"removeGroup ERROR£º{ex.Message}");
            }
        }

        /// <summary> removes all OPC-Items from this group </summary>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        private void RemoveItems()
        {
            if (_itemServerHandles == null)
            {
                return;
            }
            try
            {
                var numberOfItems = _itemServerHandles.Length;
                // Removes items
                var errors = new int[numberOfItems];

                var pItems = (int[]) _itemServerHandles.Clone();

                IntPtr pErrors;
                _opcItemManagement.RemoveItems(numberOfItems, pItems, out pErrors);

                //Evaluating Return ErrorCodes to exclude possible Errors
                Marshal.Copy(pErrors, errors, 0, numberOfItems);

                for (var xCount = 0; xCount < numberOfItems; xCount++)
                {
                    // Data has been received
                    if (errors[xCount] != 0)
                    {
                        throw new Exception(ResourceManager.GetString("EXT_OPC_FREE_ITEMIF"));
                    }
                }

                // Remove item server handles
                for (var idx = 0; idx < numberOfItems; idx++)
                {
                    _itemServerHandles[idx] = 0;
                }

                Marshal.FreeCoTaskMem(pErrors);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"removeItems ERROR£º{ex.Message}");
            }
        }

        /// <summary>Disconnects from callback</summary>
        /// <exception cref="Exception">throws and forwards any exception (with short error description)</exception>
        private void DisconnectCallback()
        {
            try
            {
                // disconnect from callback
                if ((_cookieCp == 0) || (_connectionPoint == null))
                {
                    return;
                }
                _connectionPoint.Unadvise(_cookieCp);
                _cookieCp = 0;

                Marshal.ReleaseComObject(_connectionPoint);
                _connectionPoint = null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"disconnectCallback ERROR£º{ex.Message}");
            }
        }


        // class for eventhandler-parameter
        public class OpcDataCallbackEventArgs : EventArgs
        {
            internal OpcDataCallbackEventArgs(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror,
                int dwCount,
                int[] phClientItems, object[] pvValues, short[] pwQualities,
                FILETIME[] pftTimeStamps, int[] pErrors)
            {
                Transid = dwTransid;
                HGroup = hGroup;
                HrMasterquality = hrMasterquality;
                HrMastererror = hrMastererror;
                DwCount = dwCount;
                PhClientItems = (int[]) phClientItems.Clone();
                PvValues = (object[]) pvValues.Clone();
                PwQualities = (short[]) pwQualities.Clone();
                PftTimeStamps = (FILETIME[]) pftTimeStamps.Clone();
                PErrors = pErrors;
            }

            public int Transid { get; internal set; }

            public int HGroup { get; internal set; }

            public int HrMasterquality { get; internal set; }

            public int HrMastererror { get; internal set; }

            public int DwCount { get; internal set; }

            public int[] PhClientItems { get; internal set; }

            public object[] PvValues { get; internal set; }

            public short[] PwQualities { get; internal set; }

            public FILETIME[] PftTimeStamps { get; internal set; }

            public int[] PErrors { get; internal set; }
        }

        public class OpcWriteCompletedEventArgs : EventArgs
        {
            internal OpcWriteCompletedEventArgs(int dwTransid, int hGroup, int hrMastererror, int dwCount,
                int[] pClienthandles, int[] pErrors)
            {
                Transid = dwTransid;
                HGroup = hGroup;
                HrMastererror = hrMastererror;
                DwCount = dwCount;
                PhClienthandles = (int[]) pClienthandles.Clone();
                PErrors = pErrors;
            }

            public int Transid { get; internal set; }

            public int HGroup { get; internal set; }

            public int HrMastererror { get; internal set; }

            public int DwCount { get; internal set; }

            public int[] PhClienthandles { get; internal set; }

            public int[] PErrors { get; internal set; }
        }
    }
}