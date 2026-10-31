#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramJudgmentOutcomesEntries
{
    internal void AssertProgramJudgmentOutcomes(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertProgramJudgmentOutcomes(frame);
    internal bool HasRecipientJudgmentOutcomePrevention(int targetSeat) => _resolve().HasRecipientJudgmentOutcomePrevention(targetSeat);
    internal bool IsJudgmentOutcomePrevention(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().IsJudgmentOutcomePrevention(trigger);
    internal bool IsUnclaimedJudgmentOutcomeCard(global::CardGame.Core.JudgmentFrame judgment, int cardId) => _resolve().IsUnclaimedJudgmentOutcomeCard(judgment, cardId);
    internal bool IsValidProgramJudgmentOutcome(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramJudgmentOutcomeBinding outcome) => _resolve().IsValidProgramJudgmentOutcome(frame, outcome);
    internal global::CardGame.Core.SkillProgramEffect? JudgmentOutcomeInstruction(global::CardGame.Core.ProgramSkillFrame frame, int instructionIndex) => _resolve().JudgmentOutcomeInstruction(frame, instructionIndex);
    internal bool TryReturnProgramJudgmentOutcome(global::CardGame.Core.JudgmentFrame judgment) => _resolve().TryReturnProgramJudgmentOutcome(judgment);
}
