#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuoJunOlEntries
{
    internal bool AdvanceHuoJunFenruiSubtree() => _resolve().AdvanceHuoJunFenruiSubtree();
    internal PromptChoice AiHuoJunFenrui(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHuoJunFenrui(p, f);
    internal void AnswerHuoJunFenrui(PromptChoice choice) => _resolve().AnswerHuoJunFenrui(choice);
    internal SkillProgramStepOutcome BeginHuoJunFenrui(ProgramSkillFrame supplied) => _resolve().BeginHuoJunFenrui(supplied);
    internal bool CanContinuePaidHuoJunFenrui(ProgramSkillFrame f) => _resolve().CanContinuePaidHuoJunFenrui(f);
    internal bool CanOfferHuoJunFenrui(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferHuoJunFenrui(c, t, context);
    internal void ContinueHuoJunFenrui(long id) => _resolve().ContinueHuoJunFenrui(id);
    internal bool DrainHuoJunFenruiChildren(long id) => _resolve().DrainHuoJunFenruiChildren(id);
    internal void FinishHuoJunFenrui(ProgramSkillFrame f) => _resolve().FinishHuoJunFenrui(f);
    internal bool HuoJunDamageUsed(int owner, string skill) => _resolve().HuoJunDamageUsed(owner, skill);
    internal bool HuoJunDiscardable(int seat, CardZoneKind zone, Card c) => _resolve().HuoJunDiscardable(seat, zone, c);
    internal IReadOnlyList<PromptChoice> HuoJunFenruiChoices(ProgramSkillFrame f) => _resolve().HuoJunFenruiChoices(f);
    internal bool HuoJunFenruiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HuoJunFenruiFirstChild(f, child);
    internal ProgramSkillFrame? HuoJunFenruiObserverRoot() => _resolve().HuoJunFenruiObserverRoot();
    internal bool HuoJunFenruiObserverSuffix(int root) => _resolve().HuoJunFenruiObserverSuffix(root);
    internal bool HuoJunHasAbolishedSlot(int seat) => _resolve().HuoJunHasAbolishedSlot(seat);
    internal bool HuoJunHasDiscard(int seat) => _resolve().HuoJunHasDiscard(seat);
    internal CardConversionSource HuoJunSource(ProgramSkillFrame f) => _resolve().HuoJunSource(f);
    internal bool IsHuoJunFenruiNativeEquipment(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().IsHuoJunFenruiNativeEquipment(ride, beneath);
    internal bool IsHuoJunFenruiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsHuoJunFenruiRecoveryParent(f, recovery);
    internal void IssueHuoJunRandomEquipment(ProgramSkillFrame f) => _resolve().IssueHuoJunRandomEquipment(f);
    internal void PublishHuoJunFenrui(ProgramSkillFrame f) => _resolve().PublishHuoJunFenrui(f);
    internal bool ResumeHuoJunFenrui(long id) => _resolve().ResumeHuoJunFenrui(id);
    internal bool ReturnHuoJunEquipment(CardUseFrame use) => _resolve().ReturnHuoJunEquipment(use);
    internal void ValidateHuoJunEquipmentReturn(CardUseFrame use) => _resolve().ValidateHuoJunEquipmentReturn(use);
}
