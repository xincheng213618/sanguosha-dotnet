#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouRuntime
{
    internal MouJiangWeiAnniversaryRuntime MouJiangWeiAnniversary => _mouJiangWeiAnniversaryOperations.Get();

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 70, SkillProgramTriggerWindow.DyingExited)]
    internal bool IsMouJiangWeiFuzhanExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouJiangWeiAnniversaryRuntime.Fuzhan || _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is not [{ Op: SkillProgramEffectOp.ResolveMouJiangWeiFuzhan }] || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame p || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex] != c || !_characterYanPuOlOperations.IsYanPuDyingExitEdge(_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == p.Id)))
            return false;
        return _characterJiangWeiMouOperations.CreateMouJiangWeiAnniversaryFacts().CanTrigger(c, w, SkillProgramEffectOp.ResolveMouJiangWeiFuzhan);
    }

    int IMouJiangWeiAnniversaryFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long IMouJiangWeiAnniversaryFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouJiangWeiAnniversaryFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouJiangWeiAnniversaryFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouJiangWeiAnniversaryFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    int IMouJiangWeiAnniversaryFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int IMouJiangWeiAnniversaryFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    string IMouJiangWeiAnniversaryFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    bool IMouJiangWeiAnniversaryFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    IReadOnlyList<int> IMouJiangWeiAnniversaryFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    string IMouJiangWeiAnniversaryFacts.Instance(int owner) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[owner], MouJiangWeiAnniversaryRuntime.Juemou);
    bool IMouJiangWeiAnniversaryFacts.State(int owner, string instance, string state) => _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner, MouJiangWeiAnniversaryRuntime.Juemou, instance, state);
    SkillProgram IMouJiangWeiAnniversaryFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> IMouJiangWeiAnniversaryFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    bool IMouJiangWeiAnniversaryFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != w.ParentFrameId)
            return false;
        var parent = _resolutionStackOperations._resolutionStack[i - 1];
        if (parent is DeferredTurnEndFrame ended)
            return w.Window == SkillProgramTriggerWindow.AfterTurnEnded && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && ended.OwnerSeat == f.OwnerSeat && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended));
        if (parent is ProgramCardTriggerWindowFrame card)
        {
            if (card.CandidateIndex < 0 || card.CandidateIndex >= card.Candidates.Count || w.CardUse?.CardActionId != card.Action.ActionId)
                return false;
            var c = card.Candidates[card.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        var candidate = parent switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            TurnEndingBoundaryFrame p when p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            _ => null
        };
        return candidate is not null && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    }

    bool IMouJiangWeiAnniversaryFacts.CanTrigger(ProgramTriggerCandidate c, ProgramSkillWindowContext w, SkillProgramEffectOp operation)
    {
        if (w.OwnerSeat != c.OwnerSeat || !((IMouJiangWeiAnniversaryFacts)this).Alive(c.OwnerSeat))
            return false;
        if (operation == SkillProgramEffectOp.InitializeMouJiangWeiJuemou)
            return c.SkillId == MouJiangWeiAnniversaryRuntime.Juemou && w.Window == SkillProgramTriggerWindow.GameStarting;
        if (operation == SkillProgramEffectOp.RestoreMouJiangWeiJuemou)
            return c.SkillId == MouJiangWeiAnniversaryRuntime.Juemou && ((IMouJiangWeiAnniversaryFacts)this).CausalDying(c.OwnerSeat, w)is not null;
        if (operation == SkillProgramEffectOp.ResolveMouJiangWeiJuemou)
        {
            if (c.SkillId != MouJiangWeiAnniversaryRuntime.Juemou || ((IMouJiangWeiAnniversaryFacts)this).State(c.OwnerSeat, ((IMouJiangWeiAnniversaryFacts)this).Instance(c.OwnerSeat), "yin") && ((IMouJiangWeiAnniversaryFacts)this).Living().Count < 2)
                return false;
            return ((IMouJiangWeiAnniversaryFacts)this).Action(w) > 0 || ((IMouJiangWeiAnniversaryFacts)this).State(c.OwnerSeat, ((IMouJiangWeiAnniversaryFacts)this).Instance(c.OwnerSeat), "modified") && w.Window is SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterTurnEnded && w.TargetSeat == c.OwnerSeat;
        }

        if (operation != SkillProgramEffectOp.ResolveMouJiangWeiFuzhan || c.SkillId != MouJiangWeiAnniversaryRuntime.Fuzhan || w.Window != SkillProgramTriggerWindow.DyingExited || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } exited || w.SourceSeat != ret.VictimSeat || w.TargetSeat != ret.VictimSeat || !((IMouJiangWeiAnniversaryFacts)this).Alive(ret.VictimSeat) || ((IMouJiangWeiAnniversaryFacts)this).Hp(ret.VictimSeat) <= 0)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DyingResolvedEvent>().Count(p => p.ResolutionId == ret.DyingFrameId && p.VictimSeat == ret.VictimSeat && p.Survived) == 1;
    }

    (long Cause, long Dying)? IMouJiangWeiAnniversaryFacts.CausalDying(int owner, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.DyingEntering || _coreDyingCursorRuntimeOperations.Value.ActiveDying is not { } dying || dying.VictimSeat != owner || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering, Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } entry || entry.ResumeDyingFrameId != dying.Id || entry.OwnerSeat != owner || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == dying.ParentFrameId)is not DamageFrame damage || damage.TargetSeat != owner || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damage.ParentFrameId)is not ProgramSkillFrame { MouJiangWeiAnniversary: { Operation: SkillProgramEffectOp.ResolveMouJiangWeiJuemou, Stage: MouJiangWeiAnniversaryStage.Damage } r } root || root.OwnerSeat != owner || root.AttackAttempt is not { } attempt || attempt.TargetSeat != owner || attempt.SourceSeat != r.DamageSource || r.Victim != owner || !MouJiangWeiAnniversary.Valid(root) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PlayerDyingEvent>().Any(p => p.ResolutionId == dying.Id && p.VictimSeat == owner) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiangWeiAnniversaryDamageIssuedEvent>().Count(p => p.FrameId == root.Id && p.Victim == owner && p.Source == attempt.SourceSeat) != 1)
            return null;
        return (root.Id, dying.Id);
    }

    long IMouJiangWeiAnniversaryFacts.Action(ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.CardUseCommitted && w.CardUse is { } info && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is ProgramCardTriggerWindowFrame p && p.Action.ActionId == info.CardActionId && p.Action.ActorSeat == w.OwnerSeat && CardUseCategoryCatalog.Get(p.Action.EffectiveKind)is CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick ? p.Action.ActionId : 0;
    IReadOnlyList<MouJiangWeiAnniversaryCard> IMouJiangWeiAnniversaryFacts.Discardable(int chooser, int holder) => !((IMouJiangWeiAnniversaryFacts)this).Alive(chooser) || !((IMouJiangWeiAnniversaryFacts)this).Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, holder)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, c, new(z, holder), OwnedCardMoveIntent.Discard)).Select(c => new MouJiangWeiAnniversaryCard(c.Id, new(z, holder), c.DisplayName))).ToArray());
    void IMouJiangWeiAnniversaryNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouJiangWeiAnniversaryNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouJiangWeiAnniversaryNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouJiangWeiAnniversaryNative.State(ProgramSkillFrame f, string state, bool value)
    {
        if (!MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { } r || state is not ("yin" or "modified"))
            throw new InvalidOperationException("Juemou state requires its original accepted branch or Fuzhan.");
        _coreProgramSkillChangesOperations.SetProgramBooleanState2(f, MouJiangWeiAnniversaryRuntime.Juemou, r.JuemouInstance, state, value);
    }

    MouJiangWeiAnniversaryCard IMouJiangWeiAnniversaryNative.RandomHand(ProgramSkillFrame f, IReadOnlyList<MouJiangWeiAnniversaryCard> cards)
    {
        if (!MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.Discard } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.DamageSource)))
            throw new InvalidOperationException("Juemou random Hand choice requires the actual payable hidden Hand group.");
        var before = _randomOperations._random.State;
        var selected = cards[_randomOperations._random.Next(cards.Count)];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiAnniversaryHandChosenEvent(f.Id, selected.Id, before, _randomOperations._random.State, cards.Count));
        return selected;
    }

    void IMouJiangWeiAnniversaryNative.Discard(ProgramSkillFrame f)
    {
        if (!MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.DiscardChildren, Card: { } card } r || !_characterJiangWeiMouOperations.CreateMouJiangWeiAnniversaryFacts().Discardable(r.Victim, r.DamageSource).Contains(card))
            throw new InvalidOperationException("Juemou discard requires the chosen actual other-role HE entity.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Victim, [card.Id], new(MouJiangWeiAnniversaryRuntime.CostReason), (_, records) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiangWeiAnniversary!;
            if (!MouJiangWeiAnniversary.Valid(parent) || records is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= paid.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Juemou lost its primary real foreign discard receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJiangWeiAnniversary = paid with { After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiAnniversaryDiscardedEvent(f.Id, paid.Victim, paid.DamageSource, card.Id, card.From, m.Sequence));
        });
    }

    void IMouJiangWeiAnniversaryNative.Damage(ProgramSkillFrame f)
    {
        if (!MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.Damage } r)
            throw new InvalidOperationException("Juemou damage requires its accepted real Yin/Yang branch.");
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Victim, 1, explicitSourceSeat: r.DamageSource);
    }

    void IMouJiangWeiAnniversaryNative.Recover(ProgramSkillFrame f, int amount)
    {
        if (!MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.RecoveryChildren } r || r.Requested != amount)
            throw new InvalidOperationException("Juemou/Fuzhan recovery requires its real native recovery cursor.");
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
    }
}
