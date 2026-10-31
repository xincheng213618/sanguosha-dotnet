#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess : IProgramRuntimeOperations
{
    ref long _giftHandRetentionSequence { get; }

    ref readonly List<GiftHandRetentionObligation> _giftHandRetentionObligations { get; }
}
