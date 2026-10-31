#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCardActionsState : Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess
{
    private long _cardActionSequence = default !;
    internal ref long ReferenceToCardActionSequence => ref _cardActionSequence;

    ref long Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence => ref _cardActionSequence;
}
