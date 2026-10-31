#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _caoXiHistoryCount { get; }

    ref global::CardGame.Core.IGameEvent? _caoXiHistoryTail { get; }

    ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> _caoXiActiveUnits { get; }
}
