#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreGuanduGeneralQualificationCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
