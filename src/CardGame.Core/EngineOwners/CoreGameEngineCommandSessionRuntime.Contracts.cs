#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCommandSessionCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot State { get; }
}
