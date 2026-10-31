#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedLifecycleEntries
{
    internal global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().AdvancedEffectiveHandKind(owner, card);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAdvancedSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<string> candidates, int rankSum = 0) => _resolve().BeginAdvancedSelection(frame, effect, candidates, rankSum);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteAdvancedLifecycle(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().ExecuteAdvancedLifecycle(effect, frame, targetSeat);
    internal global::CardGame.Core.Card GetAdvancedCard(int id) => _resolve().GetAdvancedCard(id);
    internal int GetWeaponAttackRange(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card weapon) => _resolve().GetWeaponAttackRange(owner, weapon);
    internal bool IsFarthestInRange(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target) => _resolve().IsFarthestInRange(owner, target);
    internal void ObtainAdvancedCards(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().ObtainAdvancedCards(frame, cards);
    internal bool RestoreEquipmentSlot(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot slot) => _resolve().RestoreEquipmentSlot(player, slot);
    internal void SetEquipmentSlotCapacity(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot slot, int capacity) => _resolve().SetEquipmentSlotCapacity(player, slot, capacity);
}
