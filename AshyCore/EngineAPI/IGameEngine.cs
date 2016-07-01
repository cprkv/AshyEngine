using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshyCore.EngineAPI
{
    public interface IGameEngine : IEngine
    {
        // we will use it later
        //World       World { get; }

        GameLevel   Level { get; }
    }
}
