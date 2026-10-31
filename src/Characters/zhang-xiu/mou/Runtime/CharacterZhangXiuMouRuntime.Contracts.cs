#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhangXiuMouCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterZhangXiuMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterZhangXiuMouMouZhangXiuAnniversaryOperations : IProgramRuntimeOperations
{
    MouZhangXiuAnniversaryRuntime Get();
}
