#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetBudgetRuntime
{
    internal bool TracksDamageTargetBudgetTargets => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AddDamageMarkerBudgetTargets);

    internal static bool DamageBudgetCard(CardActionContext a) => RuntimeSkills.RuntimeRules_Core_DamageTargetBudget.DamageBudgetCard(a);
    internal CardConversionSource BudgetSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool DamageBudgetParticipantMatches(SkillProgramTrigger trigger, int owner, ProgramSkillWindowContext context) => trigger.Subject switch
    {
        SkillProgramTriggerSubject.Any => context.SourceSeat == owner || context.TargetSeat == owner,
        SkillProgramTriggerSubject.DamageSource => context.SourceSeat == owner,
        _ => false
    };
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 230)]
    internal bool CanOfferDamageTargetBudget(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var e])
            return true;
        if (e.Op == SkillProgramEffectOp.GainDamageOccurrenceTargetBudget)
            return context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, OccurrenceIndex: 0 } && DamageBudgetParticipantMatches(t, c.OwnerSeat, context);
        if (e.Op != SkillProgramEffectOp.AddDamageMarkerBudgetTargets)
            return true;
        return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Markers.GetValueOrDefault(e.Marker!.Value) > 0 && _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(c.OwnerSeat, context, out var use, out var window) && DamageBudgetCard(use.Action!) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, window) && use.DamageTargetBudget is null;
    }

    internal SkillProgramStepOutcome ApplyDamageTargetBudget(SkillProgramEffect e, ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (e.Op == SkillProgramEffectOp.GainDamageOccurrenceTargetBudget)
        {
            var c = f.WindowContext ?? throw new InvalidOperationException("A damage budget lost its native occurrence.");
            var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
            if (f.InstructionIndex != 1 || c.Window != SkillProgramTriggerWindow.AfterDamageApplied || c.OccurrenceIndex != 0 || c.Amount <= 0 || !DamageBudgetParticipantMatches(_coreProgramLifecycleOperations.GetProgramTrigger(f), f.OwnerSeat, c) || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame w || w.Id != c.ParentFrameId || w.ParentFrameId != c.DamageFrameId || w.TriggerWindow != c.Window || w.TargetSeat != c.TargetSeat || (_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(w).IsSourceLess ? c.SourceSeat is not null : w.SourceSeat != c.SourceSeat) || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == w.ParentFrameId)is not { } damage || damage.Amount != c.Amount || damage.TargetSeat != c.TargetSeat || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageTargetBudgetGainedEvent>().Any(x => x.DamageWindowFrameId == w.Id && x.Source == BudgetSource(f)))
                throw new InvalidOperationException("A damage budget requires one exact applied damage occurrence matching its declared participant.");
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, e.Marker!.Value, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetBudgetGainedEvent(f.Id, w.Id, BudgetSource(f), e.Marker.Value, c.SourceSeat, c.TargetSeat!.Value, c.Amount, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(e.Marker.Value)));
            return SkillProgramStepOutcome.Continue;
        }

        var use = RequireDamageBudgetUse(f, out var parent);
        if (use.CardKind == CardKind.DrawTwo && use.TargetSeats.Count == 0)
        {
            var targets = Array.AsReadOnly(new[] { use.SourceSeat });
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action }, action);
            use = RequireDamageBudgetUse(f, out parent);
        }

        var r = new DamageTargetBudgetReceipt
        {
            ProgramFrameId = f.Id,
            WindowFrameId = parent.Id,
            CardUseFrameId = use.Id,
            ActionId = use.Action!.ActionId,
            Source = BudgetSource(f),
            GameplayHash = f.GameplayHash,
            Marker = e.Marker!.Value,
            FrozenBudget = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(e.Marker.Value),
            OriginalTargets = use.TargetSeats
        };
        r = r with
        {
            Choices = BuildDamageBudgetChoices(f, use, r)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageTargetBudget = r });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { DamageTargetBudget = r with { Choices = [] } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetBudgetOfferedEvent(f.Id, r));
        return ExposeDamageBudgetChoice(f);
    }

    internal CardUseFrame RequireDamageBudgetUse(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window)
    {
        if (f.TriggerId is not { } trigger || f.InstructionIndex != 1 || f.WindowContext is not { } c || !_coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(f.OwnerSeat, c, out var use, out window) || !DamageBudgetCard(use.Action!) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, trigger, f.SkillInstanceId, f.GameplayHash, 0), c, window))
            throw new InvalidOperationException("A marker target budget lost its actual actor, native Use or exact designation candidate.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id)
            throw new InvalidOperationException("A marker target budget requires its immediate typed designation parent.");
        return use;
    }

    internal IReadOnlyList<PromptChoice> BuildDamageBudgetChoices(ProgramSkillFrame f, CardUseFrame use, DamageTargetBudgetReceipt r)
    {
        var pairSize = use.CardKind == CardKind.BorrowedSword ? 2 : 1;
        Dictionary<string, string> Params(string branch) => new()
        {
            ["program-action"] = "damage-target-budget",
            ["branch"] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["action-id"] = r.ActionId.ToString(CultureInfo.InvariantCulture),
            ["selection-index"] = (r.AddedTargets.Count / pairSize).ToString(CultureInfo.InvariantCulture)
        };
        var choices = new List<PromptChoice>();
        if (r.AddedTargets.Count / pairSize < r.FrozenBudget)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) ? _coreRecipientCategorySlashTargetsOperations.CanBeRecipientCategorySlashTarget(use, p) : _coreUniqueLeaderTrickTargetsOperations.CanBeUniqueLeaderTrickTarget(use, p)).OrderBy(p => p.Seat))
            {
                if (pairSize == 2)
                    foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, v)).OrderBy(p => p.Seat))
                        choices.Add(new(new($"damage-budget.{(f.Id)}.{(r.AddedTargets.Count)}.add.{(target.Seat)}.{(victim.Seat)}"), $"追加：{(target.Name)} 对 {(victim.Name)} 使用杀", [], [target.Seat, victim.Seat], Params("add")));
                else
                    choices.Add(new(new($"damage-budget.{(f.Id)}.{(r.AddedTargets.Count)}.add.{(target.Seat)}"), $"追加目标：{(target.Name)}", [], [target.Seat], Params("add")));
            }

        choices.Add(new(new($"damage-budget.{(f.Id)}.{(r.AddedTargets.Count)}.done"), "完成选择", [], [], Params("done")));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal SkillProgramStepOutcome ExposeDamageBudgetChoice(ProgramSkillFrame f)
    {
        var r = f.DamageTargetBudget!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"可额外指定至多 {(r.FrozenBudget)} 个目标；已追加 {(r.AddedTargets.Count / (_coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId)!.CardKind == CardKind.BorrowedSword ? 2 : 1))} 个。", [], r.Choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = f.OwnerSeat,
            Choices = r.Choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("damage-target-budget")]
    internal void ResolveDamageBudgetChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("No owning budget choice.");
        var use = RequireDamageBudgetUse(f, out _);
        var r = f.DamageTargetBudget!;
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !BuildDamageBudgetChoices(f, use, r).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [c])))
            throw new InvalidOperationException("A budget answer changed its current legal frozen selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "add")
        {
            var targets = Array.AsReadOnly(use.TargetSeats.Concat(choice.Targets).ToArray());
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            r = r with
            {
                AddedTargets = r.AddedTargets.Concat(choice.Targets).ToArray()
            };
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
            r = r with
            {
                Choices = BuildDamageBudgetChoices(f, use, r)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageTargetBudget = r });
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { DamageTargetBudget = r with { Choices = [] } });
            ExposeDamageBudgetChoice(f);
            return;
        }

        r = r with
        {
            Completed = true,
            Choices = []
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { DamageTargetBudget = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetBudgetResolvedEvent(f.Id, r, use.TargetSeats));
        foreach (var target in r.AddedTargets.Where((_, index) => use.CardKind != CardKind.BorrowedSword || index % 2 == 0))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(f.Id, f.SkillId, f.OwnerSeat, use.Id, target));
        _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(use, r.AddedTargets.Where((_, index) => use.CardKind != CardKind.BorrowedSword || index % 2 == 0).ToArray());
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetBudget = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void SettleDamageMarkerBudgetTargets(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use?.DamageTargetBudget is not { Completed: true, Settled: false } r)
            return;
        if (use.Action is not { } action || action.ActionId != r.ActionId || !IsDamageBudgetResolvedFact(use, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageTargetBudgetResolvedEvent>().Single(x => x.ProgramFrameId == r.ProgramFrameId), r.OriginalTargets, r.Source.OwnerSeat))
            throw new InvalidOperationException("A final designation budget lost its issued completed actual Use.");
        var count = action.EffectiveDesignatedTargetSeats.Distinct().Count();
        var currentMarkers = _coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].Markers.GetValueOrDefault(r.Marker);
        // Selection entitlement is frozen when it is offered. The text's later
        // shortfall policy observes the actual marks at the final designation,
        // including intervening genuine damage or a nested actual card Use.
        var policy = DamageBudgetSourceEffect(r)!.DamageTargetShortfallPolicy ?? DamageTargetBudgetShortfallPolicy.RemoveHalfRoundedUp;
        var removed = _coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].IsAlive && count < currentMarkers ? policy switch
        {
            DamageTargetBudgetShortfallPolicy.RemoveAll => currentMarkers,
            DamageTargetBudgetShortfallPolicy.RemoveHalfRoundedUp => currentMarkers / 2 + currentMarkers % 2,
            _ => throw new InvalidOperationException("Unknown damage target budget shortfall policy.")} : 0;
        // This is an issued designation effect. Losing the skill after choosing
        // does not reissue it or abandon its final-use debit.
        if (removed > 0)
            _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat], new(r.Marker, removed), r.Source.SkillId, r.Source.BindingId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { DamageTargetBudget = r with { Settled = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageTargetBudgetSettledEvent(id, action.ActionId, r.Source, r.Marker, r.FrozenBudget, currentMarkers, count, removed, _coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].Markers.GetValueOrDefault(r.Marker)));
    }

    [ProgramAiChoice(SkillProgramEffectOp.AddDamageMarkerBudgetTargets)]
    internal PromptChoice SelectAiDamageBudgetChoice(PendingDecision p, ProgramSkillFrame f)
    {
        var use = RequireDamageBudgetUse(f, out _);
        foreach (var c in p.Choices.Where(c => c.Parameters["branch"] == "add"))
        {
            var target = _coreMatchParticipantsPlayersAccess._players[c.Targets[use.CardKind == CardKind.BorrowedSword ? 1 : 0]];
            var beneficial = use.CardKind is CardKind.DrawTwo or CardKind.FiveGrains or CardKind.PeachGarden || use.CardKind == CardKind.IronChain && target.IsChained;
            if (beneficial == CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], target))
                return c;
        }

        return p.Choices.Single(c => c.Parameters["branch"] == "done");
    }

    internal SkillProgramEffect? DamageBudgetSourceEffect(DamageTargetBudgetReceipt receipt) => _contentRegistryOperations._contentRegistry.GetSkill(receipt.Source.SkillId).Program is { } program && program.GameplayHash == receipt.GameplayHash && program.Triggers.SingleOrDefault(t => t.Id == receipt.Source.BindingId)?.Effects is [{ Op: SkillProgramEffectOp.AddDamageMarkerBudgetTargets } effect] && effect.Marker == receipt.Marker ? effect : null;
    internal bool IsDamageBudgetResolvedFact(CardUseFrame use, DamageTargetBudgetResolvedEvent fact, IReadOnlyList<int> before, int? currentProofActor = null)
    {
        var r = fact.Receipt;
        var pairSize = use.CardKind == CardKind.BorrowedSword ? 2 : 1;
        if (use.Action is not { } a || fact.ProgramFrameId != r.ProgramFrameId || r.CardUseFrameId != use.Id || r.ActionId != a.ActionId || r.Source.OwnerSeat != (currentProofActor ?? a.ActorSeat) || !r.Completed || r.FrozenBudget <= 0 || r.AddedTargets.Count % pairSize != 0 || r.AddedTargets.Count / pairSize > r.FrozenBudget || !r.OriginalTargets.SequenceEqual(before) || !fact.ResultTargets.SequenceEqual(before.Concat(r.AddedTargets)) || r.OriginalTargets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.AddedTargets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.AddedTargets.Where((_, i) => i % pairSize == 0).Distinct().Count() != r.AddedTargets.Count / pairSize || r.AddedTargets.Where((_, i) => i % pairSize == 0).Any(s => r.OriginalTargets.Where((_, i) => i % pairSize == 0).Contains(s)) || DamageBudgetSourceEffect(r)is null)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<DamageTargetBudgetResolvedEvent>().Count(x => x.ProgramFrameId == r.ProgramFrameId) == 1 && history.OfType<DamageTargetBudgetOfferedEvent>().Count(x => x.ProgramFrameId == r.ProgramFrameId && x.Receipt.Source == r.Source && x.Receipt.FrozenBudget == r.FrozenBudget && x.Receipt.OriginalTargets.SequenceEqual(before)) == 1 && history.OfType<ProgramBindingStartedEvent>().Count(x => x.FrameId == r.ProgramFrameId && x.OwnerSeat == r.Source.OwnerSeat && x.SkillId == r.Source.SkillId && x.BindingId == r.Source.BindingId && x.SkillInstanceId == r.Source.SkillInstanceId && x.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1;
    }

    internal bool HasDamageTargetBudgetTail(CardUseFrame use) => use.DamageTargetBudget is { Completed: true, AddedTargets.Count: > 0 } r && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageTargetBudgetResolvedEvent>().Any(x => x.ProgramFrameId == r.ProgramFrameId && IsDamageBudgetResolvedFact(use, x, r.OriginalTargets, r.Source.OwnerSeat));
    SkillProgramStepOutcome IDamageTargetBudgetHost.ApplyDamageTargetBudget(SkillProgramEffect e, ProgramSkillFrame f) => ApplyDamageTargetBudget(e, f);
}
