#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupAvailableGeneralsAccess : IProgramRuntimeOperations
{
    ref readonly List<GeneralDefinition> _availableGenerals { get; }
}
