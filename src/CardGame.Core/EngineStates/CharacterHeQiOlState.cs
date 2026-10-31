#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHeQiOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess
{
    // A source relation remembers its first effective instance through equipment suit exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.EquipmentSuitSkillGrantSource Source), string> _equipmentSuitSkillInstances = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.EquipmentSuitSkillGrantSource Source), string> ReferenceToEquipmentSuitSkillInstances => ref _equipmentSuitSkillInstances;

    private bool _synchronizingEquipmentSuitSkillGrants = default !;
    internal ref bool ReferenceToSynchronizingEquipmentSuitSkillGrants => ref _synchronizingEquipmentSuitSkillGrants;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._synchronizingEquipmentSuitSkillGrants => ref _synchronizingEquipmentSuitSkillGrants;

    ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.EquipmentSuitSkillGrantSource Source), string> global::CardGame.Core.EngineStates.Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess._equipmentSuitSkillInstances => ref _equipmentSuitSkillInstances;
}
