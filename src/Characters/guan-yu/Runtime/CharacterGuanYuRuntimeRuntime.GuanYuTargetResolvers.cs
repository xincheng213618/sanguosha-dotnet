#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    int global::CardGame.Core.IGuanYuTargetSelectionQuery.RoundNumber => _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber;

    long? global::CardGame.Core.IGuanYuTargetSelectionQuery.CurrentProgramFrameId => (_resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame)?.Id;

    IReadOnlyList<ProgramTargetParticipantFacts> global::CardGame.Core.IGuanYuTargetSelectionQuery.Participants => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => new ProgramTargetParticipantFacts(player.Seat, player.IsAlive, player.HasHpValue, player.Hp, player.MaxHp)).ToArray());

    IReadOnlyList<DemonRoundFocusEvent> global::CardGame.Core.IGuanYuTargetSelectionQuery.FocusHistory => Array.AsReadOnly(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonRoundFocusEvent>().ToArray());

    IReadOnlyList<DemonSupportGrantedEvent> global::CardGame.Core.IGuanYuTargetSelectionQuery.SupportHistory => Array.AsReadOnly(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonSupportGrantedEvent>().ToArray());
}
