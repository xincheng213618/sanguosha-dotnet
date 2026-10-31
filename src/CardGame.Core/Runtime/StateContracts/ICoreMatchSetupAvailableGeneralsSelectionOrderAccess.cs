#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupAvailableGeneralsSelectionOrderAccess : IProgramRuntimeOperations
{
    ref readonly List<GeneralDefinition> _availableGenerals { get; }

    ref readonly List<int> _selectionOrder { get; }
}
