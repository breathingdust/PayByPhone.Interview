using System.Web.Mvc;
using StructureMap;
namespace PayByPhoneInterview.Web {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.AssemblyContainingType<TwitterClient>();
                                        scan.AddAllTypesOf<IController>();
                                        scan.WithDefaultConventions();
                                    });
                        });
            return ObjectFactory.Container;
        }
    }
}