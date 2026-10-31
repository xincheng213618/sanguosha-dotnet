#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardPoliciesEntries
{
    internal IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> CardPolicies(CharacterState owner, SkillProgramCardPolicyKind kind, CardKind? effectiveKind = default, CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => _resolve().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
    internal Suit GetProgramEffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation = default) => _resolve().GetProgramEffectiveSuit(owner, card, identityLocation);
    internal int GetProgramRequiredResponseCount(CharacterState owner, int sourceSeat, int responderSeat, CardKind incomingKind, CardKind requiredKind) => _resolve().GetProgramRequiredResponseCount(owner, sourceSeat, responderSeat, incomingKind, requiredKind);
    internal bool HasCardPolicy(CharacterState owner, SkillProgramCardPolicyKind kind, CardKind? effectiveKind = default) => _resolve().HasCardPolicy(owner, kind, effectiveKind);
    internal bool HasOutsideAttackRangeSlashQuota(CharacterState owner, CharacterState target, CardKind kind, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().HasOutsideAttackRangeSlashQuota(owner, target, kind, excludedEquipmentSlot);
    internal bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats) => _resolve().IsNearbyTargetResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal bool IsSuitSlashResponseProhibited(NativeOwners.CardAttackHandle attack) => _resolve().IsSuitSlashResponseProhibited(attack);
    internal bool OutsideAttackRangeSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().OutsideAttackRangeSlashQuotaAfterLimit(query);
}
