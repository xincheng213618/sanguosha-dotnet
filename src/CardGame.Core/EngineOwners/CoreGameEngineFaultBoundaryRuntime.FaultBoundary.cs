#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFaultBoundaryRuntime
{
    internal GameCheckpoint BuildTrustedCheckpoint(long revision) => _commandSessionOperations._commandSession.BuildTrustedCheckpoint(_kernelCoreGameEngineCommandSessionOperations.CommandRuntime, revision);
}
