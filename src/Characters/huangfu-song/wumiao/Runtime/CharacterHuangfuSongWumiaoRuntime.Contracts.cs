#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangfuSongWumiaoCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterHuangfuSongWumiaoCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
