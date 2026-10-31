#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWenYangOlEntries
{
    internal bool AdvanceWenYangChildren() => _resolve().AdvanceWenYangChildren();
    internal void AnswerWenYang(PromptChoice choice) => _resolve().AnswerWenYang(choice);
    internal void AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendWenYangEndingItems(items);
    internal SkillProgramStepOutcome BeginWenYang(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginWenYang(input, op);
    internal bool CanOfferWenYang(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferWenYang(candidate, t, c);
    internal PromptChoice ChooseWenYangAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseWenYangAi(p, f);
    internal bool CollectWenYang(SkillProgramTrigger t) => _resolve().CollectWenYang(t);
    internal bool ContinuePaidWenYang(ProgramSkillFrame f) => _resolve().ContinuePaidWenYang(f);
    internal void ContinueWenYang(long id) => _resolve().ContinueWenYang(id);
    internal WenYangQingjiaoDebtIssuedEvent? CurrentWenYangDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentWenYangDebt(c, context);
    internal bool DrainWenYang(long id) => _resolve().DrainWenYang(id);
    internal void FinishWenYang(ProgramSkillFrame f) => _resolve().FinishWenYang(f);
    internal bool IsIssuedWenYangQingjiaoEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedWenYangQingjiaoEndingCandidate(c, context);
    internal bool IsWenYangChoujueCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsWenYangChoujueCandidate(candidate, c);
    internal bool IsWenYangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsWenYangRecoveryParent(f, recovery);
    internal void MoveWenYang(ProgramSkillFrame f, IReadOnlyList<WenYangEntity> cards, WenYangStage stage, string reason, bool discard) => _resolve().MoveWenYang(f, cards, stage, reason, discard);
    internal void PublishWenYang(ProgramSkillFrame f) => _resolve().PublishWenYang(f);
    internal bool ResumeWenYang(long id) => _resolve().ResumeWenYang(id);
    internal bool ValidWenYang(ProgramSkillFrame f) => _resolve().ValidWenYang(f);
    internal bool ValidWenYangDebt(WenYangQingjiaoDebtIssuedEvent debt) => _resolve().ValidWenYangDebt(debt);
    internal int WenYangAwakeningLevel(int owner) => _resolve().WenYangAwakeningLevel(owner);
    internal bool WenYangAwakeningParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().WenYangAwakeningParent(candidate, c);
    internal IReadOnlyList<PromptChoice> WenYangChoices(ProgramSkillFrame f) => _resolve().WenYangChoices(f);
    internal bool WenYangChoujueParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().WenYangChoujueParent(candidate, c);
    internal bool WenYangDamageParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().WenYangDamageParent(candidate, c);
    internal bool WenYangDebtConsumed(long id) => _resolve().WenYangDebtConsumed(id);
    internal IReadOnlyList<WenYangEntity> WenYangDiscardable(int owner, string instance, bool handOnly) => _resolve().WenYangDiscardable(owner, instance, handOnly);
    internal bool WenYangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WenYangFirstChild(f, child);
    internal bool WenYangHasAwakened(int owner, int level) => _resolve().WenYangHasAwakened(owner, level);
    internal int WenYangLvliLimit(int owner) => _resolve().WenYangLvliLimit(owner);
    internal int WenYangLvliUses(int owner) => _resolve().WenYangLvliUses(owner);
    internal bool WenYangMovementMatches(ProgramSkillFrame f, CardMovementRecord m) => _resolve().WenYangMovementMatches(f, m);
    internal ProgramSkillFrame? WenYangObserverRoot() => _resolve().WenYangObserverRoot();
    internal bool WenYangOriginalParent(ProgramSkillFrame f) => _resolve().WenYangOriginalParent(f);
    internal bool WenYangPlayParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().WenYangPlayParent(candidate, c);
    internal IReadOnlyList<WenYangEntity> WenYangRandomInventory() => _resolve().WenYangRandomInventory();
    internal bool HasWenYangCapability { get => _resolve().HasWenYangCapability; }
}
