#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouEnAnniversaryEntries
{
    internal bool AdvanceXiahouEnObserver() => _resolve().AdvanceXiahouEnObserver();
    internal SkillProgramStepOutcome BeginAnniversaryXiahouEn(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginAnniversaryXiahouEn(supplied, operation);
    internal bool CanOfferAnniversaryXiahouEn(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryXiahouEn(candidate, trigger, context);
    internal void ContinueAnniversaryXiahouEn(long id) => _resolve().ContinueAnniversaryXiahouEn(id);
    internal bool HasXiahouEnObserverDying() => _resolve().HasXiahouEnObserverDying();
    internal bool KeepIssuedAnniversaryXiahouEn(ProgramSkillFrame f) => _resolve().KeepIssuedAnniversaryXiahouEn(f);
    internal void RestoreXiahouEnSwordAtActualTurnEnd(TurnEndedEvent ended) => _resolve().RestoreXiahouEnSwordAtActualTurnEnd(ended);
    internal bool ResumeAnniversaryXiahouEn(long id) => _resolve().ResumeAnniversaryXiahouEn(id);
    internal bool ReturnAnniversaryXiahouEnMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryXiahouEnMovement(f);
    internal bool ReturnXiahouEnShiwuAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnXiahouEnShiwuAttack(completion);
    internal bool ValidXiahouEnShiwuUse(CardUseFrame use, ProgramSelectedActorDuelOrigin origin) => _resolve().ValidXiahouEnShiwuUse(use, origin);
    internal XiahouEnShiwuOrigin? XiahouEnDamageOrigin(int owner, ProgramSkillWindowContext context) => _resolve().XiahouEnDamageOrigin(owner, context);
    internal bool XiahouEnFirstObserverChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().XiahouEnFirstObserverChild(f, child);
    internal ProgramSkillFrame? XiahouEnObserverRoot() => _resolve().XiahouEnObserverRoot();
    internal CardKind? XiahouEnVirtualQinggang(int seat, EquipmentSlot slot) => _resolve().XiahouEnVirtualQinggang(seat, slot);
    internal void XiahouEnVirtualSwordRange(ProgramAttackRangeQuery query, List<RuleQueryBaseTerm> terms) => _resolve().XiahouEnVirtualSwordRange(query, terms);
}
