#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeValidation(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime) : IGameStartingDrawNativeValidation
{
    public bool IsValidNativeDrawInvocation(NativeDrawInvocationProof proof) => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().ValidNativeDrawInvocation(proof);
}
