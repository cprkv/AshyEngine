using System;
using AshyCore.EngineAPI;
using AshyCore.Resource;
using AshyCore.VFS;

namespace AshyCore
{
    public class Engine : ICoreEngine
    {
        internal static Engine      I { get; set; }

        public IFileSystem          FS { get; set; }

        public Log                  Log { get; set; }

        public ResourceManager      RM { get; set; }

        public ConfigTable          UserConfig { get; set; }

        public EngineStatus         Status { get; set; }

        public void Tick(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
