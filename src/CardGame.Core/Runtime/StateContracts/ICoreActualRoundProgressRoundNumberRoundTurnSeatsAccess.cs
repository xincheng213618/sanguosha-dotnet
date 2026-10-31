#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess : IProgramRuntimeOperations
{
    ref int _roundNumber { get; }

    ref readonly HashSet<int> _roundTurnSeats { get; }
}
