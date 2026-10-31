namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Only a derived lookup cache. Its complete source is the immutable committed/pending rule facts.
    private ref int _duXiSuitProjectionEventCount => ref _characterDuXiOl.ReferenceToDuXiSuitProjectionEventCount;
    private ref global::CardGame.Core.IGameEvent? _duXiSuitProjectionLastFact => ref _characterDuXiOl.ReferenceToDuXiSuitProjectionLastFact;
    private ref global::System.Collections.Generic.IReadOnlyDictionary<int, int> _duXiSuitProjection => ref _characterDuXiOl.ReferenceToDuXiSuitProjection;
}
