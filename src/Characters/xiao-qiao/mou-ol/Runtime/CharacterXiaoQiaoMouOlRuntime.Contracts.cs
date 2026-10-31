#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterXiaoQiaoMouOlCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterXiaoQiaoMouOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterXiaoQiaoMouOlMouXiaoQiaoOlOperations : IProgramRuntimeOperations
{
    MouXiaoQiaoOlRuntime Get();
}
