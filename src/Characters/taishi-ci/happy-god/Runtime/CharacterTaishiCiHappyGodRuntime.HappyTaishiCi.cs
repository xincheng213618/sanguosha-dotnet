#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaishiCiHappyGodRuntime
{
    internal const string HappyTscDulie = "happy:dulie", HappyTscChongwei = "happy:chongwei", HappyTscPowei = "happy:powei", HappyTscShenzhu = "happy:shenzhu";
    internal static bool IsShenTscPoweiFamily(string skill) => skill is "mobile-god:powei" or HappyTscChongwei;
    internal int HappyTscFieldWei() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Sum(p => p.Markers.GetValueOrDefault(PlayerMarkerKind.ShenTaishiCiWei));
    internal bool HappyTscAwakened(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyTaishiCiAwakenedEvent>().Any(e => e.Source.OwnerSeat == owner);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41740)]
    internal bool CanOfferHappyTaishiCi(ProgramTriggerLegalityContext g)
    {
        if (g.SkillId == HappyTscDulie)
            return g.Trigger.Id == "other-slash-heart" ? g.Context.CardUse is { } use && use.ActorSeat != g.OwnerSeat : HappyTscFieldWei() > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.VictimSeat == g.OwnerSeat) == 1;
        if (g.SkillId == HappyTscChongwei)
            return g.Trigger.Id switch
            {
                "initialize" => true,
                "rotate" => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _characterTaishiCiMobileGodOperations.ShenTscWei(p.Seat, g.OwnerSeat) > 0),
                "damage-removes-wei" => g.Context.TargetSeat is { } target && _characterTaishiCiMobileGodOperations.ShenTscWei(target, g.OwnerSeat) > 0,
                "marked-turn" => g.Context.SourceSeat is { } actor && actor != g.OwnerSeat && _characterTaishiCiMobileGodOperations.ShenTscWei(actor, g.OwnerSeat) > 0 && (_characterTaishiCiMobileGodOperations.ShenTscDiscardCards(g.OwnerSeat).Length > 0 || _characterTaishiCiMobileGodOperations.ShenTscMayTake(g.OwnerSeat, actor)),
                _ => false
            };
        if (g.SkillId == HappyTscPowei)
            return !HappyTscAwakened(g.OwnerSeat) && HappyTscFieldWei() == 0;
        if (g.SkillId == HappyTscShenzhu)
            return g.Trigger.Id == "start-slash" || g.Context.CardUse is { } completed && completed.ActorSeat == g.OwnerSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(completed.ParentCardUseFrameId)?.Action is { PhysicalCards.Count: 1, ConversionChain.Count: 0 };
        return true;
    }

    // Transfers also emit negative marker deltas. Only the native damage-removal
    // instruction is a removed Wei for Powei's current-round draw.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 41740)]
    internal void CountHappyTaishiCiRemovedWei(PlayerMarkerChangedEvent fact)
    {
        if (fact.Marker != PlayerMarkerKind.ShenTaishiCiWei || fact.Delta >= 0 || !_coreMatchParticipantsPlayersAccess._players.Any(p => _coreNativeSkillContextQueriesOperations.HasProgramSkill(p, HappyTscPowei)) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TriggerId: "damage-removes-wei" } f || !IsShenTscPoweiFamily(f.SkillId) || f.Id != fact.ResolutionId || fact.SkillOwnerSeat != f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target } || target != fact.PlayerSeat)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyTaishiCiWeiRemovedEvent(f.Id, CharacterTaishiCiMobileGodRuntime.ShenTscSource(f), target, -fact.Delta, _coreActualRoundProgressRoundNumberAccess._roundNumber));
    }

    SkillProgramStepOutcome IHappyTaishiCiHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveHappyTaishiCiChongwei => _characterTaishiCiMobileGodOperations.ResolveShenTscPowei(f),
        SkillProgramEffectOp.RecoverHappyTaishiCiDulie => RecoverHappyTsc(f),
        _ => BeginHappyTsc(f, op)};
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyTaishiCiChongwei)]
    internal PromptChoice ChooseHappyTaishiCiChongwei(PendingDecision p, ProgramSkillFrame f) => _characterTaishiCiMobileGodOperations.ChooseShenTscPowei(p, f);
    internal SkillProgramStepOutcome RecoverHappyTsc(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var count = HappyTscFieldWei();
        if (f.SkillId != HappyTscDulie || f.TriggerId != "first-dying-recover" || f.WindowContext?.Window != SkillProgramTriggerWindow.DyingEntering || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.VictimSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.VictimSeat == f.OwnerSeat) != 1 || count <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyTaishiCiDulieRecoveredEvent>().Any(e => e.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Happy Dulie requires its first actual dying entry and existing field Wei.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyTaishiCiDulieRecoveredEvent(f.Id, f.OwnerSeat, dying.FrameId, count));
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, Math.Max(0, Math.Min(count, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp) - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp), null, null);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome BeginHappyTsc(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyTaishiCi is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Happy Taishi Ci requires its original unissued instruction.");
        var awake = op == SkillProgramEffectOp.AwakenHappyTaishiCiPowei;
        if (awake ? f.SkillId != HappyTscPowei || context.Window != SkillProgramTriggerWindow.AfterTurnEnded || HappyTscAwakened(f.OwnerSeat) || HappyTscFieldWei() != 0 : op != SkillProgramEffectOp.ObtainHappyTaishiCiStartSlash || f.SkillId != HappyTscShenzhu || context.Window != SkillProgramTriggerWindow.TurnBeforeStart)
            throw new InvalidOperationException("Happy Taishi Ci lost its actual awakening or own-turn start.");
        var count = awake ? _coreProgramEventHistoryOperations.ProgramEventHistory<HappyTaishiCiWeiRemovedEvent>().Where(e => e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber).Sum(e => e.Count) : 1;
        var r = new HappyTaishiCiReceipt(CharacterTaishiCiMobileGodRuntime.ShenTscSource(f), f.GameplayHash, context.ParentFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, op, count, awake ? HappyTaishiCiStage.GrantChildren : HappyTaishiCiStage.MovementChildren, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyTaishiCi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyTaishiCiStartedEvent(f.Id, r.Source, r.Hash, r.Parent, r.Turn, r.Round, r.Op, r.Count));
        if (awake)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyTaishiCiAwakenedEvent(f.Id, r.Source, r.Round, r.Count));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().GrantSkills(f, [HappyTscShenzhu]);
        }
        else
        {
            // The draw pile's first printed Slash is a real existing entity;
            // hand identity conversions do not change cards in this public source.
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(c => CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind));
            if (card is not null)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyTaishiCi = r with { CardId = card.Id } });
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), HappyTscSlashReason, (_, _) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { HappyTaishiCi = current.HappyTaishiCi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                });
                if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                    return SkillProgramStepOutcome.AwaitChild;
            }
        }

        ResumeHappyTsc(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static CardMoveReason HappyTscSlashReason => new("skill.happy.shenzhu.start-slash");
    internal static CardMoveReason HappyTscAwakenDrawReason => new("skill.happy.powei.round-removed");

    [ProgramContinuation(ProgramContinuationKind.Resume, 41740)]
    internal bool ResumeHappyTsc(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyTaishiCi: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (r.Stage == HappyTaishiCiStage.Drawing)
            return true;
        var movement = r.Stage == HappyTaishiCiStage.MovementChildren;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.HappyTaishiCi!;
        if (r.Stage == HappyTaishiCiStage.GrantChildren && r.Count > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyTaishiCi = r with { Stage = HappyTaishiCiStage.Drawing }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Count, HappyTscAwakenDrawReason);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyTaishiCi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41740)]
    internal bool ReturnHappyTscMovement(ProgramSkillFrame f) => f.HappyTaishiCi is not null && ResumeHappyTsc(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.AwakenHappyTaishiCiPowei)]
    internal void ReturnHappyTscDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.HappyTaishiCi!;
        if (r.Stage != HappyTaishiCiStage.Drawing || r.Draw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Count || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Happy Powei lost its once-frozen native round-count draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyTaishiCi = r with { Stage = HappyTaishiCiStage.MovementChildren, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyTaishiCiDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ResumeHappyTsc(f.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41740)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41740)]
    internal bool RetainHappyTsc(ProgramSkillFrame f) => f.HappyTaishiCi is not null;
}
