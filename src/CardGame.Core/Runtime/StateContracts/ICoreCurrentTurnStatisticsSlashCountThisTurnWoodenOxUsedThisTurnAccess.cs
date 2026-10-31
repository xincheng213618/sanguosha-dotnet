#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess : IProgramRuntimeOperations
{
    ref int _slashCountThisTurn { get; }

    ref bool _woodenOxUsedThisTurn { get; }
}
