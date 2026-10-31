#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuanShuJunOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterHuanShuJunOlCoreCommandSessionQueriesOperations : IProgramRuntimeOperations
{
    long Revision { get; }
}
