#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedWeaponsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> AliveSeatsBetweenInclusive(int start, int end) => _resolve().AliveSeatsBetweenInclusive(start, end);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWeaponReclaim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginWeaponReclaim(frame, effect);
    internal void CommitWeaponInheritance(global::CardGame.Core.ProgramSkillFrame frame, int cardId) => _resolve().CommitWeaponInheritance(frame, cardId);
    internal void CopyFirstReplacedWeapon(global::CardGame.Core.Card card, global::CardGame.Core.Card replaced) => _resolve().CopyFirstReplacedWeapon(card, replaced);
    internal string DescribeGeneralWeaponChoice(string generalId) => _resolve().DescribeGeneralWeaponChoice(generalId);
    internal void EquipGeneralWeapon(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ContentGeneralDefinition general) => _resolve().EquipGeneralWeapon(frame, general);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipmentRuleCards(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().GetEquipmentRuleCards(player, excludedEquipmentSlot);
    internal string[] GetGeneralWeaponSkillIds(global::CardGame.Core.ContentGeneralDefinition general) => _resolve().GetGeneralWeaponSkillIds(general);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardKind> GetWeaponAbilityKinds(global::CardGame.Core.Card card) => _resolve().GetWeaponAbilityKinds(card);
    internal bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => _resolve().HasWeaponAbility(player, kind);
    internal bool MoveProcessingCardUnlessDestroyed(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default) => _resolve().MoveProcessingCardUnlessDestroyed(card, destination, reason, beforeFact);
    internal void PlaceAdvancedEquipment(global::CardGame.Core.Card card, int targetSeat) => _resolve().PlaceAdvancedEquipment(card, targetSeat);
    internal void PlaceAdvancedWeapon(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.CardKind kind) => _resolve().PlaceAdvancedWeapon(frame, targetSeat, kind);
    internal void ResolveDynamicEquipmentSkillGrants(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().ResolveDynamicEquipmentSkillGrants(card, from, to);
}
