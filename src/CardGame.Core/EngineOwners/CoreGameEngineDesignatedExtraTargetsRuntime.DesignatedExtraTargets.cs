#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDesignatedExtraTargetsRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 10)]
    internal bool DesignatedExtraTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget);
    internal CardConversionSource DesignatedExtraTargetSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal bool TryGetDesignatedExtraTargetUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } foundWindow || foundWindow.ParentFrameId != identity.ParentCardUseFrameId || foundWindow.CompletedResponseReturn is not null || foundWindow.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } foundUse || action.ActionId != identity.CardActionId || action.ActorSeat != owner || foundUse.SourceSeat != owner || action.EffectiveKind != foundUse.CardKind || foundWindow.Action.ActionId != action.ActionId || foundWindow.Action.ActorSeat != owner || !foundWindow.Action.TargetSeats.SequenceEqual(foundUse.TargetSeats) || !action.TargetSeats.SequenceEqual(foundUse.TargetSeats) || foundUse.DyingResponse is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(foundUse.CardKind) != SkillProgramCardCategory.Basic && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(foundUse.CardKind) || foundUse.TargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)))
            return false;
        var windowIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == foundWindow.Id);
        if (windowIndex < 1 || _resolutionStackOperations._resolutionStack[windowIndex - 1].Id != foundUse.Id)
            return false;
        if (action.PhysicalCards.Count > 0)
        {
            if (_coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(foundUse.Id))
            {
                // This native producer already paid two exact Authority entities
                // to Discard; its existing paid-virtual target loop owns the tail.
                if (foundUse.PhysicalCardIds is not { Count: 0 } || action.PhysicalCards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.DiscardPile))
                    return false;
            }
            else if (!action.PhysicalCards.Select(c => c.CardId).SequenceEqual(foundUse.PhysicalCardIds ?? [foundUse.CardId]) || action.PhysicalCards.Where(c => !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(foundUse.Id, c.CardId) && !_coreResponseEntityExchangeOperations.IsExchangedCardClaim(action.ActionId, c.CardId)).Any(c => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, c.CardId) ? _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing : _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.Processing))
                return false;
        }
        else if (foundUse.CardId != 0 || foundUse.PhysicalCardIds is not { Count: 0 } || !_coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(foundUse.Id) && !_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(foundUse.Id) && foundUse.VirtualBasicReturn is null && foundUse.CardAttack?.ProgramSkillCardUseFrameId is null && foundUse.AdjustedSlashReturn is null && !_coreDamageTargetObtainProgramsOperations.IsIssuedZeroEntityDuel(foundUse.Id))
            return false;
        if (foundUse.CardKind == CardKind.BorrowedSword && (foundUse.TargetSeats.Count < 2 || foundUse.TargetSeats.Count % 2 != 0 || foundUse.TargetSeats.Where((_, i) => i % 2 == 0).Distinct().Count() != foundUse.TargetSeats.Count / 2))
            return false;
        if (foundUse.TargetSeats.Count == 0 && foundUse.CardKind != CardKind.DrawTwo)
            return false;
        use = foundUse;
        window = foundWindow;
        return true;
    }

    internal bool DesignatedExtraTargetCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramCardTriggerWindowFrame window)
    {
        if (window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        return current.OwnerSeat == candidate.OwnerSeat && current.SkillId == candidate.SkillId && current.TriggerId == candidate.BindingId && current.SkillInstanceId == candidate.SkillInstanceId && current.GameplayHash == candidate.GameplayHash && _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) == context;
    }

    internal bool HasDiscardedDesignatedCardName(CardKind kind) => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Any(card => global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(card.Kind) == global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind));
    internal bool WasDesignatedExtraTargetOffered(CardUseFrame use, CardConversionSource source) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DesignatedExtraTargetOfferedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ActionId == use.Action!.ActionId && e.Source == source);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 220)]
    internal bool CanOfferDesignatedExtraTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget))
            return true;
        return TryGetDesignatedExtraTargetUse(candidate.OwnerSeat, context, out var use, out var window) && DesignatedExtraTargetCandidateMatches(candidate, context, window) && !HasDiscardedDesignatedCardName(use.CardKind) && !WasDesignatedExtraTargetOffered(use, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId)) && DesignatedExtraTargetCandidates(use).Count > 0;
    }

    internal CardUseFrame RequireDesignatedExtraTargetParent(ProgramSkillFrame frame, out ProgramCardTriggerWindowFrame window)
    {
        if (frame.TriggerId is not { } binding || frame.WindowContext is not { } context || !TryGetDesignatedExtraTargetUse(frame.OwnerSeat, context, out var use, out window) || !DesignatedExtraTargetCandidateMatches(new(frame.OwnerSeat, frame.SkillId, binding, frame.SkillInstanceId, frame.GameplayHash, 0), context, window) || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program is not { } program || program.GameplayHash != frame.GameplayHash || frame.InstructionIndex != 1 || ProgramInstructionResolver.Default.Resolve(frame, program).GetPausedInstruction(1).Effect.Op != SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget)
            throw new InvalidOperationException("A designated extra target lost its exact actor, candidate, instruction or owning use.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id)
            throw new InvalidOperationException("A designated extra target requires its immediate typed designation parent.");
        return use;
    }

    internal bool CanBeDistanceFreeDesignatedTarget(CardUseFrame use, CharacterState target)
    {
        var action = use.Action!;
        var actor = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var kind = use.CardKind;
        var originalMainTargets = kind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, i) => i % 2 == 0) : action.EffectiveDesignatedTargetSeats;
        if (!target.IsAlive || originalMainTargets.Contains(target.Seat) || kind == CardKind.DrawTwo && use.TargetSeats.Count == 0 && target.Seat == actor.Seat || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind, use.Id) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, action.EffectiveSuit ?? Suit.None, global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, action.EffectiveSuit) || _coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, kind, [target.Seat]) || target.Seat == actor.Seat && action.ConversionChain.Any(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.ExcludeOwnerEffects == true))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, new(-1, kind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0), action.ConversionChain.FirstOrDefault(), kind, ignoreDistance: true, existingUseFrameId: use.Id, specificEffectiveRank: action.EffectiveRank, physicalCardIds: action.PhysicalCards.Select(c => c.CardId).ToArray());
        return kind switch
        {
            CardKind.Peach => _coreTreatedWoundedOperations.IsRuleWounded(target),
            CardKind.Alcohol or CardKind.DrawTwo or CardKind.IronChain => true,
            CardKind.StarShift or CardKind.HandRedistribution => _coreWenheCardsOperations.CanBeWenheAdditionalTarget(use, target),
            CardKind.Intoxication or CardKind.StoreGrain or CardKind.Reinforcements => _coreGuanduCardsOperations.CanBeGuanduAdditionalTarget(use, target),
            CardKind.SameBoat or CardKind.AgainstCurrent or CardKind.Zong => _coreDragonBoatCardsOperations.CanBeDragonBoatAdditionalTarget(use, target),
            CardKind.Duel => target.Seat != actor.Seat,
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            CardKind.Dismantlement or CardKind.Snatch => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(target),
            CardKind.BorrowedSword => target.Seat != actor.Seat && _coreNativeBorrowedSwordOperations.GetWeapon(target)is not null && _coreMatchParticipantsPlayersAccess._players.Any(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, victim)),
            CardKind.BarbarianAssault or CardKind.ArrowBarrage => target.Seat != actor.Seat && !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind),
            CardKind.PeachGarden or CardKind.FiveGrains => !_coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind),
            _ => false
        };
    }

    internal IReadOnlyList<IReadOnlyList<int>> DesignatedExtraTargetCandidates(CardUseFrame use)
    {
        var candidates = new List<IReadOnlyList<int>>();
        foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => CanBeDistanceFreeDesignatedTarget(use, p)).OrderBy(p => p.Seat))
            if (use.CardKind == CardKind.BorrowedSword)
                foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, v)).OrderBy(v => v.Seat))
                    candidates.Add(Array.AsReadOnly(new[] { target.Seat, victim.Seat }));
            else
                candidates.Add(Array.AsReadOnly(new[] { target.Seat }));
        return Array.AsReadOnly(candidates.ToArray());
    }

    internal IReadOnlyList<PromptChoice> BuildDesignatedExtraTargetChoices(ProgramSkillFrame frame, CardUseFrame use)
    {
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "designated-extra-target",
            ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture),
            ["card-use-id"] = use.Id.ToString(CultureInfo.InvariantCulture),
            ["action-id"] = use.Action!.ActionId.ToString(CultureInfo.InvariantCulture)
        };
        var choices = DesignatedExtraTargetCandidates(use).Select(pair => new PromptChoice(new($"designated-extra-target.{(frame.Id)}.{(string.Join(".", pair))}"), pair.Count == 2 ? $"额外令 {(_coreMatchParticipantsPlayersAccess._players[pair[0]].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[pair[1]].Name)} 使用杀" : $"额外指定 {(_coreMatchParticipantsPlayersAccess._players[pair[0]].Name)}", [], pair, Parameters())).ToList();
        choices.Add(new(new($"designated-extra-target.{(frame.Id)}.decline"), "不追加目标", [], [], Parameters()));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal SkillProgramStepOutcome AddOneDistanceFreeCurrentUseTarget(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireDesignatedExtraTargetParent(frame, out _);
        if (frame.DesignatedExtraTargetDraft is not null)
            throw new InvalidOperationException("A designated extra target instruction was offered twice.");
        var source = DesignatedExtraTargetSource(frame);
        if (WasDesignatedExtraTargetOffered(use, source))
            throw new InvalidOperationException("A designated extra target lost its already issued choice receipt.");
        if (HasDiscardedDesignatedCardName(use.CardKind) || DesignatedExtraTargetCandidates(use).Count == 0)
            return SkillProgramStepOutcome.Continue;
        // DrawTwo's original self target is implicit in the ordinary host input.
        // Normalize only this new opt-in producer, preserving its action identity.
        if (use.CardKind == CardKind.DrawTwo && use.TargetSeats.Count == 0)
        {
            var targets = Array.AsReadOnly(new[] { use.SourceSeat });
            var previous = use.Action!;
            var action = CloneDesignatedExtraTargetAction(previous, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action }, action);
            use = RequireDesignatedExtraTargetParent(frame, out _);
        }

        var choices = BuildDesignatedExtraTargetChoices(frame, use);
        var draft = new DesignatedExtraTargetDraft(use.Id, use.Action!.ActionId, frame.InstructionIndex, source, frame.GameplayHash)
        {
            OriginalTargetSeats = use.TargetSeats,
            Choices = choices
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DesignatedExtraTargetDraft = draft });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetOfferedEvent(frame.Id, use.Id, draft.ActionId, frame.InstructionIndex, source, frame.GameplayHash, draft.OriginalTargetSeats, Array.AsReadOnly(choices.SelectMany(c => c.Targets.Take(1)).Distinct().ToArray())));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "可为此牌额外指定一个目标，无距离限制。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            Choices = draft.Choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal CardActionContext CloneDesignatedExtraTargetAction(CardActionContext action, IReadOnlyList<int> targets) => new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, targets, action.PhysicalCards, action.ConversionChain, action.EffectiveKind == CardKind.BorrowedSword ? targets.Where((_, i) => i % 2 == 0).ToArray() : targets, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, action.FactionOrigin);
    [ProgramAnswer("designated-extra-target")]
    internal void ResolveDesignatedExtraTargetChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A designated extra target lost its owning program.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        AssertDesignatedExtraTargetDraft(frame, paused);
        var draft = frame.DesignatedExtraTargetDraft!;
        var choice = draft.Choices.SingleOrDefault(c => c.Id == selected.Id);
        if (choice is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected]))
            throw new InvalidOperationException("A designated extra target answer changed its frozen public choice.");
        var use = RequireDesignatedExtraTargetParent(frame, out var window);
        var targets = draft.OriginalTargetSeats;
        if (choice.Targets.Count > 0)
        {
            targets = Array.AsReadOnly(draft.OriginalTargetSeats.Concat(choice.Targets).ToArray());
            var previous = use.Action!;
            var action = CloneDesignatedExtraTargetAction(previous, targets);
            var simple = use.AdjustedSimpleContinuation;
            if (window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
            {
                var continuation = window.SimpleContinuation ?? throw new InvalidOperationException("A basic extra target lost its native simple continuation.");
                if (continuation.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol))
                    throw new InvalidOperationException("A basic extra target requires its real recovery or Alcohol effect.");
                if (simple is not null && (simple.CardId != continuation.CardId || simple.Effect != continuation.Effect || simple.RecoveryAmount != continuation.RecoveryAmount || !(simple.RecoveryPolicySources ?? []).SequenceEqual(continuation.RecoveryPolicySources ?? [])))
                    throw new InvalidOperationException("A basic extra target cannot replace an existing native continuation.");
                simple ??= continuation with
                {
                    RecoveryPolicySources = continuation.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
                };
            }

            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, AdjustedSimpleContinuation = simple, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, choice.Targets[0]));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetResolvedEvent(frame.Id, use.Id, draft.ActionId, frame.InstructionIndex, draft.Source, frame.GameplayHash, draft.OriginalTargetSeats, choice.Targets, targets));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DesignatedExtraTargetDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void AssertDesignatedExtraTargetDraft(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        var offered = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DesignatedExtraTargetOfferedEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray();
        var resolved = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DesignatedExtraTargetResolvedEvent>().Any(e => e.ProgramFrameId == frame.Id);
        if (frame.DesignatedExtraTargetDraft is not { } draft)
        {
            if (offered.Length > 0 && !resolved)
                throw new InvalidOperationException("A designated extra target lost its already issued choice receipt.");
            return;
        }

        var use = RequireDesignatedExtraTargetParent(frame, out _);
        if (paused.Op != SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget || draft.CardUseFrameId != use.Id || draft.ActionId != use.Action!.ActionId || draft.InstructionIndex != frame.InstructionIndex || draft.Source != DesignatedExtraTargetSource(frame) || draft.GameplayHash != frame.GameplayHash || !draft.OriginalTargetSeats.SequenceEqual(use.TargetSeats) || offered.Length != 1 || resolved || offered[0].CardUseFrameId != use.Id || offered[0].ActionId != draft.ActionId || offered[0].Source != draft.Source || offered[0].GameplayHash != draft.GameplayHash || offered[0].InstructionIndex != draft.InstructionIndex || !offered[0].OriginalTargetSeats.SequenceEqual(draft.OriginalTargetSeats) || !offered[0].CandidateTargetSeats.SequenceEqual(draft.Choices.SelectMany(c => c.Targets.Take(1)).Distinct()) || HasDiscardedDesignatedCardName(use.CardKind) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(draft.Choices, BuildDesignatedExtraTargetChoices(frame, use)) || !ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: false } prompt || prompt.PlayerSeat != frame.OwnerSeat || prompt.SourceSeat != frame.OwnerSeat || prompt.TargetSeat != frame.OwnerSeat || prompt.SkillPrompt?.SkillId != frame.SkillId || prompt.ValidCardIds.Count != 0 || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, draft.Choices) || !prompt.ValidTargetSeats.SequenceEqual(draft.Choices.SelectMany(c => c.Targets).Distinct()))
            throw new InvalidOperationException("A designated extra target lost its exact source, prefix, native parent or public prompt.");
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 240)]
    internal void AssertRegisteredDesignatedExtraTargetDraft(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        var program = _contentRegistryOperations._contentRegistry.Skills[frame.SkillId].Program!;
        if (frame.DesignatedExtraTargetDraft is not null || program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget)))
            AssertDesignatedExtraTargetDraft(frame, frame.InstructionIndex > 0 ? plan.GetPausedInstruction(frame.InstructionIndex).Effect : plan.Instructions[0]);
    }

    [ProgramAiChoice(SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget)]
    internal PromptChoice SelectAiDesignatedExtraTargetChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        var use = RequireDesignatedExtraTargetParent(frame, out _);
        bool Helpful(int target) => use.CardKind is CardKind.Peach or CardKind.Alcohol or CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains || use.CardKind == CardKind.IronChain && _coreMatchParticipantsPlayersAccess._players[target].IsChained;
        return decision.Choices.Where(c => c.Targets.Count > 0).FirstOrDefault(c => use.CardKind == CardKind.BorrowedSword ? !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[1]]) : Helpful(c.Targets[0]) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? decision.Choices.Single(c => c.Targets.Count == 0);
    }

    internal bool IsDesignatedExtraTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> originalTargets)
    {
        if (_coreShanheXiashengOperations.IsShanheXiashengTargetFact(use, fact, originalTargets))
            return true;
        if (_characterWuYiBoundaryMobileOperations.IsBoundaryWuYiTargetFact(use, fact, originalTargets))
            return true;
        if (use.Action is not { Type: CardActionType.Use } action || fact.CardUseFrameId != use.Id || fact.ActionId != action.ActionId || fact.Source.OwnerSeat != action.ActorSeat || fact.InstructionIndex != 1 || !fact.OriginalTargetSeats.SequenceEqual(originalTargets) || fact.OriginalTargetSeats.Count == 0 || fact.AddedTargetSeats.Count != (use.CardKind == CardKind.BorrowedSword ? 2 : 1) || fact.AddedTargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || (use.CardKind == CardKind.BorrowedSword ? originalTargets.Where((_, i) => i % 2 == 0) : originalTargets).Contains(fact.AddedTargetSeats[0]) || !fact.ResultTargetSeats.SequenceEqual(originalTargets.Concat(fact.AddedTargetSeats)) || _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program is not { } program || program.GameplayHash != fact.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == fact.Source.BindingId)is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized } trigger || trigger.Effects is not [{ Op: SkillProgramEffectOp.AddOneDistanceFreeCurrentUseTarget }])
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<DesignatedExtraTargetResolvedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId) == 1 && history.OfType<DesignatedExtraTargetOfferedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.ActionId == fact.ActionId && e.InstructionIndex == fact.InstructionIndex && e.Source == fact.Source && e.GameplayHash == fact.GameplayHash && e.OriginalTargetSeats.SequenceEqual(originalTargets) && e.CandidateTargetSeats.Contains(fact.AddedTargetSeats[0])) == 1 && history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.OwnerSeat == action.ActorSeat && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == trigger.Window) == 1 && history.OfType<ProgramCardUseTargetAddedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.OwnerSeat == action.ActorSeat && e.SkillId == fact.Source.SkillId && e.TargetSeat == fact.AddedTargetSeats[0]) == 1;
    }

    internal bool HasDesignatedExtraTargetTail(CardUseFrame use) => _characterChengYuMouOlOperations.HasMouLidanTargetTail(use) || _characterSunQuanOlMagicOperations.HasMoQuanyuAddedTargetTail(use) || _characterLiuPanOlOperations.HasPijingTargetTail(use) || _coreDamageTargetBudgetOperations.HasDamageTargetBudgetTail(use) || _coreAdjacentRedTargetsOperations.HasAdjacentRedTargetTail(use) || use.BeiMiHuTargetMutation is { Kind: "append" } && _characterBeiMiHuOlOperations.ValidBeiMiHuTargetMutation(use) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DesignatedExtraTargetResolvedEvent>().Any(e => IsDesignatedExtraTargetFact(use, e, e.OriginalTargetSeats) && use.Action!.TargetSeats.Count >= e.ResultTargetSeats.Count && use.Action.TargetSeats.Take(e.ResultTargetSeats.Count).SequenceEqual(e.ResultTargetSeats) && use.TargetSeats.Count == use.Action.TargetSeats.Count);
    internal void RecordDesignatedExtraTargetRedirect(ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } action } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !(HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use)))
            return;
        var source = new CardConversionSource(redirector.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(redirector), redirector.OwnerSeat, redirector.SkillInstanceId);
        var program = _contentRegistryOperations._contentRegistry.GetSkill(redirector.SkillId).Program!;
        var effect = ProgramInstructionResolver.Default.Resolve(redirector, program).GetPausedInstruction(redirector.InstructionIndex).Effect;
        if (redirector.GameplayHash != program.GameplayHash || effect.Op != SkillProgramEffectOp.RedirectCurrentAttack || newSeat != global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(redirector, effect.Target) || redirector.WindowContext is not { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } context } windowContext || context.ParentCardUseFrameId != useId || context.CardActionId != action.ActionId || context.ActorSeat != use.SourceSeat || windowContext.SourceSeat != use.SourceSeat || windowContext.TargetSeat != originalSeat || redirector.OwnerSeat != originalSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == windowContext.ParentFrameId)is not { } window || window.ParentFrameId != useId || window.Action.ActionId != action.ActionId || window.Continuation != ProgramCardContinuation.SlashTargetRedirecting || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(redirector, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.TargetSeats[use.TargetIndex] != newSeat || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DesignatedExtraTargetRedirectedEvent>().Any(e => e.ProgramFrameId == redirector.Id && e.CardUseFrameId == useId && e.InstructionIndex == redirector.InstructionIndex))
            throw new InvalidOperationException("A designated extra target redirect lost its exact native producer and target cursor.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetRedirectedEvent(redirector.Id, useId, action.ActionId, source, redirector.GameplayHash, redirector.InstructionIndex, use.TargetIndex, originalSeat, newSeat));
    }

    internal bool IsDesignatedExtraTargetRedirectFact(CardUseFrame use, DesignatedExtraTargetRedirectedEvent fact, IReadOnlyList<int> actualTargets)
    {
        if (!(HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use)) || fact.CardUseFrameId != use.Id || fact.ActionId != use.Action?.ActionId || fact.Source.OwnerSeat != fact.OriginalTargetSeat || fact.TargetIndex < 0 || fact.TargetIndex >= actualTargets.Count || actualTargets[fact.TargetIndex] != fact.OriginalTargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.NewTargetSeat) || fact.NewTargetSeat == use.SourceSeat || fact.NewTargetSeat == fact.OriginalTargetSeat || _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program is not { } program || program.GameplayHash != fact.GameplayHash || ProgramInstructionResolver.Default.Find(program, ProgramInstructionSourceKind.Trigger, fact.Source.BindingId)is not { } plan || plan.Trigger?.Window != SkillProgramTriggerWindow.SlashTargetRedirecting || fact.InstructionIndex < 1 || fact.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(fact.InstructionIndex).Effect.Op != SkillProgramEffectOp.RedirectCurrentAttack)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<DesignatedExtraTargetRedirectedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.InstructionIndex == fact.InstructionIndex) == 1 && history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.OwnerSeat == fact.Source.OwnerSeat && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.SlashTargetRedirecting) == 1;
    }

    internal bool TryGetDesignatedExtraTargetVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> targets)
    {
        targets = [];
        if ((((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use)) || HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use)) && _characterYangFuOlOperations.TryGetSuyiVirtualPrimaryReturn(use, parent, out targets))
            return true;
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !(HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardAttack is not { CardId: null, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != parent.Id || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.PhysicalCards.Count != 0 || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || parent.InstructionIndex < 1 || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != parent.Id)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var declared = history.OfType<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id);
        var declaredUse = history.OfType<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id);
        var accepted = history.OfType<CardActionAcceptedEvent>().Select(e => e.Action).FirstOrDefault(a => a.ActionId == action.ActionId);
        if (declared is not { TargetSeats.Count: 1 } || declaredUse is not { CardId: 0 } || declaredUse.SourceSeat != use.SourceSeat || declaredUse.CardKind != use.CardKind || accepted is not { Type: CardActionType.Use, TargetSeats.Count: 1, PhysicalCards.Count: 0 } || accepted.ActorSeat != action.ActorSeat || accepted.ProviderSeat != action.ProviderSeat || accepted.EffectiveKind != use.CardKind || !accepted.ConversionChain.SequenceEqual(action.ConversionChain))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(parent, program).GetPausedInstruction(parent.InstructionIndex).Effect;
        var issuer = new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId);
        var exactProducer = effect.Op switch
        {
            SkillProgramEffectOp.RepeatShenDengAiCuixin => _characterDengAiAnniversaryGodOperations.IsShenDaCuixinUse(use.Id) && parent.OwnerSeat == use.SourceSeat && action.ConversionChain.Count == 0,
            SkillProgramEffectOp.ResolveMouChengYuDanchi => _characterChengYuMouOlOperations.IsMouDanchiUse(use.Id) && parent.OwnerSeat == use.SourceSeat && action.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualSlash => parent.OwnerSeat == use.SourceSeat && action.ConversionChain.SequenceEqual([issuer]) && (effect.TargetReference is null ? parent.SelectedTargetSeats is [var selected] && selected == declared.TargetSeats[0] : effect.TargetReference.Kind == ProgramParticipantRef.EventTarget && parent.WindowContext?.TargetSeat == declared.TargetSeats[0]),
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => parent.SelectedTargetSeats is [var actor] && actor == use.SourceSeat && action.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.OwnerSeat == use.SourceSeat && parent.SelectedTargetSeats is [var selected] && selected == declared.TargetSeats[0] && (action.ConversionChain.Count == 0 || _coreDamageTargetBudgetOperations.HasDamageTargetBudgetTail(use) && action.ConversionChain.SequenceEqual([issuer])) && effect.UseCardActionWindows && effect.OutputKind == CardKind.Slash && effect.TargetRestriction == SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget,
            _ => false
        };
        if (!exactProducer)
            return false;
        targets = Array.AsReadOnly(new[] { _characterLiuPanOlOperations.HasPijingTargetTail(use) ? declared.TargetSeats[0] : use.TargetSeats[0] });
        return true;
    }

    internal bool ContinueDesignatedVirtualBasicEffect(long id, ProgramSimpleCardContinuation continuation)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use is not { VirtualBasicReturn: not null, AdjustedSimpleContinuation: not null } || !((_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use)))
            return false;
        if (continuation.CardId != 0 || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind is not (CardKind.Peach or CardKind.Alcohol) || use.VirtualBasicEffectApplied == true || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.AdjustedSimpleContinuation.Effect != continuation.Effect)
            throw new InvalidOperationException("A designated virtual basic effect lost its real zero-material continuation.");
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]];
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (use.CardKind == CardKind.Alcohol && actor.IsAlive)
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
        if (actor.IsAlive && target.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, target.Seat))
        {
            if (use.CardKind == CardKind.Peach && target.HasHpValue)
            {
                var recoveryAmount = _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(id, actor.Seat, target.Seat, continuation.RecoveryAmount);
                if (!_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(id, actor.Seat, target.Seat, recoveryAmount, new(RecoveryAttemptProducer.VirtualBasic)))
                {
                    var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(id, actor.Seat, target.Seat, recoveryAmount);
                    try
                    {
                        target.Hp = Math.Min(target.MaxHp, target.Hp + recoveryAmount);
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(actor.Seat, target.Seat, recoveryAmount, target.Hp));
                    }
                    finally
                    {
                        _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
                    }
                }
            }
            else if (use.CardKind == CardKind.Alcohol)
            {
                _coreExtraCardEffectRoundsOperations.ApplyExtraCapableAlcoholEffect(id, target);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholAppliedEvent(id, target.Seat, 1));
            }
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, frame => frame with { VirtualBasicEffectApplied = true });
        _coreVirtualBasicOffersOperations.FinishVirtualBasicUse(id);
        return true;
    }

    internal bool TryContinueDesignatedVirtualBasicUse(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use is not { VirtualBasicReturn: not null, AdjustedSimpleContinuation: { } continuation } || !((_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use)))
            return false;
        var next = use.TargetIndex + 1;
        while (next < use.TargetSeats.Count && !_coreMatchParticipantsPlayersAccess._players[use.TargetSeats[next]].IsAlive)
            next++;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || next >= use.TargetSeats.Count)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, frame => frame with { AdjustedSimpleContinuation = null });
            return false;
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, frame => frame with { TargetIndex = next, VirtualBasicEffectApplied = false });
        _coreCardUseLifecycleOperations.ContinueSimpleCardUse(id, continuation);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IDesignatedExtraTargetProgramHost.AddOneDistanceFreeCurrentUseTarget(ProgramSkillFrame frame) => AddOneDistanceFreeCurrentUseTarget(frame);
}
