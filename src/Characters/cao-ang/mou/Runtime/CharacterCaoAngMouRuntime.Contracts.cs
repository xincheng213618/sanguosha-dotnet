#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoAngMouCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterCaoAngMouMouCaoAngOperations : IProgramRuntimeOperations
{
    MouCaoAngRuntime Get();
}

internal interface ICharacterCaoAngMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterCaoAngMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
