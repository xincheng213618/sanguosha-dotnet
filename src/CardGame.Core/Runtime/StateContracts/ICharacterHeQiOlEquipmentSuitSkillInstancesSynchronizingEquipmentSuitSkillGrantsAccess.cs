#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess : IProgramRuntimeOperations
{
    ref bool _synchronizingEquipmentSuitSkillGrants { get; }

    ref readonly Dictionary<(int OwnerSeat, EquipmentSuitSkillGrantSource Source), string> _equipmentSuitSkillInstances { get; }
}
