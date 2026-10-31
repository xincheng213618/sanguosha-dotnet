#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterCaoXiOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess
{
    private global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> _caoXiActiveUnits = [];
    internal ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> ReferenceToCaoXiActiveUnits => ref _caoXiActiveUnits;

    private int _caoXiHistoryCount = -1;
    internal ref int ReferenceToCaoXiHistoryCount => ref _caoXiHistoryCount;

    private global::CardGame.Core.IGameEvent? _caoXiHistoryTail = default !;
    internal ref global::CardGame.Core.IGameEvent? ReferenceToCaoXiHistoryTail => ref _caoXiHistoryTail;

    ref int global::CardGame.Core.EngineStates.Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryCount => ref _caoXiHistoryCount;

    ref global::CardGame.Core.IGameEvent? global::CardGame.Core.EngineStates.Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryTail => ref _caoXiHistoryTail;

    ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> global::CardGame.Core.EngineStates.Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiActiveUnits => ref _caoXiActiveUnits;
}
