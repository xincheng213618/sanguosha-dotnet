namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string DuanhunSkillId = "shanhe:duanhun-14079";

    // The ordinary policy covers damage to others. This weapon's native
    // qualification also covers damage to its own actual source character.
    private bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess,
        IReadOnlySet<string>? visibleSkillIds = null) =>
        !sourceLess && IsValidPlayerSeat(sourceSeat) && _players[sourceSeat].IsAlive &&
        CardPolicies(_players[sourceSeat], SkillProgramCardPolicyKind.BypassOtherParticipantArmor)
            .Any(binding => binding.Source.SkillId == DuanhunSkillId &&
                (visibleSkillIds is null || visibleSkillIds.Contains(binding.Source.SkillId)) &&
                IsActiveDuanhunPolicySource(sourceSeat, binding.Source));

    private bool IsActiveDuanhunPolicySource(int ownerSeat, IndexedSkillProgramInstance source) =>
        source.SkillId == DuanhunSkillId && IsValidPlayerSeat(ownerSeat) &&
        source.SkillInstanceId is { } instance && _players[ownerSeat].IsAlive &&
        HasRuntimeSkillInstance(_players[ownerSeat], source.SkillId, instance) &&
        KongwuEffectiveEquipment(_players[ownerSeat]).Any(card => card.Id > 0 && !card.IsGeneralWeapon &&
            card.Kind == CardKind.Duanhun && _cardZones.GetLocation(card.Id) == CardLocation.Equipment(ownerSeat) &&
            IsActiveProgramSourceEquipmentCard(ownerSeat, source.SkillId, instance, card));
}
