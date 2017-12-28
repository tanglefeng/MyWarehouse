//====================================================================
// Function:
// this list, which is derived vom a .NET-arraylist, only accepts
// OPCItemExtender-objects; Exception are going to be thrown otherwise;
// to use this class as "global" OPCItemExtenderList some more work needs to done
// to provide a secure type list
//====================================================================

using System;
using System.Collections;
using System.Resources;

namespace Kengic.Was.CrossCutting.Opc.OpcItemManages
{
    /// <summary>
    ///     this list, which is derived vom a .NET-arraylist, only accepts
    ///     OPCItemExtender-objects; <see cref="Exception" /> are going to be thrown
    ///     otherwise; to use this class as "global" OPCItemExtenderList some more
    ///     work needs to done to provide a secure type list
    /// </summary>
    public class OpcItemExtenderList : ArrayList
    {
        //with this hashtable a "indexed" access via a given groupName is possible!
        private readonly Hashtable _htGroupMapping = new Hashtable();

        public OpcItemExtenderList(ResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }
            ResourceManager = resourceManager;
        }

        //the resourcemanager is necessary to provide internationalisation
        private ResourceManager ResourceManager { get; }

        /// <summary>
        ///     Get OPCItemExtender in the OPCItemExtenderlist list by some
        ///     parameter
        /// </summary>
        /// <param name="groupName">the opc groupName</param>
        /// <returns>
        ///     OPCItemExtender-object
        /// </returns>
        public OpcItemExtender this[string groupName]
        {
            get
            {
                for (var i = 0; i < Count; i++)
                {
                    if (((OpcItemExtender) base[i]).GroupName == groupName)
                    {
                        return (OpcItemExtender) base[i];
                    }
                }
                return null;
            }
        }

        /// <summary>
        ///     adds a new OPCItemExtender to the list
        /// </summary>
        /// <param name="itemList">the OPCItemExtender-object</param>
        /// <exception cref="ArgumentException">
        ///     the OPCItemExtender has to be different from all other group in this
        ///     list!
        /// </exception>
        /// <returns>
        ///     the index of the added OPCItemExtender to in this list
        /// </returns>
        public int Add(OpcItemExtender itemList)
        {
            if (_htGroupMapping.ContainsKey(itemList.GroupName))
            {
                var errorMessage = ResourceManager.GetString("EXC_OPCGRPLIST_GROUP_EXISTS1") + "\n" +
                                   ResourceManager.GetString("EXC_OPCGRPLIST_GROUP_EXISTS2");
                throw new ArgumentException(errorMessage);
            }

            _htGroupMapping.Add(itemList.GroupName, itemList);

            return base.Add(itemList);
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
            var errorMessage = ResourceManager.GetString("EXC_OPCGRPLIST_NO_OBJECT");
            throw new ArgumentException(errorMessage);
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
            var errorMessage = ResourceManager.GetString("EXC_OPCGRPLIST_NO_OBJECT");
            throw new ArgumentException(errorMessage);
        }

        /// <summary>
        ///     not supported with this list
        /// </summary>
        /// <param name="itemExtender"></param>
        /// <exception cref="ArgumentException">
        ///     no collections can be inserted
        /// </exception>
        public void Remove(OpcItemExtender itemExtender)
        {
            //remove the item in Hashtable by itemID
            for (var j = 0; j < ((Hashtable) _htGroupMapping[itemExtender.GroupName]).Count; j++)
            {
                ((Hashtable) _htGroupMapping[itemExtender.GroupName]).Remove(
                    ((OpcItemExtenderNumber) itemExtender[j]).ItemId);
            }

            //remove the item in OPCItemExtender 
            for (var i = 0; i < itemExtender.Count; i++)
            {
                itemExtender.Remove((OpcItemExtenderNumber) itemExtender[i]);
            }

            _htGroupMapping.Remove(itemExtender);
            base.Remove(itemExtender);
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
            var errorMessage = ResourceManager.GetString("EXC_OPCGRPLIST_NO_OBJECT");
            throw new ArgumentException(errorMessage);
        }

        /// <summary>
        ///     removes the OPCItemExtender-object; NOTE: the caller must ensure
        ///     that the clienthandles are going to be reset to the new values!
        ///     otherwise the callback-mechanism will fail!
        /// </summary>
        /// <param name="index">the index</param>
        public override void RemoveAt(int index)
        {
            var itemExtender = (OpcItemExtender) this[index];

            //remove the item in Hashtable by itemID
            for (var j = 0; j < ((Hashtable) _htGroupMapping[itemExtender.GroupName]).Count; j++)
            {
                ((Hashtable) _htGroupMapping[itemExtender.GroupName]).Remove(
                    ((OpcItemExtenderNumber) itemExtender[j]).ItemId);
            }

            //remove the item in OPCItemExtender 
            for (var i = 0; i < itemExtender.Count; i++)
            {
                itemExtender.Remove((OpcItemExtenderNumber) itemExtender[i]);
            }

            _htGroupMapping.Remove(itemExtender.GroupName);
            base.RemoveAt(index);
        }

        /// <summary>
        ///     removes the OPCItemExtender-object; NOTE: the caller must ensure
        ///     that the clienthandles are going to be reset to the new values!
        ///     otherwise the callback-mechanism will fail!
        /// </summary>
        /// <param name="index">index to start from</param>
        /// <param name="count">number of items to invalidate</param>
        public override void RemoveRange(int index, int count)
        {
            // remove the items from the hashtable
            for (var i = index; i < count; i++)
            {
                var itemExtender = (OpcItemExtender) this[i];

                //remove the item in Hashtable by itemID
                for (var j = 0; j < ((Hashtable) _htGroupMapping[itemExtender.GroupName]).Count; j++)
                {
                    ((Hashtable) _htGroupMapping[itemExtender.GroupName]).Remove(
                        ((OpcItemExtenderNumber) itemExtender[j]).ItemId);
                }

                //remove the item in OPCItemExtender 
                for (var m = 0; m < itemExtender.Count; m++)
                {
                    itemExtender.Remove((OpcItemExtenderNumber) itemExtender[m]);
                }

                _htGroupMapping.Remove(itemExtender.GroupName);
            }

            base.RemoveRange(index, count);
        }
    }
}