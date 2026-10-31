#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossRuntime
{
    internal static bool IsOrderedPrintedSkillLossTrigger(SkillProgramTrigger trigger) => RuntimeSkills.RuntimeRules_Core_OrderedPrintedSkillLoss.IsOrderedPrintedSkillLossTrigger(trigger);
    internal long OrderedPrintedLossMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static string OrderedPrintedLossDrawReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_OrderedPrintedSkillLoss.OrderedPrintedLossDrawReason(f);
    internal int? OrderedPrintedLossActualTurnOwner() => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) ? _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat : null;
    internal bool CanOfferOrderedPrintedSkillLoss(int ownerSeat, SkillProgramTrigger trigger, GameEngine.IDamageAttempt attack) => !IsOrderedPrintedSkillLossTrigger(trigger) || _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && OrderedPrintedLossActualTurnOwner()is not null && attack.DamageWasApplied && attack.DamageAmount > 0 && (ownerSeat == attack.TargetSeat || !attack.IsSourceLess && ownerSeat == attack.SourceSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 980)]
    internal bool CanOfferOrderedPrintedSkillLoss(int ownerSeat, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !IsOrderedPrintedSkillLossTrigger(trigger) || _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && OrderedPrintedLossActualTurnOwner()is not null && context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } && context.OwnerSeat == ownerSeat && (context.SourceSeat == ownerSeat || context.TargetSeat == ownerSeat);
    internal (string SourceId, GeneralDefinition General) OrderedPrintedLossTemplate(ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var origin = owner.SkillGrants.Grants.FirstOrDefault(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId && g.SourceId is CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource);
        return origin?.SourceId == CharacterState.SecondarySkillSource && owner.SecondaryGeneral is { } secondary ? (CharacterState.SecondarySkillSource, secondary) : (CharacterState.PrimarySkillSource, owner.General);
    }

    internal SkillProgramStepOutcome ExecuteOrderedPrintedSkillLoss(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.OrderedPrintedSkillLoss is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !IsOrderedPrintedSkillLossTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)) || !CanOfferOrderedPrintedSkillLoss(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f), context) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out var attack) || OrderedPrintedLossActualTurnOwner()is not { } turnOwner)
            throw new InvalidOperationException("Ordered printed-skill loss requires its exact positive damage participant and ongoing actual turn.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var(templateSource, general) = OrderedPrintedLossTemplate(f);
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(f.OwnerSeat);
        var currentGrants = owner.SkillGrants.Grants;
        // Removed printed skills no longer occupy a currently owned slot. Keep
        // the original card order; neither grant IDs nor skill IDs define it.
        var lostIds = general.Skills.Where(s => s.ContentId is not null && currentGrants.Any(g => g.SkillId == s.ContentId && !g.IsModeRule)).Select(s => s.ContentId!).Distinct(StringComparer.Ordinal).Take(Math.Max(0, range)).ToArray();
        var grants = currentGrants.Where(g => !g.IsModeRule && lostIds.Contains(g.SkillId, StringComparer.Ordinal)).ToArray();
        var lease = new OrderedPrintedSkillLossLease
        {
            ProgramFrameId = f.Id,
            Source = new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            ActualTurnNumber = _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber,
            ActualTurnOwnerSeat = turnOwner,
            TemplateSourceId = templateSource,
            GeneralId = general.Id,
            FrozenAttackRange = range,
            LostSkillIds = lostIds,
            RemovedGrants = grants
        };
        var receipt = new ProgramOrderedPrintedSkillLossReceipt
        {
            InstructionIndex = 1,
            DamageWindowId = window.Id,
            DamageFrameId = damage.Id,
            AttackFrameId = damage.ParentFrameId,
            DamageSourceSeat = attack.IsSourceLess ? null : attack.SourceSeat,
            DamageTargetSeat = attack.TargetSeat,
            DamageAmount = attack.DamageAmount,
            DamageNature = _coreNativeCardClassificationsOperations.GetDamageNature(attack),
            SourceLess = attack.IsSourceLess,
            Lease = lease,
            Stage = OrderedPrintedSkillLossStage.LossChildren
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrderedPrintedSkillLoss = receipt, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        // A first ownership change must be observable even before any earlier
        // program has initialized the revision baseline.
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        foreach (var grant in grants)
            if (!owner.SkillGrants.RemoveGrant(grant.GrantId))
                throw new InvalidOperationException("An ordered skill-loss payment changed before its atomic removal.");
        _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess._orderedPrintedSkillLossLeases.Add(lease);
        _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess._orderedPrintedSkillLossStateStarted = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillLossIssuedEvent(f.Id, lease.Source, lease.GameplayHash, lease.ActualTurnNumber, lease.ActualTurnOwnerSeat, templateSource, general.Id, range, lostIds.Length, grants.Length));
        for (var ordinal = 0; ordinal < lostIds.Length; ordinal++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillLostEvent(f.Id, ordinal, lostIds[ordinal]));
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillGrantRemovedEvent(f.Id, grant));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 120)]
    internal bool ResumeOrderedPrintedSkillLoss(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.OrderedPrintedSkillLoss is not { } r)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        if (r.Stage == OrderedPrintedSkillLossStage.LossChildren)
        {
            var before = OrderedPrintedLossMovementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrderedPrintedSkillLoss = r with { Stage = OrderedPrintedSkillLossStage.DrawChildren, DrawIssued = true, DrawBefore = before, DrawAfter = before } });
            var actual = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], r.Lease.LostSkillIds.Count, true, new(OrderedPrintedLossDrawReason(f)), r.Lease.Source).Count : 0;
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            var after = OrderedPrintedLossMovementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { OrderedPrintedSkillLoss = active.OrderedPrintedSkillLoss!with { DrawActual = actual, DrawAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillLossDrawIssuedEvent(id, r.Lease.LostSkillIds.Count, actual, before, after));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage != OrderedPrintedSkillLossStage.DrawChildren)
            throw new InvalidOperationException("An ordered skill-loss continuation completed twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrderedPrintedSkillLoss = r with { Stage = OrderedPrintedSkillLossStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillLossCompletedEvent(id, r.Lease.LostSkillIds.Count, r.DrawActual));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 100)]
    internal bool ReturnOrderedPrintedSkillLossMovement(ProgramSkillFrame f)
    {
        if (f.OrderedPrintedSkillLoss is null)
            return false;
        if (f.OrderedPrintedSkillLoss.Stage != OrderedPrintedSkillLossStage.DrawChildren || f.PendingMovementContinuation is null)
            throw new InvalidOperationException("An ordered skill-loss movement returned outside its once-issued Draw.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 570)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 590)]
    internal bool CanContinueOrderedPrintedSkillLoss(ProgramSkillFrame f) => f.OrderedPrintedSkillLoss is not null && _coreOrderedPrintedSkillLossObserversOperations.ValidOrderedPrintedSkillLossReceipt(f);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 40)]
    internal void RestoreOrderedPrintedSkillsAfterTurnEnded(TurnEndedEvent ended) => RestoreOrderedPrintedSkillsAtTurnEnd(ended.TurnNumber, ended.ActorSeat);
    internal void RestoreOrderedPrintedSkillsAtTurnEnd(int turnNumber, int turnOwnerSeat)
    {
        var due = _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess._orderedPrintedSkillLossLeases.Where(l => l.ActualTurnNumber == turnNumber && l.ActualTurnOwnerSeat == turnOwnerSeat).ToArray();
        if (due.Length == 0)
            return;
        if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.TurnNumber == turnNumber && e.ActorSeat == turnOwnerSeat))
            throw new InvalidOperationException("Ordered printed skills return only after their exact actual TurnEnded fact.");
        var decisions = new List<(OrderedPrintedSkillLossLease Lease, SkillGrant Grant, bool Restore, string Outcome)>();
        foreach (var lease in due)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[lease.Source.OwnerSeat];
            var general = lease.TemplateSourceId == CharacterState.SecondarySkillSource ? owner.SecondaryGeneral : owner.General;
            foreach (var grant in lease.RemovedGrants)
            {
                var lossFact = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OrderedPrintedSkillGrantRemovedEvent>().Single(e => e.FrameId == lease.ProgramFrameId && e.Grant == grant);
                var outcome = _coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(owner.Seat, grant, lossFact) ? "permanently-lost" : !owner.IsAlive ? "owner-dead" : general?.Id != lease.GeneralId ? "template-changed" : (grant.TurnExpiry is { } expiry && expiry.TurnNumber <= turnNumber || _coreNextActualTurnStartSkillGrantsOperations.HasNextActualTurnStartSkillGrantExpired(grant)) ? "grant-expired" : "restored";
                if (outcome == "restored" && owner.SkillGrants.Grants.SingleOrDefault(g => g.GrantId == grant.GrantId)is { } existing)
                {
                    if (existing != grant)
                        throw new InvalidOperationException("A returning ordered printed grant conflicts with a newly issued identity.");
                    outcome = "already-present";
                }

                decisions.Add((lease, grant, outcome is "restored" or "already-present", outcome));
            }
        }

        // Validate every identity before restoring any source. A source acquired
        // during the loss is independent and is never overwritten or removed.
        foreach (var lease in due)
            _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess._orderedPrintedSkillLossLeases.Remove(lease);
        foreach (var d in decisions.Where(d => d.Outcome == "restored"))
            _coreMatchParticipantsPlayersAccess._players[d.Lease.Source.OwnerSeat].SkillGrants.Grant(d.Grant);
        foreach (var d in decisions)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillGrantRestoredEvent(d.Lease.ProgramFrameId, d.Lease.Source.OwnerSeat, d.Grant, d.Restore, d.Outcome));
        foreach (var lease in due)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrderedPrintedSkillLossExpiredEvent(lease.ProgramFrameId, lease.Source.OwnerSeat, turnNumber, turnOwnerSeat, decisions.Count(d => d.Lease == lease && d.Restore), decisions.Count(d => d.Lease == lease && !d.Restore)));
    }

    SkillProgramStepOutcome IOrderedPrintedSkillLossProgramHost.ExecuteOrderedPrintedSkillLoss(ProgramSkillFrame frame) => ExecuteOrderedPrintedSkillLoss(frame);
}
