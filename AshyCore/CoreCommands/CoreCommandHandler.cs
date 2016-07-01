using AshyCore.EngineAPI.EngineCommands;

namespace AshyCore.CoreCommands
{
    internal class CoreCommandHandler : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            EngineCommandResult result;
            switch ( c.Type )
            {
                case EngineCommandType.LoadLevel:
                    result      = LoadLevel.Process();
                    break;

                case EngineCommandType.DestroyLevel:
                    result      = DestroyLevel.Process();
                    break;

                case EngineCommandType.ChangeLevel:
                    result      = ChangeLevel.Process();
                    break;

                // nothing to do
                case EngineCommandType.AddEntity:
                case EngineCommandType.OpenConsole:
                case EngineCommandType.CloseConsole:
                case EngineCommandType.PauseGame:
                default:
                    result      = EngineCommandResult.Success;
                    break;
            }

            return              ( result );
        }
    }
}
