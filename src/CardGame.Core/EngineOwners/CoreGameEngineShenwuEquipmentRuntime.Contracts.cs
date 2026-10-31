#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreShenwuEquipmentCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
