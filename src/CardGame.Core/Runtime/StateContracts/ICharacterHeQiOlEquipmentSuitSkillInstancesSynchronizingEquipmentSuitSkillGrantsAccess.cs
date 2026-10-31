#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _synchronizingEquipmentSuitSkillGrants { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.EquipmentSuitSkillGrantSource Source), string> _equipmentSuitSkillInstances { get; }
}
