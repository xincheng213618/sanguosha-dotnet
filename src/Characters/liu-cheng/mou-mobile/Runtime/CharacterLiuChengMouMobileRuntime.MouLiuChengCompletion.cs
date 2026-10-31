#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuChengMouMobileRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 30)]
    internal bool MouLiuChengFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => HasMouLiuChengCapability && (CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) || IsMouLiuChengNonDamageOrdinary(query.Kind));
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 10)]
    internal bool MouLiuChengImplicitSelfDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasMouLiuChengCapability;
    internal const string MouLiuChengDrawReason = "skill-program.mou-mobile:liu-cheng.draw";
    internal static CardConversionSource MouLiuChengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_liu_cheng_mou_mobile_MouLiuChengCompletion.MouLiuChengSource(f);
    internal bool HasMouLiuChengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLiuChengCompletion);

    internal bool MouLiuChengKind(string skill, CardKind kind) => skill == MouLiuChengComposition.Lueying ? CoreNativeCardClassificationsRuntime.IsSlashCard(kind) : skill == MouLiuChengComposition.Yingwu && IsMouLiuChengNonDamageOrdinary(kind);
    internal static bool IsMouLiuChengNonDamageOrdinary(CardKind kind) => RuntimeSkills.RuntimeRules_liu_cheng_mou_mobile_MouLiuChengCompletion.IsMouLiuChengNonDamageOrdinary(kind);
    internal bool MouLiuChengTrueUse(CardActionContext a) => a.Type == CardActionType.Use || a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Nullification && _coreResponseUseCompletionOperations.IsCompletedResponseUse(a, ProgramCardContinuation.NullificationResponse);
    internal void CaptureMouLiuChengCompletion(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasMouLiuChengCapability || window != SkillProgramTriggerWindow.CardUseCompleted || !MouLiuChengTrueUse(action) || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent)
            return;
        var claims = new List<MouLiuChengUseClaim>();
        foreach (var b in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).GetInstanceTriggers(window).Where(b => MouLiuChengComposition.IsSkill(b.SkillId) && b.Trigger.Id == "after-use" && MouLiuChengComposition.IsProgram(b.Program) && MouLiuChengKind(b.SkillId, action.EffectiveKind)))
        {
            if (parent.MouLiuChengClaims?.Any(q => q.ActionId == action.ActionId && q.Source.SkillId == b.SkillId && q.Source.SkillInstanceId == b.SkillInstanceId) == true)
                continue;
            var claim = new MouLiuChengUseClaim(new(b.SkillId, b.Trigger.Id, action.ActorSeat, b.SkillInstanceId), b.Program.GameplayHash, parent.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            claims.Add(claim);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengCompletionQualifiedEvent(claim));
        }

        if (claims.Count > 0)
        {
            parent = _resolutionStackOperations._resolutionStack.Single(f => f.Id == parent.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouLiuChengClaims = (parent.MouLiuChengClaims ?? []).Concat(claims).ToArray() });
        }
    }

    internal bool ValidMouLiuChengClaim(MouLiuChengUseClaim q, CardActionContext a) => MouLiuChengComposition.IsSkill(q.Source.SkillId) && q.Source.BindingId == "after-use" && q.Source.OwnerSeat == q.ActorSeat && q.ActionId == a.ActionId && q.ActorSeat == a.ActorSeat && q.ProviderSeat == a.ProviderSeat && q.EffectiveKind == a.EffectiveKind && MouLiuChengKind(q.Source.SkillId, a.EffectiveKind) && q.MovementSequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash == q.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengCompletionQualifiedEvent>().Count(e => e.Claim == q) == 1;
    internal MouLiuChengUseClaim? CompletedMouLiuChengClaim(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (!MouLiuChengComposition.IsSkill(candidate.SkillId) || candidate.BindingId != "after-use" || c.Window != SkillProgramTriggerWindow.CardUseCompleted || c.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != c.Window || window.Action.ActionId != identity.CardActionId || window.Action.Type == CardActionType.Response && window.CompletedResponseReturn is not { IsCommitted: false } || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == window.ParentFrameId)?.MouLiuChengClaims is not { } claims)
            return null;
        return claims.SingleOrDefault(q => q.NativeParentId == window.ParentFrameId && q.ActionId == identity.CardActionId && q.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) && q.GameplayHash == candidate.GameplayHash && ValidMouLiuChengClaim(q, window.Action));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29401, SkillProgramEffectOp.ResolveMouLiuChengCompletion)]
    internal bool CanCompleteMouLiuCheng(ProgramTriggerLegalityContext c) => c.Owner.IsAlive && MouLiuChengComposition.IsProgram(c.Program) && CompletedMouLiuChengClaim(c.Candidate, c.Context)is { } q && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouLiuChengChui) >= 2 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengCompletionStartedEvent>().Any(e => e.Claim.ActionId == q.ActionId && e.Claim.Source.OwnerSeat == c.OwnerSeat && e.Claim.Source.SkillId == c.SkillId);
    internal SkillProgramStepOutcome BeginMouLiuChengCompletion(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouLiuChengCompletion is not null)
            throw new InvalidOperationException("Liu Cheng cannot reissue an already paid completion.");
        if (f.TriggerId != "after-use" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { } c || CompletedMouLiuChengClaim(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), c)is not { } claim || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame w || !MouLiuChengCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), c, w))
            throw new InvalidOperationException("Liu Cheng requires the exact completed true Use and its source-bound native parent.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var before = owner.Markers.GetValueOrDefault(PlayerMarkerKind.MouLiuChengChui);
        if (before < 2 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengCompletionStartedEvent>().Any(e => e.Claim.ActionId == claim.ActionId && e.Claim.Source.OwnerSeat == f.OwnerSeat && e.Claim.Source.SkillId == f.SkillId))
            throw new InvalidOperationException("Liu Cheng must pay exactly two shared Chui once per matching completion.");
        var r = new MouLiuChengCompletionReceipt(claim, w.Id)
        {
            Stage = MouLiuChengStage.Drawing,
            Paid = true,
            MarkBefore = before,
            MarkAfter = before - 2,
            PaidSequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            OutputKind = f.SkillId == MouLiuChengComposition.Lueying ? CardKind.Dismantlement : CardKind.Slash
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengCompletion = r });
        ChangeMouLiuChengChui(f, -2);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengCompletionStartedEvent(f.Id, claim, w.Id, before, before - 2));
        return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(MouLiuChengDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouLiuChengCompletion)]
    internal void ReturnMouLiuChengDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouLiuChengCompletion is not { Stage: MouLiuChengStage.Drawing } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLiuChengDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Liu Cheng cannot draw or pay its completed-use marks twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengCompletion = r with { Stage = MouLiuChengStage.DrawChildren, DrawOutcome = o }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengDrawReturnedEvent(f.Id, 1, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId));
        ContinueMouLiuCheng(f.Id);
    }

    internal IReadOnlyList<PromptChoice> MouLiuChengChoices(ProgramSkillFrame f)
    {
        var plans = MouLiuChengNativePlans(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.MouLiuChengCompletion!.OutputKind, MouLiuChengSource(f));
        return Array.AsReadOnly(plans.Select((p, i) => new PromptChoice(new($"mou-liu-cheng.{(f.Id)}.use.{(i)}"), p.Description, [], p.Targets, new Dictionary<string, string> { ["program-action"] = "mou-liu-cheng-use", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "use", ["plan-index"] = i.ToString(CultureInfo.InvariantCulture), ["output-kind"] = p.Kind.ToString() })).Append(new PromptChoice(new($"mou-liu-cheng.{(f.Id)}.decline"), "不视为使用牌", [], [], new Dictionary<string, string> { ["program-action"] = "mou-liu-cheng-use", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "decline", ["output-kind"] = f.MouLiuChengCompletion.OutputKind.ToString() })).ToArray());
    }

    internal void ContinueMouLiuCheng(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLiuChengCompletion: { } r } f || f.Id != id)
            return;
        if (r.Stage is MouLiuChengStage.NativeUse or MouLiuChengStage.NativeIssuing or MouLiuChengStage.Drawing)
            throw new InvalidOperationException("Liu Cheng cannot resume before its exact native draw or Use returns.");
        if (DrainMouLiuChengChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouLiuChengCompletion!;
        if (r.Stage == MouLiuChengStage.NativeChildren || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishMouLiuCheng(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengCompletion = r with { Stage = MouLiuChengStage.UseChoice }, PendingMovementContinuation = null });
        var choices = MouLiuChengChoices(f);
        if (choices.Count == 1)
        {
            FinishMouLiuCheng(f);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, "已移除两个椎并完成摸牌：可以选择合法目标视为使用牌，或取消。", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool DrainMouLiuChengChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramAnswer("mou-liu-cheng-use")]
    internal void AnswerMouLiuCheng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLiuChengCompletion: { Stage: MouLiuChengStage.UseChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MouLiuChengRootMatches(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MouLiuChengChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Liu Cheng accepts only its own current published native-use plan.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "decline")
        {
            FinishMouLiuCheng(f);
            return;
        }

        var plans = MouLiuChengNativePlans(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], r.OutputKind, MouLiuChengSource(f));
        DispatchMouLiuCheng(f, plans[int.Parse(choice.Parameters["plan-index"], CultureInfo.InvariantCulture)]);
    }

    internal void FinishMouLiuCheng(ProgramSkillFrame f)
    {
        var r = f.MouLiuChengCompletion!;
        RestoreMouLiuChengParentAttack(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLiuChengCompletedEvent(f.Id, r.Claim.ActionId, r.UseId > 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLiuChengCompletion = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29401)]
    internal bool ResumeMouLiuCheng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLiuChengCompletion: not null } f || f.Id != id)
            return false;
        ContinueMouLiuCheng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29401)]
    internal bool ReturnMouLiuChengMovement(ProgramSkillFrame f)
    {
        if (f.MouLiuChengCompletion is null || f.PendingMovementContinuation is null)
            return false;
        ContinueMouLiuCheng(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouLiuChengCompletion)]
    internal PromptChoice AiMouLiuChengUse(PendingDecision p, ProgramSkillFrame f)
    {
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
        var plans = MouLiuChengNativePlans(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.MouLiuChengCompletion!.OutputKind, MouLiuChengSource(f));
        double Score(PromptChoice c) => c.Parameters["branch"] != "use" ? 0 : plans[int.Parse(c.Parameters["plan-index"], CultureInfo.InvariantCulture)].Targets.Sum(t => _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(snapshot, t, f.MouLiuChengCompletion.OutputKind == CardKind.Slash ? new(0, 0, 0, 0, 0, 0, false, false) { TargetDamage = 1, TargetHealthEffects = [new(PublicHealthEffectKind.Damage, 1)] } : new(0, 0, 0, 0, 0, 0, false, false) { TargetValueAdjustment = -8 }));
        return p.Choices.OrderByDescending(Score).ThenBy(c => c.Parameters["branch"] == "decline" ? 0 : 1).First();
    }

    SkillProgramStepOutcome IMouLiuChengHost.Accrue(ProgramSkillFrame f) => AccrueMouLiuCheng(f);
    SkillProgramStepOutcome IMouLiuChengHost.Complete(ProgramSkillFrame f) => BeginMouLiuChengCompletion(f);
}
