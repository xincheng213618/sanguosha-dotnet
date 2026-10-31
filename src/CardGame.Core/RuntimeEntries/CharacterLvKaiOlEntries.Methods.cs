#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvKaiOlEntries
{
    internal bool AdvanceBijingPaidChildren() => _resolve().AdvanceBijingPaidChildren();
    internal bool BijingFirstPaidChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().BijingFirstPaidChild(frame, child);
    internal bool BijingLionChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().BijingLionChild(frame, child);
    internal ProgramSkillFrame? BijingPaidObserverRoot() => _resolve().BijingPaidObserverRoot();
    internal SkillProgramStepOutcome BijingProgramMarkHandCards(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BijingProgramMarkHandCards(frame, effect);
    internal SkillProgramStepOutcome BijingProgramPunishDiscardPhase(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BijingProgramPunishDiscardPhase(frame, effect);
    internal SkillProgramStepOutcome BijingProgramRecastMarkedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BijingProgramRecastMarkedCards(frame, effect);
    internal IReadOnlyList<PromptChoice> BijingPunishChoices(ProgramSkillFrame frame, int payer) => _resolve().BijingPunishChoices(frame, payer);
    internal void ContinueBijingPaidGroup(long id) => _resolve().ContinueBijingPaidGroup(id);
    internal void ExecuteTunanEquipmentUse(ProgramSkillFrame frame, Card card, int user) => _resolve().ExecuteTunanEquipmentUse(frame, card, user);
    internal void ExecuteTunanPeachUse(ProgramSkillFrame frame, Card card, int user) => _resolve().ExecuteTunanPeachUse(frame, card, user);
    internal bool IsBijingPaidDying() => _resolve().IsBijingPaidDying();
    internal bool IsBijingRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsBijingRecoveryParent(frame, recovery);
    internal bool KeepPaidBijingGroup(ProgramSkillFrame frame) => _resolve().KeepPaidBijingGroup(frame);
    internal void PresentBijingPunishCardPrompt(ProgramSkillFrame frame) => _resolve().PresentBijingPunishCardPrompt(frame);
    internal Card? ReadTunanRevealedCard(ProgramSkillCardSetBinding? binding) => _resolve().ReadTunanRevealedCard(binding);
    internal void ResolveBijingPunishChoice(PromptChoice selected) => _resolve().ResolveBijingPunishChoice(selected);
    internal void ResolveTunanBranchChoice(PromptChoice selected) => _resolve().ResolveTunanBranchChoice(selected);
    internal void ResolveTunanTargetChoice(PromptChoice selected) => _resolve().ResolveTunanTargetChoice(selected);
    internal bool ResumeBijingPaidGroup(long id) => _resolve().ResumeBijingPaidGroup(id);
    internal bool ReturnBijingPaidMovement(ProgramSkillFrame frame) => _resolve().ReturnBijingPaidMovement(frame);
    internal SkillProgramStepOutcome TunanProgramUseRevealedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TunanProgramUseRevealedCard(frame, effect);
}
