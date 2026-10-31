#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _giftHandRetentionSequence { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GiftHandRetentionObligation> _giftHandRetentionObligations { get; }
}
