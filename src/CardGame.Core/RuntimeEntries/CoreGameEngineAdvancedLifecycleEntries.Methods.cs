#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedLifecycleEntries
{
    internal CardKind AdvancedEffectiveHandKind(CharacterState owner, Card card) => _resolve().AdvancedEffectiveHandKind(owner, card);
    internal SkillProgramStepOutcome BeginAdvancedSelection(ProgramSkillFrame frame, SkillProgramEffect effect, IReadOnlyList<string> candidates, int rankSum = 0) => _resolve().BeginAdvancedSelection(frame, effect, candidates, rankSum);
    internal SkillProgramStepOutcome ExecuteAdvancedLifecycle(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => _resolve().ExecuteAdvancedLifecycle(effect, frame, targetSeat);
    internal Card GetAdvancedCard(int id) => _resolve().GetAdvancedCard(id);
    internal int GetWeaponAttackRange(CharacterState owner, Card weapon) => _resolve().GetWeaponAttackRange(owner, weapon);
    internal bool IsFarthestInRange(CharacterState owner, CharacterState target) => _resolve().IsFarthestInRange(owner, target);
    internal void ObtainAdvancedCards(ProgramSkillFrame frame, IReadOnlyList<Card> cards) => _resolve().ObtainAdvancedCards(frame, cards);
    internal bool RestoreEquipmentSlot(CharacterState player, EquipmentSlot slot) => _resolve().RestoreEquipmentSlot(player, slot);
    internal void SetEquipmentSlotCapacity(CharacterState player, EquipmentSlot slot, int capacity) => _resolve().SetEquipmentSlotCapacity(player, slot, capacity);
}
