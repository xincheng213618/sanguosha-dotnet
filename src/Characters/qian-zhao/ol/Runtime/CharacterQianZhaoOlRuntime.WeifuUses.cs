#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 110)]
    internal bool WeifuNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasWeifuCapability;
    internal bool HasWeifuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWeifuJudgment);

    internal IReadOnlyList<WeifuGrantedEvent> ActiveWeifuGrants(int actor, CardKind kind)
    {
        if (!HasWeifuCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive)
            return[];
        var category = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
        if (category == SkillProgramCardCategory.Equipment)
            return[];
        var consumed = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantConsumedEvent>().Select(e => e.GrantFrameId).ToHashSet();
        var expired = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantExpiredEvent>().Select(e => e.GrantFrameId).ToHashSet();
        return Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantedEvent>().Where(e => e.Source.OwnerSeat == actor && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Category == category && !consumed.Contains(e.FrameId) && !expired.Contains(e.FrameId) && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash).ToArray());
    }

    internal void IssueWeifuUse(long id, CardActionContext action)
    {
        if (action.Type != CardActionType.Use || ActiveWeifuGrants(action.ActorSeat, action.EffectiveKind)is not { Count: > 0 } grants)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("Weifu's matching actual use has no owning native frame.");
        if (use.WeifuUse is not null || use.Action?.ActionId != action.ActionId || use.SourceSeat != action.ActorSeat || use.CardKind != action.EffectiveKind)
            throw new InvalidOperationException("Weifu cannot duplicate or replace its actual effective-category use.");
        var category = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, f => f with { WeifuUse = new(id, action.ActionId, action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, category) { Grants = grants, OriginalTargets = f.TargetSeats, ProviderSeat = action.ProviderSeat } });
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuGrantConsumedEvent(grant.FrameId, id, action.ActionId, action.ActorSeat, category, false));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuUseIssuedEvent(id, action.ActionId, action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, category, grants.Count, action.ProviderSeat));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26621)]
    internal void ConsumeWeifuCounterspell(IGameEvent payload)
    {
        if (!HasWeifuCapability || payload is not CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } a } || a.ActorSeat != a.ProviderSeat || a.ResponderSeat != a.ActorSeat || a.RequesterSeat is not null || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } window || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.Action is not { } parent || a.ParentActionId != parent.ActionId || a.OpponentSeat != window.SourceSeat)
            return;
        foreach (var grant in ActiveWeifuGrants(a.ActorSeat, a.EffectiveKind))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuGrantConsumedEvent(grant.FrameId, window.Id, a.ActionId, a.ActorSeat, SkillProgramCardCategory.Trick, true));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26623)]
    internal void ConsumeWeifuActualDodge(IGameEvent payload)
    {
        if (!HasWeifuCapability || payload is not CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge } a } || a.ActorSeat != a.ProviderSeat || a.ResponderSeat != a.ActorSeat || a.RequesterSeat is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { EffectiveCardKind: { } incoming } attack || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) || attack.TargetSeat != a.ActorSeat || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], CardKind.Dodge) || a.OpponentSeat != attack.SourceSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action is not { Type: CardActionType.Use } parent || parent.ActionId != a.ParentActionId || parent.ActorSeat != attack.SourceSeat || parent.EffectiveKind != incoming)
            return;
        foreach (var grant in ActiveWeifuGrants(a.ActorSeat, CardKind.Dodge))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuGrantConsumedEvent(grant.FrameId, attack.ResolutionId, a.ActionId, a.ActorSeat, SkillProgramCardCategory.Basic, false, CardKind.Dodge));
    }

    internal void ExpireWeifuActualTurnGrants()
    {
        if (!HasWeifuCapability)
            return;
        var consumed = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantConsumedEvent>().Select(e => e.GrantFrameId).ToHashSet();
        var expired = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantExpiredEvent>().Select(e => e.GrantFrameId).ToHashSet();
        foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && !consumed.Contains(e.FrameId) && !expired.Contains(e.FrameId)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuGrantExpiredEvent(grant.FrameId, grant.ActualTurn, grant.TurnOwnerSeat));
    }

    internal bool HasWeifuUseDistance(CharacterState actor, CardKind kind, long? cardUseFrameId) => cardUseFrameId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { WeifuUse: { } r, Action: { } a } use && r.UseId == id && r.ActionId == a.ActionId && a.ActorSeat == actor.Seat && use.SourceSeat == actor.Seat && use.CardKind == kind && r.Category == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) && WeifuUseProof(use) : ActiveWeifuGrants(actor.Seat, kind).Count > 0;
    internal bool WeifuUseProof(CardUseFrame use) => use.WeifuUse is { Grants.Count: > 0 } r && use.Action is { } a && r.UseId == use.Id && r.ActionId == a.ActionId && WeifuCurrentActorMatches(use, r) && a.ActorSeat == use.SourceSeat && a.EffectiveKind == use.CardKind && r.Category == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) && r.Grants.Select(g => g.FrameId).Distinct().Count() == r.Grants.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuUseIssuedEvent>().Count(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.ActorSeat == r.ActorSeat && e.ProviderSeat == r.ProviderSeat && e.ActualTurn == r.ActualTurn && e.TurnOwnerSeat == r.TurnOwnerSeat && e.Category == r.Category && e.GrantCount == r.Grants.Count) == 1 && r.Grants.All(g => g.Source.OwnerSeat == r.ActorSeat && g.ActualTurn == r.ActualTurn && g.TurnOwnerSeat == r.TurnOwnerSeat && g.Category == r.Category && _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantedEvent>().Count(e => e == g) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantConsumedEvent>().Count(e => e.GrantFrameId == g.FrameId && e.UseId == use.Id && e.ActionId == a.ActionId && e.ActorSeat == r.ActorSeat && e.Category == r.Category && !e.Counterspell) == 1);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 16)]
    internal void CollectIssuedWeifuCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { WeifuUse: { Offered: false } r } use || use.Action?.ActionId != action.ActionId || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !WeifuCanAddTargets(use) || !WeifuUseProof(use))
            return;
        foreach (var grant in r.Grants.DistinctBy(g => g.Source.SkillInstanceId))
        {
            var p = _contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program!;
            var t = p.Triggers.Single(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferWeifuCategoryTargets));
            if (result.Any(c => c.OwnerSeat == r.ActorSeat && c.SkillId == p.Id && c.SkillInstanceId == grant.Source.SkillInstanceId && c.TriggerId == t.Id))
                continue;
            result.Add(new(r.ActorSeat, r.ActorSeat, p.Id, t.Id, p.GameplayHash, grant.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, r.ActorSeat, r.ActorSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], action))));
        }
    }

    internal static bool WeifuCanAddTargets(CardUseFrame use) => global::CardGame.Core.RuntimeSkills.RuntimeRules_qian_zhao_ol_WeifuUses.WeifuCanAddTargets(use);
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 140)]
    internal bool HasIssuedWeifuCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:weifu" || context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } card } || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { WeifuUse: { } r, Action: { } a } use || !WeifuUseProof(use) || a.ActionId != card.CardActionId || r.ActorSeat != c.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != use.Id || window.Action.ActionId != a.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var candidate = window.Candidates[window.CandidateIndex];
        return candidate.OwnerSeat == c.OwnerSeat && candidate.SkillId == c.SkillId && candidate.TriggerId == c.BindingId && candidate.GameplayHash == c.GameplayHash && candidate.SkillInstanceId == c.SkillInstanceId && _coreCardActionsOperations.CreateCardActionProgramContext2(window, candidate) == context && r.Grants.Any(g => g.Source.SkillId == c.SkillId && g.Source.SkillInstanceId == c.SkillInstanceId && g.GameplayHash == c.GameplayHash) && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.Triggers.Any(t => t.Id == c.BindingId && t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferWeifuCategoryTargets));
    }

    internal CardUseFrame RequireWeifuTargetUse(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context || !HasIssuedWeifuCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), context))
            throw new InvalidOperationException("Weifu's target child lost its original issued native use.");
        return _coreCardUseLifecycleOperations.LifecycleCardUse(context.CardUse!.ParentCardUseFrameId)!;
    }

    internal SkillProgramStepOutcome BeginWeifuTargets(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireWeifuTargetUse(f);
        if (f.WeifuTargets is not null || use.WeifuUse!.Offered || f.InstructionIndex != 1)
            throw new InvalidOperationException("Weifu's matching actual use can offer its category stack only once.");
        if (use.CardKind == CardKind.DrawTwo && use.TargetSeats.Count == 0)
        {
            var targets = Array.AsReadOnly(new[] { use.SourceSeat });
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action }, action);
            use = RequireWeifuTargetUse(f);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WeifuTargets = new(QianZhaoSource(f), f.GameplayHash, f.InstructionIndex, use.Id, use.Action!.ActionId, use.WeifuUse!.Grants.Count) });
        if (!WeifuCanAddTargets(use) || WeifuTargetChoices(f).Count == 1)
            CompleteWeifuTargets(f);
        else
            PublishQianZhaoChoice(f.OwnerSeat, f, WeifuTargetChoices(f), "威抚：选择额外目标，或完成选择。", false);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> WeifuTargetChoices(ProgramSkillFrame f)
    {
        var r = f.WeifuTargets!;
        var use = RequireWeifuTargetUse(f);
        var result = new List<PromptChoice>();
        var width = use.CardKind == CardKind.BorrowedSword ? 2 : 1;
        if (r.Selected.Count / width < r.Maximum)
        {
            var targets = Array.AsReadOnly(use.TargetSeats.Concat(r.Selected).ToArray());
            var preview = use with
            {
                TargetSeats = targets,
                Action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets)
            };
            foreach (var seats in _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidates(preview))
                result.Add(new(new($"weifu.{(f.Id)}.target.{(string.Join('.', seats))}"), "增加目标：" + string.Join("、", seats.Select(s => _coreMatchParticipantsPlayersAccess._players[s].Name)), [], seats, new Dictionary<string, string> { ["program-action"] = "qian-zhao", ["branch"] = "weifu-target" }));
        }

        result.Add(new(new($"weifu.{(f.Id)}.finish"), "完成额外目标选择", [], [], new Dictionary<string, string> { ["program-action"] = "qian-zhao", ["branch"] = "weifu-finish" }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void CompleteWeifuTargets(ProgramSkillFrame f)
    {
        var r = f.WeifuTargets!;
        var use = RequireWeifuTargetUse(f);
        var targets = Array.AsReadOnly(use.TargetSeats.Concat(r.Selected).ToArray());
        var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
        var simple = use.AdjustedSimpleContinuation;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == f.WindowContext!.ParentFrameId);
        if (r.Selected.Count > 0 && window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
        {
            var continuation = window.SimpleContinuation ?? throw new InvalidOperationException("Weifu's actual basic card lost its native effect.");
            if (continuation.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol))
                throw new InvalidOperationException("Weifu cannot fabricate a simple-card effect.");
            simple ??= continuation with
            {
                RecoveryPolicySources = continuation.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
            };
        }

        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, AdjustedSimpleContinuation = simple, TargetsAdjusted = use.TargetsAdjusted || r.Selected.Count > 0, WeifuUse = use.WeifuUse!with { Offered = true, OriginalTargets = use.TargetSeats, AddedTargets = r.Selected }, Enhancements = r.Selected.Count == 0 ? use.Enhancements : use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
        for (var i = 0; i < r.Selected.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuTargetAddedEvent(f.Id, use.Id, action.ActionId, r.Source, r.Selected[i], i));
        FinishQianZhao(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void PublishQianZhaoChoice(int chooser, ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices, string text, bool isPrivate)
    {
        if (choices.Count == 0)
            throw new InvalidOperationException("Qian Zhao has no legal native continuation choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = isPrivate,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 90)]
    internal bool DistanceCardUseExemption90HasWeifuUseDistance(ProgramCardDistanceQuery q) => HasWeifuUseDistance(q.Actor, q.Kind, q.CardUseFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 40)]
    internal bool DistancePhysicalSlash40HasWeifuUseDistance(ProgramSlashDistanceQuery q) => q.ExistingUseFrameId is not null && HasWeifuUseDistance(q.Actor, q.Kind, q.ExistingUseFrameId);
}
