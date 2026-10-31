#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeEquipmentUseCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
