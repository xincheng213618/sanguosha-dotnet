#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramJudgmentOutcomesEntries
{
    internal bool HasRecipientJudgmentOutcomePrevention(int targetSeat) => _resolve().HasRecipientJudgmentOutcomePrevention(targetSeat);
    internal bool IsJudgmentOutcomePrevention(SkillProgramTrigger trigger) => _resolve().IsJudgmentOutcomePrevention(trigger);
    internal bool IsUnclaimedJudgmentOutcomeCard(JudgmentFrame judgment, int cardId) => _resolve().IsUnclaimedJudgmentOutcomeCard(judgment, cardId);
    internal bool IsValidProgramJudgmentOutcome(ProgramSkillFrame frame, ProgramJudgmentOutcomeBinding outcome) => _resolve().IsValidProgramJudgmentOutcome(frame, outcome);
    internal SkillProgramEffect? JudgmentOutcomeInstruction(ProgramSkillFrame frame, int instructionIndex) => _resolve().JudgmentOutcomeInstruction(frame, instructionIndex);
    internal bool TryReturnProgramJudgmentOutcome(JudgmentFrame judgment) => _resolve().TryReturnProgramJudgmentOutcome(judgment);
}
