#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTargetedOrdinaryTrickRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 140)]
    internal bool TargetedOrdinaryTrickFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasTargetedOrdinaryTrickAdjustment && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind);
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 140)]
    internal bool TargetedOrdinaryTrickNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasTargetedOrdinaryTrickAdjustment;
    internal bool HasTargetedOrdinaryTrickAdjustment => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AdjustTargetedOrdinaryTrick);

    internal bool TryGetTargetedOrdinaryTrickUse(ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { } action } current || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(current.CardKind))
            return false;
        // Earlier role changes preserve the same actual action but can change its
        // user. Legality follows that current user while the issued source stays frozen.
        var live = context with
        {
            CardUse = identity with
            {
                ActorSeat = action.ActorSeat
            }
        };
        return _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(action.ActorSeat, live, out use, out window) && window.Continuation == ProgramCardContinuation.FinalizedTrick;
    }

    internal bool TargetedTrickEffectIsEffective(CardUseFrame use, int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && use.IneffectiveTargetSeats?.Contains(seat) != true && _turnCardUseEffectsOperations._turnCardUseEffects.GetRuleModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, seat, SkillRuleQuery.CardEffectImmunity, use.CardKind).Count == 0;
    internal int[] TargetedTrickPrimaryTargets(CardUseFrame use) => use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, index) => index % 2 == 0).ToArray() : use.CardKind == CardKind.DrawTwo && use.TargetSeats.Count == 0 ? [use.SourceSeat] : use.TargetSeats.ToArray();
    internal static bool TargetedTrickRemovesTargets(SkillProgramEffect effect) => effect.StateId == "remove-target";
    internal bool TargetedTrickRemovesTargets(ProgramSkillFrame frame) => TargetedTrickRemovesTargets(_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)!);
    internal bool CanAddTargetedTrickTarget(CardUseFrame use, CharacterState target, bool removeTargets) => !removeTargets || use.CardKind != CardKind.Snatch ? _coreUniqueLeaderTrickTargetsOperations.CanBeUniqueLeaderTrickTarget(use, target) : use.Action is { } action && (!_coreCardUseModulesOperations.HasTurnCardTargetRestriction(action.ActorSeat, SkillProgramCardTargetRestriction.SelfOnly) || target.Seat == action.ActorSeat) && _coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, target);
    internal static int TargetedTrickTargetIndex(CardKind kind, IReadOnlyList<int> targets, int primary) => Enumerable.Range(0, targets.Count).FirstOrDefault(index => targets[index] == primary && (kind != CardKind.BorrowedSword || index % 2 == 0), -1);
    internal static IReadOnlyList<int> RemoveTargetedTrickTarget(CardKind kind, IReadOnlyList<int> targets, int primary)
    {
        var index = TargetedTrickTargetIndex(kind, targets, primary);
        var count = kind == CardKind.BorrowedSword ? 2 : 1;
        if (index < 0 || kind == CardKind.BorrowedSword && index % 2 != 0 || targets.Count <= count)
            throw new InvalidOperationException("A targeted trick removal must retain another complete primary target.");
        return Array.AsReadOnly(targets.Where((_, cursor) => cursor < index || cursor >= index + count).ToArray());
    }

    internal IReadOnlyList<PromptChoice> TargetedTrickChoices(long frameId, CardUseFrame use, bool removeTargets = false)
    {
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = "targeted-ordinary-trick",
            ["branch"] = branch,
            ["frame-id"] = frameId.ToString(CultureInfo.InvariantCulture),
            ["card-use-id"] = use.Id.ToString(CultureInfo.InvariantCulture),
            ["action-id"] = use.Action!.ActionId.ToString(CultureInfo.InvariantCulture)
        };
        var choices = new List<PromptChoice>();
        var primary = TargetedTrickPrimaryTargets(use);
        foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => !primary.Contains(p.Seat) && CanAddTargetedTrickTarget(use, p, removeTargets)).OrderBy(p => p.Seat))
        {
            if (use.CardKind == CardKind.BorrowedSword)
                foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, v)).OrderBy(v => v.Seat))
                    choices.Add(new(new($"targeted-trick.{(frameId)}.add.{(target.Seat)}.{(victim.Seat)}"), $"增加目标：{(target.Name)} 对 {(victim.Name)} 使用杀", [], [target.Seat, victim.Seat], Parameters("add")));
            else
                choices.Add(new(new($"targeted-trick.{(frameId)}.add.{(target.Seat)}"), $"增加目标：{(target.Name)}", [], [target.Seat], Parameters("add")));
        }

        var removable = removeTargets ? primary : primary.Where(s => TargetedTrickEffectIsEffective(use, s)).ToArray();
        if (removable.Length > 1)
            foreach (var seat in removable)
                choices.Add(removeTargets ? new(new($"targeted-trick.{(frameId)}.remove.{(seat)}"), $"减少目标：{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", [], [seat], Parameters("remove")) : new(new($"targeted-trick.{(frameId)}.ineffective.{(seat)}"), $"令此牌对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 无效", [], [seat], Parameters("ineffective")));
        choices.Add(new(new($"targeted-trick.{(frameId)}.decline"), "不改变效果", [], [], Parameters("decline")));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 110)]
    internal bool CanOfferTargetedOrdinaryTrick(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.AdjustTargetedOrdinaryTrick))
            return true;
        return TryGetTargetedOrdinaryTrickUse(context, out var use, out var window) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && TargetedTrickPrimaryTargets(use).Contains(candidate.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetedOrdinaryTrickOfferedEvent>().Any(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.Source.OwnerSeat == candidate.OwnerSeat && e.Receipt.Source.SkillId == candidate.SkillId && e.Receipt.Source.BindingId == candidate.BindingId) && TargetedTrickChoices(0, use, TargetedTrickRemovesTargets(trigger.Effects.Single())).Any(c => c.Parameters["branch"] != "decline");
    }

    internal CardUseFrame RequireTargetedOrdinaryTrickParent(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window)
    {
        if (f.TriggerId is not { } binding || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !TryGetTargetedOrdinaryTrickUse(context, out var use, out window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, 0), context, window) || !TargetedTrickPrimaryTargets(use).Contains(f.OwnerSeat) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } program || program.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.AdjustTargetedOrdinaryTrick)
            throw new InvalidOperationException("A targeted trick adjustment lost its exact source, owning Use or target candidate.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id)
            throw new InvalidOperationException("A targeted trick adjustment requires its immediate typed designation parent.");
        return use;
    }

    internal SkillProgramStepOutcome BeginTargetedOrdinaryTrick(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireTargetedOrdinaryTrickParent(f, out var window);
        if (f.TargetedOrdinaryTrick is not null)
            throw new InvalidOperationException("A targeted trick adjustment was already issued.");
        var r = new TargetedOrdinaryTrickReceipt
        {
            WindowFrameId = window.Id,
            CardUseFrameId = use.Id,
            ActionId = use.Action!.ActionId,
            Source = _coreUnequalHandHpEndingOperations.TargetedTrickHandHpSource(f),
            GameplayHash = f.GameplayHash,
            InstructionIndex = f.InstructionIndex,
            OriginalTargets = use.TargetSeats,
            Choices = TargetedTrickChoices(f.Id, use, TargetedTrickRemovesTargets(f))
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TargetedOrdinaryTrick = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetedOrdinaryTrickOfferedEvent(f.Id, r));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, TargetedTrickRemovesTargets(f) ? "增加或减少一个普通锦囊目标（保留至少一个目标）。" : "增加一个普通锦囊目标或令其对一个目标无效（保留至少一个有效目标）。", [], r.Choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = r.Choices,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("targeted-ordinary-trick")]
    internal void ResolveTargetedOrdinaryTrickChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("No owning targeted trick frame.");
        var use = RequireTargetedOrdinaryTrickParent(f, out _);
        var r = f.TargetedOrdinaryTrick ?? throw new InvalidOperationException("No issued target receipt.");
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], r.Choices.Where(c => c.Id == selected.Id).ToArray()) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], TargetedTrickChoices(f.Id, use, TargetedTrickRemovesTargets(f)).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("A targeted trick answer lost its current legal participant or last-effective-target protection.");
        var branch = selected.Parameters["branch"];
        var before = use.TargetSeats;
        IReadOnlyList<int> added = branch == "add" ? selected.Targets : [];
        int? ineffective = branch == "ineffective" ? selected.Targets.Single() : null;
        int? removed = branch == "remove" ? selected.Targets.Single() : null;
        if (ineffective is { } seat)
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, seat);
        if (added.Count > 0 || removed is not null)
        {
            var result = removed is { } primary ? RemoveTargetedTrickTarget(use.CardKind, before, primary) : Array.AsReadOnly(before.Concat(added).ToArray());
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, result);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = result, Action = action, TargetsAdjusted = true }, action);
        }

        use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetedOrdinaryTrickResolvedEvent(f.Id, use.Id, use.Action!.ActionId, r.Source, r.GameplayHash, before, added, ineffective, use.TargetSeats) { RemovedTarget = removed });
        if (ineffective is { } canceled)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, use.SourceSeat, canceled, use.CardKind, CardEffectSkipReason.SkillNullified));
        if (added.Count > 0)
            _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(use, [added[0]]);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TargetedOrdinaryTrick = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.AdjustTargetedOrdinaryTrick)]
    internal PromptChoice SelectAiTargetedOrdinaryTrick(PendingDecision p, ProgramSkillFrame f)
    {
        var use = RequireTargetedOrdinaryTrickParent(f, out _);
        return p.Choices.OrderByDescending(c => _coreTargetedTrickHandHpAiOperations.TargetedTrickPublicChoiceScore(f.OwnerSeat, use, c)).First();
    }

    internal bool IsTargetedOrdinaryTrickResolvedFact(CardUseFrame use, TargetedOrdinaryTrickResolvedEvent fact, IReadOnlyList<int> before)
    {
        var pair = use.CardKind == CardKind.BorrowedSword ? 2 : 1;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program;
        var removesTargets = program?.Triggers.SingleOrDefault(t => t.Id == fact.Source.BindingId)?.Effects is [var effect] && effect.Op == SkillProgramEffectOp.AdjustTargetedOrdinaryTrick && TargetedTrickRemovesTargets(effect);
        var removedIndex = fact.RemovedTarget is { } removed ? TargetedTrickTargetIndex(use.CardKind, before, removed) : -1;
        if (fact.RemovedTarget is not null && (!removesTargets || removedIndex < 0 || removedIndex % pair != 0 || before.Count <= pair || fact.AddedTargets.Count != 0 || fact.IneffectiveTarget is not null))
            return false;
        if (removesTargets && fact.IneffectiveTarget is not null)
            return false;
        IEnumerable<int> result = fact.RemovedTarget is { } removedPrimary ? RemoveTargetedTrickTarget(use.CardKind, before, removedPrimary) : before.Concat(fact.AddedTargets);
        if (fact.CardUseFrameId != use.Id || fact.ActionId != use.Action?.ActionId || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || !fact.BeforeTargets.SequenceEqual(before) || fact.AddedTargets.Count is not 0 && fact.AddedTargets.Count != pair || fact.AddedTargets.Count > 0 && fact.IneffectiveTarget is not null || !fact.ResultTargets.SequenceEqual(result) || !_coreCompletedCategoryUseProofOperations.ActualHandGainMutationBinding(fact.ProgramFrameId, fact.Source.SkillId, fact.Source.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.AdjustTargetedOrdinaryTrick) || _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program?.GameplayHash != fact.GameplayHash)
            return false;
        var original = before.Where((_, index) => index % pair == 0).ToArray();
        if (!original.Contains(fact.Source.OwnerSeat) || fact.AddedTargets.Count > 0 && original.Contains(fact.AddedTargets[0]) || fact.IneffectiveTarget is { } target && (!original.Contains(target) || use.IneffectiveTargetSeats?.Contains(target) != true))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetedOrdinaryTrickOfferedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.Receipt.CardUseFrameId == use.Id && e.Receipt.ActionId == fact.ActionId && e.Receipt.Source == fact.Source && e.Receipt.GameplayHash == fact.GameplayHash && e.Receipt.OriginalTargets.SequenceEqual(before) && e.Receipt.Choices.Any(c => c.Parameters["branch"] == (fact.AddedTargets.Count > 0 ? "add" : fact.RemovedTarget is not null ? "remove" : fact.IneffectiveTarget is not null ? "ineffective" : "decline") && c.Targets.SequenceEqual(fact.AddedTargets.Count > 0 ? fact.AddedTargets : fact.RemovedTarget is { } reduced ? [reduced] : fact.IneffectiveTarget is { } canceled ? [canceled] : []))) == 1;
    }

    internal bool HasTargetedOrdinaryTrickTail(CardUseFrame use) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetedOrdinaryTrickResolvedEvent>().Any(e => (e.AddedTargets.Count > 0 || e.RemovedTarget is not null) && IsTargetedOrdinaryTrickResolvedFact(use, e, e.BeforeTargets) && use.TargetSeats.Count >= e.ResultTargets.Count && use.TargetSeats.Take(e.ResultTargets.Count).SequenceEqual(e.ResultTargets));
}
