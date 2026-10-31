#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUniqueLeaderTrickTargetsRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 30)]
    internal bool LargestHandTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 40)]
    internal bool LargestHpTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage);
    internal bool TracksUniqueLeaderTrickTargets => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage);

    internal void CaptureUniqueLeaderTrickQualification(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!TracksUniqueLeaderTrickTargets || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind) || action.ResponderSeat is not null || action.OpponentSeat is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Action?.ActionId != action.ActionId || use.SourceSeat != action.ActorSeat || use.CardKind != action.EffectiveKind || !use.TargetSeats.SequenceEqual(action.TargetSeats))
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickQualificationEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (history.Length != 0)
        {
            if (history.Length != 1 || history[0].ActionId != action.ActionId)
                throw new InvalidOperationException("A native trick cannot replace its frozen unique-leader qualification.");
            return;
        }

        var living = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
        int? Unique(Func<CharacterState, int> value)
        {
            if (living.Length == 0)
                return null;
            var maximum = living.Max(value);
            var winners = living.Where(p => value(p) == maximum).ToArray();
            return winners.Length == 1 ? winners[0].Seat : null;
        }

        var primary = action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : action.TargetSeats.Count == 0 && action.EffectiveKind == CardKind.DrawTwo ? [action.ActorSeat] : action.EffectiveDesignatedTargetSeats.ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueLeaderTrickQualificationEvent(use.Id, action.ActionId, action.ActorSeat, Unique(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count), Unique(p => p.Hp), primary.Length, primary.Length == 1 ? primary[0] : null));
    }

    internal UniqueLeaderTrickQualificationEvent? UniqueLeaderTrickQualification(CardUseFrame use) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickQualificationEvent>().SingleOrDefault(e => e.CardUseFrameId == use.Id && e.ActionId == use.Action?.ActionId);
    internal bool TryGetUniqueLeaderTrickUse(ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        return context.CardUse is { } identity && _coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(identity.ActorSeat, context, out use, out window) && window.Continuation == ProgramCardContinuation.FinalizedTrick && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) && UniqueLeaderTrickQualification(use)is { } qualification && qualification.ActorSeat == identity.ActorSeat;
    }

    internal bool UniqueLeaderTrickEligible(CardUseFrame use, int owner, SkillProgramEffectOp operation)
    {
        var qualification = UniqueLeaderTrickQualification(use);
        return _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && qualification is not null && (operation == SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition ? qualification.UniqueLargestHandSeat == qualification.ActorSeat : operation == SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage && qualification.OriginalPrimaryTargetCount == 1 && qualification.OriginalSinglePrimaryTargetSeat is { } sole && sole != owner && qualification.UniqueLargestHpSeat == sole);
    }

    internal bool CanBeUniqueLeaderTrickTarget(CardUseFrame use, CharacterState target)
    {
        if (use.Action is not { } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || !target.IsAlive || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(action.ActorSeat, SkillProgramCardTargetRestriction.SelfOnly) && target.Seat != action.ActorSeat)
            return false;
        if (use.CardKind == CardKind.UnexpectedAssault)
            return !action.EffectiveDesignatedTargetSeats.Contains(target.Seat) && _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, target) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], use.CardKind, [target.Seat]);
        if (!_coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, target))
            return false;
        if (use.CardKind != CardKind.Snatch)
            return true;
        var actor = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        return _characterMiHengLeOperations.HasIssuedSiruiUse(use.Id, actor.Seat) || _coreProvenanceCardPoliciesOperations.HasIssuedProvenanceUseDistance(use.Id, actor.Seat) || _coreGrantedEntityPhasesOperations.HasIssuedGrantedPhaseEntityDistance(use.Id, actor.Seat) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance2(actor, use.PhysicalCardIds) || _coreGrantedEntityPhasesOperations.HasGrantedPhaseEntityDistance(actor, use.PhysicalCardIds) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, target, use.CardKind, use.Id) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, use.CardKind) || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, target.Seat) == 1;
    }

    internal IReadOnlyList<IReadOnlyList<int>> UniqueLeaderTrickTargetCandidates(CardUseFrame use, int owner, SkillProgramEffectOp operation)
    {
        if (!UniqueLeaderTrickEligible(use, owner, operation))
            return Array.AsReadOnly(Array.Empty<IReadOnlyList<int>>());
        var result = new List<IReadOnlyList<int>>();
        foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => (operation != SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage || p.Seat == owner) && CanBeUniqueLeaderTrickTarget(use, p)).OrderBy(p => p.Seat))
            if (use.CardKind == CardKind.BorrowedSword)
                foreach (var victim in _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, p)).OrderBy(p => p.Seat))
                    result.Add(Array.AsReadOnly(new[] { target.Seat, victim.Seat }));
            else
                result.Add(Array.AsReadOnly(new[] { target.Seat }));
        return Array.AsReadOnly(result.ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 630)]
    internal bool CanOfferUniqueLeaderTrickTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [{ } effect] || !UniqueLeaderTrickTargetContract.IsTargetOperation(effect.Op))
            return true;
        return TryGetUniqueLeaderTrickUse(context, out var use, out var window) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickTargetOfferedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId) && UniqueLeaderTrickTargetCandidates(use, candidate.OwnerSeat, effect.Op).Count > 0;
    }

    internal CardUseFrame RequireUniqueLeaderTrickParent(ProgramSkillFrame frame, out ProgramCardTriggerWindowFrame window)
    {
        if (frame.TriggerId is not { } binding || frame.WindowContext is not { } context || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || !TryGetUniqueLeaderTrickUse(context, out var use, out window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(frame.OwnerSeat, frame.SkillId, binding, frame.SkillInstanceId, frame.GameplayHash, 0), context, window) || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program is not { } program || program.GameplayHash != frame.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == binding)?.Effects is not [{ } effect] || !UniqueLeaderTrickTargetContract.IsTargetOperation(effect.Op) || !UniqueLeaderTrickEligible(use, frame.OwnerSeat, effect.Op))
            throw new InvalidOperationException("A unique-leader trick addition lost its exact native use, frozen qualification, observer or instruction.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[index - 2].Id != use.Id || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.BindingId == binding && e.SkillInstanceId == frame.SkillInstanceId && e.Window == context.Window) != 1)
            throw new InvalidOperationException("A unique-leader trick addition requires its immediate typed designation parent.");
        return use;
    }

    internal IReadOnlyList<PromptChoice> BuildUniqueLeaderTrickTargetChoices(ProgramSkillFrame frame, CardUseFrame use, SkillProgramEffectOp operation)
    {
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "unique-leader-trick-target",
            ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture),
            ["card-use-id"] = use.Id.ToString(CultureInfo.InvariantCulture),
            ["action-id"] = use.Action!.ActionId.ToString(CultureInfo.InvariantCulture)
        };
        var choices = UniqueLeaderTrickTargetCandidates(use, frame.OwnerSeat, operation).Select(pair => new PromptChoice(new($"unique-leader-trick-target.{(frame.Id)}.{(string.Join(".", pair))}"), pair.Count == 2 ? $"额外令 {(_coreMatchParticipantsPlayersAccess._players[pair[0]].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[pair[1]].Name)} 使用杀" : $"额外指定 {(_coreMatchParticipantsPlayersAccess._players[pair[0]].Name)}", [], pair, Parameters())).ToList();
        choices.Add(new(new($"unique-leader-trick-target.{(frame.Id)}.decline"), "不追加目标", [], [], Parameters()));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal SkillProgramStepOutcome OfferUniqueLeaderTrickTarget(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireUniqueLeaderTrickParent(frame, out _);
        if (frame.UniqueLeaderTrickTargetDraft is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickTargetOfferedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Source.OwnerSeat == frame.OwnerSeat && e.Source.SkillId == frame.SkillId))
            throw new InvalidOperationException("A unique-leader trick target instruction cannot issue its prompt twice.");
        if (UniqueLeaderTrickTargetCandidates(use, frame.OwnerSeat, operation).Count == 0)
            return SkillProgramStepOutcome.Continue;
        if (use.CardKind == CardKind.DrawTwo && use.TargetSeats.Count == 0)
        {
            var targets = Array.AsReadOnly(new[] { use.Action!.ActorSeat });
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action }, action);
            use = RequireUniqueLeaderTrickParent(frame, out _);
        }

        var source = new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId);
        var choices = BuildUniqueLeaderTrickTargetChoices(frame, use, operation);
        var draft = new UniqueLeaderTrickTargetDraft(use.Id, use.Action!.ActionId, 1, source, frame.GameplayHash, operation)
        {
            OriginalTargetSeats = use.TargetSeats,
            Choices = choices
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { UniqueLeaderTrickTargetDraft = draft });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueLeaderTrickTargetOfferedEvent(frame.Id, use.Id, draft.ActionId, 1, source, frame.GameplayHash, operation, draft.OriginalTargetSeats, Array.AsReadOnly(choices.SelectMany(c => c.Targets.Take(1)).Distinct().ToArray())));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "可以为此锦囊牌追加一个合法目标。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            Choices = draft.Choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("unique-leader-trick-target")]
    internal void ResolveUniqueLeaderTrickTargetChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A trick target choice lost its owning program.");
        AssertUniqueLeaderTrickTargetDraft(frame);
        var draft = frame.UniqueLeaderTrickTargetDraft ?? throw new InvalidOperationException("The trick target draft is missing.");
        var choice = draft.Choices.SingleOrDefault(c => c.Id == selected.Id);
        if (choice is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected]))
            throw new InvalidOperationException("The trick target answer changed its published choice.");
        var use = RequireUniqueLeaderTrickParent(frame, out _);
        var targets = draft.OriginalTargetSeats;
        if (choice.Targets.Count > 0)
        {
            if (!UniqueLeaderTrickTargetCandidates(use, frame.OwnerSeat, draft.Operation).Any(pair => pair.SequenceEqual(choice.Targets)))
                throw new InvalidOperationException("The extra trick target is no longer legal.");
            targets = Array.AsReadOnly(targets.Concat(choice.Targets).ToArray());
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, choice.Targets[0]));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueLeaderTrickTargetResolvedEvent(frame.Id, use.Id, draft.ActionId, 1, draft.Source, frame.GameplayHash, draft.Operation, draft.OriginalTargetSeats, choice.Targets, targets));
        if (choice.Targets.Count > 0 && draft.Operation == SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage)
        {
            var benefit = new JoinedTrickDamageBenefit(frame.Id, use.Id, draft.ActionId, draft.Source, frame.GameplayHash);
            var current = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
            if (current.JoinedTrickDamageBenefits?.Any(b => b.Source.OwnerSeat == frame.OwnerSeat && b.Source.SkillId == frame.SkillId) == true)
                throw new InvalidOperationException("A joined trick cannot duplicate one owner's benefit.");
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, native => native with { JoinedTrickDamageBenefits = (current.JoinedTrickDamageBenefits ?? []).Append(benefit).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JoinedTrickDamageBenefitIssuedEvent(benefit));
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { UniqueLeaderTrickTargetDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 200)]
    internal void AssertUniqueLeaderTrickTargetDraft(ProgramSkillFrame frame)
    {
        if (frame.UniqueLeaderTrickTargetDraft is null && (frame.TriggerId is null || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.Triggers.SingleOrDefault(t => t.Id == frame.TriggerId)?.Effects.Any(e => UniqueLeaderTrickTargetContract.IsTargetOperation(e.Op)) != true))
            return;
        var offered = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickTargetOfferedEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray();
        var resolved = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickTargetResolvedEvent>().Any(e => e.ProgramFrameId == frame.Id);
        if (frame.UniqueLeaderTrickTargetDraft is not { } draft)
        {
            if (offered.Length != 0 && !resolved)
                throw new InvalidOperationException("An offered unique-leader trick cannot lose its owning draft.");
            return;
        }

        var use = RequireUniqueLeaderTrickParent(frame, out _);
        if (draft.CardUseFrameId != use.Id || draft.ActionId != use.Action!.ActionId || draft.InstructionIndex != 1 || draft.Source != new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId) || draft.GameplayHash != frame.GameplayHash || !draft.OriginalTargetSeats.SequenceEqual(use.TargetSeats) || offered.Length != 1 || resolved || offered[0].CardUseFrameId != use.Id || offered[0].ActionId != draft.ActionId || offered[0].InstructionIndex != 1 || offered[0].Source != draft.Source || offered[0].GameplayHash != draft.GameplayHash || offered[0].Operation != draft.Operation || !offered[0].OriginalTargetSeats.SequenceEqual(draft.OriginalTargetSeats) || !offered[0].CandidateTargetSeats.SequenceEqual(draft.Choices.SelectMany(c => c.Targets.Take(1)).Distinct()) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(draft.Choices, BuildUniqueLeaderTrickTargetChoices(frame, use, draft.Operation)) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: false } prompt || prompt.PlayerSeat != frame.OwnerSeat || prompt.SourceSeat != frame.OwnerSeat || prompt.TargetSeat != frame.OwnerSeat || prompt.SkillPrompt?.SkillId != frame.SkillId || prompt.ValidCardIds.Count != 0 || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, draft.Choices) || !prompt.ValidTargetSeats.SequenceEqual(draft.Choices.SelectMany(c => c.Targets).Distinct()))
            throw new InvalidOperationException("A unique-leader trick lost its exact source, native prefix or published target choice.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition, SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage)]
    internal PromptChoice SelectAiUniqueLeaderTrickTargetChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        var use = RequireUniqueLeaderTrickParent(frame, out _);
        var draft = frame.UniqueLeaderTrickTargetDraft!;
        if (draft.Operation == SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage)
        {
            var beneficial = use.CardKind is CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains || use.CardKind == CardKind.IronChain && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsChained;
            // A healthy joining player may trade one possible native damage for two cards.
            // Public HP alone is used; no foreign hand identities inform this choice.
            var affordable = global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(use.CardKind) && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp > 1;
            return beneficial || affordable ? decision.Choices.First(c => c.Targets.Count > 0) : decision.Choices.Single(c => c.Targets.Count == 0);
        }

        bool Helpful(int target) => use.CardKind is CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains || use.CardKind == CardKind.IronChain && _coreMatchParticipantsPlayersAccess._players[target].IsChained;
        return decision.Choices.Where(c => c.Targets.Count > 0).FirstOrDefault(c => use.CardKind == CardKind.BorrowedSword ? !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[1]]) : Helpful(c.Targets[0]) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? decision.Choices.Single(c => c.Targets.Count == 0);
    }

    internal bool IsUniqueLeaderTrickTargetFact(CardUseFrame use, UniqueLeaderTrickTargetResolvedEvent fact, IReadOnlyList<int> originalTargets)
    {
        var normalizationOnly = use.CardKind == CardKind.DrawTwo && fact.AddedTargetSeats.Count == 0 && originalTargets is [var self] && self == UniqueLeaderTrickQualification(use)?.ActorSeat;
        if (use.Action is not { Type: CardActionType.Use } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || fact.CardUseFrameId != use.Id || fact.ActionId != action.ActionId || fact.InstructionIndex != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.Source.OwnerSeat) || !UniqueLeaderTrickTargetContract.IsTargetOperation(fact.Operation) || !fact.OriginalTargetSeats.SequenceEqual(originalTargets) || !normalizationOnly && fact.AddedTargetSeats.Count != (use.CardKind == CardKind.BorrowedSword ? 2 : 1) || fact.AddedTargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || !normalizationOnly && (use.CardKind == CardKind.BorrowedSword ? originalTargets.Where((_, i) => i % 2 == 0) : originalTargets).Contains(fact.AddedTargetSeats[0]) || !fact.ResultTargetSeats.SequenceEqual(originalTargets.Concat(fact.AddedTargetSeats)) || _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program is not { } program || program.GameplayHash != fact.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == fact.Source.BindingId)is not { } trigger || trigger.Effects is not [{ } effect] || effect.Op != fact.Operation)
            return false;
        var qualification = UniqueLeaderTrickQualification(use);
        if (qualification is null || (fact.Operation == SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition ? qualification.UniqueLargestHandSeat != qualification.ActorSeat : qualification.OriginalPrimaryTargetCount != 1 || qualification.OriginalSinglePrimaryTargetSeat == fact.Source.OwnerSeat || qualification.OriginalSinglePrimaryTargetSeat != qualification.UniqueLargestHpSeat || !normalizationOnly && fact.AddedTargetSeats[0] != fact.Source.OwnerSeat))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<UniqueLeaderTrickTargetResolvedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId) == 1 && history.OfType<UniqueLeaderTrickTargetOfferedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.ActionId == fact.ActionId && e.Source == fact.Source && e.GameplayHash == fact.GameplayHash && e.Operation == fact.Operation && e.InstructionIndex == 1 && e.OriginalTargetSeats.SequenceEqual(originalTargets) && (normalizationOnly ? e.CandidateTargetSeats.Count > 0 : e.CandidateTargetSeats.Contains(fact.AddedTargetSeats[0]))) == 1 && history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.OwnerSeat == fact.Source.OwnerSeat && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && (normalizationOnly ? !history.OfType<ProgramCardUseTargetAddedEvent>().Any(e => e.FrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id) : history.OfType<ProgramCardUseTargetAddedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.OwnerSeat == fact.Source.OwnerSeat && e.SkillId == fact.Source.SkillId && e.TargetSeat == fact.AddedTargetSeats[0]) == 1);
    }

    internal bool HasUniqueLeaderTrickTargetTail(CardUseFrame use) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueLeaderTrickTargetResolvedEvent>().Any(e => e.AddedTargetSeats.Count > 0 && IsUniqueLeaderTrickTargetFact(use, e, e.OriginalTargetSeats) && use.Action!.TargetSeats.Count >= e.ResultTargetSeats.Count && use.Action.TargetSeats.Take(e.ResultTargetSeats.Count).SequenceEqual(e.ResultTargetSeats) && use.TargetSeats.SequenceEqual(use.Action.TargetSeats));
    SkillProgramStepOutcome global::CardGame.Core.IUniqueLeaderTrickTargetHost.OfferUniqueLargestHandTrickTargetAddition(ProgramSkillFrame frame) => OfferUniqueLeaderTrickTarget(frame, SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition);
    SkillProgramStepOutcome global::CardGame.Core.IUniqueLeaderTrickTargetHost.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage(ProgramSkillFrame frame) => OfferUniqueLeaderTrickTarget(frame, SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage);
    SkillProgramStepOutcome global::CardGame.Core.IUniqueLeaderTrickTargetHost.DrawAfterJoinedTrickDamage(ProgramSkillFrame frame) => _coreJoinedTrickDamageRewardsOperations.BeginJoinedTrickDamageReward(frame);
}
