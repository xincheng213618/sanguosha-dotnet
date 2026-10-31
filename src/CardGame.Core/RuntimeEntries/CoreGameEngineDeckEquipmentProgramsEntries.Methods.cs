#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeckEquipmentProgramsEntries
{
    internal void ContinueProgramAfterRandomEquipmentUse() => _resolve().ContinueProgramAfterRandomEquipmentUse();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterBeneficiarySuitShieldActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> actions) => _resolve().FilterBeneficiarySuitShieldActions(actor, actions);
    internal void GrantProgramRandomSkillAndSuitShield(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::System.Collections.Generic.IReadOnlyList<string> skills, global::CardGame.Core.Suit suit) => _resolve().GrantProgramRandomSkillAndSuitShield(frame, targetSeat, skills, suit);
    internal bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit) => _resolve().HasBeneficiarySuitShield(actor, target, suit);
    internal bool IsShieldedRescueSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().IsShieldedRescueSelection(owner, selection);
    internal void ObserveBeneficiarySuitShield(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveBeneficiarySuitShield(payload);
    internal bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().RandomEquipmentFrameRidesOn(ride, beneath);
    internal bool ReturnedRandomEquipmentMovementMatches(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame moved) => _resolve().ReturnedRandomEquipmentMovementMatches(parent, moved);
    internal global::CardGame.Core.SkillProgramStepOutcome UseProgramRandomDeckEquipment(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string bind) => _resolve().UseProgramRandomDeckEquipment(frame, targetSeat, bind);
}
