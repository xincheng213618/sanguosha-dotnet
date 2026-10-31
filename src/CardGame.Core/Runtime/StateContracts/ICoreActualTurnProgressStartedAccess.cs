#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressStartedAccess : IProgramRuntimeOperations
{
    ref bool _started { get; }
}
