#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualRoundProgressRoundTurnSeatsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.HashSet<int> _roundTurnSeats { get; }
}
