#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePrivateWhirlState : Contracts.ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess
{
    // Capability latch only: replayed commands rebuild it, it contains no use ID,
    // entity, pending cost or continuation. Ordinary matches never scan its history.
    private bool _hasIssuedPrivateWhirl = default !;
    internal ref bool ReferenceToHasIssuedPrivateWhirl => ref _hasIssuedPrivateWhirl;

    ref bool Contracts.ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess._hasIssuedPrivateWhirl => ref _hasIssuedPrivateWhirl;
}
