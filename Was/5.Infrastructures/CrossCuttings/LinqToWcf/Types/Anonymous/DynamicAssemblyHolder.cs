using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     A class that holds a <see cref="AssemblyBuilder">dynamic assembly</see>.
    /// </summary>
    internal class DynamicAssemblyHolder
    {
        private static DynamicAssemblyHolder _instance;

        private AssemblyBuilder _assembly;

        /// <summary>
        ///     Private constructor to avoid external instantiation.
        /// </summary>
        private DynamicAssemblyHolder()
        {
        }

        /// <summary>
        ///     Singleton instance of the <see cref="DynamicAssemblyHolder" />.
        /// </summary>
        public static DynamicAssemblyHolder Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof (DynamicAssemblyHolder))
                    {
                        if (_instance == null)
                        {
                            _instance = new DynamicAssemblyHolder();
                            _instance.Initialize();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        ///     Gets a <see cref="ModuleBuilder" /> to create types in it.
        /// </summary>
        public ModuleBuilder ModuleBuilder { get; private set; }

        /// <summary>
        ///     Initializes the <see cref="DynamicAssemblyHolder" />.
        /// </summary>
        private void Initialize()
        {
            // get the current appdomain
            var ad = AppDomain.CurrentDomain;

            // create a new dynamic assembly
            var an = new AssemblyName
            {
                Name = "InterLinq.Types.Anonymous.Assembly",
                Version = new Version("1.0.0.0")
            };

            _assembly = ad.DefineDynamicAssembly(
                an, AssemblyBuilderAccess.Run);

            // create a new module to hold code in the assembly
            ModuleBuilder = _assembly.GetDynamicModule("InterLinq.Types.Anonymous.Module") ??
                            _assembly.DefineDynamicModule("InterLinq.Types.Anonymous.Module");
        }
    }
}