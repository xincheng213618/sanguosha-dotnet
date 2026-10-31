#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePhaseStartCardControlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
