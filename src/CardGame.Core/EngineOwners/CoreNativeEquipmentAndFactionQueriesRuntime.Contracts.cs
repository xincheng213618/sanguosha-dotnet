#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}
