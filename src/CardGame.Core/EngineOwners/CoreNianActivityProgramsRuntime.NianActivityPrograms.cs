#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNianActivityProgramsRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 20)]
    [ProgramTargetRule(ProgramTargetRulePoint.DeclaredGlobalProhibition, 10)]
    internal bool NianDongmianDirectedTargetProhibited(ProgramDirectedTargetQuery query) => NianDongmianForbidsTarget(query.ActorSeat, query.TargetSeat);
    SkillProgramStepOutcome INianActivityProgramHost.ExecuteNianActivity(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteNianActivity(f, e);
    internal bool NianEnabled(CharacterState player, string id) => player.IsAlive && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(id) && _coreNationalSkillsOperations.HasRuntimeSkill(player, id);
    internal bool NianOwnActualTurn(int seat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished);
    internal bool NianPhaseWasActuallySkipped(int seat, SkillProgramTurnPhase phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<NianActualPhaseSkippedEvent>().Any(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == seat && e.Phase == phase);
    internal void RecordNianActualPhaseSkips(int owner, bool draw, bool play)
    {
        if (!NianEnabled(_coreMatchParticipantsPlayersAccess._players[owner], "nian:mengtai") || !NianOwnActualTurn(owner))
            return;
        foreach (var phase in new[]
        {
            SkillProgramTurnPhase.Draw,
            SkillProgramTurnPhase.Play
        }

        )
            if ((phase == SkillProgramTurnPhase.Draw ? draw : play) && !NianPhaseWasActuallySkipped(owner, phase))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianActualPhaseSkippedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, owner, phase));
    }

    internal SkillProgramStepOutcome ExecuteNianActivity(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        switch (effect.Op)
        {
            case SkillProgramEffectOp.ApplyNianMengtaiDiscard:
                return NianPhaseWasActuallySkipped(owner.Seat, SkillProgramTurnPhase.Play) ? _coreProgramTurnPhaseOperations.SkipProgramTurnPhases(frame, [SkillProgramTurnPhase.Discard]) : SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.DrawNianMengtaiEnding:
                if (NianPhaseWasActuallySkipped(owner.Seat, SkillProgramTurnPhase.Draw))
                    return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, owner.Seat, 3, reason);
                break;
            case SkillProgramEffectOp.DiscardNianRuizhiRemainder:
            {
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } other } context || other == frame.OwnerSeat || other != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[other].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is not { OwnerSeat: var turnOwner } || turnOwner != other)
                    throw new InvalidOperationException("Ruizhi requires the exact other character's actual preparation window.");
                var hand = _coreProgramLifecycleOperations.GetProgramCardSet(frame, "ruizhi-hand");
                var equipment = _coreProgramLifecycleOperations.GetProgramCardSet(frame, "ruizhi-equipment");
                if (hand.CardIds.Count > 1 || equipment.CardIds.Count > 1 || hand.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(other)) || equipment.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Equipment(other)))
                    throw new InvalidOperationException("Ruizhi lost a privately selected kept entity in its original zone.");
                var keep = hand.CardIds.Concat(equipment.CardIds).ToHashSet();
                var discarded = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[other]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[other])).Where(c => !keep.Contains(c.Id) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(other, c, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(_coreMatchParticipantsPlayersAccess._players[other], c), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray();
                if (discarded.Length == 0)
                    break;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(other, 0, null) });
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, other, discarded, new("program.nian.ruizhi.discard"));
                break;
            }

            case SkillProgramEffectOp.RecoverNianJingjueLoss:
            {
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } context || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)?.Batch != batch)
                    throw new InvalidOperationException("Jingjue requires its actual atomic loss batch.");
                if (MatchingNianJingjueMovements(batch, owner.Seat).Length == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<NianJingjueLossRecoveredEvent>().Any(e => e.MovementBatchId == batch.Id && e.OwnerSeat == owner.Seat))
                    break;
                // The receipt precedes the real recovery and its child windows, so two
                // qualifying movement bindings can never repay a single loss batch.
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianJingjueLossRecoveredEvent(batch.Id, owner.Seat));
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, owner.Seat, owner.Seat, 1, null, null);
                break;
            }

            case SkillProgramEffectOp.DrawNianRenxingDamage:
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } damage || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().LastOrDefault(w => w.Id == damage.ParentFrameId)is not { } damageParent || _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damageParent).DamageAmount <= 0)
                    throw new InvalidOperationException("Renxing requires an actual positive applied-damage occurrence, including source-less damage.");
                if (!NianOwnActualTurn(owner.Seat))
                    return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, owner.Seat, 1, reason);
                break;
            case SkillProgramEffectOp.DrawNianRenxingRecovery:
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery, Amount: > 0 } })
                    throw new InvalidOperationException("Renxing requires an actual positive recovered-HP occurrence.");
                if (!NianOwnActualTurn(owner.Seat))
                    return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, owner.Seat, 1, reason);
                break;
            case SkillProgramEffectOp.DrawNianBaonuReplacement:
                // The source describes endpoints 4 and current HP, but not their
                // reversed order below 4. This local completion uses both inclusively.
                var lower = Math.Min(4, owner.Hp);
                var upper = Math.Max(4, owner.Hp);
                var count = lower + _randomOperations._random.Next(checked(upper - lower + 1));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianBaonuDrawRolledEvent(frame.Id, owner.Seat, owner.Hp, count));
                return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, owner.Seat, count, reason);
            default:
                throw new InvalidOperationException("Unsupported Nian activity operation.");
        }

        return SkillProgramStepOutcome.Continue;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawNianMengtaiEnding, SkillProgramEffectOp.DrawNianRenxingDamage, SkillProgramEffectOp.DrawNianRenxingRecovery, SkillProgramEffectOp.DrawNianBaonuReplacement)]
    internal void ReturnNianOperationDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        var operation = plan.Instructions[frame.InstructionIndex - 1].Op;
        var expected = operation == SkillProgramEffectOp.DrawNianBaonuReplacement ? _coreProgramEventHistoryOperations.ProgramEventHistory<NianBaonuDrawRolledEvent>().Single(e => e.ProgramFrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat).DrawCount : operation == SkillProgramEffectOp.DrawNianMengtaiEnding ? 3 : 1;
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(operation)}");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.NativeDraw is not null || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != expected || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || outcome.ActualDrawIds.Count > expected || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frame.Id && e.RecipientSeat == frame.OwnerSeat && e.RequestedCount == expected && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("Nian's native draw lost its exact paid operation, requested count or typed replacement return.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(frame.Id, frame.OwnerSeat, null, SkillProgramCardSetVisibility.Private, reason, 0, outcome.ActualDrawIds);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool NianDongmianForbidsTarget(int actor, int target) => actor != target && _coreActualRoundProgressRoundNumberAccess._roundNumber == 1 && _coreMatchParticipantsPlayersAccess._players[target].HasHpValue && _coreMatchParticipantsPlayersAccess._players[target].Hp <= 6 && NianEnabled(_coreMatchParticipantsPlayersAccess._players[target], "nian:dongmian");
    [ProgramDistanceRule(ProgramDistanceStage.CardUseLimitExemption, 10)]
    internal bool NianShouyiUnlimitedCardDistance(CharacterState player) => NianEnabled(player, "nian:shouyi");
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 250)]
    internal int NianBaonuDamageBonus(GameEngine.IDamageAttempt attack) => !attack.IsSourceLess && !attack.IsChainPropagation && attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Hp < 5 && NianEnabled(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], "nian:baonu") ? 1 : 0;
    internal CardMovementBatchContext CaptureNianOwnedTransferBatch(CardMovementBatchContext batch, ProgramSkillFrame? origin)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecoverNianJingjueLoss) || !_coreMatchParticipantsPlayersAccess._players.Any(p => NianEnabled(p, "nian:jingjue")))
            return batch;
        var proofs = new List<NianOwnedTransferProof>();
        // Native Snatch has its own card-use parent and TargetCardTaken invoice;
        // the hidden selected hand identity need not become a public target ID.
        if (batch.ParentFrameId is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { CardKind: CardKind.Snatch } use && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == useId && e.SourceSeat == use.SourceSeat && e.CardKind == CardKind.Snatch))
            foreach (var exit in batch.Movements.Where(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(use.SourceSeat)))
            {
                var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == exit.CardId && m.Sequence < exit.Sequence);
                if (entry is not { From.Zone: CardZoneKind.Hand or CardZoneKind.Equipment, From.OwnerSeat: { } originalOwner } || originalOwner == use.SourceSeat || !use.TargetSeats.Contains(originalOwner) || entry.To != CardLocation.Processing || entry.Sequence >= exit.Sequence || entry.Reason != CardMoveReasons.Snatch || exit.Reason != CardMoveReasons.SnatchFinished || !_coreProgramEventHistoryOperations.ProgramEventHistory<TargetCardTakenEvent>().Any(e => e.ResolutionId == useId && e.SourceSeat == use.SourceSeat && e.TargetSeat == originalOwner && e.FromZone == entry.From.Zone && (e.PublicCardId is null || e.PublicCardId == exit.CardId)))
                    continue;
                var entryBatch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.Id != batch.Id && b.Movements.Contains(entry) && b.ParentFrameId == batch.ParentFrameId && b.ParentBatchId == batch.ParentBatchId && b.TurnNumber == batch.TurnNumber);
                if (entryBatch is not null)
                    proofs.Add(new(exit.CardId, originalOwner, entry.From, use.SourceSeat, entryBatch.Id, entry.Sequence, exit.Sequence, useId, null, null, null, NativeSnatch: true));
            }

        if (origin is null || batch.ParentFrameId != origin.Id || batch.OriginOwnerSeat != origin.OwnerSeat || batch.OriginSkillId != origin.SkillId || batch.OriginSkillInstanceId != origin.SkillInstanceId || origin.InstructionIndex < 1 || _contentRegistryOperations._contentRegistry.GetSkill(origin.SkillId).Program?.GameplayHash != origin.GameplayHash)
            return proofs.Count == 0 ? batch : batch with
            {
                NianOwnedTransfers = proofs
            };
        var effect = ProgramInstructionResolver.Default.Resolve(origin, _contentRegistryOperations._contentRegistry.GetSkill(origin.SkillId).Program!).GetPausedInstruction(origin.InstructionIndex).Effect;
        if (effect is not { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.SelectedTargetHand, CardOwnerRef: { } ownerRef, TargetReference: { } recipientRef })
            return batch;
        var owner = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(origin, ownerRef);
        var recipient = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(origin, recipientRef);
        if (owner == recipient)
            return batch;
        foreach (var exit in batch.Movements.Where(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient)))
        {
            var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == exit.CardId && m.Sequence < exit.Sequence);
            if (entry is not { From.Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || entry.From.OwnerSeat != owner || entry.To != CardLocation.Processing || entry.Sequence >= exit.Sequence || entry.TurnNumber != batch.TurnNumber || entry.Reason != exit.Reason || !effect.Zones.Contains(entry.From.Zone))
                continue;
            var entryBatch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.Id != batch.Id && b.Movements.Contains(entry) && b.ParentFrameId == batch.ParentFrameId && b.ParentBatchId == batch.ParentBatchId && b.TurnNumber == batch.TurnNumber && b.OriginSkillId == origin.SkillId && b.OriginSkillInstanceId == origin.SkillInstanceId && b.OriginOwnerSeat == origin.OwnerSeat);
            if (entryBatch is null)
                continue;
            proofs.Add(new(exit.CardId, owner, entry.From, recipient, entryBatch.Id, entry.Sequence, exit.Sequence, origin.Id, new(origin.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(origin), origin.OwnerSeat, origin.SkillInstanceId), origin.GameplayHash, origin.InstructionIndex - 1));
        }

        return proofs.Count == 0 ? batch : batch with
        {
            NianOwnedTransfers = proofs
        };
    }

    internal static bool IsNianJingjueTrigger(SkillProgramTrigger trigger) => trigger.Window == SkillProgramTriggerWindow.CardsMoved && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RecoverNianJingjueLoss);
    internal int[] MatchingNianJingjueMovements(CardMovementBatchContext batch, int owner) => batch.Movements.Select((m, i) => (Move: m, Index: i)).Where(item => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(item.Move)is { Zone: CardZoneKind.Hand or CardZoneKind.Equipment, OwnerSeat: var discarder } && discarder == owner || item.Move.From is { Zone: CardZoneKind.Hand or CardZoneKind.Equipment, OwnerSeat: var source } && source == owner && item.Move.To is { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } && recipient != owner || batch.NianOwnedTransfers?.Any(p => p.OwnerSeat == owner && p.CardId == item.Move.CardId && p.ExitSequence == item.Move.Sequence && p.ParentFrameId == batch.ParentFrameId && p.OriginalFrom.OwnerSeat == owner && p.RecipientSeat != owner && item.Move.From == CardLocation.Processing && item.Move.To == CardLocation.Hand(p.RecipientSeat) && p.EntrySequence < p.ExitSequence && (p.NativeSnatch ? p.Source is null && p.GameplayHash is null && p.InstructionIndex is null && item.Move.Reason == CardMoveReasons.SnatchFinished && _coreProgramEventHistoryOperations.ProgramEventHistory<TargetCardTakenEvent>().Any(e => e.ResolutionId == p.ParentFrameId && e.SourceSeat == p.RecipientSeat && e.TargetSeat == owner && e.FromZone == p.OriginalFrom.Zone) : p.Source is { } producer && p.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(producer.SkillId).Program?.GameplayHash && p.InstructionIndex >= 0 && batch.OriginSkillId == producer.SkillId && batch.OriginSkillInstanceId == producer.SkillInstanceId && batch.OriginOwnerSeat == producer.OwnerSeat) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == p.CardId && m.Sequence == p.EntrySequence && m.From == p.OriginalFrom && m.To == CardLocation.Processing && m.TurnNumber == batch.TurnNumber && (p.NativeSnatch ? m.Reason == CardMoveReasons.Snatch : m.Reason == item.Move.Reason))) == true).Select(item => item.Index).ToArray();
    internal IEnumerable<ProgramTriggerCandidate> CollectNianJingjueCandidates(CardMovementBatchContext batch) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsMoved)).Where(c => IsNianJingjueTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) && MatchingNianJingjueMovements(batch, c.OwnerSeat).Length != 0);
    internal ProgramSkillWindowContext CreateNianJingjueContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsMoved, parent.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { MovedCardCount = MatchingNianJingjueMovements(parent.Batch, candidate.OwnerSeat).Length }, MovementBatch: parent.Batch);
    internal bool CanRunNianJingjueCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.MovementBatch is { } batch && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == candidate.OwnerSeat && context.TargetSeat == candidate.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is { } parent && parent.Id == batch.Id && parent.Batch == batch && parent.Candidates.ElementAtOrDefault(parent.CandidateIndex) == candidate && MatchingNianJingjueMovements(batch, candidate.OwnerSeat).Length != 0;
    internal static bool IsNianRecoveryTrigger(SkillProgramTrigger trigger) => trigger.Window == SkillProgramTriggerWindow.AfterHpRecovered && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawNianRenxingRecovery);
    internal IEnumerable<ProgramTriggerCandidate> CollectNianRecoveryCandidates(HpChangeContext change) => change.Kind != HpChangeKind.Recovery || change.Amount <= 0 ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.AfterHpRecovered)).Where(c => IsNianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)));
    internal ProgramSkillWindowContext CreateNianRecoveryContext(HpChangeContext change, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, candidate.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { HpChangeAmount = change.Amount, HpBeforeChange = change.HpBefore, HpAfterChange = change.HpAfter }, HpChange: change);
    internal bool IsNianRecoveryCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsNianRecoveryTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && context.HpChange is { Kind: HpChangeKind.Recovery, Amount: > 0 } hp && context.OwnerSeat == candidate.OwnerSeat && context.ParentFrameId == hp.Id && context.TargetSeat == hp.TargetSeat && _resolutionStackOperations._resolutionStack.OfType<HpChangedTriggerWindowFrame>().LastOrDefault()is { } parent && parent.Id == hp.Id && parent.Change == hp && parent.Candidates.ElementAtOrDefault(parent.CandidateIndex) == candidate;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 60)]
    internal bool DistanceCardUseExemption60NianShouyiUnlimitedCardDistance(ProgramCardDistanceQuery q) => NianShouyiUnlimitedCardDistance(q.Actor);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 800)]
    internal void CollectWindow_CollectNianJingjueCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectNianJingjueCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1700)]
    internal bool ExcludeOrdinaryMovement_IsNianJingjueTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsNianJingjueTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1900)]
    internal ProgramSkillWindowContext? TryCreateNianJingjueContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsNianJingjueTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateNianJingjueContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 100)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureNianOwnedTransferBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureNianOwnedTransferBatch(completed, batch.OriginProgram);
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 30, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveNianRecovery(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsNianRecoveryTrigger(trigger) ? IsNianRecoveryCandidate(candidate, context) : null;
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 80, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveNianJingjueMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsNianJingjueTrigger(trigger) ? CanRunNianJingjueCandidate(candidate, context) : null;
}
