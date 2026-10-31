#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeckEquipmentProgramsEntries
{
    internal void ContinueProgramAfterRandomEquipmentUse() => _resolve().ContinueProgramAfterRandomEquipmentUse();
    internal IReadOnlyList<LegalAction> FilterBeneficiarySuitShieldActions(CharacterState actor, IReadOnlyList<LegalAction> actions) => _resolve().FilterBeneficiarySuitShieldActions(actor, actions);
    internal void GrantProgramRandomSkillAndSuitShield(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<string> skills, Suit suit) => _resolve().GrantProgramRandomSkillAndSuitShield(frame, targetSeat, skills, suit);
    internal bool HasBeneficiarySuitShield(int actor, int target, Suit? suit) => _resolve().HasBeneficiarySuitShield(actor, target, suit);
    internal bool IsShieldedRescueSelection(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().IsShieldedRescueSelection(owner, selection);
    internal void ObserveBeneficiarySuitShield(IGameEvent payload) => _resolve().ObserveBeneficiarySuitShield(payload);
    internal bool RandomEquipmentFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().RandomEquipmentFrameRidesOn(ride, beneath);
    internal bool ReturnedRandomEquipmentMovementMatches(ProgramSkillFrame parent, CardsMovedTriggerWindowFrame moved) => _resolve().ReturnedRandomEquipmentMovementMatches(parent, moved);
    internal SkillProgramStepOutcome UseProgramRandomDeckEquipment(ProgramSkillFrame frame, int targetSeat, string bind) => _resolve().UseProgramRandomDeckEquipment(frame, targetSeat, bind);
}
