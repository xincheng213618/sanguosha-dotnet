#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHuxinMirrorEntries
{
    internal global::CardGame.Core.Card? ActiveHuxinMirror(int owner, string instance) => _resolve().ActiveHuxinMirror(owner, instance);
    internal bool CanOfferHuxinMirror(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanOfferHuxinMirror(context);
    internal void ContinueHuxinMirrorPlacement(long id) => _resolve().ContinueHuxinMirrorPlacement(id);
    internal bool HuxinMirrorDamageQualifies(int owner, int amount, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HuxinMirrorDamageQualifies(owner, amount, attack);
    internal bool HuxinMirrorIsEligible(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext window) => _resolve().HuxinMirrorIsEligible(owner, instance, window);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> HuxinMirrorPlacementActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().HuxinMirrorPlacementActions(actor, cards);
    internal bool KeepPaidHuxinMirror(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepPaidHuxinMirror(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome PayHuxinMirrorAndPrevent(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PayHuxinMirrorAndPrevent(supplied);
    internal bool ResumePaidHuxinMirror(long id) => _resolve().ResumePaidHuxinMirror(id);
    internal void ReturnHuxinMirrorPlacementMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame window) => _resolve().ReturnHuxinMirrorPlacementMovement(window);
    internal bool ReturnPaidHuxinMirrorMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPaidHuxinMirrorMovement(frame);
    internal bool TryPlaceHandHuxinMirror(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryPlaceHandHuxinMirror(actor, action);
    internal bool ValidPaidHuxinMirror(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPaidHuxinMirror(frame);
}
