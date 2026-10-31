#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNextActualTurnStartSkillGrantsRuntime
{
    internal bool HasNextActualTurnStartSkillGrantExpired(SkillGrant grant) => grant.NextActualTurnStartExpiry is { } expiry && _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Any(started => started.ActorSeat == expiry.RecipientSeat && started.TurnNumber > expiry.GrantedTurnNumber);
    internal (CharacterState Recipient, SkillGrant Grant)[] NextActualTurnStartSkillGrantsDue(TurnStartedEvent started) => _coreMatchParticipantsPlayersAccess._players.OrderBy(player => player.Seat).SelectMany(recipient => recipient.SkillGrants.Grants.Where(grant => grant.NextActualTurnStartExpiry is { } expiry && expiry.RecipientSeat == recipient.Seat && recipient.Seat == started.ActorSeat && expiry.GrantedTurnNumber < started.TurnNumber).Select(grant => (Recipient: recipient, Grant: grant))).ToArray();
    internal static void RequireIssuedNextActualTurnStartSkillGrant(int recipientSeat, SkillGrant grant, IReadOnlyList<IGameEvent> history) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_NextActualTurnStartSkillGrants.RequireIssuedNextActualTurnStartSkillGrant(recipientSeat, grant, history);
    // No binding-backed state may carry an expired skill into this new turn.
    // Keep the receipt on the disabled owning grant until TurnStarted is queued;
    // then the post-fact observer removes it and issues the exact expiry fact.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 1)]
    internal void DisableNextActualTurnStartSkillGrants(TurnStartedEvent started)
    {
        var due = NextActualTurnStartSkillGrantsDue(started);
        if (due.Length == 0)
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (started.TurnNumber <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(started.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[started.ActorSeat].IsAlive || _resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Draw || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber != started.TurnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != started.ActorSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != started.TurnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != started.ActorSeat || history.OfType<TurnStartedEvent>().Any(fact => fact.TurnNumber == started.TurnNumber))
            throw new InvalidOperationException("A next-start skill disable requires the exact incoming real turn-start boundary.");
        foreach (var(recipient, grant)in due)
            RequireIssuedNextActualTurnStartSkillGrant(recipient.Seat, grant, history);
        // SetEnabled changes the binding-shard revision before projection and
        // derived-grant observers run. Already disabled leases still expire.
        foreach (var(recipient, grant)in due)
            recipient.SkillGrants.SetEnabled(grant.GrantId, false);
    }

    // Real extra turns and face-down skipped turns issue TurnStarted. Inserted
    // phases do not. The receipt belongs to each grant, independent of its donor.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 35)]
    internal void ExpireNextActualTurnStartSkillGrants(TurnStartedEvent started)
    {
        var due = NextActualTurnStartSkillGrantsDue(started);
        if (due.Length == 0)
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (started.TurnNumber <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(started.ActorSeat) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber != started.TurnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != started.ActorSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != started.TurnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != started.ActorSeat || history.OfType<TurnStartedEvent>().Count(fact => fact == started) != 1)
            throw new InvalidOperationException("A next-start skill expiry requires its exact real TurnStarted fact.");
        foreach (var(recipient, grant)in due)
        {
            RequireIssuedNextActualTurnStartSkillGrant(recipient.Seat, grant, history);
            if (grant.IsEnabled || !recipient.SkillGrants.RemoveGrant(grant.GrantId))
                throw new InvalidOperationException("A next-start skill expiry requires its disabled exact owning grant.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramNextActualTurnStartSkillGrantExpiredEvent(recipient.Seat, grant, started.TurnNumber, started.ActorSeat));
        }
    }
}
