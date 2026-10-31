#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupAvailableGeneralsSelectionOrderAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> _availableGenerals { get; }

    ref readonly global::System.Collections.Generic.List<int> _selectionOrder { get; }
}
