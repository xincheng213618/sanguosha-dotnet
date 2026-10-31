#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreJiqiaoProgramsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
