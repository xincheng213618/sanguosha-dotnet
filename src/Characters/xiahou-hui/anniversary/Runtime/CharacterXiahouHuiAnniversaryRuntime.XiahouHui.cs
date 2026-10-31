#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouHuiAnniversaryRuntime
{
    internal const string XiahouHuiAction = "anniversary-xiahou-hui";
    internal static CardMoveReason XiahouHuiGiveReason => new("skill-program.anniversary:dujun.give-drawn");
    internal static CardMoveReason XiahouHuiObtainReason => new("skill-program.anniversary:jikun.random-obtain");
    internal bool HasXiahouHuiDujun => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.BindAnniversaryXiahouHuiDujun);
    internal bool HasXiahouHuiJikun => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXiahouHuiJikun);

    internal static CardConversionSource XiahouHuiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static ProgramTriggerCandidate XiahouHuiCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext!.OccurrenceIndex);
    SkillProgramStepOutcome IAnniversaryXiahouHuiHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp operation) => BeginXiahouHui(f, operation);
    internal XiahouHuiBoundEvent? XiahouHuiPartner(int owner, string? instance = null, bool requireCurrent = true) => !HasXiahouHuiDujun ? null : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiBoundEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner && e.Source.SkillId == XiahouHuiComposition.Dujun && e.Source.BindingId == "choose-partner" && (instance is null || e.Source.SkillInstanceId == instance) && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(e.Source.SkillId, out var skill) && skill.Program?.GameplayHash == e.Hash && (!requireCurrent || _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], e.Source.SkillId, e.Source.SkillInstanceId)));
    internal bool AnniversaryXiahouHuiCannotRespond(int sourceSeat, int responderSeat) => HasXiahouHuiDujun && sourceSeat != responderSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(responderSeat) && XiahouHuiPartner(responderSeat)?.Partner == sourceSeat;
    internal bool AnniversaryXiahouHuiResponseForbidden(int actorSeat, CardKind requiredKind)
    {
        if (!HasXiahouHuiDujun || requiredKind is CardKind.Peach or CardKind.Alcohol && _coreDyingCursorRuntimeOperations.Value.ActiveDying is not null)
            return false;
        var source = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault()is { } counter ? counter.ChainDepth == 0 ? counter.SourceSeat : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(e => e.ResolutionId == counter.ParentFrameId && e.WindowFrameId == counter.Id && e.EffectCardId == counter.EffectCardId && e.EffectCardKind == counter.EffectCardKind && e.ChainDepth == counter.ChainDepth)?.ResponderSeat : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.CardUserSeat ?? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.SourceSeat;
        return source is { } seat && AnniversaryXiahouHuiCannotRespond(seat, actorSeat);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 46200)]
    internal bool XiahouHuiNativeResponseBan(ProgramNativeResponseQuery q) => !q.DyingUse && AnniversaryXiahouHuiResponseForbidden(q.Provider.Seat, q.Kind);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeIntrinsicUseBan, 46200)]
    internal bool XiahouHuiCounterspellBan(ProgramCardUseQuery q) => (q.Kind == CardKind.Nullification || q.ActionType == CardActionType.Response) && AnniversaryXiahouHuiResponseForbidden(q.ActorSeat, q.Kind);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46200)]
    internal void CaptureXiahouHuiFacts(IGameEvent fact)
    {
        if (HasXiahouHuiDujun && fact is DamageAppliedEvent { Amount: > 0 } hit && _resolutionStackOperations._resolutionStack.LastOrDefault()is DamageFrame damage && damage.SourceSeat == hit.SourceSeat && damage.TargetSeat == hit.TargetSeat && damage.Amount == hit.Amount && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiDamageRecordedEvent>().Any(e => e.DamageId == damage.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouHuiDamageRecordedEvent(damage.Id, _coreActualTurnProgressTurnNumberAccess._turnNumber, hit.SourceLess ? null : hit.SourceSeat, hit.TargetSeat));
        if (!HasXiahouHuiJikun || fact is not CardMovedEvent moved || moved.From == moved.To || moved.From.OwnerSeat is not { } owner || moved.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()is not { } actual || actual.CardId != moved.CardId || actual.From != moved.From || actual.To != moved.To || actual.Reason != moved.Reason)
            return;
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).ProgramInstances.Where(i => i.SkillId == XiahouHuiComposition.Jikun && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], i.SkillId, i.SkillInstanceId)).ToArray())
        {
            var previous = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiLossRecordedEvent>().Where(e => e.Owner == owner && e.Instance == instance.SkillInstanceId && e.Hash == instance.Program.GameplayHash).ToArray();
            if (previous.Any(e => e.Sequence == actual.Sequence))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouHuiLossRecordedEvent(owner, instance.SkillInstanceId, instance.Program.GameplayHash, actual.Sequence, checked((previous.LastOrDefault()?.Count ?? 0) + 1)));
        }
    }

    internal bool XiahouHuiDamageCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c, bool issued = false)
    {
        if (candidate.SkillId != XiahouHuiComposition.Dujun || c is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } || c.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.TriggerWindow != c.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex].ToProgramCandidate() != candidate || c.DamageFrameId != window.ParentFrameId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiDamageRecordedEvent>().SingleOrDefault(e => e.DamageId == window.ParentFrameId)is not { } actual || actual.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber || actual.Source != c.SourceSeat || actual.Target != c.TargetSeat)
            return false;
        var partner = XiahouHuiPartner(candidate.OwnerSeat, candidate.SkillInstanceId, !issued);
        if (partner is null)
            return false;
        var participant = candidate.BindingId.StartsWith("self-", StringComparison.Ordinal) ? candidate.OwnerSeat : partner.Partner;
        var caused = candidate.BindingId.EndsWith("-caused", StringComparison.Ordinal);
        return XiahouHuiComposition.DamageBindings.Contains(candidate.BindingId, StringComparer.Ordinal) && (caused ? actual.Source == participant : actual.Target == participant) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiDamageRecordedEvent>().First(e => e.Turn == actual.Turn && (caused ? e.Source == participant : e.Target == participant)).DamageId == actual.DamageId;
    }

    internal XiahouHuiLossRecordedEvent? XiahouHuiLossCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (!HasXiahouHuiJikun || candidate.SkillId != XiahouHuiComposition.Jikun || c is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch, MovementIndex: { } index } || c.OwnerSeat != candidate.OwnerSeat || c.SourceSeat != candidate.OwnerSeat || index < 0 || index >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.Batch.Id != batch.Id || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate)
            return null;
        var m = batch.Movements[index];
        var expectedZone = candidate.BindingId == "five-hand-loss" ? CardZoneKind.Hand : candidate.BindingId == "five-equipment-loss" ? CardZoneKind.Equipment : (CardZoneKind? )null;
        if (expectedZone is null || m.From != new CardLocation(expectedZone.Value, candidate.OwnerSeat) || m.From == m.To || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return null;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiLossRecordedEvent>().SingleOrDefault(e => e.Owner == candidate.OwnerSeat && e.Instance == candidate.SkillInstanceId && e.Hash == candidate.GameplayHash && e.Sequence == m.Sequence && e.Count > 0 && e.Count % 5 == 0);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46200)]
    internal bool CanOfferXiahouHui(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => candidate.SkillId == XiahouHuiComposition.Dujun ? t.Id == "choose-partner" || XiahouHuiDamageCandidate(candidate, c) : candidate.SkillId != XiahouHuiComposition.Jikun || XiahouHuiLossCandidate(candidate, c)is not null;
    internal SkillProgramStepOutcome BeginXiahouHui(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.XiahouHui is not null || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Xiahou Hui requires her original typed trigger frame and exact content identity.");
        if (operation == SkillProgramEffectOp.BindAnniversaryXiahouHuiDujun)
        {
            if (f.SkillId != XiahouHuiComposition.Dujun || f.TriggerId != "choose-partner" || f.InstructionIndex != 2 || f.SelectedTargetSeats is not [var partner] || partner == f.OwnerSeat || c.Window != SkillProgramTriggerWindow.GameStarting || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.Window != c.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiBoundEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId))
                throw new InvalidOperationException("Dujun's partner requires the exact original GameStarting owner/candidate and living other selection.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouHuiBoundEvent(f.Id, XiahouHuiSource(f), f.GameplayHash, window.Id, partner));
            return SkillProgramStepOutcome.Continue;
        }

        var gift = operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards;
        if (f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || gift && (f.InstructionIndex != 2 || !XiahouHuiDamageCandidate(XiahouHuiCandidate(f), c, true)) || !gift && (operation != SkillProgramEffectOp.ResolveAnniversaryXiahouHuiJikun || f.InstructionIndex != 1 || XiahouHuiLossCandidate(XiahouHuiCandidate(f), c)is null))
            throw new InvalidOperationException("Xiahou Hui lost her real first damage role or original five-card threshold.");
        var drawn = gift ? _coreProgramLifecycleOperations.GetProgramCardSet(f, XiahouHuiComposition.Drawn).CardIds.ToArray() : [];
        var threshold = gift ? 0 : XiahouHuiLossCandidate(XiahouHuiCandidate(f), c)!.Sequence;
        var drawInvoice = gift ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Requested == 2 && e.Reason == new CardMoveReason($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.Draw)}")) : null;
        if (drawn.Length > 0 && drawInvoice is null)
            throw new InvalidOperationException("Dujun's draw binding requires its original native Draw2 invoice.");
        var r = new XiahouHuiReceipt(XiahouHuiSource(f), f.GameplayHash, c.ParentFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber, operation, ThresholdSequence: threshold, DrawInvocationId: drawInvoice?.InvocationId ?? 0)
        {
            Drawn = drawn
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiahouHui = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouHuiStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, operation, threshold, drawn.Length));
        PublishXiahouHui(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<XiahouHuiCandidateCard> XiahouHuiObtainable(ProgramSkillFrame f, int donor, int? recipient = null) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, donor, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Obtain).Select(c =>
    {
        var from = new CardLocation(Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]), donor);
        return new XiahouHuiCandidateCard(_cardZonesOperations._cardZones.CardsAt(from)[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id, from);
    }).Where(c => recipient is null || c.From != CardLocation.Hand(recipient.Value)).OrderBy(c => c.CardId).ToArray());
    internal IReadOnlyList<PromptChoice> XiahouHuiChoices(ProgramSkillFrame f)
    {
        if (f.XiahouHui is not { Stage: XiahouHuiStage.Recipient } r)
            return[];
        var choices = new List<PromptChoice>();
        if (r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards)
        {
            if (!r.Drawn.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)))
                return[];
            foreach (var recipient in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                choices.Add(new(new($"xiahou-hui.{(f.Id)}.give.{(recipient.Seat)}"), $"将本次仍可交出的牌全部交给{(recipient.Name)}", [], [recipient.Seat], new Dictionary<string, string> { ["program-action"] = XiahouHuiAction, ["mode"] = "give" }));
        }
        else
        {
            var maximum = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Max(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count);
            foreach (var donor in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count == maximum))
                foreach (var recipient in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && XiahouHuiObtainable(f, donor.Seat, p.Seat).Count > 0))
                    choices.Add(new(new($"xiahou-hui.{(f.Id)}.obtain.{(recipient.Seat)}.{(donor.Seat)}"), $"令{(recipient.Name)}随机获得{(donor.Name)}的一张牌", [], [recipient.Seat, donor.Seat], new Dictionary<string, string> { ["program-action"] = XiahouHuiAction, ["mode"] = "obtain" }));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    [ProgramAiChoice(SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards, SkillProgramEffectOp.ResolveAnniversaryXiahouHuiJikun)]
    internal PromptChoice SelectAiXiahouHuiTransfer(PendingDecision decision, ProgramSkillFrame frame)
    {
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var result = frame.XiahouHui?.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards ? brain.ChooseSupportDrawTargets(view, decision.Choices, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence) : brain.ChooseSupportFirstTransferTargets(view, decision.Choices, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(result.Thought);
        return decision.Choices.Single(choice => choice.Id == result.Choice);
    }

    internal void PublishXiahouHui(ProgramSkillFrame f)
    {
        var choices = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None ? XiahouHuiChoices(f) : [];
        if (choices.Count == 0)
        {
            FinishXiahouHui(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, f.SkillId == XiahouHuiComposition.Dujun ? "笃君：将本次实际摸到的牌交给同一名角色。" : "济困：选择受益者和手牌最多的出牌者。", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(XiahouHuiAction)]
    internal void AnswerXiahouHui(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XiahouHui: { Stage: XiahouHuiStage.Recipient } r } f || !ValidXiahouHui(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], XiahouHuiChoices(f).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Xiahou Hui requires her exact original recipient menu and unpaid movement.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var recipient = answer.Targets[0];
        var donor = r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards ? f.OwnerSeat : answer.Targets[1];
        var candidates = r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards ? r.Drawn.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)).Select(id => new XiahouHuiCandidateCard(id, CardLocation.Hand(f.OwnerSeat))).ToArray() : XiahouHuiObtainable(f, donor, recipient).ToArray();
        var beforeRandom = _randomOperations._random.State;
        var ids = r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards ? candidates.Select(c => c.CardId).ToArray() : [candidates[_randomOperations._random.Next(candidates.Length)].CardId];
        if (recipient == f.OwnerSeat && donor == f.OwnerSeat || ids.Length == 0)
        {
            FinishXiahouHui(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, XiahouHui = r with { Stage = XiahouHuiStage.MovementChildren, Recipient = recipient, Donor = donor, Before = before, After = before, RandomBefore = beforeRandom, RandomAfter = _randomOperations._random.State, Candidates = candidates }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouHuiCommittedEvent(f.Id, recipient, donor, ids.Length, candidates.Length, before));
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { XiahouHui = active.XiahouHui!with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Moves = moves } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouHuiMovedEvent(f.Id, recipient, donor, moves.Count, batch, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards ? XiahouHuiGiveReason : XiahouHuiObtainReason, r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, ids, before, Paid) : Paid, disclosure: CardMovementDisclosure.FaceDown);
        ContinueXiahouHui(f.Id);
    }

    internal void FinishXiahouHui(ProgramSkillFrame f)
    {
        // These are the exact native draw ids, not a fresh hand-card payment.
        if (f.XiahouHui?.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards)
            f = f with
            {
                CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Where(b => b.Name != XiahouHuiComposition.Drawn).ToArray())
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 46200)]
    internal bool ContinueXiahouHui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XiahouHui: { } r } f || f.Id != id)
            return false;
        if (!ValidXiahouHui(f))
            throw new InvalidOperationException("Xiahou Hui lost her original draw/threshold, once-paid movement or frozen random receipt.");
        if (r.Stage == XiahouHuiStage.Recipient)
        {
            PublishXiahouHui(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        FinishXiahouHui(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46200)]
    internal bool ReturnXiahouHui(ProgramSkillFrame f) => f.XiahouHui is not null && ContinueXiahouHui(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46200)]
    internal bool KeepXiahouHuiPaid(ProgramSkillFrame f) => f.XiahouHui is not null ? ValidXiahouHui(f) : f.SkillId == XiahouHuiComposition.Dujun && f.TriggerId is not null && f.ActivationId == f.TriggerId && f.WindowContext is { } c && XiahouHuiDamageCandidate(XiahouHuiCandidate(f), c, true) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId && e.Window == c.Window) == 1;
}
