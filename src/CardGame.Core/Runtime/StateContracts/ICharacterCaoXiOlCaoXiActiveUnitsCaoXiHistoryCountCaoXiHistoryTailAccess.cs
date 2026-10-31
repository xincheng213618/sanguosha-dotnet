#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess : IProgramRuntimeOperations
{
    ref int _caoXiHistoryCount { get; }

    ref IGameEvent? _caoXiHistoryTail { get; }

    ref IReadOnlyList<CaoXiGangshuIncreasedEvent> _caoXiActiveUnits { get; }
}
