#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineConvertingGiftsState : Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess
{
    private List<GiftHandRetentionObligation> _giftHandRetentionObligations = [];
    internal ref readonly List<GiftHandRetentionObligation> ReferenceToGiftHandRetentionObligations => ref _giftHandRetentionObligations;

    private long _giftHandRetentionSequence = default !;
    internal ref long ReferenceToGiftHandRetentionSequence => ref _giftHandRetentionSequence;

    ref long Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionSequence => ref _giftHandRetentionSequence;

    ref readonly List<GiftHandRetentionObligation> Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations => ref _giftHandRetentionObligations;
}
