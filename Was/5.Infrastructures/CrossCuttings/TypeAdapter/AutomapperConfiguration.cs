using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.CrossCutting.TypeAdapter
{
    public static class AutomapperBootstrapper
    {
        public static void Run()
        {
            //scan all assemblies finding Automapper Profile
            var profiles = Assembly.Load(
                "Kengic.Was.Application.WasModel.Dto").GetTypes()
                .Where(t => (t.BaseType == typeof (Profile)) && (t.FullName != "AutoMapper.SelfProfiler`2"));
            Mapper.Initialize(cfg =>
            {
                foreach (var item in profiles)
                {
                    var profile = ServiceLocator.Current.GetInstance(item) as Profile;
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}