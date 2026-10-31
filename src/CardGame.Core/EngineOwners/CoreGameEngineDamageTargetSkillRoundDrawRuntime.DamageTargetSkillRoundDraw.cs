#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetSkillRoundDrawRuntime
{
    internal static ProgramSkillGrantIdentity RoundSkillIdentity(SkillGrant grant) => RuntimeSkills.RuntimeRules_Core_DamageTargetSkillRoundDraw.RoundSkillIdentity(grant);
    internal static bool SameRoundSkillIdentity(SkillGrant grant, ProgramSkillGrantIdentity identity) => RuntimeSkills.RuntimeRules_Core_DamageTargetSkillRoundDraw.SameRoundSkillIdentity(grant, identity);
    internal IEnumerable<SkillGrant> OwnedAndRestorableRoundSkillGrants(CharacterState owner, string skillId)
    {
        foreach (var grant in owner.SkillGrants.Grants.Where(g => g.SkillId == skillId))
            yield return grant;
        foreach (var lease in _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess._orderedPrintedSkillLossLeases.Where(l => l.Source.OwnerSeat == owner.Seat))
        {
            var general = lease.TemplateSourceId == CharacterState.SecondarySkillSource ? owner.SecondaryGeneral : owner.General;
            if (general?.Id != lease.GeneralId)
                continue;
            foreach (var grant in lease.RemovedGrants.Where(g => g.SkillId == skillId))
            {
                var loss = _coreProgramEventHistoryOperations.ProgramEventHistory<OrderedPrintedSkillGrantRemovedEvent>().Single(e => e.FrameId == lease.ProgramFrameId && e.Grant == grant);
                if (!_coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(owner.Seat, grant, loss) && !_coreRandomDamageTargetSkillsOperations.RandomDamageTargetSkillGrantExpired(grant))
                    yield return grant;
            }
        }

        foreach (var loss in _coreHellSoulCardsOperations.HellSoulOutstandingLosses(owner.Seat))
            if (loss.Grant.SkillId == skillId && owner.General.Id == loss.GeneralId && owner.SecondaryGeneral?.Id == loss.SecondaryGeneralId && !_coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(owner.Seat, loss.Grant, loss) && loss.Grant.PhaseExpiry is null && !_coreRandomDamageTargetSkillsOperations.RandomDamageTargetSkillGrantExpired(loss.Grant))
                yield return loss.Grant;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1247)]
    internal bool CanOfferDamageTargetRoundSkill(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => !GrantDamageTargetSkillAndDrawByRoundDescriptor.IsTrigger(trigger) || context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target } && source == ownerSeat && source != target && _coreMatchParticipantsPlayersAccess._players[source].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[target].TeamRevealed && _coreTeamActivityProgramsOperations.PublicTeamEnemy(source, _coreMatchParticipantsPlayersAccess._players[target]) && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && !_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window).IsSourceLess;
    internal SkillProgramStepOutcome GrantDamageTargetSkillAndDrawByRound(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || frame.DamageTargetSkillRoundDraw is not null || !GrantDamageTargetSkillAndDrawByRoundDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(frame)) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(frame, out var window, out var damage, out var attack) || attack.IsSourceLess || attack.SourceSeat != frame.OwnerSeat || attack.DamageAmount <= 0 || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].TeamRevealed || !_coreMatchParticipantsPlayersAccess._players[damage.TargetSeat].TeamRevealed || !_coreTeamActivityProgramsOperations.PublicTeamEnemy(frame.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[damage.TargetSeat]))
            throw new InvalidOperationException("A damage target skill grant requires its exact positive native source segment and public enemy.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[damage.TargetSeat];
        if (!owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var effect = _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects.Single();
        var skill = effect.SkillIds.Single();
        _ = _contentRegistryOperations._contentRegistry.GetSkill(skill);
        var existing = target.IsAlive ? OwnedAndRestorableRoundSkillGrants(target, skill).OrderBy(g => g.GrantId, StringComparer.Ordinal).FirstOrDefault() : null;
        var sourceId = $"acquired:damage-target-round:{(frame.OwnerSeat)}:{(frame.SkillId)}:{(frame.SkillInstanceId)}:{(frame.Id)}";
        var grant = existing is null && target.IsAlive ? new SkillGrant(sourceId + ":" + skill, skill, sourceId + ":" + skill, sourceId) : null;
        var receipt = new DamageTargetSkillRoundDrawReceipt(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, window.Id, damage.Id, target.Seat, damage.Amount, damage.Nature, _coreActualRoundProgressRoundNumberAccess._roundNumber, Math.Min(effect.Amount, Math.Max(0, _coreActualRoundProgressRoundNumberAccess._roundNumber)), skill, grant is null ? null : RoundSkillIdentity(grant), existing is null ? null : RoundSkillIdentity(existing))
        {
            TargetDeadAtGrant = !target.IsAlive
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DamageTargetSkillRoundDraw = receipt });
        if (grant is not null)
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
            target.SkillGrants.Grant(grant);
            _coreConversionStatesOperations.RegisterTaggedConversionSkill(target, skill);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetSkillRoundGrantedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.DamageWindowId, receipt.DamageFrameId, receipt.TargetSeat, receipt.DamageAmount, receipt.Nature, receipt.RoundNumber, receipt.DrawCount, skill, receipt.GrantedGrant, receipt.ExistingGrant, receipt.TargetDeadAtGrant));
        if (grant is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(target.Seat, frame.SkillId, Array.AsReadOnly(new[] { skill })));
        ResumeDamageTargetSkillRoundDraw(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1247)]
    internal bool ResumeDamageTargetSkillRoundDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DamageTargetSkillRoundDraw: { } r } frame || frame.Id != id)
            return false;
        RequireDamageTargetSkillRoundDraw(frame);
        if (r.Stage == DamageTargetSkillRoundDrawStage.DrawAttempt)
            throw new InvalidOperationException("A round draw must return through its owned native request.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = frame.DamageTargetSkillRoundDraw!;
        if (r.Stage == DamageTargetSkillRoundDrawStage.GrantChildren && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None && r.DrawCount > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DamageTargetSkillRoundDraw = r with { Stage = DamageTargetSkillRoundDrawStage.DrawAttempt }, PendingMovementContinuation = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetSkillRoundDrawRequestedEvent(id, frame.OwnerSeat, r.DrawCount));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, r.DrawCount, CoreGameEngineDamageTargetSkillRoundDrawDefinitions.DamageTargetSkillRoundDrawReason);
            return true;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetSkillRoundCompletedEvent(id, r.DrawOutcome is not null, r.Cancelled || r.Stage == DamageTargetSkillRoundDrawStage.GrantChildren && (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 1247)]
    internal bool ReturnDamageTargetSkillRoundMovement(ProgramSkillFrame frame) => frame.DamageTargetSkillRoundDraw is not null && ResumeDamageTargetSkillRoundDraw(frame.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.GrantDamageTargetSkillAndDrawByRound)]
    internal void ReturnDamageTargetSkillRoundDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        RequireDamageTargetSkillRoundDraw(frame);
        var r = frame.DamageTargetSkillRoundDraw!;
        var cancelled = !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DamageTargetSkillRoundDraw = r with { Stage = DamageTargetSkillRoundDrawStage.DrawChildren, DrawOutcome = outcome, Cancelled = cancelled }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetSkillRoundDrawPaidEvent(frame.Id, frame.OwnerSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId, outcome.Replacement is not null, cancelled));
        ResumeDamageTargetSkillRoundDraw(frame.Id);
    }

    internal void RequireDamageTargetSkillRoundDraw(ProgramSkillFrame frame)
    {
        if (frame.DamageTargetSkillRoundDraw is not { } r || r.ProgramFrameId != frame.Id || frame.InstructionIndex != 1 || r.Source != new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || r.GameplayHash != frame.GameplayHash || !GrantDamageTargetSkillAndDrawByRoundDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(frame)) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(frame, out var window, out var damage, out var attack) || attack.IsSourceLess || attack.SourceSeat != frame.OwnerSeat || r.DamageWindowId != window.Id || r.DamageFrameId != damage.Id || r.TargetSeat != damage.TargetSeat || r.DamageAmount != damage.Amount || r.Nature != damage.Nature || r.DamageAmount <= 0 || r.RoundNumber < 0 || r.DrawCount != Math.Min(Math.Max(0, r.RoundNumber), _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects[0].Amount) || r.GrantedSkillId != _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects[0].SkillIds.Single() || !Enum.IsDefined(r.Stage) || r.GrantedGrant is { } grant && (grant.SkillId != r.GrantedSkillId || grant.SourceId != $"acquired:damage-target-round:{(frame.OwnerSeat)}:{(frame.SkillId)}:{(frame.SkillInstanceId)}:{(frame.Id)}" || grant.GrantId != grant.SourceId + ":" + r.GrantedSkillId || grant.SkillInstanceId != grant.GrantId || r.ExistingGrant is not null) || r.ExistingGrant is { } prior && prior.SkillId != r.GrantedSkillId || (r.TargetDeadAtGrant ? r.GrantedGrant is not null || r.ExistingGrant is not null : (r.GrantedGrant is null) == (r.ExistingGrant is null)) || _coreProgramEventHistoryOperations.ProgramEventHistory<DamageTargetSkillRoundGrantedEvent>().Count(e => e == new DamageTargetSkillRoundGrantedEvent(frame.Id, r.Source, r.GameplayHash, r.DamageWindowId, r.DamageFrameId, r.TargetSeat, r.DamageAmount, r.Nature, r.RoundNumber, r.DrawCount, r.GrantedSkillId, r.GrantedGrant, r.ExistingGrant, r.TargetDeadAtGrant)) != 1)
            throw new InvalidOperationException("A damage target skill/round draw lost its exact paid source, segment or grant identity.");
        var requests = _coreProgramEventHistoryOperations.ProgramEventHistory<DamageTargetSkillRoundDrawRequestedEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray();
        if (r.Stage == DamageTargetSkillRoundDrawStage.GrantChildren)
        {
            if (requests.Length != 0 || r.DrawOutcome is not null || frame.NativeDraw is not null)
                throw new InvalidOperationException("An unrequested round draw cannot have a native payment.");
            return;
        }

        if (requests is not [var request] || request.OwnerSeat != frame.OwnerSeat || request.RequestedCount != r.DrawCount || r.DrawCount <= 0)
            throw new InvalidOperationException("A round draw changed its once-frozen request.");
        if (r.Stage == DamageTargetSkillRoundDrawStage.DrawAttempt)
        {
            if (r.DrawOutcome is not null || frame.PendingMovementContinuation is not null || frame.NativeDraw is { } native && (native.Operation != SkillProgramEffectOp.GrantDamageTargetSkillAndDrawByRound || native.ReturnKind != NativeDrawAttemptReturnKind.ProgramOperation || native.RequestedCount != r.DrawCount || !native.Recipients.SequenceEqual([frame.OwnerSeat]) || native.Reason != CoreGameEngineDamageTargetSkillRoundDrawDefinitions.DamageTargetSkillRoundDrawReason))
                throw new InvalidOperationException("A round draw lost its exact outstanding native attempt.");
            return;
        }

        if (r.DrawOutcome is not { } outcome || frame.NativeDraw is not null || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != r.DrawCount || outcome.SequenceBefore > outcome.SequenceAfter || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Count > r.DrawCount || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || (r.Cancelled || outcome.Replacement is not null) && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason == CoreGameEngineDamageTargetSkillRoundDrawDefinitions.DamageTargetSkillRoundDrawReason).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds) || _coreProgramEventHistoryOperations.ProgramEventHistory<DamageTargetSkillRoundDrawPaidEvent>().Count(e => e == new DamageTargetSkillRoundDrawPaidEvent(frame.Id, frame.OwnerSeat, r.DrawCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId, outcome.Replacement is not null, r.Cancelled)) != 1 || outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frame.Id && e.RecipientSeat == frame.OwnerSeat && e.RequestedCount == r.DrawCount && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("A round draw lost its exact actual native materials or paid outcome.");
    }

    internal bool ValidDamageTargetSkillRoundDraw(ProgramSkillFrame frame)
    {
        RequireDamageTargetSkillRoundDraw(frame);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 1247)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1247)]
    internal bool ContinuePaidDamageTargetRoundSkill(ProgramSkillFrame frame)
    {
        if (frame.DamageTargetSkillRoundDraw is null)
            return false;
        RequireDamageTargetSkillRoundDraw(frame);
        return true;
    }
    SkillProgramStepOutcome IDamageTargetSkillRoundDrawHost.GrantDamageTargetSkillAndDrawByRound(ProgramSkillFrame frame) => GrantDamageTargetSkillAndDrawByRound(frame);
}
