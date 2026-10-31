#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    int IGuanYuTargetSelectionQuery.RoundNumber => _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber;

    long? IGuanYuTargetSelectionQuery.CurrentProgramFrameId => (_resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame)?.Id;

    IReadOnlyList<ProgramTargetParticipantFacts> IGuanYuTargetSelectionQuery.Participants => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => new ProgramTargetParticipantFacts(player.Seat, player.IsAlive, player.HasHpValue, player.Hp, player.MaxHp)).ToArray());

    IReadOnlyList<DemonRoundFocusEvent> IGuanYuTargetSelectionQuery.FocusHistory => Array.AsReadOnly(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonRoundFocusEvent>().ToArray());

    IReadOnlyList<DemonSupportGrantedEvent> IGuanYuTargetSelectionQuery.SupportHistory => Array.AsReadOnly(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonSupportGrantedEvent>().ToArray());
}
