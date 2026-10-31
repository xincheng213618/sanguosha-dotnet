#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWenYangOlEntries
{
    internal bool AdvanceWenYangChildren() => _resolve().AdvanceWenYangChildren();
    internal void AnswerWenYang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerWenYang(choice);
    internal void AppendWenYangEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendWenYangEndingItems(items);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWenYang(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginWenYang(input, op);
    internal bool CanOfferWenYang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferWenYang(candidate, t, c);
    internal global::CardGame.Core.PromptChoice ChooseWenYangAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseWenYangAi(p, f);
    internal bool CollectWenYang(global::CardGame.Core.SkillProgramTrigger t) => _resolve().CollectWenYang(t);
    internal bool ContinuePaidWenYang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWenYang(f);
    internal void ContinueWenYang(long id) => _resolve().ContinueWenYang(id);
    internal global::CardGame.Core.WenYangQingjiaoDebtIssuedEvent? CurrentWenYangDebt(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentWenYangDebt(c, context);
    internal bool DrainWenYang(long id) => _resolve().DrainWenYang(id);
    internal void FinishWenYang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWenYang(f);
    internal bool IsIssuedWenYangQingjiaoEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedWenYangQingjiaoEndingCandidate(c, context);
    internal bool IsWenYangChoujueCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsWenYangChoujueCandidate(candidate, c);
    internal bool IsWenYangRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsWenYangRecoveryParent(f, recovery);
    internal void MoveWenYang(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.WenYangEntity> cards, global::CardGame.Core.WenYangStage stage, string reason, bool discard) => _resolve().MoveWenYang(f, cards, stage, reason, discard);
    internal void PublishWenYang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWenYang(f);
    internal bool ResumeWenYang(long id) => _resolve().ResumeWenYang(id);
    internal bool ValidWenYang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWenYang(f);
    internal bool ValidWenYangDebt(global::CardGame.Core.WenYangQingjiaoDebtIssuedEvent debt) => _resolve().ValidWenYangDebt(debt);
    internal int WenYangAwakeningLevel(int owner) => _resolve().WenYangAwakeningLevel(owner);
    internal bool WenYangAwakeningParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().WenYangAwakeningParent(candidate, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WenYangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WenYangChoices(f);
    internal bool WenYangChoujueParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().WenYangChoujueParent(candidate, c);
    internal bool WenYangDamageParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().WenYangDamageParent(candidate, c);
    internal bool WenYangDebtConsumed(long id) => _resolve().WenYangDebtConsumed(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.WenYangEntity> WenYangDiscardable(int owner, string instance, bool handOnly) => _resolve().WenYangDiscardable(owner, instance, handOnly);
    internal bool WenYangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WenYangFirstChild(f, child);
    internal bool WenYangHasAwakened(int owner, int level) => _resolve().WenYangHasAwakened(owner, level);
    internal int WenYangLvliLimit(int owner) => _resolve().WenYangLvliLimit(owner);
    internal int WenYangLvliUses(int owner) => _resolve().WenYangLvliUses(owner);
    internal bool WenYangMovementMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().WenYangMovementMatches(f, m);
    internal global::CardGame.Core.ProgramSkillFrame? WenYangObserverRoot() => _resolve().WenYangObserverRoot();
    internal bool WenYangOriginalParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WenYangOriginalParent(f);
    internal bool WenYangPlayParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().WenYangPlayParent(candidate, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.WenYangEntity> WenYangRandomInventory() => _resolve().WenYangRandomInventory();
    internal bool HasWenYangCapability { get => _resolve().HasWenYangCapability; }
}
