#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePrivateWhirlState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess
{
    // Capability latch only: replayed commands rebuild it, it contains no use ID,
    // entity, pending cost or continuation. Ordinary matches never scan its history.
    private bool _hasIssuedPrivateWhirl = default !;
    internal ref bool ReferenceToHasIssuedPrivateWhirl => ref _hasIssuedPrivateWhirl;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess._hasIssuedPrivateWhirl => ref _hasIssuedPrivateWhirl;
}
