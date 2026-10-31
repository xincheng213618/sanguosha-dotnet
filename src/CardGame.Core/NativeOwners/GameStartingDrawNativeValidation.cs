#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeValidation(Func<RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime) : IGameStartingDrawNativeValidation
{
    public bool IsValidNativeDrawInvocation(NativeDrawInvocationProof proof) => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().ValidNativeDrawInvocation(proof);
}
