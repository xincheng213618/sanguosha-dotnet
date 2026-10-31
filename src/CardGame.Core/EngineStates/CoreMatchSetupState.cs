#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchSetupState : global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> _availableGenerals = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> ReferenceToAvailableGenerals => ref _availableGenerals;

    private bool _gameStartingProgramsResolved = default !;
    internal ref bool ReferenceToGameStartingProgramsResolved => ref _gameStartingProgramsResolved;

    private int? _modeInitialActorSeat = default !;
    internal ref int? ReferenceToModeInitialActorSeat => ref _modeInitialActorSeat;

    private int _selectionIndex = default !;
    internal ref int ReferenceToSelectionIndex => ref _selectionIndex;

    private global::System.Collections.Generic.List<int> _selectionOrder = [];
    internal ref readonly global::System.Collections.Generic.List<int> ReferenceToSelectionOrder => ref _selectionOrder;

    private bool _setupComplete = default !;
    internal ref bool ReferenceToSetupComplete => ref _setupComplete;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess._setupComplete => ref _setupComplete;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsAccess._availableGenerals => ref _availableGenerals;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess._gameStartingProgramsResolved => ref _gameStartingProgramsResolved;

    ref int? global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._modeInitialActorSeat => ref _modeInitialActorSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionIndex => ref _selectionIndex;

    ref readonly global::System.Collections.Generic.List<int> global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionOrder => ref _selectionOrder;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._availableGenerals => ref _availableGenerals;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._setupComplete => ref _setupComplete;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GeneralDefinition> global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess._availableGenerals => ref _availableGenerals;

    ref readonly global::System.Collections.Generic.List<int> global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess._selectionOrder => ref _selectionOrder;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._setupComplete => ref _setupComplete;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._gameStartingProgramsResolved => ref _gameStartingProgramsResolved;
}
