#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFaultBoundaryRuntime
{
    internal GameCheckpoint BuildTrustedCheckpoint(long revision) => _commandSessionOperations._commandSession.BuildTrustedCheckpoint(_kernelCoreGameEngineCommandSessionOperations.CommandRuntime, revision);
}
