#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupSetupCompleteAccess : IProgramRuntimeOperations
{
    ref bool _setupComplete { get; }
}
