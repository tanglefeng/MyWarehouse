//====================================================================
// Function:
// This class provides connecting and disconnecting from the OPC-Server.
// It implements the IDisposble-Interface.
//====================================================================

using System;
using System.Resources;
using System.Runtime.InteropServices;
using Kengic.Was.CrossCutting.Opc.OpcGroupManages;
using Microsoft.Practices.ServiceLocation;
using OpcRcw.Comn;
using OpcRcw.Da;

namespace Kengic.Was.CrossCutting.Opc.OpcSeverManages
{
    /// <summary>
    ///     This class provides connecting and disconnecting from the OPC-Server. It
    ///     implements the IDisposble-Interface.
    /// </summary>
    public class OpcServerManagement : IDisposable
    {
        /// <summary>
        ///     this list is needed to dispose the opcgroups before freeing the
        ///     OPCServer!
        /// </summary>
        private readonly OpcGroupManagementList _groupManagementList = null;

        /// <summary>
        ///     determines whether this object has already been connected. this
        ///     object is only allowed to connect exactly one time!
        /// </summary>
        private bool _connected;

        /// <summary>
        ///     determines whether this object has already been disposed. all
        ///     <see langword="public" /> operations made after disposing the object
        ///     (except re-connecting) are forbidden.
        /// </summary>
        private bool _disposed;

        private int _localeId;
        private IOPCCommon _opcCommon;

        /// <summary>
        ///     only the opc-server and the common-interface is needed in here; the
        ///     rest of it is encapsulated in the class OPCManagement
        /// </summary>
        private IOPCServer _opcServer;

        public OpcServerManagement(ResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }
            ResourceManager = resourceManager;
        }

        /// <summary>
        ///     necessary to support internationalisation
        /// </summary>
        public ResourceManager ResourceManager { get; set; }

        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Establishes the connection to the SimaticNET OPC Server.
        ///     <param name="programId">
        ///         The locale ID returned from <see cref="IOPCCommon" /> the OPC-Server
        ///     </param>
        ///     <exception cref="T:System.InvalidOperationException">
        ///         if the opcserver is already connected
        ///     </exception>
        /// </summary>
        public void ConnectOpcServer(string programId)
        {
            if (!_connected)
            {
                try
                {
                    //Get the Type from ProgID.Text
                    var typeofOpcserver = Type.GetTypeFromProgID(programId);

                    var message = ResourceManager.GetString("EXT_OPC_CONNECT_SERVER");

                    if (typeofOpcserver == null)
                    {
                        throw new Exception(message);
                    }

                    // Must be freed with Marshal.ReleaseComObject(myOPCServer)
                    _opcServer = ServiceLocator.Current.GetInstance<IOPCServer>();
                    if (_opcServer == null)
                    {
                        message = ResourceManager.GetString("EXT_OPC_CREATE_INST_FAILED");
                        throw new Exception(message);
                    }

                    // Don't free it - we don't AddRef it!!!
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    _opcCommon = _opcServer as IOPCCommon;
                    if (_opcCommon == null)
                    {
                        message = ResourceManager.GetString("EXT_OPC_COMMON_FAILED");
                        throw new Exception(message);
                    }

                    _opcCommon.GetLocaleID(out _localeId);
                    _connected = true;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"connectOPCServer ERROR：{ex.Message}");
                }
            }
            else
            {
                throw new InvalidOperationException("Already connected to OPC-Server!");
            }
        }

        /// <summary>
        ///     Use C# destructor syntax for finalization code. This destructor will
        ///     run only if the <see cref="Dispose" /> method does not get called. It
        ///     gives your base class the opportunity to finalize. Do not provide
        ///     destructors in types derived from this class.
        /// </summary>
        ~OpcServerManagement()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        /// <summary>
        ///     Dispose(bool disposing) executes in two distinct scenarios. If
        ///     <paramref name="disposing" /> equals true, the method has been called
        ///     directly or indirectly by a user's code. If
        ///     <paramref name="disposing" /> equals false, the method has been
        ///     called by the runtime from inside the finalizer and you should not
        ///     reference other objects. ///
        /// </summary>
        /// <param name="disposing">
        ///     determines whether it is called from the finalizer or from the user.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                if (_opcServer != null)
                {
                    try
                    {
                        // Do NOT forget to call dispose for the management-objects!
                        // in all properbility you will get an exception otherwise!
                        if (_groupManagementList != null)
                        {
                            foreach (OpcGroupManagement theManager in _groupManagementList)
                            {
                                theManager.Dispose();
                            }
                            _groupManagementList.Clear();
                        }
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                    finally
                    {
                        // MUST NOT be freed because we got it with
                        //myOPCCommon = (IOPCCommon) myOPCServer;

                        _disposed = true;
                        // We must free it here, because we got it with
                        // myOPCServer = (IOPCServer) Activator.CreateInstance(typeofOPCserver);
                        if (_opcServer != null)
                        {
                            Marshal.ReleaseComObject(_opcServer);
                            _opcServer = null;
                        }
                    }
                }
                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.
            }
            _disposed = true;
        }

        /// <summary>
        ///     encapsulates the addGroup-method of the opcserver by using the
        ///     OPCGroupManagement-class
        /// </summary>
        /// <param name="groupName">the group name-组的名称</param>
        /// <param name="groupActive">
        ///     determines whether the group should be active-创建组时，组是否被激活
        /// </param>
        /// <param name="groupClntHndl">
        ///     the clienthandle of the group－客户号
        /// </param>
        /// <param name="requestUpdateRate">
        ///     the requested update rate for the group－返回组中变量改变时的最短通知时间间隔
        /// </param>
        /// <param name="callBackReceiver">
        ///     the object which wants to receive callbacks－
        /// </param>
        /// <param name="revisedUpdateRate">
        ///     the revised update rate of the group
        /// </param>
        /// <exception cref="NullReferenceException">
        ///     if the object is disposed
        /// </exception>
        /// <exception cref="Exception">
        ///     forwarded exception from the opcserver
        /// </exception>
        /// <returns>
        ///     the manager object for the group
        /// </returns>
        public OpcGroupManagement AddGroup(string groupName, bool groupActive, int groupClntHndl, int requestUpdateRate,
            object callBackReceiver,
            out int revisedUpdateRate)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            try
            {
                // let the manager create a opc-group
                var theManager = new OpcGroupManagement(
                    _opcServer,
                    groupName,
                    groupActive,
                    groupClntHndl,
                    requestUpdateRate,
                    callBackReceiver,
                    ResourceManager);

                revisedUpdateRate = theManager.AddOpcGroup(_opcCommon);
                return theManager;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"addGroup ERROR：{ex.Message}");
            }
        }

        /// <summary>
        ///     encapsulates the method "GetErrorString"
        /// </summary>
        /// <param name="error">
        ///     the error-code returned from an opc-operation
        /// </param>
        /// <exception cref="NullReferenceException">
        ///     if the object is disposed
        /// </exception>
        /// <exception cref="Exception">
        ///     forwarded exception from the opcserver
        /// </exception>
        /// <returns>
        ///     the error-string with the current locale-id
        /// </returns>
        public string GetErrorString(int error)
        {
            if (_disposed)
            {
                throw new NullReferenceException("This object has been disposed!");
            }

            try
            {
                string errorString;

                _opcServer.GetErrorString(error, _localeId, out errorString);

                return errorString;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"getErrorString ERROR：{ex.Message}");
            }
        }
    }
}