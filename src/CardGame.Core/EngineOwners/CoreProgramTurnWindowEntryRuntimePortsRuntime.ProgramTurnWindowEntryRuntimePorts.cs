#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTurnWindowEntryRuntimePortsRuntime
{
    Winner IProgramTurnWindowEntryFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IProgramTurnWindowEntryFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    ActualTurnKind IProgramTurnWindowEntryFacts.TurnKind => _coreActualTurnProgressTurnProgressionAccess._turnProgression.Kind;

    bool IProgramTurnWindowEntryFacts.HasHongjiCapability => _characterZhangShiPingOlOperations.HasHongjiCapability;

    int IProgramTurnWindowEntryFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    bool IProgramTurnWindowIssuedCandidates.HasUnissuedTurnStartObligation(int ownerSeat) => _programPipelineHooksOperations._programPipelineHooks.HasUnissuedTurnStartObligation(ownerSeat);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.PhaseHandDebt(CharacterState owner, SkillProgramTriggerWindow window, int occurrence) => _corePhaseHandSeizureOperations.IssuedPhaseHandDebtCandidates(owner, window, occurrence);
    bool IProgramTurnWindowIssuedCandidates.HasPhaseNamePredictions => _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Count != 0;

    bool IProgramTurnWindowIssuedCandidates.HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => _characterZhangShiPingOlOperations.HasOutstandingHongjiPhase(owner, window);
    IReadOnlyList<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _characterZhangShiPingOlOperations.IncludeHongjiPhaseCandidates(owner, window, ordinary);
    void IProgramTurnWindowIssuedCandidates.AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates) => _corePhaseNamePredictionOperations.AppendPhaseNamePredictionCandidates(candidates);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.ZhuijiEnding(CharacterState owner) => _coreZhuijiProgramsOperations.IssuedZhuijiEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.QingzhongEnding(CharacterState owner) => _characterLuZhiWeiOlOperations.IssuedQingzhongEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.WeiluEnding(CharacterState owner) => _characterLvQianOlOperations.IssuedWeiluEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.WangKuangRenxiaEnding(CharacterState owner) => _characterWangKuangOlOperations.IssuedWangKuangRenxiaEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.TongduPlayEnding(CharacterState owner) => _characterLiuBaOlOperations.IssuedTongduPlayEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.MouCaoHongPlayEnding(CharacterState owner) => _characterCaoHongMouOperations.IssuedMouCaoHongPlayEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> IProgramTurnWindowIssuedCandidates.AnniversaryZhangXunPlayEnding(CharacterState owner) => _characterZhangXunAnniversaryOperations.IssuedAnniversaryZhangXunPlayEndingCandidates(owner);
    WangKuangRenxiaPhaseKey? IProgramTurnWindowPhaseReceipts.CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => _characterWangKuangOlOperations.CaptureWangKuangRenxiaEndingPhase(owner);
    WeiluPhaseKey? IProgramTurnWindowPhaseReceipts.CaptureWeiluEndingPhase(CharacterState owner) => _characterLvQianOlOperations.CaptureWeiluEndingPhase(owner);
    QingzhongPlayPhaseKey? IProgramTurnWindowPhaseReceipts.CaptureQingzhongEndingPhase(CharacterState owner) => _characterLuZhiWeiOlOperations.CaptureQingzhongEndingPhase(owner);
    ZhuijiPlayPhaseKey? IProgramTurnWindowPhaseReceipts.CaptureZhuijiEndingPhase(CharacterState owner) => _coreZhuijiProgramsOperations.CaptureZhuijiEndingPhase(owner);
    bool IProgramTurnWindowPhaseReceipts.TracksPhaseNamePredictions => _corePhaseNamePredictionOperations.TracksPhaseNamePredictions;

    int IProgramTurnWindowPhaseReceipts.PhaseInstanceId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;
}
