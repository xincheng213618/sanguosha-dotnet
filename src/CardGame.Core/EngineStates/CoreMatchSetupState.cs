#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchSetupState : Contracts.ICoreMatchSetupSetupCompleteAccess, Contracts.ICoreMatchSetupAvailableGeneralsAccess, Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess, Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess, Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess, Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess
{
    private List<GeneralDefinition> _availableGenerals = [];
    internal ref readonly List<GeneralDefinition> ReferenceToAvailableGenerals => ref _availableGenerals;

    private bool _gameStartingProgramsResolved = default !;
    internal ref bool ReferenceToGameStartingProgramsResolved => ref _gameStartingProgramsResolved;

    private int? _modeInitialActorSeat = default !;
    internal ref int? ReferenceToModeInitialActorSeat => ref _modeInitialActorSeat;

    private int _selectionIndex = default !;
    internal ref int ReferenceToSelectionIndex => ref _selectionIndex;

    private List<int> _selectionOrder = [];
    internal ref readonly List<int> ReferenceToSelectionOrder => ref _selectionOrder;

    private bool _setupComplete = default !;
    internal ref bool ReferenceToSetupComplete => ref _setupComplete;

    ref bool Contracts.ICoreMatchSetupSetupCompleteAccess._setupComplete => ref _setupComplete;

    ref readonly List<GeneralDefinition> Contracts.ICoreMatchSetupAvailableGeneralsAccess._availableGenerals => ref _availableGenerals;

    ref bool Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess._gameStartingProgramsResolved => ref _gameStartingProgramsResolved;

    ref int? Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._modeInitialActorSeat => ref _modeInitialActorSeat;

    ref int Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionIndex => ref _selectionIndex;

    ref readonly List<int> Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionOrder => ref _selectionOrder;

    ref readonly List<GeneralDefinition> Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._availableGenerals => ref _availableGenerals;

    ref bool Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._setupComplete => ref _setupComplete;

    ref readonly List<GeneralDefinition> Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess._availableGenerals => ref _availableGenerals;

    ref readonly List<int> Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess._selectionOrder => ref _selectionOrder;

    ref bool Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._setupComplete => ref _setupComplete;

    ref bool Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._gameStartingProgramsResolved => ref _gameStartingProgramsResolved;
}
