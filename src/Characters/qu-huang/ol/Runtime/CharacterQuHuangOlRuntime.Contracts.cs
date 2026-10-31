#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterQuHuangOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterQuHuangOlQuHuangPhaseEndLedgerOperations : IProgramRuntimeOperations
{
    QuHuangPhaseEndLedger Get();
}

internal interface ICharacterQuHuangOlQuHuangOperations : IProgramRuntimeOperations
{
    QuHuangRuntime Get();
}
