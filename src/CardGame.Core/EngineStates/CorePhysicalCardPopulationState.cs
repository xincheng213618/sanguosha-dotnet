#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CorePhysicalCardPopulationState : Contracts.ICorePhysicalCardPopulationInitialCardCountAccess
{
    private int _initialCardCount = default !;
    internal ref int ReferenceToInitialCardCount => ref _initialCardCount;

    ref int Contracts.ICorePhysicalCardPopulationInitialCardCountAccess._initialCardCount => ref _initialCardCount;
}
