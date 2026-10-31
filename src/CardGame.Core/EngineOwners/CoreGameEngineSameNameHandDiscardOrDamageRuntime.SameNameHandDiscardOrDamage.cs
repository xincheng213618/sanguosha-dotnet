#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageRuntime
{
    internal string SameNameHandDiscardReason(ProgramSkillFrame f) => $"skill-program.{(f.SkillId)}.same-name-hand.discard";
    internal int[] SameNameHandTargets(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => p.Seat).ToArray();
    internal SameNameHandMaterial[] SameNameHandMaterials(int seat, CardKind name) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(c => CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(c.Kind) == name && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard)).Select(c => new SameNameHandMaterial(c.Id, c.Kind, CardLocation.Hand(seat))).ToArray();
    internal bool SameNameHandActionParent(int owner, ProgramSkillWindowContext context, out ProgramCardTriggerWindowFrame window)
    {
        window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)!;
        if (window is null || context.OwnerSeat != owner || context.CardUse is not { } use || use.ActorSeat != owner || (window.CardSupplyCompletion?.ProviderSeat ?? window.ResponseCompletion?.CompletionActorSeat ?? window.Action.ActorSeat) != owner || use.CardActionId != window.Action.ActionId || use.EffectiveKind != window.Action.EffectiveKind || use.ParentCardUseFrameId != window.ParentFrameId || context.Window != CoreGameEngineCardActionsRuntime.GetCardActionWindow(window))
            return false;
        if (context.Window == SkillProgramTriggerWindow.CardResponseCompleted)
            return window.ResponseCompletion is { CostsDrained: true } && _coreResponseCompletionOperations.ValidResponseCompletionWindow(window);
        if (context.Window == SkillProgramTriggerWindow.CardSupplyCompleted)
            return window.CardSupplyCompletion is { CostsDrained: true } && _coreCardSupplyCompletionOperations.ValidCardSupplyCompletionWindow(window);
        return context.Window == SkillProgramTriggerWindow.CardUseCompleted && window.CardSupplyCompletion is null && window.ResponseCompletion is null && window.CompletedResponseReturn is null && window.Action.Type == CardActionType.Use && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { } card && card.Action?.ActionId == window.Action.ActionId && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == card.Id && e.CardKind == use.EffectiveKind) == 1;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1050)]
    internal bool CanOfferSameNameHandDiscardOrDamage(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferSameNameHandDiscardOrDamage))
            return true;
        return _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && candidate.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && SameNameHandActionParent(candidate.OwnerSeat, context, out _) && SameNameHandTargets(candidate.OwnerSeat).Length > 0;
    }

    internal SkillProgramStepOutcome BeginSameNameHandDiscardOrDamage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (f.InstructionIndex != 1 || f.SameNameHandDiscardOrDamage is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || plan.Trigger is not { } trigger || trigger.Effects is not [{ Op: SkillProgramEffectOp.OfferSameNameHandDiscardOrDamage }] || f.WindowContext is not { } context || !SameNameHandActionParent(f.OwnerSeat, context, out var window) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != window.Id || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
            throw new InvalidOperationException("A same-name hand demand requires its exact completed actor candidate.");
        if (!CanOfferSameNameHandDiscardOrDamage(CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex]), trigger, context))
            return SkillProgramStepOutcome.Continue;
        var a = window.Action;
        var r = new ProgramSameNameHandReceipt
        {
            Source = new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            ActionId = a.ActionId,
            CardWindowId = window.Id,
            OriginalParentFrameId = window.ParentFrameId,
            EffectiveKind = a.EffectiveKind,
            NormalizedName = CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(a.EffectiveKind),
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            Stage = SameNameHandStage.ChoosingTarget,
            CandidateSeats = SameNameHandTargets(f.OwnerSeat)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameNameHandDiscardOrDamage = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameNameHandStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActionId, r.CardWindowId, r.OriginalParentFrameId, r.EffectiveKind, r.NormalizedName, r.ActualTurnNumber, r.ActualTurnOwnerSeat));
        PublishSameNameHandChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice SameNameHandChoice(ProgramSkillFrame f, string step, string label, int? card = null, int? target = null) => new(new($"same-name-hand.{(f.Id)}.{(step)}.{(card ?? target ?? -1)}"), label, card is { } c ? [c] : [], target is { } t ? [t] : [], new Dictionary<string, string> { ["program-action"] = "same-name-hand", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["step"] = step });
    internal IReadOnlyList<PromptChoice> SameNameHandChoices(ProgramSkillFrame f)
    {
        var r = f.SameNameHandDiscardOrDamage!;
        if (r.Stage == SameNameHandStage.ChoosingTarget)
            return Array.AsReadOnly(r.CandidateSeats.Select(s => SameNameHandChoice(f, "target", $"令{(_coreMatchParticipantsPlayersAccess._players[s].Name)}弃置同名手牌或受到1点伤害", target: s)).ToArray());
        if (r.Stage != SameNameHandStage.ChoosingPayment)
            return[];
        return Array.AsReadOnly(r.EligibleMaterials.Select(m => SameNameHandChoice(f, "discard", $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】", card: m.CardId)).Append(SameNameHandChoice(f, "damage", "受到1点伤害")).ToArray());
    }

    internal void PublishSameNameHandChoice(ProgramSkillFrame f)
    {
        var r = f.SameNameHandDiscardOrDamage!;
        var chooser = r.Stage == SameNameHandStage.ChoosingTarget ? f.OwnerSeat : r.TargetSeat!.Value;
        var choices = SameNameHandChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == SameNameHandStage.ChoosingTarget ? "选择一名有手牌的其他角色。" : $"弃置一张【{(CardCatalog.Get(r.NormalizedName).DisplayName)}】手牌，或受到1点伤害。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("same-name-hand")]
    internal void ResolveSameNameHandDiscardOrDamageChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A same-name demand lost its owning program.");
        var r = f.SameNameHandDiscardOrDamage!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !_coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandDiscardOrDamageChoice(f, prompt) || !SameNameHandChoices(f).Any(c => SameNameHandChoicesEqual(c, choice)))
            throw new InvalidOperationException("A same-name demand requires its current private published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == SameNameHandStage.ChoosingTarget)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            {
                FinishSameNameHand(f);
                return;
            }

            var target = choice.Targets.Single();
            if (!SameNameHandTargets(f.OwnerSeat).Contains(target))
                throw new InvalidOperationException("The original target must still be another living hand owner.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameNameHandDiscardOrDamage = r with { Stage = SameNameHandStage.ChoosingPayment, TargetSeat = target, EligibleMaterials = SameNameHandMaterials(target, r.NormalizedName) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameNameHandTargetSelectedEvent(f.Id, target));
            PublishSameNameHandChoice(f);
            return;
        }

        if (r.Stage != SameNameHandStage.ChoosingPayment || r.TargetSeat is not { } seat)
            throw new InvalidOperationException("A same-name demand changed its original payment stage.");
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishSameNameHand(f);
            return;
        }

        if (choice.Parameters["step"] == "damage")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameNameHandDiscardOrDamage = r with { Stage = SameNameHandStage.DamageIssued, DamageIssued = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameNameHandDamageIssuedEvent(f.Id, f.OwnerSeat, seat, 1));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, seat, 1);
            return;
        }

        var material = r.EligibleMaterials.Single(m => m.CardId == choice.Cards.Single());
        if (!SameNameHandMaterials(seat, r.NormalizedName).Contains(material))
            throw new InvalidOperationException("The same-name payment must remain its exact legal hand entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameNameHandDiscardOrDamage = r with { Stage = SameNameHandStage.DiscardChildren, PaidMaterial = material, SequenceBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence, SequenceAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(seat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, seat, [material.CardId], new(SameNameHandDiscardReason(f)), (batch, records) =>
        {
            if (records is not [var paid] || paid.CardId != material.CardId || paid.CardKind != material.PrintedKind || paid.From != material.From || paid.To != CardLocation.DiscardPile && !_coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(paid, material.From, new(SameNameHandDiscardReason(f))))
                throw new InvalidOperationException("The same-name demand did not commit exactly its selected hand payment.");
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = active.SameNameHandDiscardOrDamage!;
            _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { SameNameHandDiscardOrDamage = receipt with { SequenceAfter = paid.Sequence, BatchId = batch } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameNameHandDiscardPaidEvent(active.Id, seat, material.CardId, receipt.SequenceBefore, paid.Sequence, batch));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal static bool SameNameHandChoicesEqual(PromptChoice a, PromptChoice b) => RuntimeSkills.RuntimeRules_Core_SameNameHandDiscardOrDamage.SameNameHandChoicesEqual(a, b);
    [ProgramContinuation(ProgramContinuationKind.Resume, 170)]
    internal bool ResumeSameNameHandDiscardOrDamage(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SameNameHandDiscardOrDamage: { } r } f || f.Id != id)
            return false;
        if (r.Stage is SameNameHandStage.ChoosingTarget or SameNameHandStage.ChoosingPayment)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishSameNameHandChoice(f);
            return true;
        }

        if (r.Stage == SameNameHandStage.DiscardChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        }

        if (r.Stage == SameNameHandStage.DamageIssued && f.AttackAttempt is not null)
            return false;
        FinishSameNameHand(f);
        return true;
    }

    internal void FinishSameNameHand(ProgramSkillFrame f)
    {
        var r = f.SameNameHandDiscardOrDamage!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameNameHandDiscardOrDamage = r with { Stage = SameNameHandStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameNameHandCompletedEvent(f.Id, r.TargetSeat, r.PaidMaterial is not null, r.DamageIssued));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 150)]
    internal bool ReturnSameNameHandDiscardOrDamageMovement(ProgramSkillFrame f)
    {
        if (f.SameNameHandDiscardOrDamage is not { Stage: SameNameHandStage.DiscardChildren } || f.PendingMovementContinuation is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 520)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 540)]
    internal bool CanContinueSameNameHandDiscardOrDamage(ProgramSkillFrame f) => f.SameNameHandDiscardOrDamage is { Stage: SameNameHandStage.DiscardChildren or SameNameHandStage.DamageIssued or SameNameHandStage.Complete } && _coreSameNameHandDiscardOrDamageObserversOperations.ValidSameNameHandReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.OfferSameNameHandDiscardOrDamage)]
    internal PromptChoice SelectAiSameNameHandDiscardOrDamage(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.SameNameHandDiscardOrDamage!.Stage == SameNameHandStage.ChoosingTarget)
        {
            var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
            var harm = new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
            return decision.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(snapshot, c.Targets.Single(), harm)).ThenBy(c => c.Targets.Single()).First();
        }

        var discard = decision.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat])).ThenBy(c => c.Cards[0]).FirstOrDefault();
        return discard ?? decision.Choices.Single(c => c.Parameters["step"] == "damage");
    }

    SkillProgramStepOutcome ISameNameHandDiscardOrDamageProgramHost.OfferSameNameHandDiscardOrDamage(ProgramSkillFrame f) => BeginSameNameHandDiscardOrDamage(f);
}
