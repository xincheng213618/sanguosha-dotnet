#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouOlRuntime
{
    internal void IssueMouZhangXiuOlSlash(ProgramSkillFrame f)
    {
        var r = f.MouZhangXiuOl!;
        if (!CanUseMouZhangXiuOlHunt(f.OwnerSeat, r.TargetSeat))
        {
            FinishMouZhangXiuOl(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, MouZhangXiuOl = r with { Stage = MouZhangXiuOlStage.Issuing } });
        _coreProgramVirtualSlashOffersOperations.BeginOfferedProgramVirtualSlash(f, f.OwnerSeat, r.TargetSeat);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33000)]
    internal void BindMouZhangXiuOlSlash(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is not { } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MouZhangXiuOl: { Stage: MouZhangXiuOlStage.Issuing } r } f)
            return;
        if (!ValidMouZhangXiuOl(f) || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } action || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.RequesterSeat is not null || !action.TargetSeats.SequenceEqual([r.TargetSeat]) || !use.TargetSeats.SequenceEqual([r.TargetSeat]))
            throw new InvalidOperationException("Qiulie must issue its own real zero-material native Slash against its accepted target.");
        var origin = new MouZhangXiuOlQiulieOrigin(f.Id, r.Source, f.GameplayHash, use.Id, action.ActionId, r.TargetSeat, r.Phase!);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouZhangXiuOl = r with { Stage = MouZhangXiuOlStage.NativeUse, UseId = use.Id, ActionId = action.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouZhangXiuOlQiulie = origin });
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && action.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && use.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], null))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(use.Id, action.ActorSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(action.ActorSeat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlQiulieIssuedEvent(origin));
    }

    internal bool IsMouZhangXiuOlSlash(CardUseFrame use)
    {
        if (use.MouZhangXiuOlQiulie is not { } o || use.CardId != 0 || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActionId != o.ActionId || o.UseId != use.Id || a.ProviderSeat != o.Source.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.FrameId)is not ProgramSkillFrame { MouZhangXiuOl: { Stage: MouZhangXiuOlStage.NativeUse, NativeReturned: false } r } parent || !ValidMouZhangXiuOl(parent) || o.Source != r.Source || o.GameplayHash != parent.GameplayHash || r.UseId != use.Id || r.ActionId != a.ActionId || o.TargetSeat != r.TargetSeat || o.Phase != r.Phase || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlQiulieIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        return _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Source.OwnerSeat, o.Source.OwnerSeat);
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 33000)]
    internal bool FinishMouZhangXiuOlSlashAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouZhangXiuOlQiulie: not null } use || !IsMouZhangXiuOlSlash(use))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(33000)]
    internal void ValidateMouZhangXiuOlSlash(CardUseFrame use)
    {
        if (use.MouZhangXiuOlQiulie is not null && (!IsMouZhangXiuOlSlash(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Qiulie requires the complete original native Use before returning.");
    }

    [NativeUseReturn(33000)]
    internal void ReturnMouZhangXiuOlSlash(CardUseFrame use)
    {
        if (use.MouZhangXiuOlQiulie is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouZhangXiuOl!;
        if (r.UseId != use.Id || r.ActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Qiulie cannot repeat or bypass its whole native Slash return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouZhangXiuOl = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            CompleteMouZhangXiuOlSlash(o.FrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 33000)]
    internal bool ReturnMouZhangXiuOlSlashAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhangXiuOl: { Stage: MouZhangXiuOlStage.NativeUse, NativeReturned: true } r } f || r.UseId != completion.ResolutionId)
            return false;
        if (completion.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Qiulie returned to another program frame.");
        CompleteMouZhangXiuOlSlash(f.Id);
        return true;
    }

    internal void CompleteMouZhangXiuOlSlash(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouZhangXiuOl!;
        if (!r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlQiulieReturnedEvent>().Any(e => e.FrameId == id))
            throw new InvalidOperationException("Qiulie cannot pay or return its native Slash twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlQiulieReturnedEvent(id, r.UseId, r.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r with { Stage = MouZhangXiuOlStage.Children }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueMouZhangXiuOl(id);
    }

    internal CardUseFrame? MouZhangXiuOlContextUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is not { Action: { } a } use || a.ActionId != info.CardActionId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame parent || parent.ParentFrameId != use.Id || parent.Action.ActionId != a.ActionId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        return use;
    }

    [ProgramIssuedTrigger(600)]
    internal bool CanRunIssuedMouZhangXiuOl(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (MouZhangXiuOlContextUse(c, w)is not { } use)
            return false;
        if (c.SkillId == MouZhangXiuOlComposition.Qiulie && c.BindingId == "defend-hunt" && w.Window == SkillProgramTriggerWindow.CardUseBeforeTargetEffects && use.MouZhangXiuOlQiulie is { } origin)
            return c.OwnerSeat == origin.Source.OwnerSeat && c.SkillInstanceId == origin.Source.SkillInstanceId && c.GameplayHash == origin.GameplayHash && IsMouZhangXiuOlSlash(use) && _coreMatchParticipantsPlayersAccess._players[origin.TargetSeat].IsAlive && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlStartedEvent>().Any(e => e.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash && e.UseId == use.Id);
        if (c.SkillId != MouZhangXiuOlComposition.Zhuijiao || MouZhangXiuOlEntry(use, c)is not { } entry)
            return false;
        return c.BindingId == "previous-undamaged" && w.Window == SkillProgramTriggerWindow.CardUseCommitted && !entry.Boosted && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || c.BindingId == "complete-undamaged" && w.Window == SkillProgramTriggerWindow.CardUseCompleted && entry.Boosted && entry.DrawReturned && !entry.PenaltyIssued && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && !MouZhangXiuOlUseDamaged(use.Id, null) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33002, SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash, SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao, SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao)]
    internal bool QualifyMouZhangXiuOlUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => CanRunIssuedMouZhangXiuOl(c, w);
    internal SkillProgramStepOutcome BeginMouZhangXiuOlDefense(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.MouZhangXiuOl is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !CanRunIssuedMouZhangXiuOl(c, w))
            throw new InvalidOperationException("Qiulie defense requires its signed, paid, current native Slash.");
        var use = MouZhangXiuOlContextUse(c, w)!;
        var origin = use.MouZhangXiuOlQiulie!;
        var r = new MouZhangXiuOlReceipt(MouZhangXiuOlSource(f), f.GameplayHash, SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash, MouZhangXiuOlStage.Defense)
        {
            UseId = use.Id,
            ActionId = use.Action!.ActionId,
            TargetSeat = origin.TargetSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, use.Id));
        PublishMouZhangXiuOl(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool IsMouZhangXiuOlQiulieIneffective(CardUseFrame use)
    {
        if (use.MouZhangXiuOlQiulie is not { } origin || !IsMouZhangXiuOlSlash(use))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlQiulieNullifiedEvent>().Any(n => n.UseId == use.Id && n.TargetSeat == origin.TargetSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlStartedEvent>().Count(s => s.FrameId == n.FrameId && s.UseId == use.Id && s.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash && s.Source.OwnerSeat == origin.Source.OwnerSeat && s.Source.SkillId == origin.Source.SkillId && s.Source.SkillInstanceId == origin.Source.SkillInstanceId && s.Source.BindingId == "defend-hunt" && s.GameplayHash == origin.GameplayHash) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlDiscardPaidEvent>().Count(p => p.FrameId == n.FrameId && p.CardId == n.PaidCardId && p.PayerSeat == origin.TargetSeat && p.Defense && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == p.CardId && m.From.OwnerSeat == p.PayerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == MouZhangXiuOlDefenseReason) == 1) == 1);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 33000)]
    internal void CollectIssuedMouZhangXiuOl(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == action.ActionId);
        if (use is null)
            return;
        void Add(CardConversionSource source, string hash, string binding, int target)
        {
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.Triggers.Single(t => t.Id == binding);
            if (!result.Any(c => c.OwnerSeat == source.OwnerSeat && c.SkillId == source.SkillId && c.SkillInstanceId == source.SkillInstanceId && c.TriggerId == binding))
                result.Add(new(source.OwnerSeat, target, source.SkillId, binding, hash, source.SkillInstanceId, trigger.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, source.OwnerSeat, target, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], action))));
        }

        if (window == SkillProgramTriggerWindow.CardUseBeforeTargetEffects && use.MouZhangXiuOlQiulie is { } o && IsMouZhangXiuOlSlash(use) && _coreMatchParticipantsPlayersAccess._players[o.TargetSeat].IsAlive && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhangXiuOlStartedEvent>().Any(e => e.Operation == SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash && e.UseId == use.Id))
            Add(o.Source, o.GameplayHash, "defend-hunt", o.TargetSeat);
        if (use.MouZhangXiuOlZhuijiao is not { } entries)
            return;
        foreach (var e in entries.Where(e => ValidMouZhangXiuOlEntry(use, e) && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive))
            if (window == SkillProgramTriggerWindow.CardUseCommitted && !e.Boosted)
                Add(e.Source, e.GameplayHash, "previous-undamaged", e.Source.OwnerSeat);
            else if (window == SkillProgramTriggerWindow.CardUseCompleted && e.Boosted && e.DrawReturned && !e.PenaltyIssued && !MouZhangXiuOlUseDamaged(use.Id, null))
                Add(e.Source, e.GameplayHash, "complete-undamaged", e.Source.OwnerSeat);
    }
}
