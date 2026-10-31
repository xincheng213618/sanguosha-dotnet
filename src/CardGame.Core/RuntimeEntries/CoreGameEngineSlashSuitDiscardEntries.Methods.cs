#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashSuitDiscardEntries
{
    internal SkillProgramStepOutcome BeginProgramSlashSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BeginProgramSlashSuitDiscard(frame, effect);
    internal void CaptureProgramSlashSuitJudgment(JudgmentFrame judgment, Suit suit) => _resolve().CaptureProgramSlashSuitJudgment(judgment, suit);
    internal void FinishProgramSlashSuitDiscard(ProgramSkillFrame frame, bool paid) => _resolve().FinishProgramSlashSuitDiscard(frame, paid);
    internal bool IsProgramSlashSuitDiscardChoiceLegal(PromptChoice choice) => _resolve().IsProgramSlashSuitDiscardChoiceLegal(choice);
    internal bool IsSlashDodgeCancellationPrevented(NativeOwners.CardAttackHandle attack) => _resolve().IsSlashDodgeCancellationPrevented(attack);
    internal void PublishProgramSlashSuitDiscard(ProgramSkillFrame frame) => _resolve().PublishProgramSlashSuitDiscard(frame);
    internal void ResolveProgramSlashSuitDiscardChoice(PromptChoice choice) => _resolve().ResolveProgramSlashSuitDiscardChoice(choice);
    internal bool ResumeProgramSlashSuitDiscard(long frameId) => _resolve().ResumeProgramSlashSuitDiscard(frameId);
    internal PromptChoice SelectAiSlashSuitDiscard(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiSlashSuitDiscard(decision, frame);
    internal NativeOwners.CardAttackHandle SlashSuitDiscardAttack(ProgramSkillFrame frame, bool requireActive = true) => _resolve().SlashSuitDiscardAttack(frame, requireActive);
    internal IEnumerable<Card> SlashSuitDiscardCards(ProgramSlashSuitDiscardDraft draft) => _resolve().SlashSuitDiscardCards(draft);
    internal IReadOnlyList<PromptChoice> SlashSuitDiscardChoices(ProgramSkillFrame frame) => _resolve().SlashSuitDiscardChoices(frame);
    internal bool TryReturnProgramSlashSuitJudgment(JudgmentFrame judgment) => _resolve().TryReturnProgramSlashSuitJudgment(judgment);
}
