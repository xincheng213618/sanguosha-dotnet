#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatStartedAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _started { get; }

    ref int _currentSeat { get; }
}
