#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int? _modeInitialActorSeat { get; }

    ref int _selectionIndex { get; }

    ref readonly global::System.Collections.Generic.List<int> _selectionOrder { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> _availableGenerals { get; }

    ref bool _setupComplete { get; }
}
