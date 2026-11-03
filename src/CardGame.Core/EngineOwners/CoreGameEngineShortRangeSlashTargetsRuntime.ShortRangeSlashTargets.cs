#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShortRangeSlashTargetsRuntime
{
    internal bool HasShortRangeSlashCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferShortRangeSlashTarget);

    internal int FreezeShortRangeSlashResponseRequirement(long attackId, int source, int target, CardKind kind, int ordinary)
    {
        var state = _coreCardAttackStateOperations.GetCardAttackState(attackId);
        if (state.ShortRangeResponseRequirement is { } issued && issued.ActorSeat == source && issued.TargetSeat == target)
            return issued.RequiredDodgeResponses;
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || !_coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[source], SkillProgramCardPolicyKind.MinimumSlashResponseAtDistanceOne, kind, CardKind.Dodge).Any())
            return ordinary;
        // Freeze at this native target's first response window, after final targets and redirection.
        _coreCardAttackStateOperations.UpdateCardAttackState(attackId, current => current!with { ShortRangeResponseRequirement = new(source, target, ordinary) });
        return ordinary;
    }

    internal CardConversionSource ShortRangeSlashSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal CardUseFrame? ShortRangeSlashUse(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && context.CardUse is { } identity && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.ParentFrameId == identity.ParentCardUseFrameId && CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) == context.Window && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } use && action.ActionId == identity.CardActionId && window.Action.ActionId == action.ActionId && action.ActorSeat == owner && use.SourceSeat == owner && action.EffectiveKind == use.CardKind && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return use;
        if (context.Window == SkillProgramTriggerWindow.OtherActualUseTargeted && context.ActualUseTarget is { ActionId: null } legacy && legacy.ActorSeat == owner && _coreActualUseTargetProgramsOperations.MatchesActualUseTarget(legacy) && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } parent && parent.ParentFrameId == legacy.CardUseFrameId && _coreCardUseLifecycleOperations.LifecycleCardUse(legacy.CardUseFrameId)is { Action: null } old && old.SourceSeat == owner && CoreNativeCardClassificationsRuntime.IsSlashCard(old.CardKind))
            return old;
        return null;
    }

    internal bool ShortRangeSlashWasOffered(long useId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShortRangeSlashTargetResolvedEvent>().Any(e => e.CardUseFrameId == useId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 110)]
    internal bool CanRunShortRangeSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferShortRangeSlashTarget))
            return true;
        return ShortRangeSlashUse(candidate.OwnerSeat, context)is { } use && !ShortRangeSlashWasOffered(use.Id) && ShortRangeSlashTargets(use).Length > 0;
    }

    internal int[] ShortRangeSlashTargets(CardUseFrame use)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        return _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && target.Seat != actor.Seat && !use.TargetSeats.Contains(target.Seat) && _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, target.Seat) == 1 && (use.Action is not null ? _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, target) : !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, use.CardKind, use.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) && _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(actor, target, use.CardKind, null, use.Id))).OrderBy(target => _coreTableOrderOperations.RelativeTableOrder(actor.Seat, target.Seat)).Select(target => target.Seat).ToArray();
    }

    internal SkillProgramStepOutcome OfferShortRangeSlashTarget(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = frame.WindowContext is { } context ? ShortRangeSlashUse(frame.OwnerSeat, context) : null;
        if (use is null || frame.InstructionIndex != 1 || _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects is not [{ Op: SkillProgramEffectOp.OfferShortRangeSlashTarget }])
            throw new InvalidOperationException("A short-range target offer requires its exact final-use instruction.");
        if (ShortRangeSlashWasOffered(use.Id) || ShortRangeSlashTargets(use).Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShortRangeSlashTargetDraft = new(use.Id, use.Action?.ActionId, frame.InstructionIndex) });
        PublishShortRangeSlashTarget(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShortRangeSlashChoices(ProgramSkillFrame frame)
    {
        var use = ShortRangeSlashUse(frame.OwnerSeat, frame.WindowContext!) ?? throw new InvalidOperationException("The short-range choice lost its true owning use.");
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "short-range-slash-target",
            ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture)
        };
        var choices = ShortRangeSlashTargets(use).Select(target => new PromptChoice(new($"short-range.{(frame.Id)}.{(target)}"), $"额外指定距离为1的 {(_coreMatchParticipantsPlayersAccess._players[target].Name)}", [], [target], Parameters())).ToList();
        choices.Add(new(new($"short-range.{(frame.Id)}.decline"), "不追加目标", [], [], Parameters()));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishShortRangeSlashTarget(ProgramSkillFrame frame)
    {
        var choices = ShortRangeSlashChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "可额外指定一名距离为1的角色为杀的目标。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("short-range-slash-target")]
    internal void ResolveShortRangeSlashTarget(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The short-range chooser lost its owning program.");
        var choices = ShortRangeSlashChoices(frame);
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], [choices.Single(c => c.Id == selected.Id)]))
            throw new InvalidOperationException("Short-range selection changed its exact target.");
        var use = ShortRangeSlashUse(frame.OwnerSeat, frame.WindowContext!)!;
        var source = ShortRangeSlashSource(frame);
        ShortRangeSlashTargetReceipt? receipt = null;
        if (selected.Targets is [var target] && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            receipt = new(frame.Id, source, frame.GameplayHash, use.Id, use.Action?.ActionId, use.SourceSeat, target, use.TargetSeats);
            var targets = Array.AsReadOnly(use.TargetSeats.Append(target).ToArray());
            var updated = use with
            {
                TargetSeats = targets,
                ShortRangeSlashTarget = receipt,
                Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget
            };
            if (use.Action is { } action)
            {
                var newAction = _coreProgramCardUseRolesOperations.CloneRoleAction(action, action.ActorSeat, targets);
                _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(updated with { Action = newAction }, newAction);
                _coreTieredRoundActualTargetWindowsOperations.SyncIssuedTieredRoundZeroTrickTargetWindows(use.Id, action);
            }
            else
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, _ => updated);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, target));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShortRangeSlashTargetResolvedEvent(use.Id, use.Action?.ActionId, source, receipt is not null, receipt));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShortRangeSlashTargetDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferShortRangeSlashTarget)]
    internal PromptChoice SelectAiShortRangeSlash(PendingDecision decision, ProgramSkillFrame frame)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        var hint = new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
        var best = decision.Choices.Where(c => c.Targets.Count == 1).Select(c => (Choice: c, Score: brain.ScoreProgramTarget(view, c.Targets[0], hint))).OrderByDescending(item => item.Score).ThenBy(item => item.Choice.Targets[0]).FirstOrDefault();
        return best.Choice is not null && best.Score > 0 ? best.Choice : decision.Choices.Single(c => c.Targets.Count == 0);
    }




    internal void AppendShortRangeLegacyCandidate(CardUseFrame use, long windowId, List<(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context)> entries)
    {
        if (!HasShortRangeSlashCapability || use.Action is not null || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || ShortRangeSlashWasOffered(use.Id) || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || use.TargetSeats.Count == 0 || ShortRangeSlashTargets(use).Length == 0 || _coreActualUseTargetProgramsOperations.FreezeActualUseTarget(use, use.TargetSeats[0])is not { ActionId: null } identity)
            return;
        foreach (var candidate in _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], SkillProgramTriggerWindow.OtherActualUseTargeted, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat])).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.OfferShortRangeSlashTarget }]))
            entries.Add((candidate, new(SkillProgramTriggerWindow.OtherActualUseTargeted, windowId, candidate.OwnerSeat, SourceSeat: use.SourceSeat, TargetSeat: identity.TargetSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat])) { ActualUseTarget = identity }));
    }

    internal bool IsShortRangeLegacyCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.OtherActualUseTargeted || _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is not [{ Op: SkillProgramEffectOp.OfferShortRangeSlashTarget }])
            return false;
        return ShortRangeSlashUse(candidate.OwnerSeat, context)is not null && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate && window.Contexts[window.CandidateIndex] == context;
    }

    SkillProgramStepOutcome IDirectedDistanceDebtHost.OfferShortRangeSlashTarget(ProgramSkillFrame frame) => OfferShortRangeSlashTarget(frame);
    void IDirectedDistanceDebtHost.GrantFixedDistanceOneTurnPolicy(ProgramSkillFrame frame, int target) => _coreFixedDistanceOneDebtOperations.Value.GrantFixedDistanceOneTurnPolicy(frame, target);
    SkillProgramStepOutcome IDirectedDistanceDebtHost.SettleFixedDistanceOneEndingDebt(ProgramSkillFrame frame) => _coreFixedDistanceOneDebtOperations.Value.SettleFixedDistanceOneEndingDebt(frame);
}
