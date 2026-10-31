namespace CardGame.Core;

internal interface IProgramTurnWindowEntryFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    int RoundNumber { get; }
    ActualTurnKind TurnKind { get; }
    bool HasHongjiCapability { get; }
    int RelativeTableOrder(int from, int target);
}

// Receipt owners expose their issued opportunities separately. Admission order,
// deduplication and the lifetime of the combined roster belong to the runtime.
internal interface IProgramTurnWindowIssuedCandidates : IProgramSelectionQuery
{
    bool HasUnissuedTurnStartObligation(int ownerSeat);
    IEnumerable<ProgramTriggerCandidate> PhaseHandDebt(CharacterState owner, SkillProgramTriggerWindow window, int occurrence);
    bool HasPhaseNamePredictions { get; }
    bool HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window);
    IReadOnlyList<ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(CharacterState owner,
        SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary);
    void AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates);
    IEnumerable<ProgramTriggerCandidate> ZhuijiEnding(CharacterState owner);
    IEnumerable<ProgramTriggerCandidate> QingzhongEnding(CharacterState owner);
    IEnumerable<ProgramTriggerCandidate> WeiluEnding(CharacterState owner);
    IEnumerable<ProgramTriggerCandidate> WangKuangRenxiaEnding(CharacterState owner);
    IEnumerable<ProgramTriggerCandidate> TongduPlayEnding(CharacterState owner);
    IEnumerable<ProgramTriggerCandidate> MouCaoHongPlayEnding(CharacterState owner);
    IEnumerable<ProgramTriggerCandidate> AnniversaryZhangXunPlayEnding(CharacterState owner);
}

internal interface IProgramTurnWindowPhaseReceipts : IProgramSelectionQuery
{
    WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(CharacterState owner);
    WeiluPhaseKey? CaptureWeiluEndingPhase(CharacterState owner);
    QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(CharacterState owner);
    ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(CharacterState owner);
    bool TracksPhaseNamePredictions { get; }
    int PhaseInstanceId { get; }
}

public sealed partial class GameEngine
{
}
