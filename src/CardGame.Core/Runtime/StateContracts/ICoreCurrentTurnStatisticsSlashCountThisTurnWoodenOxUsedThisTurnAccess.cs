#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _slashCountThisTurn { get; }

    ref bool _woodenOxUsedThisTurn { get; }
}
