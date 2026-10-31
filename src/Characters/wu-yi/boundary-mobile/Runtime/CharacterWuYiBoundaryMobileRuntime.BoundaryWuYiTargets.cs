#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuYiBoundaryMobileRuntime
{
    internal CardUseFrame RequireBoundaryWuYiUse(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window)
    {
        if (f.WindowContext is not { } context || !_coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetUse(f.OwnerSeat, context, out var use, out window) || f.WindowContext.CardUse?.CardActionId != use.Action!.ActionId || f.WindowContext.CardUse.ParentCardUseFrameId != use.Id || BoundaryWuYiIssuedGrant(use.Id, use.Action.ActionId, f.OwnerSeat, f.SkillInstanceId)is null)
            throw new InvalidOperationException("Benxi targets require their original first eligible actual-use parent.");
        return use;
    }

    internal IReadOnlyList<IReadOnlyList<int>> BoundaryWuYiTargetCandidates(CardUseFrame use, BoundaryWuYiTargetDraft draft)
    {
        var selected = use.CardKind == CardKind.BorrowedSword ? draft.Added.Where((_, i) => i % 2 == 0) : draft.Added;
        return Array.AsReadOnly(_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidates(use).Where(pair => pair[0] != draft.Grant.Source.OwnerSeat && !selected.Contains(pair[0]) && _coreNativeRuleQueriesOperations.GetCombatDistance(draft.Grant.Source.OwnerSeat, pair[0]) == 1).ToArray());
    }

    internal SkillProgramStepOutcome BeginBoundaryWuYiTargets(ProgramSkillFrame f)
    {
        if (f.BoundaryWuYiTargets is not null || f.WindowContext is not { } context || !CanOfferBoundaryWuYiTargets(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), context))
            throw new InvalidOperationException("Benxi cannot reopen an already-issued target choice.");
        var use = RequireBoundaryWuYiUse(f, out var window);
        if (use.CardKind == CardKind.DrawTwo && use.TargetSeats.Count == 0)
        {
            IReadOnlyList<int> targets = Array.AsReadOnly(new[] { use.SourceSeat });
            var normalized = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = normalized }, normalized);
            use = RequireBoundaryWuYiUse(f, out window);
        }

        var grant = BoundaryWuYiIssuedGrant(use.Id, use.Action!.ActionId, f.OwnerSeat, f.SkillInstanceId)!;
        var draft = new BoundaryWuYiTargetDraft(grant, use.Id, use.Action.ActionId, window.Id)
        {
            Original = use.TargetSeats
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWuYiTargets = draft });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetOfferedEvent(f.Id, use.Id, draft.ActionId, f.InstructionIndex, BoundaryWuYiSource(f), f.GameplayHash, draft.Original, Array.AsReadOnly(BoundaryWuYiTargetCandidates(use, draft).Select(pair => pair[0]).Distinct().ToArray())));
        PublishBoundaryWuYiTargets(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryWuYiTargetChoices(ProgramSkillFrame f)
    {
        var draft = f.BoundaryWuYiTargets!;
        var use = RequireBoundaryWuYiUse(f, out _);
        var chosen = use.CardKind == CardKind.BorrowedSword ? draft.Added.Count / 2 : draft.Added.Count;
        var choices = new List<PromptChoice>();
        if (chosen < draft.Grant.X)
            foreach (var pair in BoundaryWuYiTargetCandidates(use, draft))
                choices.Add(new(new("boundary-benxi.target." + string.Join('.', pair)), pair.Count == 2 ? $"额外令{(_coreMatchParticipantsPlayersAccess._players[pair[0]].Name)}对{(_coreMatchParticipantsPlayersAccess._players[pair[1]].Name)}使用杀" : $"额外指定{(_coreMatchParticipantsPlayersAccess._players[pair[0]].Name)}", [], pair, new Dictionary<string, string> { ["program-action"] = "boundary-benxi-targets", ["branch"] = "target" }));
        choices.Add(new(new("boundary-benxi.finish"), chosen == 0 ? "不追加目标" : $"确认追加{(chosen)}名目标", [], [], new Dictionary<string, string> { ["program-action"] = "boundary-benxi-targets", ["branch"] = "finish" }));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishBoundaryWuYiTargets(ProgramSkillFrame f)
    {
        var choices = BoundaryWuYiTargetChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"奔袭：至多追加{(f.BoundaryWuYiTargets!.Grant.X)}名距离为1的其他目标。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = false,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("boundary-benxi-targets")]
    internal void ResolveBoundaryWuYiTargetChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Benxi lost its target-selection frame.");
        AssertBoundaryWuYiTargets(f);
        if (!BoundaryWuYiTargetChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Benxi must answer its currently published native target choice.");
        var draft = f.BoundaryWuYiTargets!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters.GetValueOrDefault("branch") == "target")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWuYiTargets = draft with { Added = [..draft.Added, ..selected.Targets] } });
            PublishBoundaryWuYiTargets(f);
            return;
        }

        var use = RequireBoundaryWuYiUse(f, out var window);
        IReadOnlyList<int> result = draft.Original;
        if (draft.Added.Count > 0)
        {
            result = Array.AsReadOnly(draft.Original.Concat(draft.Added).ToArray());
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, result);
            var simple = use.AdjustedSimpleContinuation;
            if (window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
            {
                var original = window.SimpleContinuation ?? throw new InvalidOperationException("Benxi lost its native basic-card continuation.");
                if (original.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol))
                    throw new InvalidOperationException("Benxi cannot replace a non-use basic-card effect.");
                simple ??= original with
                {
                    RecoveryPolicySources = original.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
                };
            }

            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = result, Action = action, AdjustedSimpleContinuation = simple, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            foreach (var seat in use.CardKind == CardKind.BorrowedSword ? draft.Added.Where((_, i) => i % 2 == 0) : draft.Added)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(f.Id, f.SkillId, f.OwnerSeat, use.Id, seat));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetResolvedEvent(f.Id, use.Id, draft.ActionId, f.InstructionIndex, BoundaryWuYiSource(f), f.GameplayHash, draft.Original, draft.Added, result));
        if (draft.Added.Count > 0)
            _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, draft.Added);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryWuYiTargets = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void AssertBoundaryWuYiTargets(ProgramSkillFrame f)
    {
        if (f.BoundaryWuYiTargets is not { } draft)
            return;
        var use = RequireBoundaryWuYiUse(f, out var window);
        var primary = use.CardKind == CardKind.BorrowedSword ? draft.Added.Where((_, i) => i % 2 == 0).ToArray() : draft.Added.ToArray();
        var offered = _coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetOfferedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (f.SkillId != BoundaryWuYiComposition.Skill || f.TriggerId != BoundaryWuYiComposition.Targets || f.InstructionIndex != 1 || draft.UseId != use.Id || draft.ActionId != use.Action!.ActionId || draft.WindowId != window.Id || draft.Grant != BoundaryWuYiIssuedGrant(use.Id, draft.ActionId, f.OwnerSeat, f.SkillInstanceId) || !draft.Original.SequenceEqual(use.TargetSeats) || primary.Length > draft.Grant.X || primary.Distinct().Count() != primary.Length || use.CardKind == CardKind.BorrowedSword && draft.Added.Count % 2 != 0 || offered is not [var fact] || fact.CardUseFrameId != use.Id || fact.ActionId != draft.ActionId || fact.Source != BoundaryWuYiSource(f) || fact.GameplayHash != f.GameplayHash || !fact.OriginalTargetSeats.SequenceEqual(draft.Original) || primary.Any(seat => seat == f.OwnerSeat || !fact.CandidateTargetSeats.Contains(seat)) || _coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetResolvedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Benxi lost its original first-use grant, target prefix or bounded extra-target draft.");
        if (ReferenceEquals(f, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: false } prompt || prompt.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, BoundaryWuYiTargetChoices(f))))
            throw new InvalidOperationException("Benxi must retain its owning actor's current public target menu.");
    }

    internal bool IsBoundaryWuYiTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> preceding)
    {
        if (fact.Source.SkillId != BoundaryWuYiComposition.Skill || fact.Source.BindingId != BoundaryWuYiComposition.Targets || use.Action is not { } action || fact.CardUseFrameId != use.Id || fact.ActionId != action.ActionId || fact.Source.OwnerSeat != action.ActorSeat || fact.InstructionIndex != 1 || !fact.OriginalTargetSeats.SequenceEqual(preceding) || !fact.ResultTargetSeats.SequenceEqual(preceding.Concat(fact.AddedTargetSeats)) || BoundaryWuYiIssuedGrant(use.Id, fact.ActionId, fact.Source.OwnerSeat, fact.Source.SkillInstanceId)is not { } grant || fact.GameplayHash != grant.Hash)
            return false;
        var primary = use.CardKind == CardKind.BorrowedSword ? fact.AddedTargetSeats.Where((_, i) => i % 2 == 0).ToArray() : fact.AddedTargetSeats.ToArray();
        var originalMain = use.CardKind == CardKind.BorrowedSword ? preceding.Where((_, i) => i % 2 == 0).ToArray() : preceding;
        if (primary.Length < 1 || primary.Length > grant.X || primary.Distinct().Count() != primary.Length || use.CardKind == CardKind.BorrowedSword && fact.AddedTargetSeats.Count % 2 != 0 || fact.AddedTargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || primary.Any(seat => seat == fact.Source.OwnerSeat || originalMain.Contains(seat)))
            return false;
        var offered = _coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetOfferedEvent>().Where(e => e.ProgramFrameId == fact.ProgramFrameId).ToArray();
        return offered is [var offer] && offer.CardUseFrameId == use.Id && offer.ActionId == fact.ActionId && offer.InstructionIndex == 1 && offer.Source == fact.Source && offer.GameplayHash == fact.GameplayHash && offer.OriginalTargetSeats.SequenceEqual(preceding) && primary.All(offer.CandidateTargetSeats.Contains) && _coreProgramEventHistoryOperations.ProgramEventHistory<DesignatedExtraTargetResolvedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.OwnerSeat == fact.Source.OwnerSeat && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && primary.All(seat => _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseTargetAddedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.CardUseFrameId == use.Id && e.SkillId == fact.Source.SkillId && e.OwnerSeat == fact.Source.OwnerSeat && e.TargetSeat == seat) == 1);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryWuYiTargets)]
    internal PromptChoice SelectAiBoundaryWuYiTargets(PendingDecision decision, ProgramSkillFrame f)
    {
        var kind = RequireBoundaryWuYiUse(f, out _).CardKind;
        var helpful = kind is CardKind.Peach or CardKind.Alcohol or CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains;
        return decision.Choices.FirstOrDefault(c => c.Targets.Count > 0 && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]) == helpful) ?? decision.Choices.Single(c => c.Targets.Count == 0);
    }
}
