using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Capability latch only: replayed commands rebuild it, it contains no use ID,
    // entity, pending cost or continuation. Ordinary matches never scan its history.
    private ref bool _hasIssuedPrivateWhirl => ref _privateWhirl.ReferenceToHasIssuedPrivateWhirl;
}
