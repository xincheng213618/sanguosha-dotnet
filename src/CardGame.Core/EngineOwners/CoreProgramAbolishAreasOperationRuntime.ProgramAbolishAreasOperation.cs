#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramAbolishAreasOperationRuntime
{
    internal void AbolishProgramOwnerAreas(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var abolishedSlots = zones.Contains(CardZoneKind.Equipment) ? Enum.GetValues<EquipmentSlot>().Count(slot => owner.EquipmentSlotCapacity(slot) > 0) : 0;
        if (zones.Contains(CardZoneKind.Equipment))
            owner.EquipmentAreaAbolished = true;
        if (zones.Contains(CardZoneKind.Judgment))
            owner.JudgmentAreaAbolished = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerAreasAbolishedEvent(owner.Seat, owner.EquipmentAreaAbolished, owner.JudgmentAreaAbolished));
        if (abolishedSlots > 0)
            _characterLiuHongOlOperations.ResolveTuxingAfterSlotAbolish(frame, owner.Seat, abolishedSlots);
    }
}
