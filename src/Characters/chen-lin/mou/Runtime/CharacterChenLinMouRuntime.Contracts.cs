#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterChenLinMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterChenLinMouMouChenLinOperations : IProgramRuntimeOperations
{
    MouChenLinRuntime Get();
}

internal interface ICharacterChenLinMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
