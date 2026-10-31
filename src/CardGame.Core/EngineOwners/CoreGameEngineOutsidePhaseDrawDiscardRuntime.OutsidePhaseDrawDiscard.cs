#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardRuntime
{
    internal bool TracksNativeDrawInvocations => _nativeDrawInvoiceOperations.Get().TracksInvocations;
    internal long OutsidePhaseMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static bool IsOutsidePhaseOperation(SkillProgramEffectOp op) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OutsidePhaseDrawDiscard.IsOutsidePhaseOperation(op);
    internal static string OutsidePhaseMovementReason(ProgramSkillFrame f, bool draw) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OutsidePhaseDrawDiscard.OutsidePhaseMovementReason(f, draw);
    internal NativeDrawInvocationDraft? CaptureNativeDrawInvocation(int owner, int requested, CardMoveReason reason, CardConversionSource? directProducer) => _nativeDrawInvoiceOperations.Get().Capture(owner, requested, reason, directProducer);
    internal NativeDrawProgramOrigin? CaptureNativeDrawProgramOrigin(ProgramSkillFrame frame) => _nativeDrawInvoiceOperations.Get().CaptureProgramOrigin(frame);
    internal void CompleteNativeDrawInvocation(NativeDrawInvocationDraft? draft, IReadOnlyList<int> actualIds) => _nativeDrawInvoiceOperations.Get().Complete(draft, actualIds);
    internal bool ValidNativeDrawInvocation(NativeDrawInvocationProof proof) => _nativeDrawInvoiceOperations.Get().IsValid(proof);
    internal int[] MatchingOutsideDrawIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.NativeDrawInvocation is not { } proof || proof.LastBatchId != batch.Id || proof.OwnerSeat != candidate.OwnerSeat || proof.ParentFrameId != batch.ParentFrameId || proof.ActualTurnNumber != batch.TurnNumber || proof.Phase?.Kind == ActualDiscardRecoveryPhaseKind.Draw || !ValidNativeDrawInvocation(proof) || proof.DirectProducer is { } source && source.OwnerSeat == candidate.OwnerSeat && source.SkillId == candidate.SkillId)
            return[];
        var last = proof.Materials[^1];
        return batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.Sequence == last.MovementSequence && x.m.CardId == last.CardId && x.m.CardKind == last.PrintedKind && x.m.From == CardLocation.DrawPile && x.m.To == CardLocation.Hand(candidate.OwnerSeat) && x.m.Reason == proof.Reason).Select(x => x.i).ToArray();
    }

    internal int[] MatchingOutsideDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.TurnNumber < 1 || batch.DiscardRecoveryPhase?.Kind == ActualDiscardRecoveryPhaseKind.Discard || batch.DiscardRecoveryPhase is { } phase && (!_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || phase.TurnNumber != batch.TurnNumber))
            return[];
        return batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.To == CardLocation.DiscardPile && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(x.m)is { OwnerSeat: { } owner, Zone: CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment } && owner == candidate.OwnerSeat && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(x.m)).Select(x => x.i).ToArray();
    }

    internal ProgramSkillWindowContext CreateOutsideDiscardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var indexes = MatchingOutsideDiscardIndexes(frame.Batch, candidate);
        if (indexes.Length == 0)
            throw new InvalidOperationException("An outside-discard context lost its real own discard material.");
        return new(SkillProgramTriggerWindow.DiscardPileReceived, frame.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], indexes.Length, new(CardLocation.DiscardPile, 0, 0), SkillProgramTriggerWindow.DiscardPileReceived, frame.Batch.MovementTiming), MovementBatch: frame.Batch, MovementIndex: indexes[0]);
    }

    internal bool ExactOutsidePhaseParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, out CardsMovedTriggerWindowFrame parent)
    {
        parent = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)!;
        return parent is not null && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate && context.MovementBatch is { } batch && batch.Id == parent.Batch.Id && batch.Movements.SequenceEqual(parent.Batch.Movements);
    }

    internal OutsidePhaseDiscardMaterial[] OutsidePhaseDiscardMaterials(int owner)
    {
        var result = new List<OutsidePhaseDiscardMaterial>();
        foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner))
            foreach (var zone in new[]
            {
                CardZoneKind.Hand,
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            )
            {
                var from = new CardLocation(zone, target.Seat);
                var cards = _cardZonesOperations._cardZones.CardsAt(from);
                for (var slot = 0; slot < cards.Count; slot++)
                    if (!_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, cards[slot], from, OwnedCardMoveIntent.Discard))
                        result.Add(new(cards[slot].Id, cards[slot].Kind, from, slot, cards[slot].IsGeneralWeapon));
            }

        return result.ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1040)]
    internal bool CanRunOutsidePhaseDrawDiscardTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => IsOutsidePhaseOperation(e.Op)))
            return true;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !ExactOutsidePhaseParent(candidate, context, out var parent))
            return false;
        return trigger.Effects[0].Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw ? context.Window == SkillProgramTriggerWindow.CardsGained && MatchingOutsideDrawIndexes(parent.Batch, candidate).Length == 1 : context.Window == SkillProgramTriggerWindow.DiscardPileReceived && MatchingOutsideDiscardIndexes(parent.Batch, candidate).Length > 0 && OutsidePhaseDiscardMaterials(candidate.OwnerSeat).Length > 0;
    }

    internal SkillProgramStepOutcome BeginOutsidePhaseDrawDiscard(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.OutsidePhaseDrawDiscard is not null || !IsOutsidePhaseOperation(op))
            throw new InvalidOperationException("An outside-phase obligation cannot be issued twice.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var context = f.WindowContext ?? throw new InvalidOperationException("Outside-phase draw/discard needs its real movement parent.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId);
        var candidate = parent.Candidates[parent.CandidateIndex];
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || trigger.Effects[0].Op != op || !CanRunOutsidePhaseDrawDiscardTrigger(candidate, trigger, context))
            throw new InvalidOperationException("Outside-phase draw/discard lost its exact current movement candidate.");
        var draw = op == SkillProgramEffectOp.DrawAfterActualOutsideDraw;
        var indexes = draw ? MatchingOutsideDrawIndexes(parent.Batch, candidate) : MatchingOutsideDiscardIndexes(parent.Batch, candidate);
        var r = new ProgramOutsidePhaseDrawDiscardReceipt
        {
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            Op = op,
            WindowFrameId = parent.Id,
            OriginalBatchId = parent.Batch.Id,
            OriginalMovementIndex = indexes[0],
            OriginalPhase = draw ? parent.Batch.NativeDrawInvocation!.Phase : parent.Batch.DiscardRecoveryPhase,
            OriginalDraw = draw ? parent.Batch.NativeDrawInvocation : null,
            OriginalDiscards = draw ? [] : indexes.Select(i => new DiscardRecoveryEntity(parent.Batch.Movements[i].CardId, parent.Batch.Movements[i].Sequence, _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(parent.Batch.Movements[i])!.Value)).ToArray(),
            Stage = OutsidePhaseDrawDiscardStage.Choosing,
            EligibleTargets = draw ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray() : [],
            EligibleMaterials = draw ? [] : OutsidePhaseDiscardMaterials(f.OwnerSeat)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OutsidePhaseDrawDiscard = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OutsidePhaseDrawDiscardStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowFrameId, r.OriginalBatchId, r.OriginalDraw?.InvocationId));
        PublishOutsidePhaseDrawDiscardChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> OutsidePhaseDrawDiscardChoices(ProgramSkillFrame f)
    {
        var r = f.OutsidePhaseDrawDiscard!;
        var draw = r.Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw;
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = "outside-phase-draw-discard",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (draw)
            return Array.AsReadOnly(r.EligibleTargets.Select(seat => new PromptChoice(new($"outside-phase.{(f.Id)}.draw.{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 摸一张牌。", [], [seat], Parameters("draw-target"))).ToArray());
        return Array.AsReadOnly(r.EligibleMaterials.Select(m =>
        {
            var hidden = m.From.Zone == CardZoneKind.Hand;
            var parameters = Parameters("discard-card");
            parameters["card-owner-seat"] = m.From.OwnerSeat!.Value.ToString(CultureInfo.InvariantCulture);
            parameters["source-zone"] = m.From.Zone.ToString();
            parameters["slot-index"] = m.Slot.ToString(CultureInfo.InvariantCulture);
            return new PromptChoice(new($"outside-phase.{(f.Id)}.discard.{(m.From.OwnerSeat)}.{(m.From.Zone)}.{(m.Slot)}"), hidden ? $"弃置 {(_coreMatchParticipantsPlayersAccess._players[m.From.OwnerSeat.Value].Name)} 的第 {(m.Slot + 1)} 个暗置手牌牌位。" : $"弃置 {(_coreMatchParticipantsPlayersAccess._players[m.From.OwnerSeat.Value].Name)} 的【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】。", hidden ? [] : [m.CardId], [m.From.OwnerSeat.Value], parameters);
        }).ToArray());
    }

    internal void PublishOutsidePhaseDrawDiscardChoice(ProgramSkillFrame f)
    {
        var choices = OutsidePhaseDrawDiscardChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.OutsidePhaseDrawDiscard!.Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw ? "令一名角色摸一张牌。" : "弃置一名其他角色的一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("outside-phase-draw-discard")]
    internal void ResolveOutsidePhaseDrawDiscardChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Outside-phase choice lost its owner.");
        _coreOutsidePhaseDrawDiscardObserversOperations.AssertOutsidePhaseDrawDiscard(f);
        var r = f.OutsidePhaseDrawDiscard!;
        if (r.Stage != OutsidePhaseDrawDiscardStage.Choosing || !OutsidePhaseDrawDiscardChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(choice.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Outside-phase choice changed its exact published opaque material or target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "卫戍在发行摸牌或弃牌前失去原技能资格。");
            return;
        }

        var draw = r.Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw;
        var seat = choice.Targets.Single();
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "卫戍的原目标已失效，未发行动作。");
            return;
        }

        OutsidePhaseDiscardMaterial? material = null;
        if (!draw)
        {
            material = r.EligibleMaterials.Single(m => m.From.OwnerSeat == seat && m.From.Zone.ToString() == choice.Parameters["source-zone"] && m.Slot.ToString(CultureInfo.InvariantCulture) == choice.Parameters["slot-index"]);
            if (!OutsidePhaseDiscardMaterials(f.OwnerSeat).Contains(material))
                throw new InvalidOperationException("Outside-phase discard must remove its original still-owned legal opaque slot.");
        }

        var before = OutsidePhaseMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OutsidePhaseDrawDiscard = r with { Stage = OutsidePhaseDrawDiscardStage.MovementChildren, MovementIssued = true, ParticipantSeat = seat, PaidMaterial = material, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(seat, 0, null) });
        if (draw)
        {
            var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, new(OutsidePhaseMovementReason(f, true)), r.Source).Count;
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { OutsidePhaseDrawDiscard = current.OutsidePhaseDrawDiscard!with { ActualCount = actual, SequenceAfter = OutsidePhaseMovementSequence } });
            RecordOutsidePhaseIssued(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        }
        else
        {
            var paid = material!;
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [paid.CardId], new(OutsidePhaseMovementReason(f, false)), (batch, records) =>
            {
                foreach (var movement in records)
                    _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id);
                if (records is not [var move] || move.CardId != paid.CardId || move.CardKind != paid.PrintedKind || move.From != paid.From || move.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(paid.CardId), paid.From, (paid.IsGeneralWeapon && paid.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile)))
                    throw new InvalidOperationException("Outside-phase discard did not pay its single real frozen foreign card.");
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { OutsidePhaseDrawDiscard = current.OutsidePhaseDrawDiscard!with { ActualCount = 1, SequenceAfter = move.Sequence, BatchId = batch } });
                RecordOutsidePhaseIssued(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            });
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void RecordOutsidePhaseIssued(ProgramSkillFrame f)
    {
        var r = f.OutsidePhaseDrawDiscard!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OutsidePhaseDrawDiscardMovementIssuedEvent(f.Id, r.Op, r.ParticipantSeat!.Value, r.ActualCount, r.SequenceBefore, r.SequenceAfter, r.BatchId, r.PaidMaterial));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 100)]
    internal bool ResumeOutsidePhaseDrawDiscard(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.OutsidePhaseDrawDiscard is not { } r)
            return false;
        _coreOutsidePhaseDrawDiscardObserversOperations.AssertOutsidePhaseDrawDiscard(f);
        if (r.Stage == OutsidePhaseDrawDiscardStage.Choosing)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishOutsidePhaseDrawDiscardChoice(f);
            return true;
        }

        if (r.Stage == OutsidePhaseDrawDiscardStage.MovementChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frameId))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OutsidePhaseDrawDiscard = r with { Stage = OutsidePhaseDrawDiscardStage.Complete }, PendingMovementContinuation = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OutsidePhaseDrawDiscardCompletedEvent(f.Id, r.ActualCount));
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 80)]
    internal bool ReturnOutsidePhaseDrawDiscardMovement(ProgramSkillFrame f)
    {
        if (f.OutsidePhaseDrawDiscard is not { MovementIssued: true, Stage: OutsidePhaseDrawDiscardStage.MovementChildren })
            return false;
        _coreOutsidePhaseDrawDiscardObserversOperations.AssertOutsidePhaseDrawDiscard(f);
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Outside-phase child lost its owning movement return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 210)]
    internal bool IsOutsidePhaseDrawDiscardMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect is not null && IsOutsidePhaseOperation(effect.Op) && f.OutsidePhaseDrawDiscard is { MovementIssued: true } r && r.Op == effect.Op && f.PendingMovementContinuation == pending && pending.SubjectSeat == r.ParticipantSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && _coreOutsidePhaseDrawDiscardObserversOperations.ValidOutsidePhaseDrawDiscardReceipt(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 620)]
    internal bool CanContinueOutsidePhaseDrawDiscard(ProgramSkillFrame f) => f.OutsidePhaseDrawDiscard is { MovementIssued: true } && _coreOutsidePhaseDrawDiscardObserversOperations.ValidOutsidePhaseDrawDiscardReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.DrawAfterActualOutsideDraw, SkillProgramEffectOp.DiscardAfterActualOutsideDiscard)]
    internal PromptChoice SelectAiOutsidePhaseDrawDiscard(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.OutsidePhaseDrawDiscard!.Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw)
            return decision.Choices.OrderByDescending(c => c.Targets.Single() == f.OwnerSeat).ThenBy(c => c.Targets.Single()).First();
        var(choice, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseOtherOwnedCardDiscard(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return choice;
    }

    SkillProgramStepOutcome global::CardGame.Core.IOutsidePhaseDrawDiscardProgramHost.OutsidePhaseDrawDiscard(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginOutsidePhaseDrawDiscard(f, op);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4200)]
    internal ProgramSkillWindowContext? TryCreateOutsideDiscardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.DiscardAfterActualOutsideDiscard))
            return CreateOutsideDiscardContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyMovementIndexes, 200)]
    internal int[]? SelectWindowIndexes_MatchingOutsideDrawIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.DrawAfterActualOutsideDraw))
            return MatchingOutsideDrawIndexes(batch, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 400)]
    internal int[]? SelectWindowIndexes_MatchingOutsideDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.DiscardAfterActualOutsideDiscard))
            return MatchingOutsideDiscardIndexes(batch, candidate);
        return null;
    }
}
