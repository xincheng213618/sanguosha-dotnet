#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAdjacentRedTargetsRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 120)]
    internal bool AdjacentRedTargetFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.JoinAdjacentRedCardTarget) && (query.Category == SkillProgramCardCategory.Basic || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind));
    internal bool HasAdjacentRedTargetCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.JoinAdjacentRedCardTarget);

    internal bool TryGetAdjacentRedTargetUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } w || w.ParentFrameId != identity.ParentCardUseFrameId || w.CompletedResponseReturn is not null || w.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } u || a.ActionId != identity.CardActionId || w.Action.ActionId != a.ActionId || w.Action.ActorSeat != a.ActorSeat || u.SourceSeat != a.ActorSeat || a.EffectiveKind != u.CardKind || !w.Action.TargetSeats.SequenceEqual(u.TargetSeats) || !a.TargetSeats.SequenceEqual(u.TargetSeats) || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(u.CardKind) != SkillProgramCardCategory.Basic && !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(u.CardKind))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != u.Id || u.CardKind == CardKind.BorrowedSword && (u.TargetSeats.Count < 2 || u.TargetSeats.Count % 2 != 0 || u.TargetSeats.Where((_, i) => i % 2 == 0).Distinct().Count() != u.TargetSeats.Count / 2))
            return false;
        // This is the actual accepted Use, including a Peach/Alcohol rescue Use.
        // A Dodge response has no such finalized card-use continuation.
        if (a.PhysicalCards.Count > 0 && a.PhysicalCards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.Processing && !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(u.Id, c.CardId) && !_coreResponseEntityExchangeOperations.IsExchangedCardClaim(a.ActionId, c.CardId)))
            return false;
        use = u;
        window = w;
        return true;
    }

    internal IEnumerable<int> AdjacentRedOriginalPrimaryTargets(CardUseFrame use) => use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, i) => i % 2 == 0) : use.TargetSeats.Count == 0 && use.CardKind == CardKind.DrawTwo ? [use.SourceSeat] : use.TargetSeats;
    internal bool AdjacentRedTargetEligible(CardUseFrame use, int owner)
    {
        var a = use.Action!;
        var target = _coreMatchParticipantsPlayersAccess._players[owner];
        if (!target.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || a.ActorSeat == owner || use.TargetSeats.Contains(owner) || !(a.EffectiveIsRed ?? a.EffectiveSuit is Suit.Heart or Suit.Diamond) || !AdjacentRedOriginalPrimaryTargets(use).Any(s => s != owner && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreNativeRuleQueriesOperations.GetCombatDistance(s, owner) == 1) || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(a.ActorSeat, SkillProgramCardTargetRestriction.SelfOnly) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(a.ActorSeat, owner, use.CardKind, use.Id) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, a.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(a)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(a.ActorSeat, owner, a.EffectiveSuit))
            return false;
        // Mandatory target addition ignores use distance and permits a full-HP
        // Peach target (its native recovery may be zero). Public card conditions
        // and prohibitions still apply; BorrowedSword always needs a whole pair.
        return use.CardKind switch
        {
            CardKind.StarShift or CardKind.HandRedistribution => _coreWenheCardsOperations.CanBeWenheAdditionalTarget(use, target),
            CardKind.Intoxication or CardKind.StoreGrain or CardKind.Reinforcements => _coreGuanduCardsOperations.CanBeGuanduAdditionalTarget(use, target),
            CardKind.SameBoat or CardKind.AgainstCurrent or CardKind.Zong => _coreDragonBoatCardsOperations.CanBeDragonBoatAdditionalTarget(use, target),
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            CardKind.Snatch or CardKind.Dismantlement => _corePhysicalOwnedCardQueriesOperations.HasTargetCard(target),
            CardKind.BorrowedSword => _coreNativeBorrowedSwordOperations.GetWeapon(target)is not null && _coreMatchParticipantsPlayersAccess._players.Any(p => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, p)),
            CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.PeachGarden or CardKind.FiveGrains => !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, use.CardKind),
            _ => CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardKind is CardKind.Peach or CardKind.Alcohol or CardKind.DrawTwo or CardKind.Duel or CardKind.IronChain
        };
    }

    internal CardUseFrame RequireAdjacentRedParent(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window)
    {
        if (f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { } context || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.JoinAdjacentRedCardTarget }] || !TryGetAdjacentRedTargetUse(f.OwnerSeat, context, out var use, out window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context, window))
            throw new InvalidOperationException("Adjacent red targeting lost its exact observer and real native Use.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id)
            throw new InvalidOperationException("Adjacent red targeting requires its immediate finalized native parent.");
        return use;
    }

    internal SkillProgramStepOutcome BeginAdjacentRedTarget(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireAdjacentRedParent(f, out _);
        if (f.AdjacentRedTarget is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AdjacentRedTargetAddedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId))
            throw new InvalidOperationException("Adjacent red targeting cannot add one observer twice.");
        if (!AdjacentRedTargetEligible(use, f.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        if (use.CardKind != CardKind.BorrowedSword)
        {
            ApplyAdjacentRedTarget(f, [f.OwnerSeat]);
            return SkillProgramStepOutcome.Continue;
        }

        var choices = AdjacentBorrowedSwordChoices(f, use);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AdjacentRedTarget = new() { CardUseFrameId = use.Id, ActionId = use.Action!.ActionId, Source = _coreDamageZoneAcquisitionOperations.Value.DamageAcquisitionSource(f), GameplayHash = f.GameplayHash, OriginalTargets = use.TargetSeats, Choices = choices } });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "寄篱：成为借刀杀人的额外目标，请选择你使用杀的目标。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> AdjacentBorrowedSwordChoices(ProgramSkillFrame f, CardUseFrame use) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p)).OrderBy(p => p.Seat).Select(p => PromptChoiceProjection.Freeze(new PromptChoice(new($"adjacent-red.{(f.Id)}.victim-{(p.Seat)}"), $"额外令你对 {(p.Name)} 使用杀", [], [f.OwnerSeat, p.Seat], new Dictionary<string, string> { ["program-action"] = "adjacent-red-target", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["card-use-id"] = use.Id.ToString(CultureInfo.InvariantCulture), ["action-id"] = use.Action!.ActionId.ToString(CultureInfo.InvariantCulture) }))).ToArray());
    [ProgramAnswer("adjacent-red-target")]
    internal void ResolveAdjacentRedTargetChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Adjacent red choice lost its owner.");
        var draft = f.AdjacentRedTarget!;
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], draft.Choices.Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Adjacent red choice must retain its whole published BorrowedSword pair.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        ApplyAdjacentRedTarget(f, selected.Targets);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AdjacentRedTarget = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ApplyAdjacentRedTarget(ProgramSkillFrame f, IReadOnlyList<int> added)
    {
        var use = RequireAdjacentRedParent(f, out var window);
        if (!AdjacentRedTargetEligible(use, f.OwnerSeat) || added[0] != f.OwnerSeat || (use.CardKind == CardKind.BorrowedSword ? added.Count != 2 || !_coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[added[1]]) : added.Count != 1))
            throw new InvalidOperationException("Adjacent red addition lost its current legal target or complete pair.");
        var original = use.TargetSeats.Count == 0 && use.CardKind == CardKind.DrawTwo ? Array.AsReadOnly(new[] { use.SourceSeat }) : use.TargetSeats;
        var targets = Array.AsReadOnly(original.Concat(added).ToArray());
        var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
        var simple = use.AdjustedSimpleContinuation;
        if (window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
        {
            var current = window.SimpleContinuation ?? throw new InvalidOperationException("Adjacent basic targeting lost its actual simple effect.");
            if (current.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol))
                throw new InvalidOperationException("Adjacent basic targeting supports a native Peach/Alcohol effect.");
            simple ??= current with
            {
                RecoveryPolicySources = current.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
            };
        }

        var updated = use with
        {
            Action = action,
            TargetSeats = targets,
            TargetsAdjusted = true,
            AdjustedSimpleContinuation = simple,
            Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget
        };
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(updated, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(f.Id, f.SkillId, f.OwnerSeat, use.Id, f.OwnerSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AdjacentRedTargetAddedEvent(f.Id, use.Id, action.ActionId, action.ActorSeat, _coreDamageZoneAcquisitionOperations.Value.DamageAcquisitionSource(f), f.GameplayHash, original, added, targets));
        _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(updated, [f.OwnerSeat]);
    }

    internal bool IsAdjacentRedTargetFact(CardUseFrame use, AdjacentRedTargetAddedEvent fact, IReadOnlyList<int> original, int? currentProofActor = null)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var owner = fact.Source.OwnerSeat;
        return fact.CardUseFrameId == use.Id && fact.ActionId == use.Action?.ActionId && fact.ActorSeat == (currentProofActor ?? use.Action!.ActorSeat) && fact.Source.OwnerSeat != fact.ActorSeat && fact.AddedTargets.Count == (use.CardKind == CardKind.BorrowedSword ? 2 : 1) && fact.AddedTargets[0] == owner && !original.Contains(owner) && fact.AddedTargets.All(_coreTableSeatQueriesOperations.IsValidPlayerSeat) && fact.OriginalTargets.SequenceEqual(original) && fact.ResultTargets.SequenceEqual(original.Concat(fact.AddedTargets)) && _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program is { } program && program.GameplayHash == fact.GameplayHash && program.Triggers.SingleOrDefault(t => t.Id == fact.Source.BindingId)?.Effects is [{ Op: SkillProgramEffectOp.JoinAdjacentRedCardTarget }] && history.OfType<AdjacentRedTargetAddedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id) == 1 && history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.OwnerSeat == owner && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && history.OfType<ProgramCardUseTargetAddedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.OwnerSeat == owner && e.SkillId == fact.Source.SkillId && e.TargetSeat == owner) == 1;
    }

    internal bool HasAdjacentRedTargetTail(CardUseFrame use) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AdjacentRedTargetAddedEvent>().Any(e => IsAdjacentRedTargetFact(use, e, e.OriginalTargets, e.ActorSeat) && use.Action!.TargetSeats.Take(e.ResultTargets.Count).SequenceEqual(e.ResultTargets) && use.TargetSeats.Count == use.Action.TargetSeats.Count);
}
