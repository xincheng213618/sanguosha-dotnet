#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramBindingRuntimePortsRuntime : global::CardGame.Core.IProgramBindingFacts, global::CardGame.Core.IProgramBindingProtocols
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeQiOlEntries _characterHeQiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries _coreActualPlayPhaseCardUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries _coreEarnedActualEndingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries _coreFixedDistanceOneDebtOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries _coreHpConditionalSkillGrantsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries _corePrepDiscardEndingPromisesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries _coreProgramMarkerPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileColorsEntries _corePublicPileColorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramBindingRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterHeQiOlEntries characterHeQiOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries coreActualPlayPhaseCardUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries coreEarnedActualEndingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries coreFixedDistanceOneDebtOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries coreHpConditionalSkillGrantsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries corePrepDiscardEndingPromisesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries coreProgramMarkerPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileColorsEntries corePublicPileColorsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHeQiOlOperations = characterHeQiOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActualPlayPhaseCardUsesOperations = coreActualPlayPhaseCardUsesOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreEarnedActualEndingBenefitsOperations = coreEarnedActualEndingBenefitsOperations;
        this._coreFixedDistanceOneDebtOperations = coreFixedDistanceOneDebtOperations;
        this._coreHpConditionalSkillGrantsOperations = coreHpConditionalSkillGrantsOperations;
        this._corePrepDiscardEndingPromisesOperations = corePrepDiscardEndingPromisesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramMarkerPaymentsOperations = coreProgramMarkerPaymentsOperations;
        this._corePublicPileColorsOperations = corePublicPileColorsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
