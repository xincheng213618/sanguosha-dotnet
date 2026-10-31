#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeSkillContextQueriesRuntime
{
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineFieldEquipmentEntries _coreFieldEquipmentOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries _corePassiveDamageCapsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEnginePublicDamagePredictionEntries _corePublicDamagePredictionOperations;
    private readonly RuntimeEntries.CoreGameEngineStrategicProgramsEntries _coreStrategicProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly RuntimeEntries.CoreNativeRuleQueriesEntries _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    internal CoreNativeSkillContextQueriesRuntime(RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineFieldEquipmentEntries coreFieldEquipmentOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries corePassiveDamageCapsOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEnginePublicDamagePredictionEntries corePublicDamagePredictionOperations, RuntimeEntries.CoreGameEngineStrategicProgramsEntries coreStrategicProgramsOperations, RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, RuntimeEntries.CoreNativeRuleQueriesEntries coreNativeRuleQueriesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreFieldEquipmentOperations = coreFieldEquipmentOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePassiveDamageCapsOperations = corePassiveDamageCapsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._corePublicDamagePredictionOperations = corePublicDamagePredictionOperations;
        this._coreStrategicProgramsOperations = coreStrategicProgramsOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
    }
}
