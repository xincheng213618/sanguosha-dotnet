#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardActionsCardActionSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _cardActionSequence { get; }
}
