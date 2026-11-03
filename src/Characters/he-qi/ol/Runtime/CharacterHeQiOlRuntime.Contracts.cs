#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHeQiOlCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    IProgramSelectionQueries ProgramSelectionQueries { get; }
}

internal interface ICharacterHeQiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
