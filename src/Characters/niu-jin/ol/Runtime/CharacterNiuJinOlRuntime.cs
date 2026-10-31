#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterNiuJinOlRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    internal CharacterNiuJinOlRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess coreGameEngineProgramPersistentStateProgramBooleanStatesAccess)
    {
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess = coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
        this._coreGameEngineProgramPersistentStateProgramBooleanStatesAccess = coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    }
}
