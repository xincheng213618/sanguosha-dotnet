#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDongZhuoMouOlMouDongZhuoOlOperations : IProgramRuntimeOperations
{
    MouDongZhuoOlRuntime Get();
}

internal interface ICharacterDongZhuoMouOlCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterDongZhuoMouOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
