#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhangJiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterZhangJiOlCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    IProgramSelectionQueries ProgramSelectionQueries { get; }
}
