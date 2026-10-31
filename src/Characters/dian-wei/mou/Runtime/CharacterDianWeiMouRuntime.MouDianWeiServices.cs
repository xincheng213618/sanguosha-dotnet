#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiMouRuntime
{
    internal MouDianWeiRuntime MouDianWei => _mouDianWeiOperations.Get();

    int IMouDianWeiFacts.Turn => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int IMouDianWeiFacts.TurnOwner => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long IMouDianWeiFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouDianWeiFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    int IMouDianWeiFacts.Hp(int s) => _coreMatchParticipantsPlayersAccess._players[s].Hp;
    int IMouDianWeiFacts.MaxHp(int s) => _coreMatchParticipantsPlayersAccess._players[s].MaxHp;
    int IMouDianWeiFacts.Hand(int s) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count;
    bool IMouDianWeiFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouDianWeiFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string IMouDianWeiFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    IEnumerable<T> IMouDianWeiFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    SkillProgram? IMouDianWeiFacts.Program(string s) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(s)?.Program;
    bool IMouDianWeiFacts.Parent(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.ResolveMouDianWeiKuangzhan)
            return f.SkillId == MouDianWeiComposition.Kuangzhan && f.ActivationId == "draw-and-contest" && f.TriggerId is null && f.WindowContext is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        var window = op == SkillProgramEffectOp.ResolveMouDianWeiKangyongStart ? SkillProgramTriggerWindow.TurnStartBeforeNormalFlow : SkillProgramTriggerWindow.AfterTurnEnded;
        if (f.SkillId != MouDianWeiComposition.Kangyong || f.TriggerId != f.ActivationId || f.TriggerId != (window == SkillProgramTriggerWindow.AfterTurnEnded ? "repay-actual-turn-end" : "recover-actual-turn-start") || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || c.Window != window || f.OwnerSeat != ((IMouDianWeiFacts)this).TurnOwner)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (window == SkillProgramTriggerWindow.AfterTurnEnded)
            return n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is DeferredTurnEndFrame ended && ended.Id == c.ParentFrameId && ended.OwnerSeat == f.OwnerSeat && ended.TurnNumber == ((IMouDianWeiFacts)this).Turn && c.SourceSeat == f.OwnerSeat && c.TargetSeat == f.OwnerSeat && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended));
        return n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is ProgramLifecycleTriggerWindowFrame p && p.Id == c.ParentFrameId && p.Window == window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]);
    }

    IReadOnlyList<int> IMouDianWeiFacts.Opponents(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count == 0 && !_characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[owner]) ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(owner, p.Seat) && (_corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 || _characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(p))).Select(p => p.Seat).Order().ToArray();
    IReadOnlyList<int> IMouDianWeiFacts.NotWon(int owner) => _corePlayCardHistoryOperations.EventsSinceLastBoundary(x => x is TurnStartedEvent or TurnEndedEvent).OfType<PindianResultDeterminedEvent>().SelectMany(x => new[] { x.Result.SourceSeat, x.Result.OpponentSeat }.Where(s => !x.Result.WonBy(s))).Where(s => s != owner && ((IMouDianWeiFacts)this).Alive(s)).Distinct().Order().ToArray();
    bool IMouDianWeiFacts.CanSlash(int actor, int target) => _coreForeignTurnContestsOperations.Value.CanUseForeignTurnContestSlash(actor, target);
    PindianResult IMouDianWeiFacts.Result(ProgramSkillFrame f)
    {
        var r = f.MouDianWei ?? throw new InvalidOperationException("Missing contest parent receipt.");
        var bind = MouDianWeiRuntime.Bind(r);
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDianWeiContestIssuedEvent>().Where(p => p.FrameId == f.Id && p.Index == r.Index).ToArray();
        var actual = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Where(p => p.FrameId == r.ContestId && p.SkillId == f.SkillId).ToArray();
        if (issued is not [var start] || start.ContestId != r.ContestId || start.Opponent != r.Opponent || start.Bind != bind || actual is not [var fact] || f.PindianResultBindings.SingleOrDefault(b => b.Name == bind)is not { } b || b.SourceSeat != f.OwnerSeat || b.OpponentSeat != r.Opponent || b.SourceRank != fact.Result.SourceRank || b.OpponentRank != fact.Result.OpponentRank || b.SourceWon != fact.Result.SourceWon || b.Visibility != SkillProgramCardSetVisibility.Public || fact.Result.SourceSeat != f.OwnerSeat || fact.Result.OpponentSeat != r.Opponent)
            throw new InvalidOperationException("Kuangzhan requires its exact native Pindian result and typed binding.");
        return fact.Result;
    }

    void IMouDianWeiNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouDianWeiNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouDianWeiNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouDianWeiNative.Pindian(ProgramSkillFrame f)
    {
        if (!MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.Pindian } r || !_characterDianWeiMouOperations.CreateMouDianWeiFacts().Opponents(f.OwnerSeat).Contains(r.Opponent))
            throw new InvalidOperationException("Kuangzhan requires one currently legal actual Pindian opponent.");
        var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var bind = MouDianWeiRuntime.Bind(r);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { MouDianWei = r with { ContestId = id, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDianWeiContestIssuedEvent(f.Id, r.Index, id, r.Opponent, bind));
        _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), f.OwnerSeat, r.Opponent, programResultBind: bind, programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    void IMouDianWeiNative.Slash(ProgramSkillFrame f, int actor, IReadOnlyList<int> targets)
    {
        if (!MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.Slash, Slash: null } r || targets.Count == 0 || targets.Distinct().Count() != targets.Count || targets.Any(s => !_coreForeignTurnContestsOperations.Value.CanUseForeignTurnContestSlash(actor, s)) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Kuangzhan must issue its own original comparison's legal native Slash.");
        var result = _characterDianWeiMouOperations.CreateMouDianWeiFacts().Result(f);
        var expected = (result.SourceWon ? _characterDianWeiMouOperations.CreateMouDianWeiFacts().NotWon(f.OwnerSeat) : new[]
        {
            f.OwnerSeat
        }

        ).Where(s => _coreForeignTurnContestsOperations.Value.CanUseForeignTurnContestSlash(actor, s)).ToArray();
        if (actor != (result.SourceWon ? f.OwnerSeat : result.OpponentSeat) || !targets.SequenceEqual(expected))
            throw new InvalidOperationException("Kuangzhan cannot substitute its real winner/loser or frozen target set.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var source = _coreMatchParticipantsPlayersAccess._players[actor];
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor, actor, null, null, null, CardKind.Slash, targets, [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var ret = new MouDianWeiSlash(f.Id, r.Source, f.GameplayHash, r.Index, r.ContestId, id, action.ActionId, actor)
        {
            Targets = targets
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouDianWei = r with { Slash = ret } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor, 0, CardKind.Slash, ret.Targets, PhysicalCardIds: []) { Action = action, MouDianWei = ret });
        foreach (var(target, index)in ret.Targets.Select((target, index) => (target, index)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDianWeiSlashIssuedEvent(f.Id, r.Index, r.ContestId, id, action.ActionId, actor, target, index));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Slash, actor));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, ret.Targets));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor, ret.Targets[0], card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, _coreMatchParticipantsPlayersAccess._players[ret.Targets[0]], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, source);
        source.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor, ret.Targets[0]));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, ret.Targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    void IMouDianWeiNative.Recover(ProgramSkillFrame f)
    {
        if (!MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.RecoveryChildren })
            throw new InvalidOperationException("Kangyong recovery requires its exact actual-turn-start parent.");
        _coreNextSlashProgramsOperations.RecoverProgramToMaximum(f);
    }

    void IMouDianWeiNative.Lose(ProgramSkillFrame f, int amount)
    {
        if (!MouDianWei.Valid(f) || f.MouDianWei is not { Stage: MouDianWeiStage.LossChildren } r || amount != r.ActualLoss || amount < 0 || amount > Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp - 1))
            throw new InvalidOperationException("Kangyong repays only actual recovered HP while preserving at least one HP.");
        _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, amount);
    }

    internal bool IsMouDianWeiSlashUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouDianWei: { } ret } use && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == ret.ProgramId)is { } f && MouDianWei.Valid(f) && f.MouDianWei is { Stage: MouDianWeiStage.Slash } r && r.Slash == ret && use.Id == ret.UseId && use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && (use.CurrentSlashFirePolicy?.OriginalAction ?? use.Action)is { } action && action.ActionId == ret.ActionId && action.ActorSeat == ret.Actor && action.ProviderSeat == ret.Actor && action.PhysicalCards.Count == 0 && ret.Targets.Select((target, index) => new MouDianWeiSlashIssuedEvent(f.Id, ret.Index, ret.ContestId, ret.UseId, ret.ActionId, ret.Actor, target, index)).SequenceEqual(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDianWeiSlashIssuedEvent>().Where(e => e.UseId == ret.UseId));
    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 35300)]
    internal bool MouDianWeiSlashComplete(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parent || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == parent)is not { MouDianWei: { Stage: MouDianWeiStage.Slash, Slash: { } ret } } f)
            return false;
        if (ret.UseId != completion.ResolutionId || !MouDianWei.Valid(f) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Id == ret.UseId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == ret.UseId) != 1)
            throw new InvalidOperationException("Kuangzhan's native multi-target Slash returned before its actual use finished.");
        MouDianWei.SlashReturned(f, ret);
        return true;
    }
}
