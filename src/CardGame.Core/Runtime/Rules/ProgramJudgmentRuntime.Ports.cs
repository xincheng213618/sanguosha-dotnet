namespace CardGame.Core;

// These leaf proofs retain existing native rule ownership. Generic instruction,
// content, source and parent qualification belongs to ProgramJudgmentRuntime.
internal interface IProgramJudgmentNativeProofs : IProgramSelectionQuery
{
    bool IsLishiJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsShenZfJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsShenGuoJiaJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsMouGjJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsSourceHuojieJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsNingwuJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsBinghuoJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsWeifuJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool IsZhangJiJudgment(JudgmentFrame judgment, ProgramSkillFrame frame);
    bool CanBeginLishiJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool CanBeginShenZfJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool CanBeginShenGuoJiaJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool IsWushuangLightningJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool MouGjJudgmentRequest(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool CanBeginSourceHuojieJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool CanBeginNingwuJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool CanBeginBinghuoJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool MatchesWeifuJudgmentInstruction(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool CanBeginZhangJiJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility);
    bool IsDamageJudgmentInstructionTarget(ProgramSkillFrame frame, int target);
    int Participant(ProgramSkillFrame frame, ProgramParticipantReference reference);
}

internal interface IProgramJudgmentProducer : IProgramRuntimeOperations
{
    void Begin(long programFrameId, int targetSeat, string reason, CardKind? sourceCard, int sourceSeat,
        string resultBind, SkillProgramCardSetVisibility visibility);
}

public sealed partial class GameEngine
{
}
