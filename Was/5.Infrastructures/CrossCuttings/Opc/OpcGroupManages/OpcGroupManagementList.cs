//====================================================================
// Function:
// this list, which is derived vom a .NET-arraylist, only accepts
// OPCGroupManagement-objects
//====================================================================

using System;
using System.Collections;
using System.Resources;

namespace Kengic.Was.CrossCutting.Opc.OpcGroupManages
{
    /// <summary>
    ///     this list, which is derived vom a .NET-arraylist, only accepts
    ///     OPCGroupManagement-objects;
    /// </summary>
    public class OpcGroupManagementList : ArrayList
    {
        // with this hashtable a "indexed" access via a given groupName is possible!
        private readonly Hashtable _groupMapping = new Hashtable();

        public OpcGroupManagementList(ResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }
            ResourceManager = resourceManager;
        }

        /// <summary>
        ///     Get OPCGroupManagement in the OPCGroupManagementList list by some
        ///     param
        /// </summary>
        /// <param name="groupName">the opc group name</param>
        /// <returns>
        ///     OPCGroupManagement-object
        /// </returns>
        public OpcGroupManagement this[string groupName]
        {
            get
            {
                for (var i = 0; i < Count; i++)
                {
                    if (((OpcGroupManagement) base[i]).GroupName == groupName)
                    {
                        return (OpcGroupManagement) base[i];
                    }
                }
                return null;
            }
        }

        // the resourcemanager is necessary to provide internationalisation </summary>
        private ResourceManager ResourceManager { get; }

        public string GetGroupName(int groupServerHandle)
        {
            for (var i = 0; i < Count; i++)
            {
                if (((OpcGroupManagement) base[i]).GroupServerHandle == groupServerHandle)
                {
                    return ((OpcGroupManagement) base[i]).GroupName;
                }
            }
            return null;
        }

        /// <summary>
        ///     adds a new OPCGroupManagement to the list
        /// </summary>
        /// <param name="groupManagementList">
        ///     the OPCGroupManagement-object
        /// </param>
        /// <exception cref="ArgumentException">
        ///     the OPCGroupManagement has to be different from all other group in
        ///     this list!
        /// </exception>
        /// <returns>
        ///     the index of the added OPCGroupManagement to in this list
        /// </returns>
        public int Add(OpcGroupManagement groupManagementList)
        {
            if (_groupMapping.ContainsKey(groupManagementList.GroupName))
            {
                var str = ResourceManager.GetString("EXC_OPCGRPLIST_GROUP_EXISTS1") + "\n" +
                          ResourceManager.GetString("EXC_OPCGRPLIST_GROUP_EXISTS2");
                throw new ArgumentException(str);
            }

            _groupMapping.Add(groupManagementList.GroupName, groupManagementList);

            return base.Add(groupManagementList);
        }

        /// <summary>
        ///     not supported with this list
        /// </summary>
        /// <param name="value">a object</param>
        /// <exception cref="ArgumentException">
        ///     no "native" objects can be added
        /// </exception>
        /// <returns>
        ///     nothing
        /// </returns>
        public override int Add(object value)
        {
            // adding a normal object is not possible!!!
            var strMsg = ResourceManager.GetString("EXC_OPCGRPLIST_NO_OBJECT");
            throw new ArgumentException(strMsg);
        }

        /// <summary>
        ///     removes the OPCGroupManagement-object;
        /// </summary>
        /// <param name="groupManagement">the OPCGroupManagement-object</param>
        public void Remove(OpcGroupManagement groupManagement)
        {
            _groupMapping.Remove(groupManagement.GroupName);
            base.Remove(groupManagement);
        }

        /// <summary>
        ///     not supported with this list
        /// </summary>
        /// <param name="obj">a "native" object</param>
        /// <exception cref="ArgumentException">
        ///     no "native" objects can be removed
        /// </exception>
        public override void Remove(object obj)
        {
            // removing a normal object is not possible!!!
            throw new ArgumentException("OPCGroupManagement No object");
        }
    }
}