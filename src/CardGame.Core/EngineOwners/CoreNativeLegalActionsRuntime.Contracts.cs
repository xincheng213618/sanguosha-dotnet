#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeLegalActionsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNativeLegalActionsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreNativeLegalActionsCoreProgramActionModulesOperations : IProgramRuntimeOperations
{
    ProgramActionRouter ProgramActions { get; }
}

internal interface ICoreNativeLegalActionsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
