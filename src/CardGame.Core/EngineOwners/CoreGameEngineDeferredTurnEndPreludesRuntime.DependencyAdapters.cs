#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CardsMovedWindowRuntime))]
internal sealed class CoreDeferredTurnEndPreludesCardsMovedWindowAdapter(Func<CardsMovedWindowRuntime> targetFactory) : ICoreDeferredTurnEndPreludesCardsMovedWindowOperations
{
    private readonly Func<CardsMovedWindowRuntime> _targetFactory = targetFactory;
    public CardsMovedWindowRuntime Get() => this._targetFactory();
}
