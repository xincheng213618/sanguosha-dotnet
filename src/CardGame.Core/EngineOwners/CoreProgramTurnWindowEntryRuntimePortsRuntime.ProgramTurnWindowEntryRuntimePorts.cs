#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTurnWindowEntryRuntimePortsRuntime
{
    Winner global::CardGame.Core.IProgramTurnWindowEntryFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IProgramTurnWindowEntryFacts.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    ActualTurnKind global::CardGame.Core.IProgramTurnWindowEntryFacts.TurnKind => _coreActualTurnProgressTurnProgressionAccess._turnProgression.Kind;

    bool global::CardGame.Core.IProgramTurnWindowEntryFacts.HasHongjiCapability => _characterZhangShiPingOlOperations.HasHongjiCapability;

    int global::CardGame.Core.IProgramTurnWindowEntryFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    bool global::CardGame.Core.IProgramTurnWindowIssuedCandidates.HasUnissuedTurnStartObligation(int ownerSeat) => _programPipelineHooksOperations._programPipelineHooks.HasUnissuedTurnStartObligation(ownerSeat);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.PhaseHandDebt(CharacterState owner, SkillProgramTriggerWindow window, int occurrence) => _corePhaseHandSeizureOperations.IssuedPhaseHandDebtCandidates(owner, window, occurrence);
    bool global::CardGame.Core.IProgramTurnWindowIssuedCandidates.HasPhaseNamePredictions => _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Count != 0;

    bool global::CardGame.Core.IProgramTurnWindowIssuedCandidates.HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => _characterZhangShiPingOlOperations.HasOutstandingHongjiPhase(owner, window);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _characterZhangShiPingOlOperations.IncludeHongjiPhaseCandidates(owner, window, ordinary);
    void global::CardGame.Core.IProgramTurnWindowIssuedCandidates.AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates) => _corePhaseNamePredictionOperations.AppendPhaseNamePredictionCandidates(candidates);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.ZhuijiEnding(CharacterState owner) => _coreZhuijiProgramsOperations.IssuedZhuijiEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.QingzhongEnding(CharacterState owner) => _characterLuZhiWeiOlOperations.IssuedQingzhongEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.WeiluEnding(CharacterState owner) => _characterLvQianOlOperations.IssuedWeiluEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.WangKuangRenxiaEnding(CharacterState owner) => _characterWangKuangOlOperations.IssuedWangKuangRenxiaEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.TongduPlayEnding(CharacterState owner) => _characterLiuBaOlOperations.IssuedTongduPlayEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.MouCaoHongPlayEnding(CharacterState owner) => _characterCaoHongMouOperations.IssuedMouCaoHongPlayEndingCandidates(owner);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.IProgramTurnWindowIssuedCandidates.AnniversaryZhangXunPlayEnding(CharacterState owner) => _characterZhangXunAnniversaryOperations.IssuedAnniversaryZhangXunPlayEndingCandidates(owner);
    WangKuangRenxiaPhaseKey? global::CardGame.Core.IProgramTurnWindowPhaseReceipts.CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => _characterWangKuangOlOperations.CaptureWangKuangRenxiaEndingPhase(owner);
    WeiluPhaseKey? global::CardGame.Core.IProgramTurnWindowPhaseReceipts.CaptureWeiluEndingPhase(CharacterState owner) => _characterLvQianOlOperations.CaptureWeiluEndingPhase(owner);
    QingzhongPlayPhaseKey? global::CardGame.Core.IProgramTurnWindowPhaseReceipts.CaptureQingzhongEndingPhase(CharacterState owner) => _characterLuZhiWeiOlOperations.CaptureQingzhongEndingPhase(owner);
    ZhuijiPlayPhaseKey? global::CardGame.Core.IProgramTurnWindowPhaseReceipts.CaptureZhuijiEndingPhase(CharacterState owner) => _coreZhuijiProgramsOperations.CaptureZhuijiEndingPhase(owner);
    bool global::CardGame.Core.IProgramTurnWindowPhaseReceipts.TracksPhaseNamePredictions => _corePhaseNamePredictionOperations.TracksPhaseNamePredictions;

    int global::CardGame.Core.IProgramTurnWindowPhaseReceipts.PhaseInstanceId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;
}
