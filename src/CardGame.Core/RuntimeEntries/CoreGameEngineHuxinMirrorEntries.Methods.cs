#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHuxinMirrorEntries
{
    internal Card? ActiveHuxinMirror(int owner, string instance) => _resolve().ActiveHuxinMirror(owner, instance);
    internal bool CanOfferHuxinMirror(ProgramTriggerLegalityContext context) => _resolve().CanOfferHuxinMirror(context);
    internal void ContinueHuxinMirrorPlacement(long id) => _resolve().ContinueHuxinMirrorPlacement(id);
    internal bool HuxinMirrorDamageQualifies(int owner, int amount, GameEngine.IDamageAttempt attack) => _resolve().HuxinMirrorDamageQualifies(owner, amount, attack);
    internal bool HuxinMirrorIsEligible(int owner, string instance, ProgramSkillWindowContext window) => _resolve().HuxinMirrorIsEligible(owner, instance, window);
    internal IEnumerable<LegalAction> HuxinMirrorPlacementActions(CharacterState actor, IReadOnlyList<Card> cards) => _resolve().HuxinMirrorPlacementActions(actor, cards);
    internal bool KeepPaidHuxinMirror(ProgramSkillFrame frame) => _resolve().KeepPaidHuxinMirror(frame);
    internal SkillProgramStepOutcome PayHuxinMirrorAndPrevent(ProgramSkillFrame supplied) => _resolve().PayHuxinMirrorAndPrevent(supplied);
    internal bool ResumePaidHuxinMirror(long id) => _resolve().ResumePaidHuxinMirror(id);
    internal void ReturnHuxinMirrorPlacementMovement(CardsMovedTriggerWindowFrame window) => _resolve().ReturnHuxinMirrorPlacementMovement(window);
    internal bool ReturnPaidHuxinMirrorMovement(ProgramSkillFrame frame) => _resolve().ReturnPaidHuxinMirrorMovement(frame);
    internal bool TryPlaceHandHuxinMirror(CharacterState actor, LegalAction action) => _resolve().TryPlaceHandHuxinMirror(actor, action);
    internal bool ValidPaidHuxinMirror(ProgramSkillFrame frame) => _resolve().ValidPaidHuxinMirror(frame);
}
