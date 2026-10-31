namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GiftHandRetentionObligation> _giftHandRetentionObligations => ref _convertingGifts.ReferenceToGiftHandRetentionObligations;
    private ref long _giftHandRetentionSequence => ref _convertingGifts.ReferenceToGiftHandRetentionSequence;
}
