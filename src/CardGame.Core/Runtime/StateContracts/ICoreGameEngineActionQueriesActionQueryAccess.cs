#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineActionQueriesActionQueryAccess : IProgramRuntimeOperations
{
    ref NativeOwners.ActionQuery? _actionQuery { get; }
}
