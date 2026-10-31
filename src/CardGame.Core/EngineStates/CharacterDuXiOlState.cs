#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterDuXiOlState : Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess
{
    private IReadOnlyDictionary<int, int> _duXiSuitProjection = new Dictionary<int, int>();
    internal ref IReadOnlyDictionary<int, int> ReferenceToDuXiSuitProjection => ref _duXiSuitProjection;

    // Only a derived lookup cache. Its complete source is the immutable committed/pending rule facts.
    private int _duXiSuitProjectionEventCount = -1;
    internal ref int ReferenceToDuXiSuitProjectionEventCount => ref _duXiSuitProjectionEventCount;

    private IGameEvent? _duXiSuitProjectionLastFact = default !;
    internal ref IGameEvent? ReferenceToDuXiSuitProjectionLastFact => ref _duXiSuitProjectionLastFact;

    ref int Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionEventCount => ref _duXiSuitProjectionEventCount;

    ref IGameEvent? Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjectionLastFact => ref _duXiSuitProjectionLastFact;

    ref IReadOnlyDictionary<int, int> Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess._duXiSuitProjection => ref _duXiSuitProjection;
}
