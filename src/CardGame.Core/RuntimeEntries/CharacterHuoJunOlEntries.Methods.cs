#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuoJunOlEntries
{
    internal bool AdvanceHuoJunFenruiSubtree() => _resolve().AdvanceHuoJunFenruiSubtree();
    internal global::CardGame.Core.PromptChoice AiHuoJunFenrui(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHuoJunFenrui(p, f);
    internal void AnswerHuoJunFenrui(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHuoJunFenrui(choice);
    internal void AssertHuoJunFenrui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertHuoJunFenrui(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHuoJunFenrui(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHuoJunFenrui(supplied);
    internal bool CanContinuePaidHuoJunFenrui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePaidHuoJunFenrui(f);
    internal bool CanOfferHuoJunFenrui(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferHuoJunFenrui(c, t, context);
    internal void ContinueHuoJunFenrui(long id) => _resolve().ContinueHuoJunFenrui(id);
    internal bool DrainHuoJunFenruiChildren(long id) => _resolve().DrainHuoJunFenruiChildren(id);
    internal void FinishHuoJunFenrui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHuoJunFenrui(f);
    internal bool HasHuoJunFenruiDamageObserver(long id) => _resolve().HasHuoJunFenruiDamageObserver(id);
    internal bool HuoJunDamageUsed(int owner, string skill) => _resolve().HuoJunDamageUsed(owner, skill);
    internal bool HuoJunDiscardable(int seat, global::CardGame.Core.CardZoneKind zone, global::CardGame.Core.Card c) => _resolve().HuoJunDiscardable(seat, zone, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HuoJunFenruiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuoJunFenruiChoices(f);
    internal bool HuoJunFenruiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HuoJunFenruiFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? HuoJunFenruiObserverRoot() => _resolve().HuoJunFenruiObserverRoot();
    internal bool HuoJunFenruiObserverSuffix(int root) => _resolve().HuoJunFenruiObserverSuffix(root);
    internal bool HuoJunHasAbolishedSlot(int seat) => _resolve().HuoJunHasAbolishedSlot(seat);
    internal bool HuoJunHasDiscard(int seat) => _resolve().HuoJunHasDiscard(seat);
    internal global::CardGame.Core.CardConversionSource HuoJunSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuoJunSource(f);
    internal bool IsHuoJunFenruiDying() => _resolve().IsHuoJunFenruiDying();
    internal bool IsHuoJunFenruiNativeEquipment(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().IsHuoJunFenruiNativeEquipment(ride, beneath);
    internal bool IsHuoJunFenruiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsHuoJunFenruiRecoveryParent(f, recovery);
    internal void IssueHuoJunRandomEquipment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueHuoJunRandomEquipment(f);
    internal void PublishHuoJunFenrui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHuoJunFenrui(f);
    internal bool ResumeHuoJunFenrui(long id) => _resolve().ResumeHuoJunFenrui(id);
    internal bool ReturnHuoJunEquipment(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnHuoJunEquipment(use);
    internal void ValidateHuoJunEquipmentReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateHuoJunEquipmentReturn(use);
}
