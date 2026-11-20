#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFieldEquipmentRuntime
{
    internal EquipmentSlot FieldEquipmentSlot(Card card) =>
        pipelineHooks._programPipelineHooks.GetFieldEquipmentRepresentation(card.Id) is { } representation &&
        representation.PrintedKind == card.Kind
            ? representation.OccupiedSlot
            : _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;

    internal IReadOnlyList<Card> GetEquipmentWithPrintedEffects(CharacterState player) =>
        player.SkillGrants.ArePrintedEquipmentEffectsSuppressed ? [] : cards.CardsAt(CardLocation.Equipment(player.Seat))
            .Where(card => pipelineHooks._programPipelineHooks.GetFieldEquipmentRepresentation(card.Id) is null).ToArray();
}
