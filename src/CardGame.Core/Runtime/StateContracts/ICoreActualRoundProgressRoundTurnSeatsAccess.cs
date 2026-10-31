#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualRoundProgressRoundTurnSeatsAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<int> _roundTurnSeats { get; }
}
