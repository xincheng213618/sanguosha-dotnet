#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess : IProgramRuntimeOperations
{
    ref int _duXiSuitProjectionEventCount { get; }

    ref IGameEvent? _duXiSuitProjectionLastFact { get; }

    ref IReadOnlyDictionary<int, int> _duXiSuitProjection { get; }
}
