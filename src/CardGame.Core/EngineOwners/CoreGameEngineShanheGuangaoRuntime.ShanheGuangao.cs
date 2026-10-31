#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheGuangaoRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheGuangaoHost.Resolve(ProgramSkillFrame frame) => BeginShanheGuangao(frame);
    internal CardUseFrame RequireShanheGuangaoUse(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "shanhe:companion-guangao" || frame.InstructionIndex != 1 || frame.WindowContext is not { } context || !_coreAdjacentRedTargetsOperations.TryGetAdjacentRedTargetUse(frame.OwnerSeat, context, out var use, out var window) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects is not [{ Op: SkillProgramEffectOp.ResolveShanheGuangao }] || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
            throw new InvalidOperationException("Guangao lost its exact native Slash, owner or current observer.");
        return use;
    }

    internal int[] ShanheGuangaoCandidates(int owner, CardUseFrame use)
    {
        var action = use.Action!;
        var ignoresDistance = _coreKuangfuReceiptsOperations.HasIssuedKuangfuDistance(use.Id, use.SourceSeat) || use.UnlimitedUse && !use.QuotaOnlyUseLimitExemption || use.CardAttack?.ProgramSkillCardUseFrameId is { } parentId && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == parentId)is { } parent && ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect is { Op: SkillProgramEffectOp.UseVirtualCard, TargetRestriction: SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget };
        return _coreMatchParticipantsPlayersAccess._players.Where(target => (action.ActorSeat == owner || target.Seat == owner) && (ignoresDistance ? _coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, target) : _coreRecipientCategorySlashTargetsOperations.CanBeRecipientCategorySlashTarget(use, target))).Select(target => target.Seat).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5405)]
    internal bool CanOfferShanheGuangao(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheGuangao) || candidate.SkillId == "shanhe:companion-guangao" && _coreAdjacentRedTargetsOperations.TryGetAdjacentRedTargetUse(candidate.OwnerSeat, context, out var use, out _) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && ShanheGuangaoCandidates(candidate.OwnerSeat, use).Length > 0;
    internal SkillProgramStepOutcome BeginShanheGuangao(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireShanheGuangaoUse(frame);
        if (frame.ShanheGuangao is not null)
            throw new InvalidOperationException("Guangao cannot repeat its accepted Slash instruction.");
        if (ShanheGuangaoCandidates(frame.OwnerSeat, use).Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheGuangao = new() { CardUseFrameId = use.Id, ActionId = use.Action!.ActionId, OriginalTargets = use.TargetSeats } });
        PublishShanheGuangaoPrompt(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShanheGuangaoChoices(ProgramSkillFrame frame)
    {
        var draft = frame.ShanheGuangao!;
        var use = RequireShanheGuangaoUse(frame);
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "shanhe-guangao",
            ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture),
            ["stage"] = draft.Stage
        };
        if (draft.Stage == "target")
            return new[]
            {
                new PromptChoice(new($"guangao.{(frame.Id)}.decline"), "不发动犷骜", [], [], Parameters())
            }.Concat(ShanheGuangaoCandidates(frame.OwnerSeat, use).Select(target => new PromptChoice(new($"guangao.{(frame.Id)}.add.{(target)}"), $"额外指定 {(_coreMatchParticipantsPlayersAccess._players[target].Name)}", [], [target], Parameters()))).ToArray();
        if (draft.Stage != "nullify")
            throw new InvalidOperationException("Guangao choices require a real selection stage.");
        var targets = use.TargetSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        return Enumerable.Range(0, 1 << targets.Length).Select(mask =>
        {
            var selected = targets.Where((_, index) => (mask & (1 << index)) != 0).ToArray();
            return new PromptChoice(new($"guangao.{(frame.Id)}.nullify.{(mask)}"), selected.Length == 0 ? "所有目标正常结算" : "令此杀对" + string.Join("、", selected.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)) + "无效", [], selected, Parameters());
        }).ToArray();
    }

    internal void PublishShanheGuangaoPrompt(ProgramSkillFrame frame)
    {
        var choices = ShanheGuangaoChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheGuangao = frame.ShanheGuangao!with { Choices = choices } });
        var chooser = frame.ShanheGuangao.Stage == "target" ? RequireShanheGuangaoUse(frame).SourceSeat : frame.OwnerSeat;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, frame.ShanheGuangao.Stage == "target" ? "犷骜：是否额外指定一名合法目标？" : "犷骜：选择此杀无效的目标，可不选。", [], choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shanhe-guangao")]
    internal void ResolveShanheGuangaoChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Guangao lost its owning frame.");
        AssertShanheGuangao(frame);
        var draft = frame.ShanheGuangao!;
        var choice = draft.Choices.SingleOrDefault(candidate => candidate.Id == selected.Id);
        if (choice is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected]))
            throw new InvalidOperationException("Guangao changed its issued choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var use = RequireShanheGuangaoUse(frame);
        if (draft.Stage == "target")
        {
            if (choice.Targets.Count == 0)
            {
                FinishShanheGuangao(frame);
                return;
            }

            if (choice.Targets is not [var target] || !ShanheGuangaoCandidates(frame.OwnerSeat, use).Contains(target))
                throw new InvalidOperationException("Guangao requires one current legal additional Slash target.");
            var targets = Array.AsReadOnly(use.TargetSeats.Append(target).ToArray());
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
            var updated = use with
            {
                TargetSeats = targets,
                Action = action,
                TargetsAdjusted = true,
                Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget,
                EnhancementOwnerSeat = frame.OwnerSeat
            };
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(updated, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, target));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, action.ActionId, frame.OwnerSeat, CurrentCardEnhancement.ExtraTarget, target));
            _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(updated, [target]);
            var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count;
            if (count % 2 != 0)
            {
                FinishShanheGuangao(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
                return;
            }

            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheGuangao = draft with { Stage = "draw", AddedTarget = target, HandCountBeforeDraw = count, Choices = [] } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, 1, global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuangaoDefinitions.ShanheGuangaoDrawReason);
            return;
        }

        if (draft.Stage != "nullify" || choice.Targets.Any(target => !use.TargetSeats.Contains(target)))
            throw new InvalidOperationException("Guangao can only nullify the current Slash targets.");
        foreach (var target in choice.Targets)
        {
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, use.SourceSeat, target, use.CardKind, CardEffectSkipReason.SkillNullified));
        }

        FinishShanheGuangao(frame);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheGuangao)]
    internal void ReturnShanheGuangaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        _ = RequireShanheGuangaoUse(frame);
        if (frame.ShanheGuangao is not { Stage: "draw", HandCountBeforeDraw: { } count, DrawOutcome: null } draft || count % 2 != 0 || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != 1 || frame.NativeDraw is not null)
            throw new InvalidOperationException("Guangao lost its once-issued even-hand draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheGuangao = draft with { Stage = "draw-children", DrawOutcome = outcome }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 5405)]
    internal bool IsShanheGuangaoDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) => effect?.Op == SkillProgramEffectOp.ResolveShanheGuangao && frame.ShanheGuangao is { Stage: "draw-children", DrawOutcome: not null } && movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Resume, 5405)]
    internal bool ResumeShanheGuangao(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.ShanheGuangao is not { Stage: "draw-children" } draft)
            return false;
        _ = RequireShanheGuangaoUse(frame);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheGuangao = draft with { Stage = "nullify" }, PendingMovementContinuation = null });
        PublishShanheGuangaoPrompt(frame);
        return true;
    }

    internal void FinishShanheGuangao(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheGuangao = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 5405)]
    internal void AssertShanheGuangao(ProgramSkillFrame frame)
    {
        if (frame.ShanheGuangao is not { } draft)
            return;
        var use = RequireShanheGuangaoUse(frame);
        if (draft.CardUseFrameId != use.Id || draft.ActionId != use.Action!.ActionId || draft.OriginalTargets.Count == 0 || draft.OriginalTargets.Distinct().Count() != draft.OriginalTargets.Count || draft.Stage is not ("target" or "draw" or "draw-children" or "nullify") || (draft.Stage == "target" ? !draft.OriginalTargets.SequenceEqual(use.TargetSeats) || draft.AddedTarget is not null || draft.DrawOutcome is not null : draft.AddedTarget is not { } target || draft.OriginalTargets.Contains(target) || !draft.OriginalTargets.Append(target).SequenceEqual(use.TargetSeats) || draft.HandCountBeforeDraw is not { } count || count % 2 != 0))
            throw new InvalidOperationException("Guangao lost its frozen original targets, accepted extra target or paid stage.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && (draft.Stage is not ("target" or "nullify") || prompt.PlayerSeat != (draft.Stage == "target" ? use.SourceSeat : frame.OwnerSeat) || prompt.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, draft.Choices) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(draft.Choices, ShanheGuangaoChoices(frame))))
            throw new InvalidOperationException("Guangao lost its exact public owner choice.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheGuangao)]
    internal PromptChoice SelectAiShanheGuangao(PendingDecision decision, ProgramSkillFrame frame)
    {
        var use = RequireShanheGuangaoUse(frame);
        if (frame.ShanheGuangao?.Stage == "target")
            return decision.Choices.Where(choice => choice.Targets.Count > 0).FirstOrDefault(choice => use.SourceSeat == frame.OwnerSeat ? !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[choice.Targets[0]]) : (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count % 2 == 0) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])) ?? decision.Choices.Single(choice => choice.Targets.Count == 0);
        return decision.Choices.OrderByDescending(choice => choice.Targets.Sum(target => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target]) ? 2 : -3)).ThenBy(choice => choice.Targets.Count).First();
    }
}
