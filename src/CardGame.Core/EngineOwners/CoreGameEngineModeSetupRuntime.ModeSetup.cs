#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeSetupRuntime
{
    internal IMatchModeSetupFeature ModeSetup => _modeRuntimeOperations._modeRuntime.Find<IMatchModeSetupFeature>() ?? throw new InvalidOperationException("The selected mode requires an explicitly installed setup feature.");
    internal MatchParticipantSetup ParticipantSetup => _participantSetupOperations._participantSetup ??= new(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionGeneralPoolAccess._generalPool);

    internal void ApplyModeSetupPlan(ModeSetupPlan plan) => ParticipantSetup.ApplyPlan(plan, _coreMatchParticipantsPlayersAccess._players, _coreMatchSetupAvailableGeneralsSelectionOrderAccess._availableGenerals, _coreMatchSetupAvailableGeneralsSelectionOrderAccess._selectionOrder, _kernelCoreGameEngineModeSetupOperations.CreateParticipantSetupContext());
    internal IReadOnlyList<ModeSeatSetupFacts> CaptureModeSetupSeats() => MatchParticipantSetup.CaptureSeats(_coreMatchParticipantsPlayersAccess._players, _kernelCoreGameEngineModeSetupOperations.CreateParticipantSetupContext());
    internal IReadOnlyList<(GeneralDefinition General, string CandidateId)> GetModeGeneralOffers(CharacterState player, bool fullRoster) => ParticipantSetup.GetOffers(ModeSetup, player, _coreMatchSetupAvailableGeneralsSelectionOrderAccess._availableGenerals, _coreMatchParticipantsPlayersAccess._players, fullRoster, _kernelCoreGameEngineModeSetupOperations.CreateParticipantSetupContext());
    internal void ApplyModeSelectedGeneral(CharacterState player, GeneralDefinition selected) => ParticipantSetup.ApplySelectedGeneral(ModeSetup, player, selected, _kernelCoreGameEngineModeSetupOperations.CreateParticipantSetupContext());
}
