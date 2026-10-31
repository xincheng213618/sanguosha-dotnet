#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashSuitDiscardEntries
{
    internal void AssertProgramSlashSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertProgramSlashSuitDiscard(frame, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramSlashSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginProgramSlashSuitDiscard(frame, effect);
    internal void CaptureProgramSlashSuitJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Suit suit) => _resolve().CaptureProgramSlashSuitJudgment(judgment, suit);
    internal void FinishProgramSlashSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame, bool paid) => _resolve().FinishProgramSlashSuitDiscard(frame, paid);
    internal bool IsProgramSlashSuitDiscardChoiceLegal(global::CardGame.Core.PromptChoice choice) => _resolve().IsProgramSlashSuitDiscardChoiceLegal(choice);
    internal bool IsSlashDodgeCancellationPrevented(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().IsSlashDodgeCancellationPrevented(attack);
    internal void PublishProgramSlashSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramSlashSuitDiscard(frame);
    internal void ResolveProgramSlashSuitDiscardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramSlashSuitDiscardChoice(choice);
    internal bool ResumeProgramSlashSuitDiscard(long frameId) => _resolve().ResumeProgramSlashSuitDiscard(frameId);
    internal global::CardGame.Core.PromptChoice SelectAiSlashSuitDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiSlashSuitDiscard(decision, frame);
    internal global::CardGame.Core.NativeOwners.CardAttackHandle SlashSuitDiscardAttack(global::CardGame.Core.ProgramSkillFrame frame, bool requireActive = true) => _resolve().SlashSuitDiscardAttack(frame, requireActive);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SlashSuitDiscardCards(global::CardGame.Core.ProgramSlashSuitDiscardDraft draft) => _resolve().SlashSuitDiscardCards(draft);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SlashSuitDiscardChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SlashSuitDiscardChoices(frame);
    internal bool TryReturnProgramSlashSuitJudgment(global::CardGame.Core.JudgmentFrame judgment) => _resolve().TryReturnProgramSlashSuitJudgment(judgment);
}
