#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengHuoMouMobileEntries
{
    internal bool AdvanceMouMhChildren() => _resolve().AdvanceMouMhChildren();
    internal PromptChoice AiMouMh(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouMh(p, f);
    internal void AnswerMouMh(PromptChoice selected) => _resolve().AnswerMouMh(selected);
    internal bool CanOfferMouMh(ProgramTriggerLegalityContext g) => _resolve().CanOfferMouMh(g);
    internal void CaptureMouMhUse(CardUseDeclaredEvent e) => _resolve().CaptureMouMhUse(e);
    internal void ChangeMouMhCharge(ProgramSkillFrame f, int delta) => _resolve().ChangeMouMhCharge(f, delta);
    internal void ContinueMouMh(long id) => _resolve().ContinueMouMh(id);
    internal bool DrainMouMh(ProgramSkillFrame f) => _resolve().DrainMouMh(f);
    internal void FinishMouMh(ProgramSkillFrame f) => _resolve().FinishMouMh(f);
    internal bool IsMouMengHuoUseForbidden(int actor, CardKind kind, CardActionType type, long? existingUseId) => _resolve().IsMouMengHuoUseForbidden(actor, kind, type, existingUseId);
    internal bool IsMouMhDying() => _resolve().IsMouMhDying();
    internal bool IsMouMhRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouMhRecoveryParent(f, recovery);
    internal bool IsMouMhSilverLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().IsMouMhSilverLionChild(f, child);
    internal bool KeepPaidMouMh(ProgramSkillFrame f) => _resolve().KeepPaidMouMh(f);
    internal bool MouMengHuoCardUseForbidden(ProgramCardUseQuery query) => _resolve().MouMengHuoCardUseForbidden(query);
    internal int MouMhCharge(int owner) => _resolve().MouMhCharge(owner);
    internal PromptChoice MouMhChoice(ProgramSkillFrame f, string branch, string label, int? seat = default, int? card = default) => _resolve().MouMhChoice(f, branch, label, seat, card);
    internal IReadOnlyList<PromptChoice> MouMhChoices(ProgramSkillFrame f) => _resolve().MouMhChoices(f);
    internal bool MouMhFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouMhFirstChild(f, child);
    internal ProgramSkillFrame? MouMhObserverRoot() => _resolve().MouMhObserverRoot();
    internal bool MouMhObserverSuffix(int i) => _resolve().MouMhObserverSuffix(i);
    internal bool MouMhPhaseParent(ProgramSkillFrame f) => _resolve().MouMhPhaseParent(f);
    internal MouMengHuoTurnKey? MouMhTurn() => _resolve().MouMhTurn();
    internal void PublishMouMh(ProgramSkillFrame f) => _resolve().PublishMouMh(f);
    internal SkillProgramStepOutcome ResolveMouMh(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveMouMh(supplied, op);
    internal bool RestoreMouMengHuoDamageSource(NativeOwners.GroupCardHandle group) => _resolve().RestoreMouMengHuoDamageSource(group);
    internal bool ResumeMouMh(long id) => _resolve().ResumeMouMh(id);
    internal void ReturnMouMhDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMouMhDraw(f, outcome);
    internal bool ReturnMouMhMovement(ProgramSkillFrame f) => _resolve().ReturnMouMhMovement(f);
    internal void SealMouMh(long id, long before) => _resolve().SealMouMh(id, before);
    internal bool ValidMouMh(ProgramSkillFrame f) => _resolve().ValidMouMh(f);
    internal bool HasMouMengHuoCapability { get => _resolve().HasMouMengHuoCapability; }
}
