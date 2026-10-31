#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal static SkillRuleQuery? DuFuRenPhaseQuery(ActualDiscardRecoveryPhaseKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_du_fu_ren_ol_DuFuRenPhases.DuFuRenPhaseQuery(kind);
    internal bool DuFuRenGrantConsumed(long id, ActualDiscardRecoveryPhaseKind kind) => _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenPhaseIssuedEvent>().Any(e => e.GrantId == id && e.Phase.Kind == kind);
    internal void ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind kind, int ownerSeat)
    {
        if (!HasDuFuRenShunshi || DuFuRenPhaseQuery(kind)is null)
            return;
        if (_coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || phase.Kind != kind || phase.ActorSeat != ownerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            throw new InvalidOperationException("Shunshi can only consume the current confirmed actual phase token: " + System.Text.Json.JsonSerializer.Serialize(new { Requested = kind, OwnerSeat = ownerSeat, Alive = _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive, Winner = _coreMatchOutcomeStatusWinnerAccess._winner, Status = _coreMatchOutcomeStatusWinnerAccess._status, Phase = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase, Turn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, ActualOwner = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, CurrentSeat = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, LastActual = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Where(e => e is ActualDiscardRecoveryPhaseStartedEvent or ActualDiscardRecoveryPhaseRestoredEvent or ActualDiscardRecoveryPhaseClearedEvent).TakeLast(5).Select(e => new { Type = e.GetType().Name, Payload = System.Text.Json.JsonSerializer.Serialize(e, e.GetType()) }).ToArray() }));
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenPhaseConfirmedEvent>().Any(e => e.Phase == phase))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenPhaseConfirmedEvent(phase));
        foreach (var gift in _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenGiftIssuedEvent>().Where(e => e.Source.OwnerSeat == ownerSeat && !DuFuRenGrantConsumed(e.FrameId, kind)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenPhaseIssuedEvent(gift.FrameId, phase));
    }

    internal int DuFuRenPhaseBonus(int ownerSeat, SkillRuleQuery query)
    {
        if (!HasDuFuRenShunshi || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || phase.ActorSeat != ownerSeat || DuFuRenPhaseQuery(phase.Kind) != query)
            return 0;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenPhaseConfirmedEvent>().Any(e => e.Phase == phase))
            return _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenPhaseIssuedEvent>().Count(e => e.Phase == phase);
        // A native Draw-starting instruction can freeze this count before the
        // ordinary draw is confirmed. Shunshi grants only its owner, and actual
        // own-turn damage cannot create another off-turn gift in this window.
        return query == SkillRuleQuery.DrawCount ? _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenGiftIssuedEvent>().Count(e => e.Source.OwnerSeat == ownerSeat && !DuFuRenGrantConsumed(e.FrameId, phase.Kind)) : 0;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 26521)]
    internal void ConfirmDuFuRenPlayPhase(IGameEvent payload)
    {
        if (HasDuFuRenShunshi && payload is ActualDiscardRecoveryPhaseStartedEvent { Phase.Kind: ActualDiscardRecoveryPhaseKind.Play } e)
            ConsumeDuFuRenPhase(e.Phase.Kind, e.Phase.ActorSeat);
    }
}
