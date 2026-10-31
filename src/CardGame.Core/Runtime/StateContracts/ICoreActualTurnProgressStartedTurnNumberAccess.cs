#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressStartedTurnNumberAccess : IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref bool _started { get; }
}
