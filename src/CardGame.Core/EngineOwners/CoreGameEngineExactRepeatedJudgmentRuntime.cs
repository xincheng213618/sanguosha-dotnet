#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineExactRepeatedJudgmentRuntime : IRepeatedJudgmentClaimPolicyProgramHost
{
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramRepeatedJudgmentEntries _coreProgramRepeatedJudgmentOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineExactRepeatedJudgmentRuntime(IKernelCardZoneStoreAccess cardZonesOperations,IKernelFrameStoreAccess resolutionStackOperations,IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations,RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineProgramRepeatedJudgmentEntries coreProgramRepeatedJudgmentOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations,RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramRepeatedJudgmentOperations = coreProgramRepeatedJudgmentOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
