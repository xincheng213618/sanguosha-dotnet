#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHeQiOlState : Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess
{
    // A source relation remembers its first effective instance through equipment suit exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private Dictionary<(int OwnerSeat, EquipmentSuitSkillGrantSource Source), string> _equipmentSuitSkillInstances = [];
    internal ref readonly Dictionary<(int OwnerSeat, EquipmentSuitSkillGrantSource Source), string> ReferenceToEquipmentSuitSkillInstances => ref _equipmentSuitSkillInstances;

    private bool _synchronizingEquipmentSuitSkillGrants = default !;
    internal ref bool ReferenceToSynchronizingEquipmentSuitSkillGrants => ref _synchronizingEquipmentSuitSkillGrants;

    ref bool Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._synchronizingEquipmentSuitSkillGrants => ref _synchronizingEquipmentSuitSkillGrants;

    ref readonly Dictionary<(int OwnerSeat, EquipmentSuitSkillGrantSource Source), string> Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances => ref _equipmentSuitSkillInstances;
}
