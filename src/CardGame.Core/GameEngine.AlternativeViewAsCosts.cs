namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<int, (int Turn, global::System.Collections.Generic.HashSet<global::CardGame.Core.CardKind> Kinds)> _usedProgramBasicCardNames => ref _alternativeViewAsCosts.ReferenceToUsedProgramBasicCardNames;
}
