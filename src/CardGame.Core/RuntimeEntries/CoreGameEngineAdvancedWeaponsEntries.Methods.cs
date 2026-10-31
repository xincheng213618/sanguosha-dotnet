#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedWeaponsEntries
{
    internal IReadOnlyList<int> AliveSeatsBetweenInclusive(int start, int end) => _resolve().AliveSeatsBetweenInclusive(start, end);
    internal SkillProgramStepOutcome BeginWeaponReclaim(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BeginWeaponReclaim(frame, effect);
    internal void CommitWeaponInheritance(ProgramSkillFrame frame, int cardId) => _resolve().CommitWeaponInheritance(frame, cardId);
    internal void CopyFirstReplacedWeapon(Card card, Card replaced) => _resolve().CopyFirstReplacedWeapon(card, replaced);
    internal string DescribeGeneralWeaponChoice(string generalId) => _resolve().DescribeGeneralWeaponChoice(generalId);
    internal void EquipGeneralWeapon(ProgramSkillFrame frame, ContentGeneralDefinition general) => _resolve().EquipGeneralWeapon(frame, general);
    internal IReadOnlyList<Card> GetEquipmentRuleCards(CharacterState player, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().GetEquipmentRuleCards(player, excludedEquipmentSlot);
    internal string[] GetGeneralWeaponSkillIds(ContentGeneralDefinition general) => _resolve().GetGeneralWeaponSkillIds(general);
    internal IEnumerable<CardKind> GetWeaponAbilityKinds(Card card) => _resolve().GetWeaponAbilityKinds(card);
    internal bool HasWeaponAbility(CharacterState player, CardKind kind) => _resolve().HasWeaponAbility(player, kind);
    internal bool MoveProcessingCardUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = default) => _resolve().MoveProcessingCardUnlessDestroyed(card, destination, reason, beforeFact);
    internal void PlaceAdvancedEquipment(Card card, int targetSeat) => _resolve().PlaceAdvancedEquipment(card, targetSeat);
    internal void PlaceAdvancedWeapon(ProgramSkillFrame frame, int targetSeat, CardKind kind) => _resolve().PlaceAdvancedWeapon(frame, targetSeat, kind);
    internal void ResolveDynamicEquipmentSkillGrants(Card card, CardLocation from, CardLocation to) => _resolve().ResolveDynamicEquipmentSkillGrants(card, from, to);
}
