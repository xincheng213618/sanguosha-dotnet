#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressStartedAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _started { get; }
}
