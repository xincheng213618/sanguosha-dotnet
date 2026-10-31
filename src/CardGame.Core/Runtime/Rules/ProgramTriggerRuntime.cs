namespace CardGame.Core;

/// <summary>Owns candidate preparation, ordered trigger admission, prerequisites and usage accounting.</summary>
[ProgramRuntimeModule]
internal sealed class ProgramTriggerRuntime
{
    internal IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates<TQuery>(TQuery query,
        CharacterState owner,
        SkillProgramTriggerWindow window,
        int occurrenceIndex = 0)
        where TQuery : IProgramTriggerCollectionQuery, IProgramTriggerContentQuery, IProgramTriggerWindowQuery
    {
        var shard = query.GetSkillBindingShard(owner);
        var candidates = shard.GetTriggerCandidates(window, occurrenceIndex);
        // Issued Play-ending debt belongs to its receipt, even after its source
        // grant disappears. Other empty windows have no candidates to prepare.
        if (window != SkillProgramTriggerWindow.PlayEnding ||
            owner.Seat != query.CurrentSeat || query.Phase != TurnPhase.Play ||
            query.IssuedPhaseHandDebtCount == 0)
            return candidates;
        return PrepareProgramTriggerCandidates(query, owner, window, occurrenceIndex, shard, candidates);
    }

    private IReadOnlyList<ProgramTriggerCandidate> PrepareProgramTriggerCandidates<TQuery>(TQuery query,
        CharacterState owner, SkillProgramTriggerWindow window, int occurrenceIndex,
        SkillBindingShard shard, IReadOnlyList<ProgramTriggerCandidate> candidates)
        where TQuery : IProgramTriggerCollectionQuery, IProgramTriggerContentQuery
    {
        using var issued = query.IssuedPhaseHandDebtCandidates(owner, window, occurrenceIndex).GetEnumerator();
        if (!issued.MoveNext()) return candidates;
        var result = candidates.ToList();
        var keys = candidates.Select(shard.TriggerCandidateKey).ToHashSet();
        do
        {
            var candidate = issued.Current;
            var trigger = GetProgramTrigger(query, candidate);
            var key = (candidate.SkillId, candidate.BindingId,
                trigger.NamedUsageGroup is not null || trigger.DynamicUsageLimit is not null ||
                ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.DiscardHandToNamedTurnCount) ? "" : candidate.SkillInstanceId);
            if (keys.Add(key)) result.Add(candidate);
        } while (issued.MoveNext());
        return Array.AsReadOnly(result.ToArray());
    }

    internal SkillProgramTrigger GetProgramTrigger<TQuery>(TQuery query, ProgramTriggerCandidate candidate)
        where TQuery : IProgramTriggerContentQuery =>
        ProgramInstructionResolver.Default.Resolve(query.ActivityProgram(candidate.SkillId),
            ProgramInstructionSourceKind.Trigger, candidate.BindingId).Trigger!;

    internal IReadOnlyList<ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates<TQuery>(TQuery query,
        CharacterState owner,
        SkillProgramTriggerWindow window,
        SkillProgramTriggerFacts facts)
        where TQuery : IProgramTriggerCardsQuery, IProgramTriggerCollectionQuery, IProgramTriggerContentQuery, IProgramTriggerEligibilityQuery, IProgramTriggerWindowQuery
    {
        var candidates = query.IncludeYuanJiIssuedReturns(owner, window, Array.AsReadOnly(
            CollectProgramTriggerCandidates(query, owner, window).Where(c => !GetProgramTrigger(query, c).Effects.Any(e => query.IsYuanJiReturnOp(e.Op))).ToArray()));
        candidates = query.IncludeTriggerCandidate(owner, window, candidates);
        if (candidates.Count == 0) return candidates;
        return candidates
            .Where(candidate =>
            {
                var program = query.Program(candidate.SkillId);
                var plan = ProgramInstructionResolver.Default.Resolve(program,
                    ProgramInstructionSourceKind.Trigger, candidate.BindingId);
                var trigger = plan.Trigger!;
                return !query.IsPhaseNamePredictionResolver(candidate, program) && !trigger.DeferredTurnEndOnly && (!query.IsPhaseHandDebtResolver(candidate) || query.HasProgramPhaseHandDebt(candidate)) && (trigger.EvaluateConditionAtResolution || trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId) || IsAwakeningProgramConditionOverridden(query, new(owner, program, plan, candidate, null))) &&
                    HasInitialOwnedCardSelectionCandidates(query, owner, trigger) &&
                    query.CanTrigger(ProgramTriggerLegalityPhase.Collection,
                        new(owner, program, plan, candidate, null));
            })
            .ToArray();
    }

    private bool IsAwakeningProgramConditionOverridden<TQuery>(TQuery query, ProgramTriggerLegalityContext context)
        where TQuery : IProgramTriggerContentQuery, IProgramTriggerEligibilityQuery =>
        (query.SkillTags(context.SkillId) & SkillTag.Awakening) != 0 &&
        query.OverridesAwakeningCondition(context);

    private bool HasInitialOwnedCardSelectionCandidates<TQuery>(TQuery query, CharacterState owner, SkillProgramTrigger trigger)
        where TQuery : IProgramTriggerCardsQuery
    {
        if (ProgramInstructionResolver.Default.Features(trigger).FirstNonTargetSelection is not
                {
                    Op: SkillProgramEffectOp.SelectOwnedCards,
                    Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always
                } selection)
            return true;
        var required = selection.MinimumCards > 0 ? selection.MinimumCards : selection.Amount;
        if (required <= 0) return true;
        var available = selection.Zones.Sum(zone =>
            query.CardsAt(new CardLocation(zone, owner.Seat)).Count(card =>
                (selection.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(selection.CardKinds, card.Kind)) &&
                (selection.Suits.Count == 0 || selection.Suits.Contains(query.GetProgramEffectiveSuit(owner, card)))));
        return available >= required;
    }

    internal bool CanRunProgramTrigger<TQuery>(TQuery query,
        ProgramTriggerCandidate candidate,
        ProgramSkillWindowContext context)
        where TQuery : IProgramTriggerDamageQuery, IProgramTriggerCardsQuery, IProgramTriggerContentQuery, IProgramTriggerEligibilityQuery, IProgramTriggerSelectionQuery, IProgramTriggerUsageQuery, IProgramTriggerWindowQuery
    {
        if (!query.IsValidPlayerSeat(candidate.OwnerSeat) || candidate.OwnerSeat != context.OwnerSeat) return false;
        if (query.DecideIssued(candidate, context) is { } issuedDecision) return issuedDecision;
        var owner = query.Players[candidate.OwnerSeat];
        if (context.Window != SkillProgramTriggerWindow.OwnerDied && !owner.IsAlive && query.DecideTriggerProof(ProgramTriggerProofPhase.OwnerException, new CandidateTriggerProofRequest(candidate, context)) != true ||
            !query.HasRuntimeSkillInstance(owner, candidate.SkillId, candidate.SkillInstanceId) && query.DecideTriggerProof(ProgramTriggerProofPhase.InstanceException, new CandidateTriggerProofRequest(candidate, context)) != true ||
            query.FindProgram(candidate.SkillId) is not { } program ||
            program.GameplayHash != candidate.GameplayHash)
            return false;
        var trigger = ProgramInstructionResolver.Default.FindTrigger(program, candidate.BindingId);
        if (trigger is null || trigger.Window != context.Window || !query.CanPayProgramMarkerCost(owner, trigger.MarkerCost))
            return false;
        if (trigger.RequiredPolarity is { } polarity &&
            query.GetProgramConversionPolarity(owner.Seat, candidate.SkillId) != polarity) return false;
        if (query.DecideTriggerProof(ProgramTriggerProofPhase.DesignatedTarget, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) == true) return false;
        if (query.DecideTriggerProof(ProgramTriggerProofPhase.LegacyWindow, new CandidateTriggerProofRequest(candidate, context)) is { } legacyDecision) return legacyDecision;
        var plan = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, candidate.BindingId);
        var features = plan.Features;
        var legality = new ProgramTriggerLegalityContext(owner, program, plan, candidate, context);
        if (!query.CanTrigger(ProgramTriggerLegalityPhase.BeforeGainQualification, legality)) return false;
        if (trigger.OwnerZoneLoss is { } loss && !loss.Matches(candidate.OwnerSeat, context.MovementBatch)) return false;
        if (!query.IsGainPhaseQualified(trigger, candidate.OwnerSeat, context.MovementBatch?.MovementTiming)) return false;
        if (!query.CanTrigger(ProgramTriggerLegalityPhase.BeforeAdvancedPrerequisites, legality)) return false;
        if (!query.AdvancedTriggerPrerequisites(owner, candidate.SkillId, trigger)) return false;
        if (!query.CanTrigger(ProgramTriggerLegalityPhase.BeforeCondition, legality)) return false;
        if (trigger.NoDyingAtActivation && (query.ActiveDying is not null || query.Players.Any(p => p.IsAlive && p.Hp <= 0))) return false;
        if (!query.CanTrigger(ProgramTriggerLegalityPhase.AfterDyingCheck, legality)) return false;
        if (!IsAwakeningProgramConditionOverridden(query, legality) && !trigger.Condition.Evaluate(trigger.EvaluateConditionAtResolution ? query.CaptureProgramTriggerFacts(owner) : context.Facts ?? query.CaptureProgramTriggerFacts(owner),
                candidate.SkillId, candidate.SkillInstanceId))
            return false;
        if (!HasInitialOwnedCardSelectionCandidates(query, owner, trigger)) return false;
        if (!query.CanTrigger(ProgramTriggerLegalityPhase.AfterCondition, legality)) return false;
        if (query.IsConfirmedActualStartingSubstitution(context)) return false;
        if (trigger.UsageScope is { } scope && ProgramTriggerUsageLimit(query, trigger) is { } limit &&
            query.GetUsage(
                owner.Seat, candidate.SkillId, ProgramTriggerUsageId(query, candidate), scope) >= limit)
            return false;
        // Only an initial, unconditional payment is a prerequisite. A later payment
        // may intentionally use cards or participants produced by earlier nodes.
        var initialPayments = features.InitialDiscardPayments;
        if (initialPayments.Count > 1)
        {
            var available = features.InitialDiscardPaymentZones
                .SelectMany(zone => query.CardsAt(new CardLocation(zone, owner.Seat))
                    .Where(card => !query.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, new(zone, owner.Seat), OwnedCardMoveIntent.Discard))
                    .Where(card => zone != CardZoneKind.Equipment ||
                        !query.IsActiveProgramSourceEquipmentCard(owner.Seat, candidate.SkillId,
                            candidate.SkillInstanceId, card)))
                .Select(card => card.Id).Distinct().Count();
            if (available < features.InitialDiscardPaymentAmount) return false;
        }
        if (features.FirstNonTargetSelection is
            {
                Op: SkillProgramEffectOp.SelectAndMoveOwnedCard,
                Condition.Kind: SkillProgramConditionKind.Always,
                SkipIfNoCards: false
            } payment)
        {
            var payer = payment.CardOwnerRef?.Kind switch
            {
                ProgramParticipantRef.Owner => owner.Seat,
                ProgramParticipantRef.Actor => context.CardUse?.ActorSeat,
                ProgramParticipantRef.EventTarget => context.TargetSeat,
                _ => null
            };
            if (payer is { } payerSeat && (!query.IsValidPlayerSeat(payerSeat) ||
                !payment.Zones.Any(zone => query.CardsAt(new CardLocation(zone, payerSeat)).Any(card =>
                    (payment.CardCategories.Count == 0 ||
                     query.MatchesProgramCardCategory(card.Kind, payment.CardCategories)) &&
                    (zone != CardZoneKind.Equipment || payerSeat != owner.Seat ||
                     !query.IsActiveProgramSourceEquipmentCard(owner.Seat, candidate.SkillId,
                         candidate.SkillInstanceId, card))))))
                return false;
        }
        if ((features.HasOperation(SkillProgramEffectOp.GrantFactionPopulationMarker) ||
             program.Modifiers.Any(modifier => modifier.ValueExpression == SkillRuleValueExpression.OwnerMarkerCount) &&
             query.SkillTags(candidate.SkillId).HasFlag(SkillTag.Lord)) &&
            !query.HasSkillRoleQualification(owner, candidate.SkillId, candidate.SkillInstanceId, Role.Lord)) return false;
        if (!query.CanTrigger(ProgramTriggerLegalityPhase.AfterInitialPayments, legality)) return false;
        if (features.ForOperation(SkillProgramEffectOp.SelectTarget).Any(effect =>
            effect.Condition.Kind != SkillProgramConditionKind.ChoiceIs &&
            effect.TargetKind is { } kind && !query.WuLuKangProgramSelectionSeats(owner.Seat, effect, plan.Instructions, context).Any(seat =>
                effect.Zones.Count == 0 || effect.Zones.Any(zone => zone switch
                {
                    CardZoneKind.Hand => query.GetHand(query.Players[seat]).Count > 0,
                    CardZoneKind.Equipment => query.GetEquipment(query.Players[seat]).Count > 0,
                    CardZoneKind.Judgment => query.GetJudgment(query.Players[seat]).Count > 0,
                    _ => false
                }))))
            return false;
        if (features.ForOperation(SkillProgramEffectOp.SelectTargets).Any(effect =>
            effect.TargetKind == SkillProgramTargetKind.CurrentCardUseTargets &&
            query.GetProgramTargetSeats(owner.Seat, SkillProgramTargetKind.CurrentCardUseTargets, context).Count <
                effect.MinimumTargets))
            return false;
        if (features.ForOperation(SkillProgramEffectOp.SelectTargets).Any(effect => effect.TargetKind == SkillProgramTargetKind.LivingWhoseAttackRangeIncludesLord && query.GetProgramTargetSeats(owner.Seat, effect.TargetKind.Value, context).Count < effect.MinimumTargets)) return false;
        return CanRunProgramTriggerWindow(query, owner, trigger, features, candidate, context);
    }

    private bool CanRunProgramTriggerWindow<TQuery>(TQuery query,
        CharacterState owner, SkillProgramTrigger trigger, ProgramInstructionFeatures features,
        ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
        where TQuery : IProgramTriggerDamageQuery, IProgramTriggerCardsQuery, IProgramTriggerEligibilityQuery, IProgramTriggerSelectionQuery, IProgramTriggerWindowQuery
    {
        return context.Window switch
        {
            SkillProgramTriggerWindow.ActualPhaseEnded => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.PindianCardsRevealed => true,
            SkillProgramTriggerWindow.OtherActualUseTargeted => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.SkillsChanged => true,
            SkillProgramTriggerWindow.ProgramTargetCommitted => query.IsProgramTargetContextValid(context) && query.EnabledSkillPrograms(owner).Any(item => item.Id == candidate.SkillId),
            SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp or SkillProgramTriggerWindow.CharacterEnteredChain =>
                context.TargetSeat is { } changedSeat && query.Players[changedSeat].IsAlive &&
                query.Frames.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault() is { } stateWindow &&
                stateWindow.Id == context.ParentFrameId && stateWindow.OwnerSeat == changedSeat &&
                stateWindow.Window == context.Window && stateWindow.CharacterStateContinuation is not null,
            SkillProgramTriggerWindow.JudgmentPhaseStarting =>
                context.SourceSeat == query.CurrentSeat && context.TargetSeat == query.CurrentSeat &&
                (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own ? owner.Seat == query.CurrentSeat : owner.Seat != query.CurrentSeat) &&
                query.Frames.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault() is { } judgmentStart &&
                judgmentStart.Id == context.ParentFrameId && judgmentStart.Window == context.Window,
            SkillProgramTriggerWindow.TurnBeforeStart =>
                (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow =>
                context.SourceSeat == query.CurrentSeat &&
                (owner.Seat == query.CurrentSeat
                    ? context.SourceSeat == owner.Seat
                    : trigger.TurnOwnerScope is SkillProgramTurnOwnerScope.OtherLiving or SkillProgramTurnOwnerScope.AnyLiving &&
                      query.Players[query.CurrentSeat].IsAlive),
            SkillProgramTriggerWindow.IdentityGeneralAppeared => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.NativeDrawStarting => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.RoundEnding =>
                owner.IsAlive && query.Winner == Winner.None && query.IsExactRoundEndingCandidate(candidate, context),
            SkillProgramTriggerWindow.RoundStarting =>
                owner.IsAlive && context.SourceSeat == query.CurrentSeat && query.RoundNumber > 0,
            SkillProgramTriggerWindow.DrawPhaseStarting =>
                (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own && owner.Seat == query.CurrentSeat && context.SourceSeat == owner.Seat && CanRunDrawPhaseProgramTrigger(query, owner, trigger) ||
                 features.HasOperation(SkillProgramEffectOp.AddMarkerSubjectNormalDraw) && context.SourceSeat==query.CurrentSeat && context.TargetSeat==query.CurrentSeat ||
                 query.DecideTriggerProof(ProgramTriggerProofPhase.ForeignDraw, new CandidateTriggerProofRequest(candidate, context)) == true) && query.Phase == TurnPhase.Draw,
            SkillProgramTriggerWindow.HappyGuardRemoved => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.HappyPlanSettled => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.DyingExited => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.CardEffectBeforeApply => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.AfterTurnEnded =>
                query.CanRunAfterTurnEndedCandidate(candidate, context, trigger),
            SkillProgramTriggerWindow.GameStarting =>
                query.Frames.OfType<ProgramLifecycleTriggerWindowFrame>()
                    .Any(frame => frame.Id == context.ParentFrameId && frame.Window == SkillProgramTriggerWindow.GameStarting &&
                        (query.Phase == TurnPhase.NotStarted || query.DecideTriggerProof(ProgramTriggerProofPhase.GameStart, new GameStartTriggerProofRequest(frame)) == true)),
            SkillProgramTriggerWindow.DrawPhaseSkipped => query.IsActualSkippedDrawContext(context),
            SkillProgramTriggerWindow.AfterNormalDraw or SkillProgramTriggerWindow.DrawPhaseEnded =>
                owner.Seat == query.CurrentSeat && context.SourceSeat == owner.Seat && query.Phase == TurnPhase.Draw ||
                context.Window == SkillProgramTriggerWindow.DrawPhaseEnded &&
                    (query.DecideTriggerProof(ProgramTriggerProofPhase.ForeignDrawEnd, new CandidateTriggerProofRequest(candidate, context)) == true),
            SkillProgramTriggerWindow.PlayEnding =>
                context.SourceSeat == query.CurrentSeat && query.Phase == TurnPhase.Play &&
                (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own
                    ? owner.Seat == query.CurrentSeat
                    : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.AnyLiving ||
                      owner.Seat != query.CurrentSeat),
            SkillProgramTriggerWindow.PlayPhaseStarting =>
                context.SourceSeat == query.CurrentSeat && query.Phase == TurnPhase.Play &&
                (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own
                    ? owner.Seat == query.CurrentSeat
                    : owner.Seat != query.CurrentSeat && query.Players[query.CurrentSeat].IsAlive) &&
                query.Frames.OfType<PlayPhaseStartingBoundaryFrame>().LastOrDefault() is { } starting &&
                starting.Id == context.ParentFrameId && starting.OwnerSeat == query.CurrentSeat,
            SkillProgramTriggerWindow.TurnEnding =>
                context.SourceSeat == query.CurrentSeat && context.TargetSeat == query.CurrentSeat &&
                (query.DecideTriggerProof(ProgramTriggerProofPhase.DeadOwnerEnding, new DeadOwnerTriggerProofRequest(candidate)) == true || (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.IssuedFixedDistanceEnding
                    ? query.MatchesFixedDistanceDebt(candidate, context, false)
                    : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.PaidPrepDiscardEnding
                    ? query.MatchesPrepDiscardEnding(candidate, context, false)
                    : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.EarnedActualEnding
                    ? query.MatchesEarnedActualEnding(candidate, context, false)
                    : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.OwnOrPreviousLiving
                    ? query.MatchesFrozenOwnOrPreviousEnding(candidate, context)
                    : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own
                        ? owner.Seat == query.CurrentSeat
                        : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.AnyLiving
                        ? query.Players[query.CurrentSeat].IsAlive
                        : owner.Seat != query.CurrentSeat && query.Players[query.CurrentSeat].IsAlive)) &&
                query.Frames.OfType<TurnEndingBoundaryFrame>().LastOrDefault() is { } ending &&
                ending.Id == context.ParentFrameId && ending.OwnerSeat == query.CurrentSeat,
            SkillProgramTriggerWindow.DyingEntering =>
                query.ActiveDying is { } entering && entering.VictimSeat == context.TargetSeat &&
                (trigger.Subject == SkillProgramTriggerSubject.Any || entering.VictimSeat == owner.Seat) &&
                (query.Players[entering.VictimSeat].Hp <= 0 || query.HasXuRongDyingQualification(candidate, context)) &&
                query.Frames.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault() is { } entry &&
                entry.Id == context.ParentFrameId && entry.ResumeDyingFrameId == entering.FrameId,
            SkillProgramTriggerWindow.SelfDyingResponse =>
                query.ActiveDying is { } dying && dying.FrameId == context.ParentFrameId &&
                dying.VictimSeat == owner.Seat && dying.ResponderSeat == owner.Seat && owner.Hp <= 0,
            SkillProgramTriggerWindow.DyingResponse =>
                query.ActiveDying is { } currentDying && currentDying.FrameId == context.ParentFrameId &&
                currentDying.VictimSeat == context.TargetSeat && currentDying.ResponderSeat == owner.Seat &&
                query.Players[currentDying.VictimSeat].Hp <= 0,
            SkillProgramTriggerWindow.BeforeDamageApplied =>
                query.Frames.OfType<BeforeDamageProgramWindowFrame>().LastOrDefault() is { } beforeDamage &&
                beforeDamage.Id == context.ParentFrameId &&
                beforeDamage.TargetSeat == context.TargetSeat &&
                context.SourceSeat == (query.BeforeDamageHasNoSource(beforeDamage) ? null : beforeDamage.SourceSeat) &&
                beforeDamage.Amount == context.Amount &&
                !beforeDamage.Prevented && beforeDamage.RedirectedTargetSeat is null &&
                (trigger.Subject == SkillProgramTriggerSubject.DamageTarget
                    ? owner.Seat == beforeDamage.TargetSeat &&
                      !(features.HasOperation(SkillProgramEffectOp.RedirectCurrentDamage) &&
                        query.DamageRedirected)
                    : trigger.Subject == SkillProgramTriggerSubject.DamageSource
                        ? !query.BeforeDamageHasNoSource(beforeDamage) && owner.Seat == beforeDamage.SourceSeat &&
                          (owner.Seat != beforeDamage.TargetSeat || features.HasOperation(SkillProgramEffectOp.IncreaseActualSourceDamageByYuqing) ||
                           features.HasOperation(SkillProgramEffectOp.ConsumeZengdaoForActualSourceDamage) && query.IsIssuedZengdaoCandidate(candidate, context))
                        : trigger.Subject == SkillProgramTriggerSubject.Any || owner.Seat != beforeDamage.TargetSeat),
            SkillProgramTriggerWindow.DamageAppliedBeforeDying or
                SkillProgramTriggerWindow.AfterDamageApplied =>
                context.Amount > 0 && trigger.Subject switch
                {
                    SkillProgramTriggerSubject.Owner => context.TargetSeat == owner.Seat,
                    SkillProgramTriggerSubject.DamageTarget => context.Window == SkillProgramTriggerWindow.DamageAppliedBeforeDying &&
                        features.HasOperation(SkillProgramEffectOp.DrawByDamageCardColor) && context.TargetSeat == owner.Seat,
                    SkillProgramTriggerSubject.Source =>
                        context.SourceSeat == owner.Seat &&
                        query.ActiveDamageTrigger is { } sourceDamage &&
                        query.DamageConversionSource(sourceDamage) is { } source &&
                        string.Equals(source.SkillId, trigger.SourceSkillId, StringComparison.Ordinal) &&
                        (trigger.SourceViewAsId is null ||
                         string.Equals(source.BindingId, trigger.SourceViewAsId, StringComparison.Ordinal)),
                    SkillProgramTriggerSubject.DamageSource => context.SourceSeat == owner.Seat,
                    SkillProgramTriggerSubject.Any => true,
                    _ => false
                } &&
                CanRunAfterDamageProgramTrigger(query, owner, trigger, context),
            SkillProgramTriggerWindow.OtherActualTurnStarted => (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.AfterHpLost or SkillProgramTriggerWindow.AfterHpRecovered or SkillProgramTriggerWindow.AfterHealthChanged =>
                context.HpChange is { } change &&
                (query.DecideTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, new HpTriggerProofRequest(owner, trigger, candidate, context, change)) ?? change.TargetSeat == owner.Seat) && change.Amount > 0 &&
                query.Frames.OfType<HpChangedTriggerWindowFrame>().LastOrDefault() is { } hpWindow &&
                hpWindow.Id == context.ParentFrameId && hpWindow.Change == change &&
                hpWindow.Candidates[hpWindow.CandidateIndex] == candidate,
            SkillProgramTriggerWindow.CardsMoved or SkillProgramTriggerWindow.CardsGained =>
                context.MovementBatch is { } batch &&
                batch.Id == context.ParentFrameId &&
                query.Frames.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()?.Id == batch.Id &&
                (query.DecideTriggerProof(ProgramTriggerProofPhase.MovementOwner, new MovementTriggerProofRequest(owner, trigger, features, candidate, context, batch)) ??
                    query.ProgramMovementSourceCounts(batch, context.Window).Any(item => item.Count.Location.OwnerSeat == owner.Seat &&
                        (!item.DiscardOriginOnly || trigger.MovementDiscardOnly))),
            SkillProgramTriggerWindow.FirstGameDomainCrossing =>
                context.MovementBatch is { } domainBatch && domainBatch.Id == context.ParentFrameId &&
                query.Frames.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()?.Id == domainBatch.Id &&
                query.FirstDomainFact(context) is not null,
            SkillProgramTriggerWindow.DiscardPileReceived =>
                context.MovementBatch is { } batch &&
                batch.Id == context.ParentFrameId &&
                query.Frames.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()?.Id == batch.Id &&
                query.MatchingDiscardPileIndexes(batch, candidate, trigger).Length > 0,
            SkillProgramTriggerWindow.OwnerDied =>
                !owner.IsAlive &&
                query.Frames.OfType<DeathFrame>().LastOrDefault() is { } death &&
                death.VictimSeat == owner.Seat &&
                death.KillerSeat == context.SourceSeat &&
                query.Frames.OfType<ProgramDeathTriggerWindowFrame>().LastOrDefault() is { } deathWindow &&
                deathWindow.Id == context.ParentFrameId &&
                deathWindow.DeathFrameId == death.Id &&
                deathWindow.OwnerSeat == owner.Seat &&
                deathWindow.Candidates[deathWindow.CandidateIndex] == candidate,
            SkillProgramTriggerWindow.CharacterDied =>
                owner.IsAlive &&
                query.Frames.OfType<DeathFrame>().LastOrDefault() is { } death &&
                death.KillerSeat == context.SourceSeat &&
                death.VictimSeat == context.TargetSeat &&
                query.Frames.OfType<ProgramKillTriggerWindowFrame>().LastOrDefault() is { } killWindow &&
                killWindow.Id == context.ParentFrameId &&
                killWindow.DeathFrameId == death.Id &&
                killWindow.CandidateIndex < killWindow.Candidates.Count &&
                killWindow.Candidates[killWindow.CandidateIndex] == candidate &&
                killWindow.Contexts[killWindow.CandidateIndex] == context,
            SkillProgramTriggerWindow.JudgmentFinalized or SkillProgramTriggerWindow.JudgmentBeforeResult =>
                context.Judgment is { } finalized &&
                query.ActiveJudgment is { } pendingJudgment &&
                finalized.JudgmentFrameId == pendingJudgment.Id &&
                query.Frames.LastOrDefault() is ProgramJudgmentTriggerWindowFrame judgmentWindow &&
                (judgmentWindow.ZhenyiBeforeResult == true) == (context.Window == SkillProgramTriggerWindow.JudgmentBeforeResult) &&
                judgmentWindow.Id == context.ParentFrameId &&
                judgmentWindow.CandidateIndex < judgmentWindow.Candidates.Count &&
                judgmentWindow.Candidates[judgmentWindow.CandidateIndex] ==
                    new ProgramJudgmentTriggerCandidate(candidate.OwnerSeat, candidate.SkillId,
                        candidate.BindingId, candidate.SkillInstanceId, candidate.GameplayHash) &&
                query.CanRunProgramJudgmentTrigger(judgmentWindow.Candidates[judgmentWindow.CandidateIndex],
                    trigger, finalized),
            SkillProgramTriggerWindow.JudgmentReplacing =>
                context.JudgmentReplacement is { } replacement &&
                query.ActiveJudgment is { } currentJudgment &&
                currentJudgment.Id == context.ParentFrameId &&
                replacement.JudgmentFrameId == currentJudgment.Id &&
                query.CurrentJudgmentCandidate(currentJudgment) is { } replacementCandidate &&
                replacementCandidate.OwnerSeat == candidate.OwnerSeat &&
                replacementCandidate.ProgramId == candidate.SkillId &&
                replacementCandidate.ProgramTriggerId == candidate.BindingId &&
                replacementCandidate.SkillInstanceId == candidate.SkillInstanceId &&
                query.GetJudgmentCard(currentJudgment)?.Id == replacement.OldCardId &&
                query.Frames.LastOrDefault() is JudgmentFrame replacementFrame &&
                replacementFrame.Id == currentJudgment.Id,
            SkillProgramTriggerWindow.CardUseCommitted or
                SkillProgramTriggerWindow.CardUseBeforeTargetEffects or
                SkillProgramTriggerWindow.CardUseTargetsFinalized or
                SkillProgramTriggerWindow.CardResponseAccepted or
                SkillProgramTriggerWindow.CardResponseCompleted or SkillProgramTriggerWindow.CardSupplyCompleted or
                SkillProgramTriggerWindow.CardUseCompleted =>
                context.CardUse is { } cardUse &&
                query.Frames.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault() is { } cardFrame &&
                cardFrame.Id == context.ParentFrameId && cardFrame.Action.ActionId == cardUse.CardActionId &&
                cardFrame.Candidates.Any(item => item.OwnerSeat == candidate.OwnerSeat &&
                    item.SkillId == candidate.SkillId && item.SkillInstanceId == candidate.SkillInstanceId &&
                    item.TriggerId == candidate.BindingId && item.GameplayHash == candidate.GameplayHash),
            SkillProgramTriggerWindow.SlashTargetRedirecting or
                SkillProgramTriggerWindow.SlashBeforeResponse or
                SkillProgramTriggerWindow.SlashFullyDodged =>
                context.CardUse is { } slashUse &&
                query.Frames.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault() is { } slashFrame &&
                slashFrame.Id == context.ParentFrameId && slashFrame.Action.ActionId == slashUse.CardActionId &&
                query.ActiveSlashTargetSeat is { } pendingSlashTarget && pendingSlashTarget == context.TargetSeat &&
                slashFrame.Candidates.Any(item => item.OwnerSeat == candidate.OwnerSeat &&
                    item.SkillId == candidate.SkillId && item.SkillInstanceId == candidate.SkillInstanceId &&
                    item.TriggerId == candidate.BindingId && item.GameplayHash == candidate.GameplayHash),
            SkillProgramTriggerWindow.PindianResultDetermined =>
                (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.DuelResponseFailed =>
                (query.DecideTriggerProof(ProgramTriggerProofPhase.NativeWindow, new NativeWindowTriggerProofRequest(owner, candidate, context, trigger)) ?? false),
            SkillProgramTriggerWindow.DiscardPhaseStarting =>
                context.SourceSeat == query.CurrentSeat && query.Phase == TurnPhase.Discard &&
                (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own
                    ? owner.Seat == query.CurrentSeat
                    : owner.Seat != query.CurrentSeat && query.Players[query.CurrentSeat].IsAlive),
            SkillProgramTriggerWindow.DiscardPhaseEnded =>
                (trigger.AllowOwnDiscardPhaseEnded ? owner.Seat == query.CurrentSeat : owner.Seat != query.CurrentSeat) && owner.IsAlive &&
                context.SourceSeat == query.CurrentSeat && query.Phase == TurnPhase.Discard,
            _ => false
        };
    }

    private bool CanRunDrawPhaseProgramTrigger<TQuery>(TQuery query, CharacterState owner, SkillProgramTrigger trigger)
        where TQuery : IProgramTriggerCardsQuery, IProgramTriggerEligibilityQuery, IProgramTriggerSelectionQuery, IProgramTriggerWindowQuery
    {
        foreach (var effect in trigger.Effects.Where(effect =>
                     effect.Op is (SkillProgramEffectOp.SelectTargets or SkillProgramEffectOp.SelectTarget) &&
                     effect.Condition.Evaluate(query.CreateSkillContext(owner))))
        {
            if (effect.Op == SkillProgramEffectOp.SelectTargets &&
                (effect.TargetKind is not { } targetKind ||
                 query.GetProgramTargetSeats(owner.Seat, targetKind, marker: effect.Marker).Count < effect.MinimumTargets ||
                 effect.NumberExpression == SkillProgramNumberExpression.PlannedNormalDrawCount &&
                 (query.Frames.LastOrDefault() is not ProgramLifecycleTriggerWindowFrame drawPlan ||
                  drawPlan.NormalDrawReplaced ||
                  Math.Max(0, checked((drawPlan.FrozenBaseDrawCount ?? query.GetTurnDrawCount(owner)) +
                      drawPlan.NormalDrawAdjustment)) <
                  effect.MinimumTargets)))
                return false;
            if (effect.Op == SkillProgramEffectOp.SelectTarget && effect.TargetKind is { } singleKind &&
                !query.WuLuKangProgramSelectionSeats(owner.Seat, effect, trigger.Effects).Any(seat =>
                    effect.Zones.Count == 0 || effect.Zones.Any(zone => zone switch
                    {
                        CardZoneKind.Hand => query.GetHand(query.Players[seat]).Count > 0,
                        CardZoneKind.Equipment => query.GetEquipment(query.Players[seat]).Count > 0,
                        CardZoneKind.Judgment => query.GetJudgment(query.Players[seat]).Count > 0,
                        _ => false
                    })))
                return false;
        }
        return true;
    }

    private bool CanRunAfterDamageProgramTrigger<TQuery>(TQuery query,
        CharacterState owner,
        SkillProgramTrigger trigger,
        ProgramSkillWindowContext context)
        where TQuery : IProgramTriggerDamageQuery, IProgramTriggerCardsQuery, IProgramTriggerEligibilityQuery, IProgramTriggerSelectionQuery, IProgramTriggerWindowQuery
    {
        if (query.ActiveDamageTrigger is not { } damage ||
            damage.Id != context.ParentFrameId ||
            damage.ParentFrameId != context.DamageFrameId ||
            damage.TriggerWindow != context.Window)
            return false;
        if (trigger.RequireDamageSource == true &&
            (context.SourceSeat is not { } presentSourceSeat || !query.IsValidPlayerSeat(presentSourceSeat) ||
             query.IsDamageSourceLess(damage) || query.IsDamageDelayedJudgment(damage)))
            return false;
        if (trigger.DamageCardKinds.Count > 0 &&
            (query.EffectiveDamageCardKind(damage) is not { } kind || !ProgramCardKindMatch.Contains(trigger.DamageCardKinds, kind)))
            return false;
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.StartPindian) &&
            (context.TargetSeat is not { } opponentSeat ||
             !query.IsValidPlayerSeat(opponentSeat) || !owner.IsAlive || !query.Players[opponentSeat].IsAlive ||
             !features.Legality.CanStart(query.CreateProgramLegalityParticipant(owner.Seat)) ||
             !features.Legality.CanSelectTarget(query.CreateProgramLegalityParticipant(owner.Seat),
                 query.CreateProgramLegalityParticipant(opponentSeat))))
            return false;
        foreach (var effect in features.AfterDamagePrerequisites.Where(effect =>
                     effect.Condition.Evaluate(query.CreateSkillContext(owner))))
        {
            switch (effect.Op)
            {
                case SkillProgramEffectOp.SelectTarget:
                    if (effect.TargetKind is not { } targetKind ||
                        query.WuLuKangProgramSelectionSeats(owner.Seat, effect, trigger.Effects, context).Count == 0)
                        return false;
                    break;
                case SkillProgramEffectOp.SelectSourceCard:
                    if (effect.SkipIfNoCards) break;
                    var sourceSeat = effect.CardSource switch
                    {
                        SkillProgramCardSource.Owner => owner.Seat,
                        SkillProgramCardSource.DamageSource => context.SourceSeat,
                        SkillProgramCardSource.EventTarget => context.TargetSeat,
                        _ => null
                    };
                    if (sourceSeat is not { } resolvedSourceSeat ||
                        (effect.CardSource == SkillProgramCardSource.DamageSource && resolvedSourceSeat == owner.Seat &&
                         !effect.AllowSameSource) ||
                        !query.IsValidPlayerSeat(resolvedSourceSeat) ||
                        !effect.Zones.Any(zone => zone switch
                        {
                            CardZoneKind.Hand => query.GetHand(query.Players[resolvedSourceSeat]).Count > 0,
                            CardZoneKind.Equipment => query.GetEquipment(query.Players[resolvedSourceSeat]).Any(card =>
                                effect.EquipmentSlots.Count == 0 ||
                                effect.EquipmentSlots.Contains(query.FieldEquipmentSlot(card))),
                            _ => false
                        }))
                        return false;
                    break;
                case SkillProgramEffectOp.ClaimDamageCards:
                    if (!query.PhysicalDamageCards(damage).Any(card =>
                            query.CardLocation(card.Id) == CardLocation.Processing))
                        return false;
                    break;
            }
        }
        return true;
    }

    internal void ConsumeProgramTriggerUsage<TQuery>(TQuery query, ProgramTriggerCandidate candidate)
        where TQuery : IProgramTriggerContentQuery, IProgramTriggerUsageQuery
    {
        var trigger = query.Program(candidate.SkillId).Triggers
            .Single(item => item.Id == candidate.BindingId);
        if (trigger.UsageScope is not { } scope || ProgramTriggerUsageLimit(query, trigger) is not { } limit) return;
        if (!query.TryConsumeUsage(
                candidate.OwnerSeat, candidate.SkillId, ProgramTriggerUsageId(query, candidate), scope, limit))
            throw new InvalidOperationException("The configured trigger usage was already consumed.");
        query.RecordUsageConsumed(new SkillUsageConsumedEvent(
            candidate.OwnerSeat, candidate.SkillId, candidate.BindingId, scope, 1));
    }

    internal string ProgramTriggerUsageId<TQuery>(TQuery query, ProgramTriggerCandidate candidate)
        where TQuery : IProgramTriggerContentQuery
    {
        var trigger = GetProgramTrigger(query, candidate);
        if (trigger.NamedUsageGroup is { } named) return $"named-group:{named}";
        if (trigger.DynamicUsageLimit is not null) return $"named-binding:{candidate.BindingId}";
        var usageBinding = trigger.ChoiceGroup is { } choiceGroup
            ? $"choice-group:{choiceGroup}"
            : candidate.BindingId;
        return $"{usageBinding}@{candidate.SkillInstanceId}";
    }

    internal int? ProgramTriggerUsageLimit<TQuery>(TQuery query, SkillProgramTrigger trigger)
        where TQuery : IProgramTriggerUsageQuery =>
        trigger.DynamicUsageLimit is { Kind: SkillProgramDynamicUsageLimitKind.AlivePlayersCapped } dynamic
            ? Math.Min(dynamic.Cap, query.AlivePlayerCount) : trigger.UsageLimit;

}
