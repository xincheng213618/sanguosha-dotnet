#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitsRuntime
{
    internal const string FirstSlashDuelDrawReason = "skill-program.first-slash-duel.draw";
    internal bool HasFirstSlashDuelBenefitCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseFirstSlashDuelDiamondBenefit);

    internal static bool IsFirstSlashDuelKind(CardKind kind) => RuntimeSkills.RuntimeRules_Core_FirstSlashDuelBenefits.IsFirstSlashDuelKind(kind);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 260)]
    internal void ObserveFirstSlashDuelDeclaration(IGameEvent payload)
    {
        if (!HasFirstSlashDuelBenefitCapability || payload is not CardUseDeclaredEvent declared || !_coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn || declared.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !IsFirstSlashDuelKind(declared.CardKind))
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId);
        if (use is null || use.SourceSeat != declared.SourceSeat || use.CardKind != declared.CardKind || use.CardId != declared.CardId || use.Action is { Type: not CardActionType.Use } || use.Action is { } action && (action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind))
            throw new InvalidOperationException("The first Slash/Duel ledger lost its actual declaration.");
        if (use.Action is null && !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            return;
        var old = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstSlashDuelUseRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.ActorSeat == use.SourceSeat).ToArray();
        if (old.Any(e => e.CardUseFrameId == use.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstSlashDuelUseRecordedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, use.SourceSeat, use.Id, use.Action?.ActionId, use.CardKind, checked(old.Length + 1)));
    }

    internal bool IsFirstSlashDuelAction(int owner, CardActionContext action, CardUseFrame use) => _coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn && owner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && action.Type == CardActionType.Use && action.ActorSeat == owner && IsFirstSlashDuelKind(action.EffectiveKind) && use.SourceSeat == owner && use.CardKind == action.EffectiveKind && use.Action?.ActionId == action.ActionId && use.TargetSeats is [var target] && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstSlashDuelUseRecordedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.ActorSeat == owner && e.CardUseFrameId == use.Id && e.ActionId == action.ActionId && FirstSlashDuelDeclaredKindMatches(use, e.EffectiveKind) && e.Ordinal == 1);
    internal bool FirstSlashDuelDeclaredKindMatches(CardUseFrame use, CardKind declaredKind)
    {
        if (declaredKind == use.CardKind)
            return true;
        if (use.CurrentSlashFirePolicy is not { Converted: true } policy || policy.OriginalKind != declaredKind || !CoreNativeCardClassificationsRuntime.IsSlashCard(declaredKind) || use.CardKind != CardKind.FireSlash)
            return false;
        // Declaration order remains frozen. An exact existing committed kind
        // change belongs to the same use, rather than a second first-use slot.
        _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        return true;
    }

    internal bool CanCollectFirstSlashDuelCandidate(int owner, SkillProgramTrigger trigger, CardActionContext action)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ChooseFirstSlashDuelDiamondBenefit))
            return true;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == action.ActionId);
        return use is not null && IsFirstSlashDuelAction(owner, action, use);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 300)]
    internal bool CanOfferFirstSlashDuelBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ChooseFirstSlashDuelDiamondBenefit))
            return true;
        if (_resolutionStackOperations._resolutionStack.OfType<FirstSlashDuelLegacyWindowFrame>().Any(w => w.Id == context.ParentFrameId))
            return _coreFirstSlashDuelLegacyWindowsOperations.CanRunFirstLegacyBenefit(candidate, context);
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId);
        var use = window is null ? null : _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId);
        return context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && context.CardUse?.CardActionId == window?.Action.ActionId && window is not null && use is not null && IsFirstSlashDuelAction(candidate.OwnerSeat, window.Action, use) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstSlashDuelBenefitIssuedEvent>().Any(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId));
    }

    internal CardConversionSource FirstSlashDuelSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome BeginFirstSlashDuelBenefit(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.FirstSlashDuelBenefit is not null || f.WindowContext is not { } c || !CanOfferFirstSlashDuelBenefit(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("The first Slash/Duel benefit requires its exact unpaid own-turn finalized use.");
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId);
        var legacy = _resolutionStackOperations._resolutionStack.OfType<FirstSlashDuelLegacyWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(window?.ParentFrameId ?? legacy!.ParentFrameId)!;
        var target = use.TargetSeats.Single();
        // The count is trusted pending state. Neither choice labels nor public
        // facts expose the target's suits before the owner selects an effect.
        var x = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count(card => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[target], card) == Suit.Diamond);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FirstSlashDuelBenefit = new(f.InstructionIndex, c.ParentFrameId, use.Id, use.Action?.ActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, target, x, FirstSlashDuelSource(f), f.GameplayHash, LegacyProducerFrameId: legacy?.ProducerFrameId), Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择一项。", [], [target], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = FirstSlashDuelChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> FirstSlashDuelChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { FirstSlashDuelChoice(f, "draw", "摸X张牌"), FirstSlashDuelChoice(f, "damage", "令此牌伤害+X") });
    internal static PromptChoice FirstSlashDuelChoice(ProgramSkillFrame f, string branch, string label) => RuntimeSkills.RuntimeRules_Core_FirstSlashDuelBenefits.FirstSlashDuelChoice(f, branch, label);
    [ProgramAnswer("first-slash-duel-benefit")]
    internal void ResolveFirstSlashDuelChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The first-use choice lost its owning frame.");
        _coreFirstSlashDuelBenefitObserversOperations.Value.RequireFirstSlashDuelBenefit(f);
        var r = f.FirstSlashDuelBenefit!;
        if (r.SelectedBranch is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], FirstSlashDuelChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The first-use choice is stale or already settled.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters["branch"];
        if (branch == "damage")
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId)!;
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { FirstSlashDuelDamageBonus = checked((u.FirstSlashDuelDamageBonus ?? 0) + r.FrozenDiamondCount) });
            r = r with
            {
                SelectedBranch = branch
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstSlashDuelBenefitIssuedEvent(f.Id, r));
            FinishFirstSlashDuelBenefit(f);
            return;
        }

        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FirstSlashDuelBenefit = r with { SelectedBranch = branch, MovementBefore = before, MovementAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], r.FrozenDiamondCount, true, new(FirstSlashDuelDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FirstSlashDuelBenefit = f.FirstSlashDuelBenefit!with { DrawCount = drawn.Count, MovementAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstSlashDuelBenefitIssuedEvent(f.Id, f.FirstSlashDuelBenefit!));
        if (!DrainFirstSlashDuelChildren(f))
            ReturnFirstSlashDuelMovement(f);
    }

    internal bool DrainFirstSlashDuelChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void FinishFirstSlashDuelBenefit(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FirstSlashDuelBenefit = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21100)]
    internal bool ResumeFirstSlashDuelBenefit(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FirstSlashDuelBenefit: { SelectedBranch: "draw" } } f || f.Id != id)
            return false;
        _coreFirstSlashDuelBenefitObserversOperations.Value.RequireFirstSlashDuelBenefit(f);
        if (!DrainFirstSlashDuelChildren(f))
            FinishFirstSlashDuelBenefit(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 21100)]
    internal bool ReturnFirstSlashDuelMovement(ProgramSkillFrame f)
    {
        if (f.FirstSlashDuelBenefit?.SelectedBranch != "draw")
            return false;
        _coreFirstSlashDuelBenefitObserversOperations.Value.RequireFirstSlashDuelBenefit(f);
        if (!DrainFirstSlashDuelChildren(f))
            FinishFirstSlashDuelBenefit(f);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 390)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 390)]
    internal bool CanContinueFirstSlashDuelBenefit(ProgramSkillFrame f) => f.FirstSlashDuelBenefit?.SelectedBranch == "draw" && _coreFirstSlashDuelBenefitObserversOperations.Value.ValidFirstSlashDuelBenefit(f);
    [ProgramAiChoice(SkillProgramEffectOp.ChooseFirstSlashDuelDiamondBenefit)]
    internal PromptChoice SelectAiFirstSlashDuelBenefit(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.Single(c => c.Parameters["branch"] == (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count < 3 ? "draw" : "damage"));
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 20)]
    internal int FirstSlashDuelDamage(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || !IsFirstSlashDuelKind(attack.EffectiveCardKind ?? CardKind.Dodge) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { FirstSlashDuelDamageBonus: { } bonus } use)
            return 0;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstSlashDuelBenefitIssuedEvent>().Where(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.ActionId == use.Action?.ActionId && e.Receipt.SelectedBranch == "damage").ToArray();
        if (bonus < 0 || facts.Sum(e => e.Receipt.FrozenDiamondCount) != bonus)
            throw new InvalidOperationException("A same-use first Slash/Duel damage benefit lost its actual issued facts.");
        return bonus;
    }

    SkillProgramStepOutcome IFirstSlashDuelBenefitHost.ChooseFirstSlashDuelDiamondBenefit(ProgramSkillFrame frame) => BeginFirstSlashDuelBenefit(frame);
}
