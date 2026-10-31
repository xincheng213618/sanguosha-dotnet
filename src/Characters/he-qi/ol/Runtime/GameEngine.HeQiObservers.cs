namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref bool _synchronizingEquipmentSuitSkillGrants => ref _characterHeQiOl.ReferenceToSynchronizingEquipmentSuitSkillGrants;
    // A source relation remembers its first effective instance through equipment suit exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.EquipmentSuitSkillGrantSource Source), string> _equipmentSuitSkillInstances => ref _characterHeQiOl.ReferenceToEquipmentSuitSkillInstances;
}
