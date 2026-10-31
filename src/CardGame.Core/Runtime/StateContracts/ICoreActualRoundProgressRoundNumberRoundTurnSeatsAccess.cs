#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _roundNumber { get; }

    ref readonly global::System.Collections.Generic.HashSet<int> _roundTurnSeats { get; }
}
