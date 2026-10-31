#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCommandResultProjectionCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot State { get; }
}
