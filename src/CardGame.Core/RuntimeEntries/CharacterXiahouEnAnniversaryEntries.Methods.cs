#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouEnAnniversaryEntries
{
    internal bool AdvanceXiahouEnObserver() => _resolve().AdvanceXiahouEnObserver();
    internal void AssertXiahouEnShiwuDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin) => _resolve().AssertXiahouEnShiwuDuelUse(use, origin);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryXiahouEn(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginAnniversaryXiahouEn(supplied, operation);
    internal bool CanOfferAnniversaryXiahouEn(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryXiahouEn(candidate, trigger, context);
    internal void ContinueAnniversaryXiahouEn(long id) => _resolve().ContinueAnniversaryXiahouEn(id);
    internal bool HasXiahouEnObserverDying() => _resolve().HasXiahouEnObserverDying();
    internal bool HasXiahouEnPaidDamageObserver(long id) => _resolve().HasXiahouEnPaidDamageObserver(id);
    internal bool IsAnniversaryXiahouEnPaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsAnniversaryXiahouEnPaidMovement(f, effect, pending);
    internal bool KeepIssuedAnniversaryXiahouEn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepIssuedAnniversaryXiahouEn(f);
    internal void RequireAnniversaryXiahouEn(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireAnniversaryXiahouEn(frame);
    internal void RestoreXiahouEnSwordAtActualTurnEnd(global::CardGame.Core.TurnEndedEvent ended) => _resolve().RestoreXiahouEnSwordAtActualTurnEnd(ended);
    internal bool ResumeAnniversaryXiahouEn(long id) => _resolve().ResumeAnniversaryXiahouEn(id);
    internal bool ReturnAnniversaryXiahouEnMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryXiahouEnMovement(f);
    internal bool ReturnXiahouEnShiwuAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnXiahouEnShiwuAttack(completion);
    internal bool ValidXiahouEnShiwuUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin) => _resolve().ValidXiahouEnShiwuUse(use, origin);
    internal global::CardGame.Core.XiahouEnShiwuOrigin? XiahouEnDamageOrigin(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().XiahouEnDamageOrigin(owner, context);
    internal bool XiahouEnFirstObserverChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().XiahouEnFirstObserverChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? XiahouEnObserverRoot() => _resolve().XiahouEnObserverRoot();
    internal global::CardGame.Core.CardKind? XiahouEnVirtualQinggang(int seat, global::CardGame.Core.EquipmentSlot slot) => _resolve().XiahouEnVirtualQinggang(seat, slot);
    internal void XiahouEnVirtualSwordRange(global::CardGame.Core.ProgramAttackRangeQuery query, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().XiahouEnVirtualSwordRange(query, terms);
}
