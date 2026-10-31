#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnSkillGrantsRuntime
{
    internal void GrantProgramTurnSkills(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<string> skillIds, SkillProgramTurnSkillExpiry? expiry)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(active);
        if (active != frame || active.InstructionIndex <= 0 || active.PendingMovementContinuation is not null || active.SelectedCardPayment is not null && active.SelectedCardPaymentResult is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || expiry is not null && !Enum.IsDefined(expiry.Value))
            throw new InvalidOperationException("A participant turn grant lost its exact active instruction or recipient.");
        var effect = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!).GetPausedInstruction(active.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.GrantTurnSkills || effect.TurnSkillExpiry != expiry || !effect.SkillIds.SequenceEqual(skillIds, StringComparer.Ordinal) || targetSeat != (effect.TargetReference is { } reference ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(active, reference) : active.OwnerSeat))
            throw new InvalidOperationException("A participant turn grant no longer matches its configured operation.");
        if (effect.TargetReference is { Kind: ProgramParticipantRef.Actor })
            ValidateTurnSkillGrantActor(active, targetSeat);
        else if (effect.TargetReference is not null)
            throw new InvalidOperationException("A turn skill grant requires its owner or exact card-action actor.");
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase is TurnPhase.NotStarted or TurnPhase.Finished || !_coreProgramLifecycleOperations.IsEndOwnedExtraPlayPhase() && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(ended => ended.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && ended.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat))
            throw new InvalidOperationException("A turn skill grant requires an actual ongoing turn.");
        // A paid movement child may have killed its recipient. The payment stays paid;
        // a dead participant receives no new capability and opens no second cost window.
        if (!_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId))
            return;
        var recipient = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var granted = skillIds.Distinct(StringComparer.Ordinal).ToArray();
        foreach (var skillId in granted)
            _ = _contentRegistryOperations._contentRegistry.GetSkill(skillId);
        var bindingId = global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active);
        var sourceId = $"turn:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)}:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)}:donor:{(active.OwnerSeat)}:{(active.SkillId)}:" + $"{(active.SkillInstanceId)}:{(bindingId)}:frame:{(active.Id)}:effect:{(active.InstructionIndex - 1)}";
        var turnExpiry = expiry == SkillProgramTurnSkillExpiry.ActualTurnEnd ? new SkillGrantTurnExpiry(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat) : null;
        var nextStartExpiry = expiry == SkillProgramTurnSkillExpiry.RecipientNextActualTurnStart ? new SkillGrantNextActualTurnStartExpiry(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, recipient.Seat) : null;
        if (nextStartExpiry is not null)
        {
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
            if (history.OfType<TurnStartedEvent>().Count(started => started.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && started.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat) != 1 || history.OfType<ProgramTurnSkillsGrantedEvent>().Any(issued => issued.GrantSourceId == sourceId))
                throw new InvalidOperationException("A next actual-turn-start skill grant requires its real creation turn and once-issued instruction.");
        }

        _coreProgramSkillChangesOperations.CommitProgramParticipantSkillGrants(active, targetSeat, granted, sourceId, turnExpiry, nextStartExpiry);
    }

    internal void ValidateTurnSkillGrantActor(ProgramSkillFrame frame, int targetSeat)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame parent || frame.WindowContext is not { CardUse: { } use } context || parent.Id != context.ParentFrameId || context.OwnerSeat != frame.OwnerSeat || context.Window != global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(parent) || use.ParentCardUseFrameId != parent.ParentFrameId || use.CardActionId != parent.Action.ActionId || use.ActorSeat != parent.Action.ActorSeat || use.EffectiveKind != parent.Action.EffectiveKind || targetSeat != use.ActorSeat || context.SourceSeat != use.ActorSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            throw new InvalidOperationException("A turn skill beneficiary requires its exact suspended card-action parent.");
        var candidate = parent.Candidates[parent.CandidateIndex];
        if (candidate.OwnerSeat != frame.OwnerSeat || candidate.SkillId != frame.SkillId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.TriggerId != frame.TriggerId || candidate.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("A turn skill beneficiary lost its exact card-action candidate.");
    }

    // The caller runs this only after queuing the matching actual TurnEnded fact,
    // before any deferred after-turn-ended program can observe an expired grant.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 30)]
    internal void ExpireActualTurnSkillGrantsAfterTurnEnded(TurnEndedEvent ended) => ExpireActualTurnSkillGrants(ended.TurnNumber, ended.ActorSeat);
    internal void ExpireActualTurnSkillGrants(int turnNumber, int turnOwnerSeat)
    {
        var expiry = new SkillGrantTurnExpiry(turnNumber, turnOwnerSeat);
        var due = _coreMatchParticipantsPlayersAccess._players.SelectMany(recipient => recipient.SkillGrants.Grants.Where(grant => grant.TurnExpiry == expiry).Select(grant => (Recipient: recipient, Grant: grant))).ToArray();
        if (due.Length == 0)
            return;
        if (turnNumber <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(turnOwnerSeat) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(ended => ended.TurnNumber == turnNumber && ended.ActorSeat == turnOwnerSeat))
            throw new InvalidOperationException("Actual-turn skill grants expire only after their exact turn-ended fact.");
        foreach (var(recipient, grant)in due)
        {
            recipient.SkillGrants.RemoveGrant(grant.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnSkillGrantExpiredEvent(recipient.Seat, grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.SourceId, grant.TurnExpiry!));
        }
    }
}
