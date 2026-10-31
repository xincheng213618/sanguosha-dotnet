#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterWuYiMobileRuntime : global::CardGame.Core.IMobileWuYiBenxiHost
{
    private readonly ICharacterWuYiMobileCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICharacterWuYiMobileCoreCurrentCardEnhancementsOperations _coreCurrentCardEnhancementsOperations;
    private readonly ICharacterWuYiMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterWuYiMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterWuYiMobileCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICharacterWuYiMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterWuYiMobileRuntime(ICharacterWuYiMobileCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICharacterWuYiMobileCoreCurrentCardEnhancementsOperations coreCurrentCardEnhancementsOperations, ICharacterWuYiMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterWuYiMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterWuYiMobileCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICharacterWuYiMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCurrentCardEnhancementsOperations = coreCurrentCardEnhancementsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
