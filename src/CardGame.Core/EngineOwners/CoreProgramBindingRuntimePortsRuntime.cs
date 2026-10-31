#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramBindingRuntimePortsRuntime : IProgramBindingFacts, IProgramBindingProtocols
{
    private readonly RuntimeEntries.CharacterHeQiOlEntries _characterHeQiOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries _coreActualPlayPhaseCardUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries _coreEarnedActualEndingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries _coreFixedDistanceOneDebtOperations;
    private readonly RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries _coreHpConditionalSkillGrantsOperations;
    private readonly RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries _corePrepDiscardEndingPromisesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries _coreProgramMarkerPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEnginePublicPileColorsEntries _corePublicPileColorsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramBindingRuntimePortsRuntime(RuntimeEntries.CharacterHeQiOlEntries characterHeQiOlOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries coreActualPlayPhaseCardUsesOperations, RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations, RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries coreEarnedActualEndingBenefitsOperations, RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries coreFixedDistanceOneDebtOperations, RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries coreHpConditionalSkillGrantsOperations, RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries corePrepDiscardEndingPromisesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineProgramMarkerPaymentsEntries coreProgramMarkerPaymentsOperations, RuntimeEntries.CoreGameEnginePublicPileColorsEntries corePublicPileColorsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
