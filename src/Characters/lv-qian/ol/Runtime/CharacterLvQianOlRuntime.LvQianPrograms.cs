#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvQianOlRuntime
{
    internal SkillProgramStepOutcome ExecuteLvQian(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var w = f.WindowContext;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.LvQian is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Lv Qian cannot reopen its exact instruction.");
        if (op == SkillProgramEffectOp.RecordWeiluActualDamageSource)
        {
            if (w is not { SourceSeat: { } attacker, DamageFrameId: { } damage, Amount: > 0 } || attacker == f.OwnerSeat || w.TargetSeat != f.OwnerSeat || !LvQianSourceCurrent(LvQianSource(f), f.GameplayHash) || !ExactWeiluDamageParent(f, damage, attacker, w.Amount))
                throw new InvalidOperationException("Weilu requires its exact other-source actual applied damage.");
            var future = owner.WeiluFuture ?? [];
            var source = LvQianSource(f);
            if (!future.Any(g => g.Source == source && g.DamageSourceSeat == attacker && g.DueOwnTurn is null))
                owner.WeiluFuture = [..future, new(source, f.GameplayHash, attacker, damage, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiluFutureArmedEvent(source, f.GameplayHash, damage, attacker, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.DepositZengdaoActualEquipment)
        {
            if (f.TriggerId is not null || !LvQianSourceCurrent(LvQianSource(f), f.GameplayHash) || f.SelectedTargetSeats is not [var recipient] || recipient == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || f.SelectedCardIds.Count < 1 || f.SelectedCardIds.Distinct().Count() != f.SelectedCardIds.Count || f.SelectedCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Equipment(f.OwnerSeat) || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).IsGeneralWeapon))
                throw new InvalidOperationException("Zengdao requires actual selected equipment and one other recipient.");
            var source = LvQianSource(f);
            var pileInstance = $"{(source.OwnerSeat)}:{(source.SkillInstanceId)}";
            var key = (recipient, source.SkillId, pileInstance);
            if (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.ContainsKey(key))
                throw new InvalidOperationException("Zengdao cannot pay its limited deposit twice.");
            var pileId = CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(source.SkillId, pileInstance);
            var pile = new PublicPersistentPileSource(recipient, source.SkillId, pileInstance, int.MaxValue, pileId);
            _cardZonesOperations._cardZones.EnsurePublicPersistentPile(pile.Location);
            _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Add(key, pile);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var grant = new ZengdaoPublicGrant(source, f.GameplayHash, f.Id, recipient, pile.Location, before, before)
            {
                Entities = f.SelectedCardIds
            };
            var r = new LvQianReceipt(source, f.GameplayHash, 1, 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, LvQianStage.DepositChildren, Before: before, After: before, Grant: grant)
            {
                Targets = [recipient],
                Entities = f.SelectedCardIds
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            IssueLvQianStarted(f);
            _corePhysicalMovementFacadesOperations.MoveCards(f.SelectedCardIds.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.Equipment(f.OwnerSeat), pile.Location, new(ZengdaoDepositReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            grant = grant with
            {
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            r = f.LvQian!with
            {
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                ActualCount = f.SelectedCardIds.Count,
                Grant = grant
            };
            _coreMatchParticipantsPlayersAccess._players[recipient].ZengdaoGrants = [..(_coreMatchParticipantsPlayersAccess._players[recipient].ZengdaoGrants ?? []), grant];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = r });
            IssueLvQianSegment(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ConsumeZengdaoForActualSourceDamage)
        {
            var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w?.OccurrenceIndex ?? 0);
            if (w is null || !IsIssuedZengdaoCandidate(candidate, w) || w.ZengdaoGrant is not { } grant || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame damageWindow || damageWindow.Id != w.ParentFrameId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || damageWindow.Prevented)
                throw new InvalidOperationException("Zengdao requires its granted recipient's exact native damage attempt.");
            var choices = (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ZengdaoGrants ?? []).Where(IsDurableZengdaoGrant).OrderBy(g => g.Source.OwnerSeat).ThenBy(g => g.Source.SkillInstanceId, StringComparer.Ordinal).SelectMany(g => g.Entities.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == g.Location).Select(id => new ZengdaoEntityChoice(id, g))).ToArray();
            if (choices.Length == 0)
                throw new InvalidOperationException("Zengdao has no genuinely issued public entity to pay.");
            var r = new LvQianReceipt(grant.Source, grant.GameplayHash, 1, damageWindow.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, LvQianStage.DamageChoice, Grant: grant, DamageAttemptResolutionId: attack.ResolutionId)
            {
                Targets = [damageWindow.TargetSeat],
                Choices = choices
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = r });
            IssueLvQianStarted(f);
            if (choices.Length == 1)
                PayZengdaoDamage(f, choices[0]);
            else
                PublishZengdaoChoice(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (w?.WeiluPhase is not { } phase || phase.OwnerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Weilu requires its frozen original native phase key.");
        if (op == SkillProgramEffectOp.PayWeiluNextActualPlayHpLoss)
        {
            var selected = (owner.WeiluFuture ?? []).Where(g => g.FirstPhase == phase && g.Source.SkillId == f.SkillId && g.Source.SkillInstanceId == f.SkillInstanceId && LvQianSourceCurrent(g.Source, g.GameplayHash)).ToArray();
            owner.WeiluFuture = (owner.WeiluFuture ?? []).Except(selected).ToArray();
            var targets = selected.Select(g => g.DamageSourceSeat).Distinct().ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = new(LvQianSource(f), f.GameplayHash, 1, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, LvQianStage.LoseHpChildren, Phase: phase) { Targets = targets }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            IssueLvQianStarted(f);
            PayNextWeiluTarget(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op != SkillProgramEffectOp.RestoreWeiluExactPaidPlayHp)
            throw new InvalidOperationException("Unknown Lv Qian operation.");
        var debts = (owner.WeiluDebts ?? []).Where(g => g.Phase == phase && g.Source.SkillId == f.SkillId && g.Source.SkillInstanceId == f.SkillInstanceId).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = new(LvQianSource(f), f.GameplayHash, 1, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, LvQianStage.RecoveryChildren, Phase: phase) { Targets = debts.Select(g => g.TargetSeat).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        IssueLvQianStarted(f);
        PayNextWeiluTarget(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void IssueLvQianStarted(ProgramSkillFrame f)
    {
        var r = f.LvQian!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LvQianStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.ActualTurn, r.Stage));
    }

    internal void IssueLvQianSegment(ProgramSkillFrame f)
    {
        var r = f.LvQian!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LvQianSegmentPaidEvent(f.Id, r.Source, r.GameplayHash, r.Stage, r.Before, r.After, r.ActualCount, r.Targets.Single()));
    }

    internal bool DrainLvQian(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 24900)]
    internal bool ResumeLvQian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvQian: { } r } f || f.Id != id || r.Stage == LvQianStage.DamageChoice)
            return false;
        if (DrainLvQian(f))
            return true;
        if (r.Stage is LvQianStage.LoseHpChildren or LvQianStage.RecoveryChildren)
        {
            PayNextWeiluTarget(f);
            return true;
        }

        FinishLvQian(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24900)]
    internal bool ReturnLvQianMovement(ProgramSkillFrame f) => f.LvQian is not null && ResumeLvQian(f.Id);
    internal void FinishLvQian(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome ILvQianProgramHost.ExecuteLvQian(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteLvQian(f, op);
}
