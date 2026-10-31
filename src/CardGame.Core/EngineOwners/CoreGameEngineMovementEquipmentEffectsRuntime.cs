#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineMovementEquipmentEffectsRuntime : IMovementEquipmentFacts, IMovementEquipmentOperations
{
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly RuntimeEntries.CoreNativeRecoveryFramesEntries _coreNativeRecoveryFramesOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly IKernelCreateMovementArmorOwnerOperations _kernelCoreGameEngineMovementEquipmentEffectsOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly EngineStates.Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineMovementEquipmentEffectsRuntime(RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, IKernelCardZoneStoreAccess cardZonesOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, RuntimeEntries.CoreNativeRecoveryFramesEntries coreNativeRecoveryFramesOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, IKernelCreateMovementArmorOwnerOperations kernelCoreGameEngineMovementEquipmentEffectsOperations, EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, EngineStates.Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineMovementEquipmentEffectsOperations = kernelCoreGameEngineMovementEquipmentEffectsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess = coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
