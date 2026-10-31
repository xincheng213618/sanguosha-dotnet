#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess : IProgramRuntimeOperations
{
    ref bool _woodenOxUsedThisTurn { get; }
}
