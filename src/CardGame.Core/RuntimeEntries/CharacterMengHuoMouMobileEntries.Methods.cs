#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengHuoMouMobileEntries
{
    internal bool AdvanceMouMhChildren() => _resolve().AdvanceMouMhChildren();
    internal global::CardGame.Core.PromptChoice AiMouMh(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouMh(p, f);
    internal void AnswerMouMh(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouMh(selected);
    internal bool CanOfferMouMh(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferMouMh(g);
    internal void CaptureMouMhUse(global::CardGame.Core.CardUseDeclaredEvent e) => _resolve().CaptureMouMhUse(e);
    internal void ChangeMouMhCharge(global::CardGame.Core.ProgramSkillFrame f, int delta) => _resolve().ChangeMouMhCharge(f, delta);
    internal void ContinueMouMh(long id) => _resolve().ContinueMouMh(id);
    internal bool DrainMouMh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouMh(f);
    internal void FinishMouMh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouMh(f);
    internal bool HasMouMhBeforeDamageObserver(long id) => _resolve().HasMouMhBeforeDamageObserver(id);
    internal bool HasMouMhDamageObserver(long id) => _resolve().HasMouMhDamageObserver(id);
    internal bool IsMouMengHuoUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type, long? existingUseId) => _resolve().IsMouMengHuoUseForbidden(actor, kind, type, existingUseId);
    internal bool IsMouMhDying() => _resolve().IsMouMhDying();
    internal bool IsMouMhMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsMouMhMovement(f, effect, pending);
    internal bool IsMouMhRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouMhRecoveryParent(f, recovery);
    internal bool IsMouMhSilverLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().IsMouMhSilverLionChild(f, child);
    internal bool KeepPaidMouMh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMouMh(f);
    internal bool MouMengHuoCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().MouMengHuoCardUseForbidden(query);
    internal int MouMhCharge(int owner) => _resolve().MouMhCharge(owner);
    internal global::CardGame.Core.PromptChoice MouMhChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? seat = default, int? card = default) => _resolve().MouMhChoice(f, branch, label, seat, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouMhChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouMhChoices(f);
    internal bool MouMhFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouMhFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MouMhObserverRoot() => _resolve().MouMhObserverRoot();
    internal bool MouMhObserverSuffix(int i) => _resolve().MouMhObserverSuffix(i);
    internal bool MouMhPhaseParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouMhPhaseParent(f);
    internal global::CardGame.Core.MouMengHuoTurnKey? MouMhTurn() => _resolve().MouMhTurn();
    internal void PublishMouMh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouMh(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouMh(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveMouMh(supplied, op);
    internal bool RestoreMouMengHuoDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().RestoreMouMengHuoDamageSource(group);
    internal bool ResumeMouMh(long id) => _resolve().ResumeMouMh(id);
    internal void ReturnMouMhDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMouMhDraw(f, outcome);
    internal bool ReturnMouMhMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouMhMovement(f);
    internal void SealMouMh(long id, long before) => _resolve().SealMouMh(id, before);
    internal bool ValidMouMh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouMh(f);
    internal bool HasMouMengHuoCapability { get => _resolve().HasMouMengHuoCapability; }
}
