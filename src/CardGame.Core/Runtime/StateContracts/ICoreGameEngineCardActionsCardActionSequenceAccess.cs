#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardActionsCardActionSequenceAccess : IProgramRuntimeOperations
{
    ref long _cardActionSequence { get; }
}
