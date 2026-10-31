#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICorePlayerDecisionNextPromptIdAccess : IProgramRuntimeOperations
{
    ref long _nextPromptId { get; }
}
