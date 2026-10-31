#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations : IProgramRuntimeOperations
{
    PlayerViewProjector Get();
}
