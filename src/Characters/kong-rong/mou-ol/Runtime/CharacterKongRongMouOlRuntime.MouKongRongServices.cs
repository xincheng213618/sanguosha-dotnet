#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongRongMouOlRuntime
{
    internal MouKongRongRuntime MouKongRong => _mouKongRongOperations.Get();
    internal bool HasMouKongRong => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouKongRongLiwen);

    SkillProgram IMouKongRongFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    string IMouKongRongFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    long IMouKongRongFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    bool IMouKongRongFacts.GameOver => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouKongRongFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouKongRongFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool IMouKongRongFacts.Friendly(int actor, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[actor].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(actor), target, new(0, 0, 0, 0, 1, 0, true, false)) > 0;
    int IMouKongRongFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int IMouKongRongFacts.Xian(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MouKongRongXian);
    IReadOnlyList<int> IMouKongRongFacts.Living => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    IReadOnlyList<T> IMouKongRongFacts.History<T>() => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<T>().ToArray());
    bool IMouKongRongFacts.ParentMatches(ProgramSkillFrame f) => MouKrParentMatches(f);
    CardActionContext? IMouKongRongFacts.Action(ProgramSkillFrame f) => _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == f.WindowContext?.ParentFrameId)is ProgramCardTriggerWindowFrame w ? w.Action : null;
    MouKrZhengyiStartedEvent? IMouKongRongFacts.Damage(ProgramSkillFrame f) => _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == f.WindowContext?.ParentFrameId)is BeforeDamageProgramWindowFrame { Nature: DamageNature.Normal, Amount: > 0 } w && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } a && a.TargetSeat == w.TargetSeat ? new(f.Id, MouKongRongRuntime.Source(f), f.GameplayHash, w.Id, a.ResolutionId, w.TargetSeat, w.Amount) : null;
    IReadOnlyList<LegalAction> IMouKongRongFacts.Actions(ProgramSkillFrame f, int seat) => MouKrActions(f, seat);
    bool IMouKongRongFacts.DraftValid(LegalAction a, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => cards.Count >= a.MinCardCount && cards.Count <= a.MaxCardCount && cards.Distinct().Count() == cards.Count && targets.Count >= a.MinTargetCount && targets.Count <= a.MaxTargetCount && targets.Distinct().Count() == targets.Count && cards.All(a.SelectableCardIds.Contains) && targets.All(a.SelectableTargetSeats.Contains) && (a.SelectableTargetSequences is null || a.SelectableTargetSequences.Any(t => t.SequenceEqual(targets)));
    int IMouKongRongNative.ChangeXian(ProgramSkillFrame f, int seat, int delta)
    {
        var old = _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MouKongRongXian);
        var next = Math.Clamp(old + delta, 0, 5);
        if (old != next)
            _coreStrategicProgramsOperations.SetStrategicMarker(seat, PlayerMarkerKind.MouKongRongXian, seat, next, f);
        return next - old;
    }

    void IMouKongRongNative.IssueUse(ProgramSkillFrame f, int actor, LegalAction a, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => IssueMouKrUse(f, actor, a, cards, targets);
    SkillProgramStepOutcome IMouKongRongNative.LoseHp(ProgramSkillFrame f, int seat, int amount) => _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, seat, amount);
    void IMouKongRongNative.PreventDamage(ProgramSkillFrame f) => _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
    bool IMouKongRongNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    void IMouKongRongNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouKongRongNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    internal bool MouKrParentMatches(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != c.ParentFrameId)
            return false;
        var candidate = _resolutionStackOperations._resolutionStack[index - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == c.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            DeferredTurnEndFrame p when c.Window == SkillProgramTriggerWindow.AfterTurnEnded && p.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && p.AfterTurnEnded is not null => _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p),
            ProgramCardTriggerWindowFrame p when p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => new ProgramTriggerCandidate(p.Candidates[p.CandidateIndex].OwnerSeat, p.Candidates[p.CandidateIndex].SkillId, p.Candidates[p.CandidateIndex].TriggerId, p.Candidates[p.CandidateIndex].SkillInstanceId, p.Candidates[p.CandidateIndex].GameplayHash, p.Candidates[p.CandidateIndex].Priority),
            BeforeDamageProgramWindowFrame p when c.Window == SkillProgramTriggerWindow.BeforeDamageApplied && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex].Candidate,
            _ => (ProgramTriggerCandidate? )null
        };
        return candidate is { } actual && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, actual);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 34600)]
    internal void ObserveMouKrAction(IGameEvent payload)
    {
        if (!HasMouKongRong)
            return;
        CardActionContext? action = payload switch
        {
            CardUseDeclaredEvent e => _coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)?.Action,
            CardActionAcceptedEvent { Action.Type: CardActionType.Response } e => e.Action,
            _ => null
        };
        if (action is null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouKrActionObservedEvent>().Any(e => e.ActionId == action.ActionId))
            return;
        var actor = action.ActorSeat;
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<MouKrActionObservedEvent>().LastOrDefault(e => e.ActorSeat == actor);
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind);
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[actor]).ProgramInstances.FirstOrDefault(i => i.SkillId == MouKongRongRuntime.Liwen);
        var source = instance is null ? null : new CardConversionSource(instance.SkillId, "after-use", actor, instance.SkillInstanceId);
        var matches = previous is not null && (action.EffectiveSuit is { } suit && suit != Suit.None && previous.Suit == suit || previous.Category == category);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouKrActionObservedEvent(action.ActionId, actor, action.EffectiveKind, action.EffectiveSuit, category, matches, source, instance?.Program.GameplayHash));
    }
}
