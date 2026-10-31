#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuHappyGodRuntime
{
    internal const string HappyLuBuBasicReason = "skill-program.happy-wuqian.basic";
    internal bool HasHappyLuBuWuqianCapability(CharacterState source) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse) && _coreNativeSkillContextQueriesOperations.HasProgramSkill(source, HappyLuBuComposition.Wuqian);
    internal static CardConversionSource HappyLuBuSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IHappyLuBuHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyLuBu(f, op);
    internal bool HappyLuBuFirstUse(CardUseFrame use)
    {
        if (use.SourceSeat < 0 || !_coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.CardKind != CardKind.Duel)
            return false;
        return _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or TurnEndedEvent).OfType<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.ActorSeat == use.SourceSeat && e.RoundActionType == CardActionType.Use && (use.CardKind == CardKind.Duel ? e.EffectiveCardKind == CardKind.Duel : e.EffectiveCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash)).FirstOrDefault()?.OriginFrameId == use.Id;
    }

    internal bool TryHappyLuBuUse(int owner, ProgramSkillWindowContext w, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (w.Window != SkillProgramTriggerWindow.CardUseCommitted || w.CardUse is not { } id || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is not { } p || _coreCardUseLifecycleOperations.LifecycleCardUse(id.ParentCardUseFrameId)is not { Action: { } a } u || p.Action.ActionId != a.ActionId || id.CardActionId != a.ActionId || a.Type != CardActionType.Use || a.ActorSeat != owner || u.SourceSeat != owner || !HappyLuBuFirstUse(u))
            return false;
        use = u;
        window = p;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41200, SkillProgramEffectOp.ObtainHappyShenLuBuTurnBasic, SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse, SkillProgramEffectOp.ResetHappyShenLuBuShenfen)]
    internal bool HappyLuBuEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects[0].Op == SkillProgramEffectOp.ObtainHappyShenLuBuTurnBasic)
            return _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } p && IsHappyLuBuActualStart(p) && p.CandidateIndex == 0 && p.Candidates[0] == c;
        if (t.Effects[0].Op == SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse)
            return TryHappyLuBuUse(c.OwnerSeat, w, out var use, out _) && use.HappyLuBuWuqian is null;
        var owner = _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat];
        return owner.IsAlive && owner.HasHpValue && owner.Hp == owner.MaxHp && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, HappyLuBuComposition.Shenfen, HappyLuBuShenfenActivation().UsageGroup, SkillUsageScope.Game) > 0;
    }

    internal SkillProgramActivation HappyLuBuShenfenActivation() => _contentRegistryOperations._contentRegistry.GetSkill(HappyLuBuComposition.Shenfen).Program!.Activations.Single();
    internal SkillProgramStepOutcome BeginHappyLuBu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.WindowContext is not { } w || f.HappyLuBu is not null)
            throw new InvalidOperationException("Happy Lu Bu requires the original unpaid trigger instruction.");
        if (op == SkillProgramEffectOp.ResetHappyShenLuBuShenfen)
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId) ?? throw new InvalidOperationException("Shenfen lost its real completed turn.");
            if (!_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent)) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp != _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp)
                throw new InvalidOperationException("Shenfen reset requires actual completed-turn full HP.");
            var group = HappyLuBuShenfenActivation().UsageGroup;
            var before = _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, group, SkillUsageScope.Game);
            if (before != 1 || !_skillRuntimeStateOperations._skillRuntimeState.ClearUsage(f.OwnerSeat, f.SkillId, group, SkillUsageScope.Game))
                throw new InvalidOperationException("Shenfen cannot refund an unconsumed limited use.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLuBuShenfenResetEvent(f.Id, HappyLuBuSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, parent.OwnerSeat, group, before));
            return SkillProgramStepOutcome.Continue;
        }

        CardUseFrame? use = null;
        if (op == SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse)
        {
            if (!TryHappyLuBuUse(f.OwnerSeat, w, out var original, out _) || original.HappyLuBuWuqian is not null)
                throw new InvalidOperationException("Wuqian requires the first real use of this card name.");
            use = original;
        }

        var r = new HappyLuBuReceipt(HappyLuBuSource(f), f.GameplayHash, op, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
        {
            Stage = use is null ? HappyLuBuStage.BasicChildren : HappyLuBuStage.AdditionalTarget,
            UseId = use?.Id ?? 0,
            ActionId = use?.Action!.ActionId ?? 0,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyLuBu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLuBuStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, r.UseId, r.ActionId));
        if (use is not null)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { HappyLuBuWuqian = new(f.Id, w.ParentFrameId, u.Id, r.ActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, u.CardKind, r.Source, r.GameplayHash) { OriginalTargets = u.TargetSeats } });
            ContinueHappyLuBu(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Basic).ToArray();
        if (candidates.Length > 0)
        {
            var card = candidates[_randomOperations._random.Next(candidates.Length)];
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(HappyLuBuBasicReason), (_, moves) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { HappyLuBu = current.HappyLuBu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = moves } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLuBuBasicPaidEvent(f.Id, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, moves.Count));
            });
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueHappyLuBu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool HappyLuBuExtraTarget(CardUseFrame use, CharacterState target)
    {
        if (!_coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, target))
            return false;
        return !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], target, new(-1, use.CardKind, use.Action!.EffectiveSuit ?? Suit.None, use.Action.EffectiveRank ?? 0), use.Action.ConversionChain.FirstOrDefault(), use.CardKind, existingUseFrameId: use.Id, specificEffectiveRank: use.Action.EffectiveRank, physicalCardIds: use.Action.PhysicalCards.Select(c => c.CardId).ToArray());
    }

    internal IReadOnlyList<PromptChoice> HappyLuBuChoices(ProgramSkillFrame f)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(f.HappyLuBu!.UseId) ?? throw new InvalidOperationException("Wuqian lost the original Use.");
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "happy-lu-bu-target",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        var choices = _coreMatchParticipantsPlayersAccess._players.Where(p => HappyLuBuExtraTarget(use, p)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => new PromptChoice(new($"happy-wuqian.{(f.Id)}.{(p.Seat)}"), "额外指定“" + p.Name + "”", [], [p.Seat], Parameters())).ToList();
        choices.Add(new(new($"happy-wuqian.{(f.Id)}.skip"), "不追加目标", [], [], Parameters()));
        return choices;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41200)]
    internal bool ContinueHappyLuBu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyLuBu: { } r } f || f.Id != id)
            return false;
        if (r.Stage == HappyLuBuStage.BasicChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyLuBu = f.HappyLuBu!with { Stage = HappyLuBuStage.Finished }, PendingMovementContinuation = null });
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        var choices = HappyLuBuChoices(f);
        if (choices.Count == 1)
        {
            ApplyHappyLuBuUse(f, null);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "无前：可以为此牌额外指定一名合法目标。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = f.OwnerSeat,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    [ProgramAnswer("happy-lu-bu-target")]
    internal void AnswerHappyLuBuTarget(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Wuqian lost its target parent.");
        if (!HappyLuBuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [chosen])))
            throw new InvalidOperationException("Wuqian requires its exact published target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        ApplyHappyLuBuUse(f, chosen.Targets.Count == 0 ? null : chosen.Targets.Single());
    }

    internal void ApplyHappyLuBuUse(ProgramSkillFrame f, int? added)
    {
        var r = f.HappyLuBu!;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId) ?? throw new InvalidOperationException("Wuqian lost its paid native Use.");
        if (use.HappyLuBuWuqian is not { Applied: false } policy || policy.ProgramId != f.Id || added is { } s && !HappyLuBuExtraTarget(use, _coreMatchParticipantsPlayersAccess._players[s]))
            throw new InvalidOperationException("Wuqian cannot enhance twice or add an illegal target.");
        var targets = added is { } seat ? use.TargetSeats.Append(seat).ToArray() : use.TargetSeats.ToArray();
        var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, HappyLuBuWuqian = policy with { Applied = true, AddedTarget = added }, IgnoresArmor = true, EnhancementOwnerSeat = use.EnhancementOwnerSeat ?? f.OwnerSeat, Enhancements = use.Enhancements | CurrentCardEnhancement.IgnoreArmor | (added is null ? CurrentCardEnhancement.None : CurrentCardEnhancement.ExtraTarget), TargetsAdjusted = use.TargetsAdjusted || added is not null }, action);
        if ((use.Enhancements & CurrentCardEnhancement.IgnoreArmor) == 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, r.ActionId, f.OwnerSeat, CurrentCardEnhancement.IgnoreArmor, null));
        if (added is { } extra)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, r.ActionId, f.OwnerSeat, CurrentCardEnhancement.ExtraTarget, extra));
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == use.Id)
            attack.SetIgnoresArmor(true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLuBuUseAppliedEvent(f.Id, use.Id, r.ActionId, added));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyLuBu = r with { Stage = HappyLuBuStage.Finished }, Step = ResolutionFrameStep.ResolvingEffect });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41200)]
    internal bool ReturnHappyLuBuMovement(ProgramSkillFrame f) => f.HappyLuBu is not null && ContinueHappyLuBu(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.EnhanceHappyShenLuBuFirstUse)]
    internal PromptChoice HappyLuBuAi(PendingDecision p) => p.Choices.First();
    internal int HappyLuBuRequiredResponseCount(int skillOwner, int source, int responder, CardKind incoming, CardKind required)
    {
        if (skillOwner != source || incoming == CardKind.Duel && required != CardKind.Slash || CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && required != CardKind.Dodge)
            return 1;
        var id = incoming == CardKind.Duel ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResolutionId : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId;
        return id is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { HappyLuBuWuqian: { Applied: true } r } use && r.Source.OwnerSeat == source && use.SourceSeat == source && r.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && (incoming == CardKind.Duel ? use.CardKind == CardKind.Duel : CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind)) && use.TargetSeats.Contains(responder) ? 2 : 1;
    }
}
