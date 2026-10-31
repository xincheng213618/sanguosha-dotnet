#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardPoliciesEntries
{
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => _resolve().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
    internal global::CardGame.Core.Suit GetProgramEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation = default) => _resolve().GetProgramEffectiveSuit(owner, card, identityLocation);
    internal int GetProgramRequiredResponseCount(global::CardGame.Core.CharacterState owner, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::CardGame.Core.CardKind requiredKind) => _resolve().GetProgramRequiredResponseCount(owner, sourceSeat, responderSeat, incomingKind, requiredKind);
    internal bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => _resolve().HasCardPolicy(owner, kind, effectiveKind);
    internal bool HasOutsideAttackRangeSlashQuota(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().HasOutsideAttackRangeSlashQuota(owner, target, kind, excludedEquipmentSlot);
    internal bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => _resolve().IsNearbyTargetResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal bool IsSuitSlashResponseProhibited(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().IsSuitSlashResponseProhibited(attack);
    internal bool OutsideAttackRangeSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().OutsideAttackRangeSlashQuotaAfterLimit(query);
}
