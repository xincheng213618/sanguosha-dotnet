#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess : IProgramRuntimeOperations
{
    ref int? _modeInitialActorSeat { get; }

    ref int _selectionIndex { get; }

    ref readonly List<int> _selectionOrder { get; }

    ref readonly List<GeneralDefinition> _availableGenerals { get; }

    ref bool _setupComplete { get; }
}
