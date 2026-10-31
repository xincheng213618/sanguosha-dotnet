#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterCaoXiOlState : Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess
{
    private IReadOnlyList<CaoXiGangshuIncreasedEvent> _caoXiActiveUnits = [];
    internal ref IReadOnlyList<CaoXiGangshuIncreasedEvent> ReferenceToCaoXiActiveUnits => ref _caoXiActiveUnits;

    private int _caoXiHistoryCount = -1;
    internal ref int ReferenceToCaoXiHistoryCount => ref _caoXiHistoryCount;

    private IGameEvent? _caoXiHistoryTail = default !;
    internal ref IGameEvent? ReferenceToCaoXiHistoryTail => ref _caoXiHistoryTail;

    ref int Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryCount => ref _caoXiHistoryCount;

    ref IGameEvent? Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryTail => ref _caoXiHistoryTail;

    ref IReadOnlyList<CaoXiGangshuIncreasedEvent> Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiActiveUnits => ref _caoXiActiveUnits;
}
