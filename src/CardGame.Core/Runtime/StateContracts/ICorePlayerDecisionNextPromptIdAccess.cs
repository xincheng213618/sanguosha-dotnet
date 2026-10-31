#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICorePlayerDecisionNextPromptIdAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _nextPromptId { get; }
}
