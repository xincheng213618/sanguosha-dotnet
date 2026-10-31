namespace CardGame.Core;
[ProgramRuntimeModule,ProgramCapability]
internal sealed class CardsMovedWindowRuntime : IProgramMovementWindows
{
    private readonly RuntimeEntries.DeferredRuntime<EquipmentMovementRuntime> _equipment;
    private readonly ICardsMovedWindowNativeProofs _native;
    private readonly ICardsMovedWindowCardUseProofs _cardUse;
    private readonly ICardsMovedWindowNativeReturns _returns;
    private readonly ICardsMovedWindowCardUseContinuations _cardUseContinuations;
    private readonly ICardsMovedWindowSelections _selection;
    private readonly ICardsMovedWindowOperations _operations;
    private readonly ICardsMovedWindowFacts _facts;
    private readonly ICardsMovedWindowPipeline _pipeline;
    private readonly ICardMovementBatchQueue _batches;
    private readonly IProgramEventHistoryQueries _history;
    private readonly ICardZoneQueries _zones;
    private readonly IProgramRuntimeFrames _frames;
    private readonly IProgramRuntimeEvents _events;
    private readonly IProgramRuntimePrompts _prompts;
    private readonly IProgramWindowCursors _cursors;
    internal CardsMovedWindowRuntime(ICardsMovedWindowNativeProofs native,ICardsMovedWindowCardUseProofs cardUse,ICardsMovedWindowNativeReturns returns,ICardsMovedWindowCardUseContinuations cardUseContinuations,ICardsMovedWindowSelections selection,ICardsMovedWindowOperations operations,ICardsMovedWindowFacts facts,ICardsMovedWindowPipeline pipeline,ICardMovementBatchQueue batches,IProgramEventHistoryQueries history,ICardZoneQueries zones,IProgramRuntimeFrames frames,IProgramRuntimeEvents events,IProgramRuntimePrompts prompts,IProgramWindowCursors cursors, RuntimeEntries.DeferredRuntime<EquipmentMovementRuntime> equipment)
    {
        _equipment = equipment;
        _native = native;
        _cardUse = cardUse;
        _returns = returns;
        _cardUseContinuations = cardUseContinuations;
        _selection = selection;
        _operations = operations;
        _facts = facts;
        _pipeline = pipeline;
        _batches = batches;
        _history = history;
        _zones = zones;
        _frames = frames;
        _events = events;
        _prompts = prompts;
        _cursors = cursors;
    }

    internal CardsMovedTriggerWindowFrame? CreateWindow(CardMovementBatchContext batch, long? resumeProgramFrameId = null)
    {
        _pipeline.PrepareLingkongGainBatch(batch);
        var candidates = CollectCardsMovedProgramCandidates(batch).Concat(CollectDiscardPileReceivedCandidates(batch)).Concat(_pipeline.CollectFirstDomainCandidates(batch)).ToArray();
        if (candidates.Length == 0 && !_pipeline.HasWenhePoisonDepartures(batch))
            return null;
        var window = new CardsMovedTriggerWindowFrame(batch.Id, batch, Array.AsReadOnly(candidates), ResumeProgramFrameId: resumeProgramFrameId);
        return window with
        {
            InvocationAuthorizations = Array.AsReadOnly(candidates.Where(c => _equipment.Value.IsDeparture(batch, c))
                .Select(c => new ProgramInvocationAuthorization(window.Id, batch.ParentFrameId ?? 0, SkillProgramTriggerWindow.CardsMoved, c)).ToArray()),
            Contexts = Array.AsReadOnly(candidates.Select(c => CreateCardsMovedProgramContext(window, c)).ToArray())
        };
    }

    private IEnumerable<CardMovementBatchContext> PendingBatches()
    {
        for (var index = 0; index < _batches.PendingBatchCount; index++)
            yield return _batches.PendingBatchAt(index);
    }

    private static bool UsesNativeDrawThreshold(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold }];

    bool IProgramMovementWindows.Begin(long? instructionFrameId, int? subjectSeat) => TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
    internal bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = null, int? subjectSeat = null)
    {
        var shanheNullified = _frames.Top is NullificationWindowFrame claimed && claimed.Id == instructionFrameId && _native.IsShanheNullifiedClaimAwaitingMovement(claimed) ? claimed : null;
        var coldDamage = _frames.Top is ColdDamageReplacementFrame cold && cold.Id == instructionFrameId ? cold : null;
        var kemingEcho = _frames.Top is KemingSkillEchoFrame echo && echo.Id == instructionFrameId ? echo : null;
        var invocation = _frames.Top is SkillInvocationFrame skillInvocation && skillInvocation.Id == instructionFrameId ? skillInvocation : null;
        var drawFunded = _frames.Top is DrawFundedDistinctBasicFrame funded && funded.Id == instructionFrameId ? funded : null;
        var friendResponse = _frames.Top is FriendDuelResponseFrame friend && friend.Id == instructionFrameId ? friend : null;
        var factionRequestCost = _frames.Top is { PaidFactionRequestCostRecovery: not null } paidRequest && paidRequest.Id == instructionFrameId ? paidRequest : null;
        var awaitingFrame = factionRequestCost is null && _frames.Top is ProgramSkillFrame program && (RuntimeSkills.RuntimeRules_Core_SkillPrograms.IsAwaitingProgramMovement(program) || program.Id == instructionFrameId) ? program : null;
        var declaration = _frames.Top is CardDeclarationFrame declared && declared.Id == instructionFrameId && declared.Stage is CardDeclarationStage.Paying or CardDeclarationStage.Cleaning ? declared : null;
        var recoveryReplacement = _frames.Top is RecoveryReplacementFrame recovery && recovery.Id == instructionFrameId ? recovery : null;
        var huxinPlacement = _frames.Top is HuxinMirrorPlacementFrame mirrorPlacement && mirrorPlacement.Id == instructionFrameId ? mirrorPlacement : null;
        var equipmentRecast = _frames.Top is EquipmentRecastFrame recast && recast.Id == instructionFrameId ? recast : null;
        var drawPhase = _frames.Top is DrawPhaseObligationFrame draw && draw.Id == instructionFrameId ? draw : null;
        var manualDiscard = _frames.Top is ManualDiscardFrame discard && discard.Id == instructionFrameId ? discard : null;
        var roundEnding = _frames.Top is RoundEndingBoundaryFrame round && round.Id == instructionFrameId ? round : null;
        var actualPhaseEnd = _frames.Top is ActualPhaseEndFrame end && end.Id == instructionFrameId ? end : null;
        var colorFireAttack = _frames.Top is CardUseFrame { ColorFireAttack.PaidCardId: not null } fire && fire.Id == instructionFrameId ? fire : null;
        var mouFireAttack = _frames.Top is CardUseFrame { MouZhitianPayment: not null } mouFire && mouFire.Id == instructionFrameId ? mouFire : null;
        var counterspellPayment = _frames.Top is NullificationWindowFrame { CounterspellPayment: not null } counter && counter.Id == instructionFrameId ? counter : null;
        var denglouUse = _frames.Top is CardUseFrame { DenglouNative: not null, DenglouCostDrained: false } denglou && denglou.Id == instructionFrameId ? denglou : null;
        var guoSiUse = _frames.Top is CardUseFrame { GuoSiNative: not null, GuoSiCostDrained: false } guoSi && guoSi.Id == instructionFrameId ? guoSi : null;
        var historicalEnding = _frames.Top is CardUseFrame { EndingHistoricalUseReturn: not null, EndingHistoricalCostDrained: false } history && history.Id == instructionFrameId ? history : null;
        var roundPileAlcohol = _frames.Top is CardUseFrame { RoundPileAlcoholReturn: not null, RoundPileAlcoholCostDrained: false } wine && wine.Id == instructionFrameId ? wine : null;
        var paidCardUse = _frames.Top is CardUseFrame { RecoveryPaidContinuation: { } paidContinuation } paidUse && paidUse.Id == instructionFrameId && _cardUse.HasPaidCardUseMovementTail(paidUse, paidContinuation) ? paidUse : null;
        var incomingRangeDraw = _frames.Top is CardUseFrame { SoleIncomingRangeDraw.Stage: SoleIncomingRangeDrawStage.Children } incomingDraw && incomingDraw.Id == instructionFrameId ? incomingDraw : null;
        var nativeCardDraw = _frames.Top is CardUseFrame { NativeCardEffectDraw.Stage: NativeCardEffectDrawStage.DrawingChildren } nativeDraw && nativeDraw.Id == instructionFrameId ? nativeDraw : null;
        var guanduCard = _frames.Top is CardUseFrame { GuanduCardEffect.Stage: GuanduCardEffectStage.DrawingChildren } guandu && guandu.Id == instructionFrameId ? guandu : null;
        var wumiaoFlood = _frames.Top is CardUseFrame { WumiaoFlood.Stage: WumiaoFloodStage.DrawingChildren or WumiaoFloodStage.DiscardChildren } flood && flood.Id == instructionFrameId ? flood : null;
        var dragonBoatCard = _frames.Top is CardUseFrame { DragonBoatCardEffect.Stage: DragonBoatCardEffectStage.DrawingChildren or DragonBoatCardEffectStage.DiscardChildren } dragon && dragon.Id == instructionFrameId ? dragon : null;
        var wenheCard = _frames.Top is CardUseFrame { WenheCardEffect: { Applied: true, Stage: WenheCardEffectStage.Children } } wenhe && wenhe.Id == instructionFrameId ? wenhe : null;
        bool Eligible(CardMovementBatchContext batch) => huxinPlacement is not null ? batch.ParentFrameId == huxinPlacement.Id && batch.AwaitingProgramFrameId is null : shanheNullified is not null ? _native.IsShanheNullifiedClaimMovement(shanheNullified, batch) : coldDamage is not null ? batch.ParentFrameId == coldDamage.Id && batch.AwaitingProgramFrameId is null : manualDiscard is not null ? batch.ParentFrameId == manualDiscard.Id && batch.AwaitingProgramFrameId is null : kemingEcho is not null ? _native.IsKemingEchoMovement(kemingEcho, batch) : invocation is not null ? _native.IsSkillInvocationMovementBatch(invocation, batch) : incomingRangeDraw is not null ? _cardUse.IsSoleIncomingRangeDrawMovement(incomingRangeDraw, batch) : wumiaoFlood is not null ? _cardUse.IsWumiaoFloodMovement(wumiaoFlood, batch) : guanduCard is not null ? _cardUse.IsGuanduCardEffectMovement(guanduCard, batch) : dragonBoatCard is not null ? _cardUse.IsDragonBoatCardEffectMovement(dragonBoatCard, batch) : wenheCard is not null ? _cardUse.IsWenheCardEffectMovement(wenheCard, batch) : nativeCardDraw is not null ? _cardUse.IsNativeCardEffectDrawMovement(nativeCardDraw, batch) : roundEnding is not null ? _native.IsRoundEndingMovement(roundEnding, batch) : actualPhaseEnd is not null ? actualPhaseEnd.InheritedMovementBatchIds.Contains(batch.Id) && batch.AwaitingProgramFrameId is null : friendResponse is not null ? batch.ParentFrameId == friendResponse.Id && batch.AwaitingProgramFrameId is null : paidCardUse is not null ? _cardUse.IsCardUsePaymentBatch(paidCardUse, batch) : awaitingFrame is not null && (_pipeline.IsProgramMovementSettlement(awaitingFrame, batch)) || (mouFireAttack is not null ? batch.ParentFrameId == mouFireAttack.Id && batch.AwaitingProgramFrameId is null : drawFunded is not null ? batch.ParentFrameId == drawFunded.Id && batch.AwaitingProgramFrameId is null : denglouUse is not null ? batch.ParentFrameId == denglouUse.Id && batch.AwaitingProgramFrameId is null : guoSiUse is not null ? batch.ParentFrameId == guoSiUse.Id && batch.AwaitingProgramFrameId is null : historicalEnding is not null ? batch.ParentFrameId == historicalEnding.Id && batch.AwaitingProgramFrameId is null : roundPileAlcohol is not null ? batch.ParentFrameId == roundPileAlcohol.Id && batch.AwaitingProgramFrameId is null : counterspellPayment is not null ? batch.ParentFrameId == counterspellPayment.Id && batch.AwaitingProgramFrameId is null : colorFireAttack is not null ? batch.ParentFrameId == colorFireAttack.Id && batch.AwaitingProgramFrameId is null : drawPhase is not null ? batch.ParentFrameId == drawPhase.Id || drawPhase.InheritedMovementBatchIds?.Contains(batch.Id) == true : factionRequestCost is not null ? batch.ParentFrameId == factionRequestCost.Id : equipmentRecast is not null ? batch.ParentFrameId == equipmentRecast.Id : recoveryReplacement is not null ? batch.ParentFrameId == recoveryReplacement.Id : declaration is not null ? batch.ParentFrameId == declaration.Id : awaitingFrame is null ? batch.AwaitingProgramFrameId is null : batch.AwaitingProgramFrameId == awaitingFrame.Id || batch.AwaitingProgramFrameId is null && batch.ParentFrameId == awaitingFrame.Id);
        if (_prompts.Current is not null || (_frames.Top is not null && huxinPlacement is null && shanheNullified is null && coldDamage is null && roundEnding is null && actualPhaseEnd is null && friendResponse is null && invocation is null && awaitingFrame is null && declaration is null && recoveryReplacement is null && equipmentRecast is null && factionRequestCost is null && drawPhase is null && manualDiscard is null && colorFireAttack is null && mouFireAttack is null && counterspellPayment is null && roundPileAlcohol is null && historicalEnding is null && guoSiUse is null && denglouUse is null && drawFunded is null && paidCardUse is null && incomingRangeDraw is null && nativeCardDraw is null && wenheCard is null && guanduCard is null && wumiaoFlood is null && dragonBoatCard is null) || _facts.Winner != Winner.None || _facts.Status == EngineStatus.Completed)
            return false;
        // Equipment removal may have completed a nested recovery before this movement finished.
        if (awaitingFrame is not null && RuntimeSkills.RuntimeRules_Core_SkillPrograms.IsAwaitingProgramMovement(awaitingFrame) && _operations.TryBeginHpChangedProgramWindow(awaitingFrame.Id, PostEventContinuation.AwaitedProgramMovement))
            return true;
        while (PendingBatches().Any(Eligible))
        {
            var batch = PendingBatches().Where(Eligible).OrderBy(item => item.Id).First();
            _batches.RemovePendingBatch(batch);
            var window = CreateWindow(batch, awaitingFrame is not null && !RuntimeSkills.RuntimeRules_Core_SkillPrograms.IsAwaitingProgramMovement(awaitingFrame) ? awaitingFrame.Id : null);
            if (window is null)
                continue;
            if (shanheNullified is not null)
                window = window with
                {
                    ResumeShanheNullifiedClaimWindowId = shanheNullified.Id,
                    ResumeProgramFrameId = null
                };
            if (coldDamage is not null)
                window = window with
                {
                    ResumeColdDamageReplacementFrameId = coldDamage.Id,
                    ResumeProgramFrameId = null
                };
            if (invocation is not null)
                window = window with
                {
                    ResumeSkillInvocationFrameId = invocation.Id,
                    ResumeProgramFrameId = null
                };
            if (kemingEcho is not null)
                window = window with
                {
                    ResumeKemingEchoFrameId = kemingEcho.Id,
                    ResumeProgramFrameId = null
                };
            if (awaitingFrame is not null && (_pipeline.IsProgramMovementSettlement(awaitingFrame, batch) || _native.IsNianRevivalMovementBatch(awaitingFrame, batch)))
                window = window with
                {
                    ResumeProgramFrameId = awaitingFrame.Id
                };
            if (recoveryReplacement is not null)
                window = window with
                {
                    ResumeRecoveryReplacementFrameId = recoveryReplacement.Id
                };
            if (paidCardUse is not null)
                window = window with
                {
                    ResumePaidCardUseFrameId = paidCardUse.Id
                };
            if (friendResponse is not null)
                window = window with
                {
                    ResumeFriendDuelResponseFrameId = friendResponse.Id,
                    ResumeProgramFrameId = null
                };
            if (huxinPlacement is not null)
                window = window with
                {
                    ResumeHuxinMirrorPlacementFrameId = huxinPlacement.Id,
                    ResumeProgramFrameId = null
                };
            if (equipmentRecast is not null)
                window = window with
                {
                    ResumeEquipmentRecastFrameId = equipmentRecast.Id
                };
            if (drawPhase is not null)
                window = window with
                {
                    ResumeDrawPhaseObligationFrameId = drawPhase.Id
                };
            if (manualDiscard is not null)
                window = window with
                {
                    ResumeManualDiscardFrameId = manualDiscard.Id
                };
            if (incomingRangeDraw is not null)
                window = window with
                {
                    ResumeSoleIncomingRangeDrawFrameId = incomingRangeDraw.Id,
                    ResumeProgramFrameId = null
                };
            if (nativeCardDraw is not null)
                window = window with
                {
                    ResumeNativeCardEffectDrawFrameId = nativeCardDraw.Id
                };
            if (wenheCard is not null)
                window = window with
                {
                    ResumeWenheCardEffectFrameId = wenheCard.Id,
                    ResumeProgramFrameId = null
                };
            if (guanduCard is not null)
                window = window with
                {
                    ResumeGuanduCardEffectFrameId = guanduCard.Id,
                    ResumeProgramFrameId = null
                };
            if (wumiaoFlood is not null)
                window = window with
                {
                    ResumeWumiaoFloodFrameId = wumiaoFlood.Id,
                    ResumeProgramFrameId = null
                };
            if (dragonBoatCard is not null)
                window = window with
                {
                    ResumeDragonBoatCardEffectFrameId = dragonBoatCard.Id,
                    ResumeProgramFrameId = null
                };
            if (roundEnding is not null)
            {
                window = window with
                {
                    ResumeRoundEndingBoundaryId = roundEnding.Id
                };
                _frames.ReplaceTop(roundEnding with { ActiveWindowFrameId = window.Id });
            }

            if (actualPhaseEnd is not null)
                window = window with
                {
                    ResumeActualPhaseEndFrameId = actualPhaseEnd.Id
                };
            if (colorFireAttack is not null)
                window = window with
                {
                    ResumeColorFireAttackFrameId = colorFireAttack.Id
                };
            if (mouFireAttack is not null)
                window = window with
                {
                    ResumeMouZhitianFireFrameId = mouFireAttack.Id
                };
            if (counterspellPayment is not null)
                window = window with
                {
                    ResumeCounterspellPaymentFrameId = counterspellPayment.Id
                };
            if (guoSiUse is not null)
                window = window with
                {
                    ResumeGuoSiUseFrameId = guoSiUse.Id
                };
            if (denglouUse is not null)
                window = window with
                {
                    ResumeDenglouUseFrameId = denglouUse.Id
                };
            if (historicalEnding is not null)
                window = window with
                {
                    ResumeHistoricalEndingUseFrameId = historicalEnding.Id
                };
            if (roundPileAlcohol is not null)
                window = window with
                {
                    ResumeRoundPileAlcoholUseFrameId = roundPileAlcohol.Id
                };
            if (factionRequestCost is not null)
                window = window with
                {
                    ResumeFactionRequestCostFrameId = factionRequestCost.Id,
                    ResumeProgramFrameId = null
                };
            if (drawFunded is not null)
            {
                window = window with
                {
                    ResumeDrawFundedDistinctBasicFrameId = drawFunded.Id,
                    ResumeProgramFrameId = null
                };
                _operations.BeginDrawFundedDistinctBasicChild(drawFunded.Id, window.Id);
            }

            if (declaration is not null)
            {
                window = window with
                {
                    ResumeDeclarationFrameId = declaration.Id
                };
                _frames.ReplaceTop(declaration with { ActiveChildFrameId = window.Id });
            }

            if (awaitingFrame?.SelectedCardPayment is { } payment && awaitingFrame.SelectedCardPaymentResult is null)
            {
                if (payment.ActiveChildFrameId is not null)
                    throw new InvalidOperationException("A selected-card payment already has an active child frame.");
                _frames.ReplaceTop(awaitingFrame with { SelectedCardPayment = payment with { ActiveChildFrameId = window.Id } });
            }

            _frames.Push(window);
            _frames.AdvanceTop<CardsMovedTriggerWindowFrame>();
            return true;
        }

        return false;
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectCardsMovedProgramCandidates(CardMovementBatchContext batch)
    {
        var candidates = new List<ProgramTriggerCandidate>();
        foreach (var window in new[]
        {
            SkillProgramTriggerWindow.CardsMoved,
            SkillProgramTriggerWindow.CardsGained
        }

        )
            foreach (var(count, discardOriginOnly)in _facts.ProgramMovementSourceCounts(batch, window))
            {
                var ownerSeat = count.Location.OwnerSeat!.Value;
                if (!_facts.IsValidPlayerSeat(ownerSeat))
                    continue;
                foreach (var candidate in _facts.CollectProgramTriggerCandidates(ownerSeat, window)
                    .Concat(window == SkillProgramTriggerWindow.CardsMoved ? _equipment.Value.Departures(batch, ownerSeat) : [])
                    .Distinct())
                {
                    var trigger = _facts.GetProgramTrigger(candidate);
                    if (_pipeline.ExcludeOrdinaryMovementCandidate(candidate, trigger))
                        continue;
                    if (!RuntimeSkills.RuntimeRules_Core_CardMovementPrograms.IsGainPhaseQualified(trigger, ownerSeat, batch.MovementTiming))
                        continue;
                    if (discardOriginOnly && !trigger.MovementDiscardOnly)
                        continue;
                    if (!(window == SkillProgramTriggerWindow.CardsMoved ? trigger.SourceZones : trigger.DestinationZones).Contains(count.Location.Zone))
                        continue;
                    if (trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerOwnerBatch)
                    {
                        if (window != SkillProgramTriggerWindow.CardsMoved || candidates.Any(item => item.OwnerSeat == candidate.OwnerSeat && item.SkillId == candidate.SkillId && item.BindingId == candidate.BindingId && item.SkillInstanceId == candidate.SkillInstanceId))
                            continue;
                        var matching = MatchingOwnerBatchMovementIndexes(batch, candidate, trigger);
                        var ownerBatchFacts = CaptureCardsMovedTriggerFacts(ownerSeat, matching.Length, new CardMovementSourceCount(count.Location, _facts.ProgramMovementSourceCounts(batch, window).Where(item => (!item.DiscardOriginOnly || trigger.MovementDiscardOnly) && item.Count.Location.OwnerSeat == ownerSeat && trigger.SourceZones.Contains(item.Count.Location.Zone)).Sum(item => item.Count.CountBefore), _facts.ProgramMovementSourceCounts(batch, window).Where(item => (!item.DiscardOriginOnly || trigger.MovementDiscardOnly) && item.Count.Location.OwnerSeat == ownerSeat && trigger.SourceZones.Contains(item.Count.Location.Zone)).Sum(item => item.Count.CountAfter)), window);
                        ownerBatchFacts = RuntimeSkills.RuntimeRules_Core_OwnerBatchEquipmentLoss.CaptureOwnerBatchEquipmentLoss(ownerBatchFacts, batch, candidate, trigger, matching);
                        if (matching.Length > 0 && trigger.Condition.Evaluate(ownerBatchFacts, candidate.SkillId, candidate.SkillInstanceId))
                            candidates.Add(candidate);
                        continue;
                    }

                    if (trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerSourceOwner)
                    {
                        candidates.AddRange(_selection.CollectSourceOwnerGainCandidates(batch, candidate, trigger, count));
                        continue;
                    }

                    var indexes = MatchingMovementIndexes(batch, candidate, trigger, count.Location);
                    if (indexes.Length == 0)
                        continue;
                    var facts = CaptureCardsMovedTriggerFacts(ownerSeat, _selection.FriendManjuanMovedCount(batch, ownerSeat, trigger, indexes.Length), count, window, batch.MovementTiming, RuntimeSkills.RuntimeRules_Core_NativeDrawOrdinals.UsesNativeDrawOrdinal(trigger) ? batch : null);
                    if (!trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId))
                        continue;
                    // A judgment still in flight (for example while another skill replaces its
                    // card) owns its judgment zone churn; starting a second judgment from that
                    // movement would collide with the pending one and self-feedback the same
                    // judgment, so such triggers wait for a settled zone instead.
                    if (_facts.HasActiveJudgment && ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.StartJudgment))
                        continue;
                    var occurrences = trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerBatch ? new[]
                    {
                        count.Location.Zone == CardZoneKind.PrivatePersistentPile || trigger.Effects is [{ Op: SkillProgramEffectOp.RecoverAfterLastPublicRice }] ? indexes[0] : 0
                    }

                    : indexes;
                    if (UsesNativeDrawThreshold(trigger))
                        occurrences = Enumerable.Range(0, _selection.MatchingNativeDrawThresholdProgress(batch, candidate)?.Crossings ?? 0).ToArray();
                    candidates.AddRange(occurrences.Select(index => candidate with { OccurrenceIndex = index }));
                }
            }

        _pipeline.CollectMovementCandidates(batch, candidates);
        return candidates.OrderBy(candidate => _selection.RelativeTableOrder(_facts.CurrentSeat, candidate.OwnerSeat)).ThenByDescending(candidate => candidate.Priority).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ThenBy(candidate => candidate.SkillInstanceId, StringComparer.Ordinal).ThenBy(candidate => candidate.OccurrenceIndex).ToArray();
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectDiscardPileReceivedCandidates(CardMovementBatchContext batch)
    {
        var additions = new List<ProgramTriggerCandidate>();
        for (var seat = 0; seat < _facts.PlayerCount; seat++)
        {
            if (!_facts.IsAlive(seat))
                continue;
            foreach (var candidate in _facts.CollectProgramTriggerCandidates(seat, SkillProgramTriggerWindow.DiscardPileReceived))
            {
                var trigger = _facts.GetProgramTrigger(candidate);
                if (trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ClaimLiubingUndamagingSlash))
                    continue;
                var indexes = MatchingDiscardPileIndexes(batch, candidate, trigger);
                if (indexes.Length == 0)
                    continue;
                var occurrences = trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerBatch ? new[]
                {
                    RuntimeSkills.RuntimeRules_Core_JudgedRankSlashPolicies.IsOtherActualBasicDiscardTrigger(trigger) || (trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi }]) ? indexes[0] : 0
                }

                : indexes;
                additions.AddRange(occurrences.Select(index => candidate with { OccurrenceIndex = index }));
            }
        }

        return additions;
    }

    internal int[] MatchingDiscardPileIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.CollectMouXiahouDunQingjian))
            return _selection.MatchingMouXiahouDunDiscardIndexes(batch);
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (_pipeline.SelectDiscardBatchIndexes(batch, candidate, features)is { } pipelineIndexes)
            return pipelineIndexes;
        if (features.HasOperation(SkillProgramEffectOp.ReturnDiscardedSilvEntity) || features.HasOperation(SkillProgramEffectOp.ObtainJingzhongUsedEntities))
            return[];
        if (_pipeline.SelectLegacyDiscardIndexes(batch, candidate, trigger)is { } specialized)
            return specialized;
        if (features.HasOperation(SkillProgramEffectOp.RewardDiscardedActionColor))
        {
            var action = _history.Read<ActionCardsDiscardedEvent>().LastOrDefault(e => e.BatchId == batch.Id);
            return action?.ActorSeat == candidate.OwnerSeat ? batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.To == CardLocation.DiscardPile).Select(item => item.index).ToArray() : [];
        }

        if (features.HasOperation(SkillProgramEffectOp.ReclaimFriendXuanjian))
            return batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.To == CardLocation.DiscardPile && _zones.GetLocation(item.movement.CardId) == CardLocation.DiscardPile && _facts.GetAttackCardKind(item.movement.CardId) == CardKind.Xuanjian).Select(item => item.index).ToArray();
        if (trigger.IgnoreOwnSkillMovements && batch.OriginOwnerSeat == candidate.OwnerSeat && batch.OriginSkillId == candidate.SkillId && batch.OriginSkillInstanceId == candidate.SkillInstanceId)
            return[];
        var discardPile = _zones.CardsAt(CardLocation.DiscardPile).ToDictionary(card => card.Id);
        return batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.To == CardLocation.DiscardPile && (trigger.MovementDiscardOnly ? _facts.GetProgramDiscardSource(item.movement) : item.movement.From)is { } discardSource && discardSource.OwnerSeat is { } source && (trigger.DiscardOwnerScope == SkillProgramDiscardOwnerScope.Own ? source == candidate.OwnerSeat : source != candidate.OwnerSeat) && (trigger.SourceZones.Count == 0 || trigger.SourceZones.Contains(discardSource.Zone)) && item.movement.From != item.movement.To && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value) && (trigger.Suits.Count == 0 || discardPile.TryGetValue(item.movement.CardId, out var card) && trigger.Suits.Contains(card.Suit)) && (trigger.CardCategories.Count == 0 || discardPile.TryGetValue(item.movement.CardId, out var filteredCard) && trigger.CardCategories.Any(category => RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.MatchesSkillProgramCardCategory(filteredCard.Kind, category))) && (trigger.CardKinds.Count == 0 || discardPile.TryGetValue(item.movement.CardId, out var kindCard) && ProgramCardKindMatch.Contains(trigger.CardKinds, kindCard.Kind))).Select(item => item.index).ToArray();
    }

    internal void ClaimProgramMovedCards(ProgramSkillFrame frame)
    {
        var active = _frames.ActiveProgram(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count)
            throw new InvalidOperationException("The moved-card claim lost its discard window.");
        var movement = batch.Movements[index];
        if (movement.To != CardLocation.DiscardPile || movement.From.OwnerSeat is not { } source || source == active.OwnerSeat)
            throw new InvalidOperationException("A moved-card claim must answer another player's discard into the discard pile.");
        if (!_facts.IsAlive(active.OwnerSeat) || _zones.GetLocation(movement.CardId) != CardLocation.DiscardPile)
            return;
        var card = _zones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == movement.CardId);
        _operations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(active.OwnerSeat), new CardMoveReason($"skill-program.{active.SkillId}.{SkillProgramEffectOp.ClaimMovedCards}"));
        _events.AdvanceFact(new ProgramMovedCardsClaimedEvent(active.Id, active.SkillId, active.TriggerId!, active.OwnerSeat, source, card.Id));
        _operations.AddLog("SkillTriggered", $"{_facts.PlayerName(active.OwnerSeat)} 发动【{_facts.SkillName(active.SkillId)}】，获得 {card.DisplayName}。", active.OwnerSeat, source);
    }

    internal int[] MatchingMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (trigger.OwnEquipmentOnly)
            return batch.Movements.Select((m, i) => (m, i)).Where(item =>
                (trigger.Window == SkillProgramTriggerWindow.CardsGained ? item.m.To : item.m.From) == location &&
                item.m.From != item.m.To && _equipment.Value.MatchesOwnCard(candidate, item.m)).Select(item => item.i).ToArray();
        if (UsesNativeDrawThreshold(trigger))
            return _selection.MatchingNativeDrawThresholdIndexes(batch, candidate);
        var ordinalIndexes = RuntimeSkills.RuntimeRules_Core_NativeDrawOrdinals.UsesNativeDrawOrdinal(trigger) ? _selection.MatchingNativeDrawOrdinalIndexes(batch, candidate) : null;
        if (ordinalIndexes is { Length: not 1 })
            return[];
        if (features.HasOperation(SkillProgramEffectOp.GainFaluTokensAfterActualDiscard))
            return location.OwnerSeat != candidate.OwnerSeat ? [] : batch.Movements.Select((movement, index) => (movement, index)).Where(item => _facts.ContainsMovement(item.movement) && _selection.FaluDiscardOwnedSource(item.movement) == location && item.movement.From != item.movement.To && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value)).Select(item => item.index).ToArray();
        if (features.HasOperation(SkillProgramEffectOp.PayMaxHpAndObtainActualUsedTrick))
            return location == CardLocation.Processing ? _selection.MatchingLiJueUseDiscardIndexes(batch, candidate) : [];
        if (features.HasOperation(SkillProgramEffectOp.RecoverAfterLastPublicRice))
        {
            var pile = _facts.RicePile(candidate);
            if (pile is null || pile.Location != location || !_facts.RiceProducer(pile) || !batch.SourceCounts.Any(n => n.Location == location && n.CountBefore > 0 && n.CountAfter == 0))
                return[];
        }

        if (_pipeline.SelectLegacyMovementIndexes(batch, candidate, trigger, location)is { } specialized)
            return specialized;
        if (trigger.IgnoreOwnSkillMovements && batch.OriginOwnerSeat == candidate.OwnerSeat && batch.OriginSkillId == candidate.SkillId && batch.OriginSkillInstanceId == candidate.SkillInstanceId)
            return[];
        return batch.Movements.Select((movement, index) => (movement, index)).Where(item => (trigger.Window == SkillProgramTriggerWindow.CardsGained ? item.movement.To : trigger.MovementDiscardOnly ? _facts.GetProgramDiscardSource(item.movement) : item.movement.From) == location && item.movement.From != item.movement.To && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value)).Where(item => trigger.MovementOccurrence != SkillProgramMovementOccurrence.PerSourceOwner || item.movement.From.OwnerSeat == batch.Movements[candidate.OccurrenceIndex].From.OwnerSeat).Where(item => ordinalIndexes is null || ordinalIndexes.Contains(item.index)).Select(item => item.index).ToArray();
    }

    internal int[] MatchingOwnerBatchMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _facts.ProgramMovementSourceCounts(batch, SkillProgramTriggerWindow.CardsMoved).Where(item => (!item.DiscardOriginOnly || trigger.MovementDiscardOnly) && item.Count.Location.OwnerSeat == candidate.OwnerSeat && trigger.SourceZones.Contains(item.Count.Location.Zone)).SelectMany(item => MatchingMovementIndexes(batch, candidate, trigger, item.Count.Location)).Distinct().Order().ToArray();
    internal SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(int ownerSeat, int movedCardCount, CardMovementSourceCount sourceCount, SkillProgramTriggerWindow window, CardMovementTiming? timing = null, CardMovementBatchContext? nativeDrawBatch = null)
    {
        var facts = _facts.CaptureProgramTriggerFacts(ownerSeat);
        return facts with
        {
            MovementTiming = timing,
            NativeDrawActualTurnOrdinal = nativeDrawBatch?.NativeDrawInvocation is { } draw && draw.OwnerSeat == ownerSeat ? draw.ActualTurnDrawOrdinal : null,
            MovedCardCount = movedCardCount,
            SourceZoneCountBefore = window == SkillProgramTriggerWindow.CardsMoved ? sourceCount.CountBefore : 0,
            SourceZoneCountAfter = window == SkillProgramTriggerWindow.CardsMoved ? sourceCount.CountAfter : 0,
            DestinationZoneCountBefore = window == SkillProgramTriggerWindow.CardsGained ? sourceCount.CountBefore : 0,
            DestinationZoneCountAfter = window == SkillProgramTriggerWindow.CardsGained ? sourceCount.CountAfter : 0
        };
    }

    internal ProgramSkillWindowContext CreateCardsMovedProgramContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        // All specialized dispatchers issue once when the window is created.
        // Re-entering one must keep the same frozen facts as its owning cursor.
        if (frame.Contexts is { } contexts)
            return contexts[frame.CandidateIndex];
        if (_pipeline.CreateMovementContext(frame, candidate)is { } specialized)
            return specialized;
        var trigger = _facts.GetProgramTrigger(candidate);
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (trigger.Window == SkillProgramTriggerWindow.DiscardPileReceived)
        {
            var movement = frame.Batch.Movements[candidate.OccurrenceIndex];
            return new ProgramSkillWindowContext(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: features.HasOperation(SkillProgramEffectOp.ClaimDiscardedEntityWithProvenance) ? _selection.DiscardedProvenanceOrigin(movement)?.SourceSeat : (trigger.MovementDiscardOnly ? _facts.GetProgramDiscardSource(movement) : movement.From)?.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: CaptureCardsMovedTriggerFacts(candidate.OwnerSeat, 1, new CardMovementSourceCount(CardLocation.DiscardPile, 0, 0), trigger.Window), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
        }

        if (trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerOwnerBatch)
        {
            var counts = _facts.ProgramMovementSourceCounts(frame.Batch, trigger.Window).Where(item => (!item.DiscardOriginOnly || trigger.MovementDiscardOnly) && item.Count.Location.OwnerSeat == candidate.OwnerSeat && trigger.SourceZones.Contains(item.Count.Location.Zone)).Select(item => item.Count).ToArray();
            var matching = MatchingOwnerBatchMovementIndexes(frame.Batch, candidate, trigger);
            return new ProgramSkillWindowContext(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: RuntimeSkills.RuntimeRules_Core_OwnerBatchEquipmentLoss.CaptureOwnerBatchEquipmentLoss(CaptureCardsMovedTriggerFacts(candidate.OwnerSeat, matching.Length, new CardMovementSourceCount(counts[0].Location, counts.Sum(item => item.CountBefore), counts.Sum(item => item.CountAfter)), trigger.Window), frame.Batch, candidate, trigger, matching), MovementBatch: frame.Batch);
        }

        if (trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerOtherLastHandLoss)
        {
            var movement = frame.Batch.Movements[candidate.OccurrenceIndex];
            var source = movement.From.OwnerSeat ?? throw new InvalidOperationException("A last-hand-loss candidate lost its hand source.");
            var sourceCount = _facts.ProgramMovementSourceCounts(frame.Batch, trigger.Window).FirstOrDefault(item => item.Count.Location == movement.From).Count;
            return new ProgramSkillWindowContext(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: source, OccurrenceIndex: candidate.OccurrenceIndex, Facts: CaptureCardsMovedTriggerFacts(candidate.OwnerSeat, 1, sourceCount is { } matched ? matched : new CardMovementSourceCount(movement.From, 0, 1), trigger.Window, frame.Batch.MovementTiming), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
        }

        if (trigger.MovementOccurrence is SkillProgramMovementOccurrence.PerThirdPartyHandGain or SkillProgramMovementOccurrence.PerOwnerSourceHandGain)
        {
            var movement = frame.Batch.Movements[candidate.OccurrenceIndex];
            var destinationCount = _facts.ProgramMovementSourceCounts(frame.Batch, trigger.Window).FirstOrDefault(item => item.Count.Location == movement.To).Count is { } matched ? matched : new CardMovementSourceCount(movement.To, 0, 1);
            return new ProgramSkillWindowContext(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: movement.From.OwnerSeat, TargetSeat: movement.To.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: CaptureCardsMovedTriggerFacts(candidate.OwnerSeat, 1, destinationCount, trigger.Window), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
        }

        var gained = trigger.Window == SkillProgramTriggerWindow.CardsGained;
        var kind = (gained ? trigger.DestinationZones : trigger.SourceZones).Single();
        var ricePile = trigger.Effects is [{ Op: SkillProgramEffectOp.RecoverAfterLastPublicRice }];
        var privatePile = kind == CardZoneKind.PrivatePersistentPile || ricePile;
        var location = privatePile ? gained ? frame.Batch.Movements[candidate.OccurrenceIndex].To : frame.Batch.Movements[candidate.OccurrenceIndex].From : new CardLocation(kind, candidate.OwnerSeat);
        if (privatePile && (location.Zone != kind || location.OwnerSeat != candidate.OwnerSeat || !ricePile && string.IsNullOrWhiteSpace(location.PublicPileId)))
            throw new InvalidOperationException("A named private-pile movement changed its exact pile identity.");
        var count = _facts.ProgramMovementSourceCounts(frame.Batch, trigger.Window).Single(item => item.Count.Location == location && (!item.DiscardOriginOnly || trigger.MovementDiscardOnly)).Count;
        var matchingIndexes = MatchingMovementIndexes(frame.Batch, candidate, trigger, location);
        if (privatePile && !matchingIndexes.Contains(candidate.OccurrenceIndex))
            throw new InvalidOperationException("Private pile occurrence no longer matches its trigger.");
        return new ProgramSkillWindowContext(trigger.Window, frame.Id, candidate.OwnerSeat, SourceSeat: trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerSourceOwner ? frame.Batch.Movements[candidate.OccurrenceIndex].From.OwnerSeat : gained ? frame.Batch.OriginOwnerSeat : candidate.OwnerSeat, TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: CaptureCardsMovedTriggerFacts(candidate.OwnerSeat, _selection.FriendManjuanMovedCount(frame.Batch, candidate.OwnerSeat, trigger, matchingIndexes.Length), count, trigger.Window, frame.Batch.MovementTiming, RuntimeSkills.RuntimeRules_Core_NativeDrawOrdinals.UsesNativeDrawOrdinal(trigger) ? frame.Batch : null), MovementBatch: frame.Batch, MovementIndex: trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerCard ? candidate.OccurrenceIndex : null);
    }

    internal void CompleteCardsMovedProgramWindow(CardsMovedTriggerWindowFrame frame)
    {
        _frames.Pop(frame.Id, ResolutionFrameKind.CardsMovedTriggerWindow);
        if (frame.ResumeShanheNullifiedClaimWindowId is not null)
        {
            _returns.ReturnShanheNullifiedClaimMovement(frame);
            return;
        }

        if (frame.ResumeHuxinMirrorPlacementFrameId is not null)
        {
            _returns.ReturnHuxinMirrorPlacementMovement(frame);
            return;
        }

        if (frame.ResumeColdDamageReplacementFrameId is { } coldId)
        {
            if (_frames.Top is not ColdDamageReplacementFrame cold || cold.Id != coldId || frame.Batch.ParentFrameId != coldId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Cold damage lost its original paid discard return.");
            _frames.Advance(coldId);
            return;
        }

        if (frame.ResumeKemingEchoFrameId is not null)
        {
            _returns.ReturnKemingEchoMovement(frame);
            return;
        }

        if (frame.ResumeSkillInvocationFrameId is { } invocationId)
        {
            if (_frames.Top is not SkillInvocationFrame invocation || invocation.Id != invocationId || !_native.IsSkillInvocationMovementBatch(invocation, frame.Batch))
                throw new InvalidOperationException("A skill invocation lost its exact paid movement return.");
            _frames.Advance(invocationId);
            return;
        }

        if (_returns.ReturnWenhePoisonFireAttackMovement(frame))
            return;
        if (_returns.ReturnQiaoZhouPreparationMovement(frame))
            return;
        if (_returns.ReturnCardResponseCompletionMovement(frame))
            return;
        if (_returns.ReturnCardSupplyCompletionMovement(frame))
            return;
        if (frame.ResumeGuanduCardEffectFrameId is { } guanduId)
        {
            if (_frames.Top is not CardUseFrame guandu || guandu.Id != guanduId || !_cardUse.IsGuanduCardEffectMovementReturn(guandu, frame))
                throw new InvalidOperationException("A Guandu card lost its exact movement return.");
            _cardUseContinuations.ContinueGuanduCardEffect(guanduId);
            return;
        }

        if (frame.ResumeWumiaoFloodFrameId is { } floodId)
        {
            if (_frames.Top is not CardUseFrame flood || flood.Id != floodId || !_cardUse.IsWumiaoFloodMovementReturn(flood, frame))
                throw new InvalidOperationException("Wumiao Flood movement lost its exact card return.");
            _cardUseContinuations.ContinueWumiaoFlood(floodId);
            return;
        }

        if (frame.ResumeDragonBoatCardEffectFrameId is { } dragonId)
        {
            if (_frames.Top is not CardUseFrame dragon || dragon.Id != dragonId || !_cardUse.IsDragonBoatCardEffectMovementReturn(dragon, frame))
                throw new InvalidOperationException("Dragon boat movement lost its exact card return.");
            _cardUseContinuations.ContinueDragonBoatCardEffect(dragonId);
            return;
        }

        if (frame.ResumeWenheCardEffectFrameId is { } wenheId)
        {
            if (_frames.Top is not CardUseFrame wenhe || wenhe.Id != wenheId || !_cardUse.IsWenheCardEffectMovementReturn(wenhe, frame))
                throw new InvalidOperationException("A Wenhe card lost its exact atomic movement return.");
            _cardUseContinuations.ContinueWenheCardEffect(wenheId);
            return;
        }

        if (frame.ResumeSoleIncomingRangeDrawFrameId is { } incomingDrawId)
        {
            if (_frames.Top is not CardUseFrame incomingDraw || incomingDraw.Id != incomingDrawId || !_cardUse.IsSoleIncomingRangeDrawMovementReturn(incomingDraw, frame))
                throw new InvalidOperationException("An incoming range draw lost its exact paid movement return.");
            _cardUseContinuations.ContinueSoleIncomingRangeDraw(incomingDrawId);
            return;
        }

        if (frame.ResumeNativeCardEffectDrawFrameId is { } nativeDrawId)
        {
            if (_frames.Top is not CardUseFrame nativeDraw || nativeDraw.Id != nativeDrawId || !_cardUse.IsNativeCardEffectDrawMovementReturn(nativeDraw, frame))
                throw new InvalidOperationException("A native card draw lost its exact movement return.");
            _cardUseContinuations.ContinueNativeDrawTwo(nativeDrawId);
            return;
        }

        if (frame.ResumeFriendDuelResponseFrameId is { } friendId)
        {
            if (_frames.Top is not FriendDuelResponseFrame friendParent || !_cardUse.IsFriendDuelResponseMovementReturn(friendParent, frame))
                throw new InvalidOperationException("Friend Duel response lost its actual movement return.");
            _cardUseContinuations.ContinueFriendDuelResponse(friendId);
            return;
        }

        if (frame.ResumePaidCardUseFrameId is { } paidUseId)
        {
            if (_frames.Top is not CardUseFrame paidParent || paidParent.Id != paidUseId || !_cardUse.IsPaidCardUseMovementReturn(paidParent, frame))
                throw new InvalidOperationException("A card-use payment lost its original typed movement return.");
            _cardUseContinuations.ContinueRecoveryPaidCardUse(paidUseId);
            return;
        }

        if (frame.DeferredTurnEndReturn is not null)
        {
            _returns.ReturnDeferredTurnEndPrelude(frame);
            return;
        }

        if (frame.SoleTargetQuarantineEndReturn is not null)
        {
            _returns.ReturnSoleTargetQuarantineMovement(frame);
            return;
        }

        if (_frames.Top is ProgramSkillFrame { SelectedCardPayment: { } payment, SelectedCardPaymentResult: null } parent)
        {
            if (payment.ActiveChildFrameId != frame.Id)
                throw new InvalidOperationException("A selected-card payment lost its movement child frame.");
            _frames.ReplaceTop(parent with { SelectedCardPayment = payment with { ActiveChildFrameId = null, LastCompletedChildFrameId = frame.Id } });
        }

        if (frame.ResumeDeclarationFrameId is { } declarationId)
        {
            if (_frames.Top is not CardDeclarationFrame declarationParent || declarationParent.Id != declarationId || declarationParent.ActiveChildFrameId != frame.Id)
                throw new InvalidOperationException("A declaration payment lost its exact movement child.");
            _frames.ReplaceTop(declarationParent with { ActiveChildFrameId = null });
            _frames.Advance(declarationId);
            return;
        }

        if (frame.ResumeDrawFundedDistinctBasicFrameId is not null)
        {
            _returns.ResumeDrawFundedDistinctBasicMovement(frame);
            return;
        }

        if (frame.ResumeRecoveryReplacementFrameId is { } recoveryId)
        {
            _frames.Advance(recoveryId);
            return;
        }

        if (frame.ResumeRoundEndingBoundaryId is not null)
        {
            _returns.ReturnRoundEndingMovement(frame);
            return;
        }

        if (frame.ResumeActualPhaseEndFrameId is { } phaseEndId)
        {
            if (_frames.Top is not ActualPhaseEndFrame phaseEnd || phaseEnd.Id != phaseEndId || phaseEnd.ItemsCaptured || !phaseEnd.InheritedMovementBatchIds.Contains(frame.Batch.Id) || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("An actual phase end lost its exact inherited movement return.");
            _frames.Advance(phaseEndId);
            return;
        }

        if (frame.ResumeManualDiscardFrameId is { } discardId)
        {
            if (_frames.Top is not ManualDiscardFrame discardParent || discardParent.Id != discardId || discardParent.OwnerSeat != _facts.CurrentSeat || discardParent.ActualTurnNumber != _facts.TurnNumber || _facts.Phase != TurnPhase.Discard || frame.Batch.ParentFrameId != discardId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Discard movement lost its owning manual discard frame.");
            _operations.ContinueManualDiscard(discardId);
            return;
        }

        if (frame.ResumeDrawPhaseObligationFrameId is { } drawId)
        {
            if (_frames.Top is not DrawPhaseObligationFrame drawParent || drawParent.Id != drawId || frame.Batch.ParentFrameId != drawId && drawParent.InheritedMovementBatchIds?.Contains(frame.Batch.Id) != true)
                throw new InvalidOperationException("An actual draw lost its exact gain movement return.");
            _frames.Advance(drawId);
            return;
        }

        if (frame.ResumeDenglouUseFrameId is { } denglouId)
        {
            _cardUseContinuations.ContinueDenglouNativePayment(denglouId);
            return;
        }

        if (frame.ResumeGuoSiUseFrameId is { } guoSiId)
        {
            if (_frames.Top is not CardUseFrame { GuoSiNative: not null, GuoSiCostDrained: false } guoSiParent || guoSiParent.Id != guoSiId || frame.Batch.ParentFrameId != guoSiId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Sidao lost its exact native hand-material payment return.");
            _cardUseContinuations.AssertGuoSiNativeUse(guoSiParent);
            _cardUseContinuations.ContinueGuoSiNativePayment(guoSiId);
            return;
        }

        if (frame.ResumeHistoricalEndingUseFrameId is { } historyId)
        {
            if (_frames.Top is not CardUseFrame { EndingHistoricalUseReturn: not null, EndingHistoricalCostDrained: false } historyParent || historyParent.Id != historyId || frame.Batch.ParentFrameId != historyId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Historical Ending lost its exact one-Hand movement return.");
            _cardUseContinuations.ContinueHistoricalEndingPayment(historyId);
            return;
        }

        if (frame.ResumeRoundPileAlcoholUseFrameId is { } wineId)
        {
            if (_frames.Top is not CardUseFrame { RoundPileAlcoholReturn: not null, RoundPileAlcoholCostDrained: false } wineParent || wineParent.Id != wineId || frame.Batch.ParentFrameId != wineId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Pile Alcohol lost its exact cost movement return.");
            _cardUseContinuations.ContinueRoundPileAlcoholUse(wineId);
            return;
        }

        if (frame.ResumeCounterspellPaymentFrameId is { } counterId)
        {
            if (_frames.Top is not NullificationWindowFrame { CounterspellPayment: not null } counterParent || counterParent.Id != counterId || frame.Batch.ParentFrameId != counterId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Counterspell lost its exact payment movement return.");
            _cardUseContinuations.ContinuePolicyCounterspellPayment(counterId);
            return;
        }

        if (frame.ResumeMouZhitianFireFrameId is { } mouFireId)
        {
            _cardUseContinuations.ContinueMouZhitianFirePayment(mouFireId);
            return;
        }

        if (frame.ResumeColorFireAttackFrameId is { } fireId)
        {
            if (_frames.Top is not CardUseFrame { ColorFireAttack.PaidCardId: not null } fireParent || fireParent.Id != fireId || frame.Batch.ParentFrameId != fireId || frame.Batch.AwaitingProgramFrameId is not null)
                throw new InvalidOperationException("Color Fire Attack lost its exact movement return.");
            _cardUseContinuations.ContinueColorFireAttackPayment(fireId);
            return;
        }

        if (frame.ResumeEquipmentRecastFrameId is { } recastId)
        {
            if (_frames.Top is not EquipmentRecastFrame recastParent || recastParent.Id != recastId || frame.Batch.ParentFrameId != recastId)
                throw new InvalidOperationException("Equipment recast lost its exact movement return.");
            _frames.Advance(recastId);
            return;
        }

        if (frame.ResumeProgramFrameId is { } resume)
        {
            _frames.AdvanceProgram(resume);
            return;
        }

        if (frame.ResumeFactionRequestCostFrameId is { } requestCostId)
        {
            if (_frames.Top is not { PaidFactionRequestCostRecovery: not null } requestProducer || requestProducer.Id != requestCostId || frame.Batch.ParentFrameId != requestCostId)
                throw new InvalidOperationException("Faction request cost lost its exact movement return.");
            _operations.ContinuePaidFactionRequestCost(requestCostId);
            return;
        }

        if (!TryBeginCardsMovedProgramWindow() && _frames.Top is ProgramSkillFrame { } awaited && RuntimeSkills.RuntimeRules_Core_SkillPrograms.IsAwaitingProgramMovement(awaited))
            _returns.ReturnRuntimeProgramMovement(awaited.Id);
        return;
    }

    internal void AdvanceCardsMovedProgramCandidate(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed)
    {
        _events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.OwnerSeat, CreateCardsMovedProgramContext(frame, candidate).Window, activated, completed));
        AdvanceCardsMovedProgramCursor(frame);
    }

    private void AdvanceCardsMovedProgramCursor(CardsMovedTriggerWindowFrame frame) => _cursors.AdvanceCardsMovedProgramCursor(frame);
}
