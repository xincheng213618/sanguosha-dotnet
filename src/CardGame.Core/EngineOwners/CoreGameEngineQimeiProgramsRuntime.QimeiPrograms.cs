#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineQimeiProgramsRuntime
{
    internal bool HasQimeiCapability => _coreMatchParticipantsPlayersAccess._players.Any(p => p.QimeiActivePair is not null);

    internal CardConversionSource QimeiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal IEnumerable<QimeiPairGrant> ActiveQimeiGrants()
    {
        if (!HasQimeiCapability)
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Select(p => p.QimeiActivePair).OfType<QimeiPairGrant>().Where(g => _coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[g.TargetSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], g.Source.SkillId, g.Source.SkillInstanceId));
    }

    internal SkillProgramStepOutcome ArmQimeiPair(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } actor } || actor != f.OwnerSeat)
            throw new InvalidOperationException("Qimei needs one actual other living recipient at the owner's preparation.");
        foreach (var old in ActiveQimeiGrants().Where(g => g.Source.OwnerSeat == f.OwnerSeat && g.Source.SkillInstanceId == f.SkillInstanceId).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiPairExpiredEvent(old.ProgramFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        var grant = new QimeiPairGrant(f.Id, QimeiSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, target);
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].QimeiActivePair = grant;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiPairArmedEvent(grant));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 430)]
    internal void ObserveQimeiExpiry(IGameEvent payload)
    {
        if (!HasQimeiCapability)
            return;
        var started = payload as TurnStartedEvent;
        var died = payload as PlayerDiedEvent;
        if (started is null && died is null)
            return;
        foreach (var grant in _coreMatchParticipantsPlayersAccess._players.Select(p => p.QimeiActivePair).OfType<QimeiPairGrant>().Where(g => started is not null && g.Source.OwnerSeat == started.ActorSeat && g.ActualTurnNumber < started.TurnNumber || died is not null && (g.Source.OwnerSeat == died.VictimSeat || g.TargetSeat == died.VictimSeat)).ToArray())
        {
            _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat].QimeiActivePair = null;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiPairExpiredEvent(grant.ProgramFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        }
    }

    internal bool QimeiPaidThisTurn(QimeiPairGrant grant, QimeiAttribute attribute) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QimeiEqualityConsumedEvent>().Any(e => e.Receipt.Grant.ProgramFrameId == grant.ProgramFrameId && e.Receipt.Attribute == attribute && e.Receipt.ActualTurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
    internal void CaptureQimeiMovementQualification(CardMovementBatchContext batch)
    {
        if (!HasQimeiCapability || _coreActualTurnProgressTurnNumberAccess._turnNumber < 1)
            return;
        foreach (var grant in ActiveQimeiGrants().ToArray())
        {
            if (QimeiPaidThisTurn(grant, QimeiAttribute.Hand))
                continue;
            var seats = new[]
            {
                grant.Source.OwnerSeat,
                grant.TargetSeat
            };
            var changed = seats.Select(seat => new { Seat = seat, Count = batch.SourceCounts.Concat(batch.DestinationCounts ?? []).FirstOrDefault(c => c.Location == CardLocation.Hand(seat)) }).Where(x => x.Count is { } c && c.CountBefore != c.CountAfter).OrderBy(x => batch.Movements.ToList().FindIndex(m => m.From == CardLocation.Hand(x.Seat) || m.To == CardLocation.Hand(x.Seat))).ThenBy(x => x.Seat).ToArray();
            foreach (var item in changed)
            {
                var other = item.Seat == grant.Source.OwnerSeat ? grant.TargetSeat : grant.Source.OwnerSeat;
                var value = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[other]).Count;
                if (item.Count!.CountAfter != value)
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiEqualityQualifiedEvent(new(grant, batch.Id, QimeiAttribute.Hand, _coreActualTurnProgressTurnNumberAccess._turnNumber, item.Seat, other, item.Count.CountBefore, item.Count.CountAfter, value)));
                // A single native batch can qualify both endpoints; the first native
                // mutation owns this once-turn item before the reward changes counts.
                break;
            }
        }
    }

    internal void CaptureQimeiHpQualification(HpChangeContext change)
    {
        if (!HasQimeiCapability || _coreActualTurnProgressTurnNumberAccess._turnNumber < 1 || change.HpBefore == change.HpAfter)
            return;
        foreach (var grant in ActiveQimeiGrants().Where(g => g.Source.OwnerSeat == change.TargetSeat || g.TargetSeat == change.TargetSeat).ToArray())
        {
            if (!_coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat].HasHpValue || !_coreMatchParticipantsPlayersAccess._players[grant.TargetSeat].HasHpValue)
                continue;
            if (QimeiPaidThisTurn(grant, QimeiAttribute.Hp))
                continue;
            var other = grant.Source.OwnerSeat == change.TargetSeat ? grant.TargetSeat : grant.Source.OwnerSeat;
            if (change.HpAfter != _coreMatchParticipantsPlayersAccess._players[other].Hp)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiEqualityQualifiedEvent(new(grant, change.Id, QimeiAttribute.Hp, _coreActualTurnProgressTurnNumberAccess._turnNumber, change.TargetSeat, other, change.HpBefore, change.HpAfter, _coreMatchParticipantsPlayersAccess._players[other].Hp)));
        }
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectQimeiCandidates(long nativeId, SkillProgramTriggerWindow window) => !HasQimeiCapability ? [] : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QimeiEqualityQualifiedEvent>().Where(e => e.Receipt.NativeChangeId == nativeId).SelectMany(e => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[e.Receipt.Grant.Source.OwnerSeat], window).Where(c => c.SkillId == e.Receipt.Grant.Source.SkillId && c.SkillInstanceId == e.Receipt.Grant.Source.SkillInstanceId && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.SettleQimeiPairEquality }])).Distinct().ToArray();
    internal IReadOnlyList<ProgramTriggerCandidate> CollectQimeiMovementCandidates(CardMovementBatchContext b) => CollectQimeiCandidates(b.Id, SkillProgramTriggerWindow.CardsMoved);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectQimeiHpCandidates(HpChangeContext c) => CollectQimeiCandidates(c.Id, SkillProgramTriggerWindow.AfterHealthChanged);
    internal QimeiEqualityReceipt? QimeiQualification(ProgramTriggerCandidate c, long id) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QimeiEqualityQualifiedEvent>().Select(e => e.Receipt).SingleOrDefault(r => r.NativeChangeId == id && r.Grant.Source.OwnerSeat == c.OwnerSeat && r.Grant.Source.SkillId == c.SkillId && r.Grant.Source.SkillInstanceId == c.SkillInstanceId);
    internal ProgramSkillWindowContext CreateQimeiMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate c)
    {
        var receipt = QimeiQualification(c, window.Id) ?? throw new InvalidOperationException("Qimei movement lost its native equality qualification.");
        return new(SkillProgramTriggerWindow.CardsMoved, window.Id, c.OwnerSeat, SourceSeat: receipt.ChangedSeat, TargetSeat: receipt.BeneficiarySeat, MovementBatch: window.Batch)
        {
            QimeiEquality = receipt
        };
    }

    internal ProgramSkillWindowContext CreateQimeiHpContext(HpChangeContext change, ProgramTriggerCandidate c)
    {
        var receipt = QimeiQualification(c, change.Id) ?? throw new InvalidOperationException("Qimei HP change lost its native equality qualification.");
        return new(SkillProgramTriggerWindow.AfterHealthChanged, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, HpChange: change)
        {
            QimeiEquality = receipt
        };
    }

    internal bool IsQimeiEqualityTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.SettleQimeiPairEquality }];
    internal bool HasQimeiHealthSubject(int seat) => _coreMatchParticipantsPlayersAccess._players.Any(p => p.QimeiActivePair is { } g && (g.Source.OwnerSeat == seat || g.TargetSeat == seat));
    internal bool CanRunQimeiEquality(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.QimeiEquality is not { } receipt || receipt.Grant.Source.OwnerSeat != c.OwnerSeat || receipt.Grant.Source.SkillId != c.SkillId || receipt.Grant.Source.SkillInstanceId != c.SkillInstanceId || receipt.NativeChangeId != context.ParentFrameId || receipt.After != receipt.OtherValue || receipt.Before == receipt.After || QimeiPaidThisTurn(receipt.Grant, receipt.Attribute))
            return false;
        return receipt == QimeiQualification(c, context.ParentFrameId) && _coreMatchParticipantsPlayersAccess._players[receipt.BeneficiarySeat].IsAlive && (receipt.Attribute == QimeiAttribute.Hand ? context.MovementBatch?.Id == receipt.NativeChangeId : context.HpChange is { } change && change.Id == receipt.NativeChangeId && change.TargetSeat == receipt.ChangedSeat && change.HpBefore == receipt.Before && change.HpAfter == receipt.After);
    }

    internal SkillProgramStepOutcome SettleQimeiPairEquality(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var receipt = f.WindowContext?.QimeiEquality ?? throw new InvalidOperationException("Qimei lost its owning mutation receipt.");
        if (QimeiPaidThisTurn(receipt.Grant, receipt.Attribute))
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiEqualityConsumedEvent(f.Id, receipt));
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QimeiDraw = new(receipt, f.InstructionIndex, before, before, 0), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var cards = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[receipt.BeneficiarySeat], 1, true, new("program.qimei.equal-draw"));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { QimeiDraw = f.QimeiDraw!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = cards.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QimeiEqualityDrawIssuedEvent(f.Id, receipt, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, cards.Count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 22200)]
    internal bool ResumeQimeiDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QimeiDraw: { } r } f || f.Id != id)
            return false;
        RequireQimeiDraw(f);
        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { QimeiDraw = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    internal bool ValidQimeiDraw(ProgramSkillFrame f) => f.QimeiDraw is { } r && r.InstructionIndex == f.InstructionIndex && f.WindowContext?.QimeiEquality == r.Receipt && r.Receipt.Grant.Source.OwnerSeat == f.OwnerSeat && r.Receipt.Grant.Source.SkillId == f.SkillId && r.Receipt.Grant.Source.SkillInstanceId == f.SkillInstanceId && r.Receipt.Grant.GameplayHash == f.GameplayHash && r.Before <= r.After && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.ActualCount is >= 0 and <= 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QimeiEqualityConsumedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt == r.Receipt) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QimeiEqualityDrawIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt == r.Receipt && e.Before == r.Before && e.After == r.After && e.ActualCount == r.ActualCount) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.To == CardLocation.Hand(r.Receipt.BeneficiarySeat) && m.Reason.Value == "program.qimei.equal-draw") == r.ActualCount;
    internal void RequireQimeiDraw(ProgramSkillFrame f)
    {
        if (!ValidQimeiDraw(f))
            throw new InvalidOperationException("Qimei draw lost its exact mutation/source/once-consumed invoice.");
    }
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2000)]
    internal void CollectWindow_CollectQimeiMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectQimeiMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2500)]
    internal bool ExcludeOrdinaryMovement_IsQimeiEqualityTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsQimeiEqualityTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3900)]
    internal ProgramSkillWindowContext? TryCreateQimeiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        if (IsQimeiEqualityTrigger(trigger))
            return CreateQimeiMovementContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 1000)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureQimeiMovementQualification(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureQimeiMovementQualification(completed);
        return completed;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 60, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveQimeiHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsQimeiEqualityTrigger(trigger) ? CanRunQimeiEquality(candidate, context) : null;
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 240, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveQimeiMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsQimeiEqualityTrigger(trigger) ? CanRunQimeiEquality(candidate, context) : null;
}
