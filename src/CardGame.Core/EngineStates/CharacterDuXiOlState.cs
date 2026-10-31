#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterDuXiOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess
{
    private global::System.Collections.Generic.IReadOnlyDictionary<int, int> _duXiSuitProjection = new global::System.Collections.Generic.Dictionary<int, int>();
    internal ref global::System.Collections.Generic.IReadOnlyDictionary<int, int> ReferenceToDuXiSuitProjection => ref _duXiSuitProjection;

    // Only a derived lookup cache. Its complete source is the immutable committed/pending rule facts.
    private int _duXiSuitProjectionEventCount = -1;
    internal ref int ReferenceToDuXiSuitProjectionEventCount => ref _duXiSuitProjectionEventCount;

    private global::CardGame.Core.IGameEvent? _duXiSuitProjectionLastFact = default !;
    internal ref global::CardGame.Core.IGameEvent? ReferenceToDuXiSuitProjectionLastFact => ref _duXiSuitProjectionLastFact;

    ref int global::CardGame.Core.EngineStates.Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionEventCount => ref _duXiSuitProjectionEventCount;

    ref global::CardGame.Core.IGameEvent? global::CardGame.Core.EngineStates.Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionLastFact => ref _duXiSuitProjectionLastFact;

    ref global::System.Collections.Generic.IReadOnlyDictionary<int, int> global::CardGame.Core.EngineStates.Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjection => ref _duXiSuitProjection;
}
