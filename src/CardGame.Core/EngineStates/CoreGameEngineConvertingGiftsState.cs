#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineConvertingGiftsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GiftHandRetentionObligation> _giftHandRetentionObligations = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GiftHandRetentionObligation> ReferenceToGiftHandRetentionObligations => ref _giftHandRetentionObligations;

    private long _giftHandRetentionSequence = default !;
    internal ref long ReferenceToGiftHandRetentionSequence => ref _giftHandRetentionSequence;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionSequence => ref _giftHandRetentionSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GiftHandRetentionObligation> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations => ref _giftHandRetentionObligations;
}
