#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCardActionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess
{
    private long _cardActionSequence = default !;
    internal ref long ReferenceToCardActionSequence => ref _cardActionSequence;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence => ref _cardActionSequence;
}
