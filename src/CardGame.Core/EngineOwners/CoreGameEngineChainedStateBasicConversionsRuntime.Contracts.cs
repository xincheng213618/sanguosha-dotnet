#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreChainedStateBasicConversionsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreChainedStateBasicConversionsCoreCommandSessionQueriesOperations : IProgramRuntimeOperations
{
    long Revision { get; }
}
