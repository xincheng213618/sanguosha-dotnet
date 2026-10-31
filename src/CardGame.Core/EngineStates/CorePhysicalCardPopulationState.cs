#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CorePhysicalCardPopulationState : global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess
{
    private int _initialCardCount = default !;
    internal ref int ReferenceToInitialCardCount => ref _initialCardCount;

    ref int global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess._initialCardCount => ref _initialCardCount;
}
