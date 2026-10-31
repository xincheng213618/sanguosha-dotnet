#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHealthQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries _coreCardDamageMaximumHpReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateActionQueryScopeOperations _kernelCoreGameEngineActionQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramExpressionWorldOperations _kernelCoreGameEngineProgramExpressionsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess _coreGameEngineActionQueriesActionQueryAccess;
    internal CoreGameEngineHealthQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations,global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries coreCardDamageMaximumHpReplacementOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.EngineOwners.IKernelCreateActionQueryScopeOperations kernelCoreGameEngineActionQueriesOperations,global::CardGame.Core.EngineOwners.IKernelCreateProgramExpressionWorldOperations kernelCoreGameEngineProgramExpressionsOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess coreGameEngineActionQueriesActionQueryAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardDamageMaximumHpReplacementOperations = coreCardDamageMaximumHpReplacementOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineActionQueriesOperations = kernelCoreGameEngineActionQueriesOperations;
        this._kernelCoreGameEngineProgramExpressionsOperations = kernelCoreGameEngineProgramExpressionsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineActionQueriesActionQueryAccess = coreGameEngineActionQueriesActionQueryAccess;
    }
}
