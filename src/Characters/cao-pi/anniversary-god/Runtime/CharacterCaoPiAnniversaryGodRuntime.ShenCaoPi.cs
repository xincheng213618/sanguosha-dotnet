#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiAnniversaryGodRuntime
{
    internal bool HasShenCaoPiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ApplyShenCaoPiYinzhi);

    internal static CardConversionSource ShenCaoPiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int YinzhiOwnTurns(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Count(e => e.ActorSeat == seat);
    internal bool YinzhiAwaitingOwnTurn(YinzhiIssuedEvent e) => YinzhiOwnTurns(e.TargetSeat) == e.TargetOwnTurns && !(_resolutionStackOperations._resolutionStack.FirstOrDefault()is BeiMiHuTurnPreludeFrame prelude && prelude.Proof.UpcomingOwnerSeat == e.TargetSeat && prelude.Proof.UpcomingTurnNumber > e.ActualTurn);
    internal bool IsYinzhiSkillSuppressed(int seat) => HasShenCaoPiCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiIssuedEvent>().Any(e => e.TargetSeat == seat && YinzhiAwaitingOwnTurn(e));
    internal bool IsYinzhiTurnRestricted(int actor) => HasShenCaoPiCapability && actor == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiIssuedEvent>().Any(e => e.TargetSeat == actor && YinzhiOwnTurns(actor) == e.TargetOwnTurns + 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiTurnRestrictionReleasedEvent>().Any(r => r.IssueFrameId == e.FrameId));
    internal bool YinzhiNewCardsForbidden(int actor, IEnumerable<int> ids)
    {
        if (!IsYinzhiTurnRestricted(actor))
            return false;
        var materials = ids.Where(id => id > 0).ToArray();
        return materials.Length == 0 || materials.Any(id => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && m.To == CardLocation.Hand(actor) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m)is { } original && original != CardLocation.Hand(actor)));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 39700)]
    internal void ObserveShenCaoPiRules(IGameEvent payload)
    {
        if (!HasShenCaoPiCapability)
            return;
        if (payload is TurnStartedEvent)
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        if (payload is DamageAppliedEvent { SourceLess: false, Amount: > 0 } damage)
            foreach (var issue in _coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiIssuedEvent>().Where(e => e.TargetSeat == damage.SourceSeat && e.Source.OwnerSeat == damage.TargetSeat && damage.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && YinzhiOwnTurns(e.TargetSeat) == e.TargetOwnTurns + 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiTurnRestrictionReleasedEvent>().Any(r => r.IssueFrameId == e.FrameId)).ToArray())
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinzhiTurnRestrictionReleasedEvent(issue.FrameId, _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Last(d => d.SourceSeat == damage.SourceSeat && d.TargetSeat == damage.TargetSeat).Id));
        ObserveKemingProducedFacts(payload);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 39700)]
    internal void ValidateYinzhiNewUse(ProgramSubmittedUseQuery q)
    {
        if (YinzhiNewCardsForbidden(q.ActorSeat, q.CardIds))
            throw new InvalidOperationException("Yinzhi permits only cards genuinely gained during this actual turn.");
    }

    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 39700)]
    internal bool YinzhiMaterialBan(ProgramCardMaterialQuery q) => YinzhiNewCardsForbidden(q.ActorSeat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 39700)]
    internal bool YinzhiResponseBan(ProgramNativeResponseQuery q) => YinzhiNewCardsForbidden(q.Provider.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 39700)]
    internal bool YinzhiActionBan(ProgramLegalActionQuery q) => q.EffectiveKind is not null && YinzhiNewCardsForbidden(q.Actor.Seat, q.Action.CardId is> 0 and var id ? [id] : []);
    SkillProgramStepOutcome IShenCaoPiHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginShenCaoPi(frame, op);
    internal CardActionContext? YinzhiAction(ProgramSkillWindowContext? c) => c is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)?.Action;
    internal YinzhiIssuedEvent? YinzhiRewardIssue(int owner, CardActionContext? a) => a is null || !a.PhysicalCards.Any(c => c.From == CardLocation.Hand(a.ActorSeat)) ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiIssuedEvent>().Where(e => e.Source.OwnerSeat == owner && e.TargetSeat == a.ActorSeat && YinzhiAwaitingOwnTurn(e) && !_coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiHandRewardRequestedEvent>().Any(r => r.IssueFrameId == e.FrameId && r.ActionId == a.ActionId)).OrderBy(e => e.FrameId).FirstOrDefault();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39700, SkillProgramEffectOp.DrawYinzhiHandAction, SkillProgramEffectOp.ResolveKemingDamagedSkill, SkillProgramEffectOp.ArmKemingSkillEcho)]
    internal bool CanTriggerShenCaoPi(ProgramTriggerLegalityContext g) => _coreProgramLifecycleOperations.GetProgramTrigger2(g.Candidate).Effects[0].Op switch
    {
        SkillProgramEffectOp.DrawYinzhiHandAction => YinzhiRewardIssue(g.OwnerSeat, YinzhiAction(g.WindowContext))is not null,
        SkillProgramEffectOp.ResolveKemingDamagedSkill => g.WindowContext is { SourceSeat: { } source, TargetSeat: { } target, Amount: > 0 } c && source != target && target == g.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } window && !_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window).IsSourceLess && KemingBorrowOptions(g.OwnerSeat, source).Count > 0,
        SkillProgramEffectOp.ArmKemingSkillEcho => g.WindowContext is { SourceSeat: { } s, TargetSeat: { } t, Amount: > 0 } && s == g.OwnerSeat && s != t,
        _ => false
    };
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 39700)]
    internal void CollectYinzhiIssuedReward(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasShenCaoPiCapability || window is not (SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.CardResponseCompleted))
            return;
        var binding = window == SkillProgramTriggerWindow.CardUseCompleted ? "hand-use-reward" : "hand-response-reward";
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
        {
            var issue = YinzhiRewardIssue(owner.Seat, action);
            if (issue is null)
                continue;
            if (!result.Any(c => c.OwnerSeat == owner.Seat && c.SkillId == ShenCaoPiComposition.Yinzhi && c.TriggerId == binding && c.SkillInstanceId == issue.Source.SkillInstanceId))
                result.Add(new(owner.Seat, action.ActorSeat, ShenCaoPiComposition.Yinzhi, binding, issue.GameplayHash, issue.Source.SkillInstanceId));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 39700)]
    internal bool IsIssuedShenCaoPiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != ShenCaoPiComposition.Yinzhi || c.BindingId is not ("hand-use-reward" or "hand-response-reward") || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || w.Window is not (SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.CardResponseCompleted) || YinzhiRewardIssue(c.OwnerSeat, YinzhiAction(w))is not { } issue)
            return false;
        return issue.Source.SkillInstanceId == c.SkillInstanceId && issue.GameplayHash == c.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash;
    }

    internal SkillProgramStepOutcome BeginShenCaoPi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenCaoPi is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Shen Cao Pi requires his exact original unpaid instruction.");
        var target = op == SkillProgramEffectOp.ApplyShenCaoPiYinzhi ? f.SelectedTargetSeats.Single() : f.WindowContext?.TargetSeat ?? f.OwnerSeat;
        var r = new ShenCaoPiReceipt(ShenCaoPiSource(f), f.GameplayHash, op, f.InstructionIndex, f.WindowContext?.ParentFrameId ?? 0, target, ShenCaoPiStage.Complete);
        if (op == SkillProgramEffectOp.ApplyShenCaoPiYinzhi)
        {
            var count = Math.Min(3, _coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiActivatedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.TargetSeat == target) + 1);
            r = r with
            {
                Count = count,
                Stage = ShenCaoPiStage.Drawing,
                IssuedId = f.Id
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenCaoPi = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinzhiActivatedEvent(f.Id, r.Source, r.GameplayHash, target, count, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, YinzhiOwnTurns(target)));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new("skill-program.anniversary:yinzhi.initial-draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.DrawYinzhiHandAction)
        {
            var action = YinzhiAction(f.WindowContext) ?? throw new InvalidOperationException("Yinzhi lost its real completed hand action.");
            var issue = YinzhiRewardIssue(f.OwnerSeat, action) ?? throw new InvalidOperationException("Yinzhi lost its issued pre-turn hand reward.");
            r = r with
            {
                Count = issue.Count,
                IssuedId = issue.FrameId,
                ActionId = action.ActionId,
                Stage = ShenCaoPiStage.Drawing
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenCaoPi = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinzhiHandRewardRequestedEvent(f.Id, issue.FrameId, action.ActionId, issue.Count));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, issue.Count, new("skill-program.anniversary:yinzhi.hand-reward"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveKemingDamagedSkill)
        {
            var options = KemingBorrowOptions(f.OwnerSeat, f.WindowContext!.SourceSeat!.Value);
            var fallback = KemingSourceBorrowOptions(f.WindowContext.SourceSeat.Value).Count == 0;
            var before = _randomOperations._random.State;
            var forced = fallback && options.Count > 0 ? options[_randomOperations._random.Next(options.Count)] : null;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenCaoPi = r with { Options = options, ForcedBorrow = forced, BorrowRandomBefore = before, BorrowRandomAfter = _randomOperations._random.State, Stage = ShenCaoPiStage.BorrowChoice } });
            PublishKemingBorrow(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenCaoPi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingEchoArmedEvent(f.Id, r.Source, r.GameplayHash, target, _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Select(e => e.FrameId).DefaultIfEmpty().Max()));
        FinishShenCaoPi(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainShenCaoPi(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 39700)]
    internal bool ResumeShenCaoPi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenCaoPi: { } r } f || f.Id != id)
            return false;
        if (r.Stage == ShenCaoPiStage.Drawing)
            throw new InvalidOperationException("Shen Cao Pi must await his exact native draw return.");
        if (DrainShenCaoPi(id))
            return true;
        if (r.Operation == SkillProgramEffectOp.ApplyShenCaoPiYinzhi && r.Stage == ShenCaoPiStage.Children)
        {
            var activation = _coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiActivatedEvent>().Single(e => e.FrameId == id);
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<YinzhiIssuedEvent>().Any(e => e.FrameId == id))
                throw new InvalidOperationException("Yinzhi cannot issue its post-draw suppression twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinzhiIssuedEvent(id, activation.Source, activation.GameplayHash, activation.TargetSeat, activation.Count, activation.ActualTurn, activation.TargetOwnTurns));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (r.Stage == ShenCaoPiStage.BorrowChoice && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            PublishKemingBorrow(f);
        else
            FinishShenCaoPi(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39700)]
    internal bool ReturnShenCaoPiMovement(ProgramSkillFrame f) => f.ShenCaoPi is not null && ResumeShenCaoPi(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ApplyShenCaoPiYinzhi, SkillProgramEffectOp.DrawYinzhiHandAction)]
    internal void ReturnShenCaoPiDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        var r = f.ShenCaoPi ?? throw new InvalidOperationException("Shen Cao Pi's draw lost its original paid receipt.");
        if (r.Stage != ShenCaoPiStage.Drawing || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Count || o.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenCaoPiDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Shen Cao Pi cannot repeat or alter his native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenCaoPi = r with { Stage = ShenCaoPiStage.Children, DrawOutcome = o } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenCaoPiDrawReturnedEvent(f.Id, f.OwnerSeat, r.Count, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter));
        ResumeShenCaoPi(f.Id);
    }

    internal void FinishShenCaoPi(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenCaoPi = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39700)]
    internal bool RetainShenCaoPi(ProgramSkillFrame f) => f.ShenCaoPi is not null;
}
