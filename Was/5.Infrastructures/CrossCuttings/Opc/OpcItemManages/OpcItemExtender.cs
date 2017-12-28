//====================================================================
// Function:
// this list, which is derived vom a .NET-arraylist, only accepts
// OPCItemExtenderNumber-objects; Exception are going to be thrown otherwise;
// to use this class as "global" OPCItemExtender some more work needs to done
// to provide a secure type list
//====================================================================

using System;
using System.Collections;
using System.Resources;

namespace Kengic.Was.CrossCutting.Opc.OpcItemManages
{
    /// <summary>
    ///     this list, which is derived vom a .NET-arraylist, only accepts
    ///     OPCItemExtenderNumber-objects; <see cref="Exception" /> are going to be
    ///     thrown otherwise; to use this class as "global" OPCItemExtender some
    ///     more work needs to done to provide a secure type list
    /// </summary>
    public class OpcItemExtender : ArrayList
    {
        public OpcItemExtender(ResourceManager resourceManager, string groupName)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }
            ResourceMananger = resourceManager;

            GroupName = groupName;

            ItemIdMapping = new Hashtable();
        }

        //with this hashtable a "indexed" access via a given ITEM-ID is possible!
        //So the search of a specific ITEM-ID is possible in O(1)-complexity (maximum performance)!
        //s. also the "get" and "set" methods</summary>
        private Hashtable ItemIdMapping { get; }
        //the resourcemanager is necessary to provide internationalisation </summary>
        private ResourceManager ResourceMananger { get; }
        public string GroupName { get; set; }

        /// <summary>
        ///     Get OPCItemExtenderNumber in the OPCItemExtender list by some param
        /// </summary>
        /// <param name="itemName">the item Name of the item</param>
        /// <param name="groupNameOfItem">the groupName of the item</param>
        /// <returns>
        ///     OPCItemExtenderNumber-objects
        /// </returns>
        public OpcItemExtenderNumber this[string itemName, string groupNameOfItem]
        {
            get
            {
                for (var i = 0; i < Count; i++)
                {
                    if ((((OpcItemExtenderNumber) base[i]).ItemName == itemName) &&
                        (((OpcItemExtenderNumber) base[i]).GourpName == groupNameOfItem))
                    {
                        return (OpcItemExtenderNumber) base[i];
                    }
                }
                return null;
            }
        }

        /// <summary>
        ///     adds a new OPCItemExtenderNumber to the OPCItemExtender
        /// </summary>
        /// <param name="theItem">the OPCItemExtender-object</param>
        /// <exception cref="ArgumentException">
        ///     the itemID of the OPCItemExtenderNumber has to be different from all
        ///     other itemID in this OPCItemExtender!
        /// </exception>
        /// <returns>
        ///     the index of the added item in this list
        /// </returns>
        public int Add(OpcItemExtenderNumber theItem)
        {
            if (ItemIdMapping.ContainsKey(theItem.ItemId))
            {
                var errorMessae = ResourceMananger.GetString("EXC_OPCitemLIST_item_EXISTS1") + "\n" +
                                  ResourceMananger.GetString("EXC_OPCitemLIST_item_EXISTS2");
                throw new ArgumentException(errorMessae);
            }

            ItemIdMapping.Add(theItem.ItemId, theItem);

            return base.Add(theItem);
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
            var errorMessae = ResourceMananger.GetString("EXC_OPCitemLIST_NO_OBJECT");
            throw new ArgumentException(errorMessae);
        }

        /// <summary>
        ///     not supported with this list
        /// </summary>
        /// <param name="c">an <see cref="ICollection" /></param>
        /// <exception cref="ArgumentException">
        ///     no collections can be added
        /// </exception>
        public override void AddRange(ICollection c)
        {
            // adding a normal object is not possible!!!
            var errorMessae = ResourceMananger.GetString("EXC_OPCitemLIST_NO_OBJECT");
            throw new ArgumentException(errorMessae);
        }

        /// <summary>
        ///     not supported with this list
        /// </summary>
        /// <param name="index">the index</param>
        /// <param name="value">a object</param>
        /// <exception cref="ArgumentException">
        ///     no "native" objects can be inserted
        /// </exception>
        public override void Insert(int index, object value)
        {
            // adding a normal object is not possible!!!
            var errorMessae = ResourceMananger.GetString("EXC_OPCitemLIST_NO_OBJECT");
            throw new ArgumentException(errorMessae);
        }

        /// <summary>
        ///     not supported with this list
        /// </summary>
        /// <param name="index">the index</param>
        /// <param name="c">an <see cref="ICollection" /></param>
        /// <exception cref="ArgumentException">
        ///     no collections can be inserted
        /// </exception>
        public override void InsertRange(int index, ICollection c)
        {
            // adding a normal object is not possible!!!
            var errorMessae = ResourceMananger.GetString("EXC_OPCitemLIST_NO_OBJECT");
            throw new ArgumentException(errorMessae);
        }

        /// <summary>
        ///     removes the OPCItemExtenderNumber-object; NOTE: the caller must
        ///     ensure that the clienthandles are going to be reset to the new
        ///     values! otherwise the callback-mechanism will fail!
        /// </summary>
        /// <param name="theItem">the OPCItemExtenderNumber-object</param>
        public void Remove(OpcItemExtenderNumber theItem)
        {
            var index = theItem.ClientHandle;
            ItemIdMapping.Remove(theItem.ItemId);
            base.Remove(theItem);

            // renew clienthandles of all 'following' items, because the array-index has now changed
            for (var i = index; i < Count; i++)
            {
                ((OpcItemExtenderNumber) this[i]).ClientHandle = i;
            }
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
            var errorMessae = ResourceMananger.GetString("EXC_OPCitemLIST_NO_OBJECT");
            throw new ArgumentException(errorMessae);
        }

        /// <summary>
        ///     removes the OPCItemExtenderNumber-object; NOTE: the caller must
        ///     ensure that the clienthandles are going to be reset to the new
        ///     values! otherwise the callback-mechanism will fail!
        /// </summary>
        /// <param name="index">the index</param>
        public override void RemoveAt(int index)
        {
            var item = (OpcItemExtenderNumber) this[index];
            ItemIdMapping.Remove(item.ItemId);
            base.RemoveAt(index);

            // renew clienthandles of all 'following' items, because the array-index has now changed
            for (var i = index; i < Count; i++)
            {
                ((OpcItemExtenderNumber) this[i]).ClientHandle = i;
            }
        }

        /// <summary>
        ///     removes the OPCItemExtenderNumber-object; NOTE: the caller must
        ///     ensure that the clienthandles are going to be reset to the new
        ///     values! otherwise the callback-mechanism will fail!
        /// </summary>
        /// <param name="index">index to start from</param>
        /// <param name="count">number of items to invalidate</param>
        public override void RemoveRange(int index, int count)
        {
            // remove the items from the hashtable
            for (var i = index; i < count; i++)
            {
                var opcItemExtenderNumber = (OpcItemExtenderNumber) this[i];
                ItemIdMapping.Remove(opcItemExtenderNumber.ItemId);
            }

            base.RemoveRange(index, count);

            // renew clienthandles of all 'following' items, because the array-index has now changed
            for (var i = index; i < Count; i++)
            {
                ((OpcItemExtenderNumber) this[i]).ClientHandle = i;
            }
        }

        public void SetValue(string itemId, string theValue)
        {
            var item = (OpcItemExtenderNumber) ItemIdMapping[itemId];
            item.ActiveValue = theValue;
        }

        public object GetValue(string itemId)
        {
            var item = (OpcItemExtenderNumber) ItemIdMapping[itemId];
            return item.ActiveValue;
        }

        public int GetServerHandle(string itemId)
        {
            var item = (OpcItemExtenderNumber) ItemIdMapping[itemId];
            return item.ServerHandle;
        }

        public int[] GetAllServerHandles()
        {
            // the returned srvhandles have to be in correct order!
            // otherwise the the caller could assign the values to wrong controls;
            // with hashtables the ordering might be different to the creation ordering
            // -> we have to copy the serverhandles manually.
            var arr = new int[Count];
            for (var i = 0; i < Count; i++)
            {
                arr[i] = ((OpcItemExtenderNumber) this[i]).ServerHandle;
            }

            return arr;
        }
    }
}