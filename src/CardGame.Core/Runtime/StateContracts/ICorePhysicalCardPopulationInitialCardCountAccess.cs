#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICorePhysicalCardPopulationInitialCardCountAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _initialCardCount { get; }
}
