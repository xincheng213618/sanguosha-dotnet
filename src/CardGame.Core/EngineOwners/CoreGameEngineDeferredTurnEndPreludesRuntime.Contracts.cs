#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDeferredTurnEndPreludesCardsMovedWindowOperations : IProgramRuntimeOperations
{
    CardsMovedWindowRuntime Get();
}
