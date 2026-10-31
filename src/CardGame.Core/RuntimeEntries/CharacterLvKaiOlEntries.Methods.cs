#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvKaiOlEntries
{
    internal bool AdvanceBijingPaidChildren() => _resolve().AdvanceBijingPaidChildren();
    internal bool BijingFirstPaidChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().BijingFirstPaidChild(frame, child);
    internal bool BijingLionChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().BijingLionChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? BijingPaidObserverRoot() => _resolve().BijingPaidObserverRoot();
    internal global::CardGame.Core.SkillProgramStepOutcome BijingProgramMarkHandCards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BijingProgramMarkHandCards(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome BijingProgramPunishDiscardPhase(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BijingProgramPunishDiscardPhase(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome BijingProgramRecastMarkedCards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BijingProgramRecastMarkedCards(frame, effect);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BijingPunishChoices(global::CardGame.Core.ProgramSkillFrame frame, int payer) => _resolve().BijingPunishChoices(frame, payer);
    internal void ContinueBijingPaidGroup(long id) => _resolve().ContinueBijingPaidGroup(id);
    internal void ExecuteTunanEquipmentUse(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Card card, int user) => _resolve().ExecuteTunanEquipmentUse(frame, card, user);
    internal void ExecuteTunanPeachUse(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Card card, int user) => _resolve().ExecuteTunanPeachUse(frame, card, user);
    internal bool IsBijingPaidDying() => _resolve().IsBijingPaidDying();
    internal bool IsBijingRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsBijingRecoveryParent(frame, recovery);
    internal bool KeepPaidBijingGroup(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepPaidBijingGroup(frame);
    internal void PresentBijingPunishCardPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentBijingPunishCardPrompt(frame);
    internal global::CardGame.Core.Card? ReadTunanRevealedCard(global::CardGame.Core.ProgramSkillCardSetBinding? binding) => _resolve().ReadTunanRevealedCard(binding);
    internal void ResolveBijingPunishChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveBijingPunishChoice(selected);
    internal void ResolveTunanBranchChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTunanBranchChoice(selected);
    internal void ResolveTunanTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTunanTargetChoice(selected);
    internal bool ResumeBijingPaidGroup(long id) => _resolve().ResumeBijingPaidGroup(id);
    internal bool ReturnBijingPaidMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBijingPaidMovement(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome TunanProgramUseRevealedCard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TunanProgramUseRevealedCard(frame, effect);
}
