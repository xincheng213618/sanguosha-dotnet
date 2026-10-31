#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFulueQueriesRuntime
{
    private readonly ICoreFulueQueriesCharacterPeiYuanShaoOlOperations _characterPeiYuanShaoOlOperations;
    private readonly ICoreFulueQueriesResolutionStackOperations _resolutionStackOperations;
    internal CoreGameEngineFulueQueriesRuntime(ICoreFulueQueriesCharacterPeiYuanShaoOlOperations characterPeiYuanShaoOlOperations, ICoreFulueQueriesResolutionStackOperations resolutionStackOperations)
    {
        this._characterPeiYuanShaoOlOperations = characterPeiYuanShaoOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
