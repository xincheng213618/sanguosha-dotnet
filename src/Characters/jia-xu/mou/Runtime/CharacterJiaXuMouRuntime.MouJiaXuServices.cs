#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiaXuMouRuntime
{
    internal MouJiaXuRuntime MouJiaXu => _mouJiaXuOperations.Get();
    internal bool HasMouJiaXu => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouJiaXuFumou);

    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 36500)]
    internal bool MouJxImplicitDrawTwo(ProgramTargetCardWindowQuery q) => HasMouJiaXu;
    int IMouJiaXuFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    long IMouJiaXuFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouJiaXuFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouJiaXuFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouJiaXuFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool IMouJiaXuFacts.ActualPlay(int s) => ((IMouJiaXuFacts)this).Alive(s) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == s && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > 0;
    int IMouJiaXuFacts.Hp(int s) => _coreMatchParticipantsPlayersAccess._players[s].Hp;
    int IMouJiaXuFacts.MaximumHp(int s) => _coreMatchParticipantsPlayersAccess._players[s].MaxHp;
    string IMouJiaXuFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    SkillProgram? IMouJiaXuFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> IMouJiaXuFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<int> IMouJiaXuFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    IReadOnlyList<MouJiaXuCard> IMouJiaXuFacts.Hand(int seat, bool discard) => !((IMouJiaXuFacts)this).Alive(seat) ? [] : Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(c => !discard || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard)).Select(c => new MouJiaXuCard(c.Id, seat, c.DisplayName, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], c))).ToArray());
    string IMouJiaXuFacts.FumouInstance(int owner) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[owner], MouJiaXuRuntime.Fumou);
    bool IMouJiaXuFacts.Yin(int owner, string instance) => _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner, MouJiaXuRuntime.Fumou, instance, "yin");
    bool IMouJiaXuFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((f.SkillId, f.ActivationId)is (MouJiaXuRuntime.Fumou, "view-show-and-use") or (MouJiaXuRuntime.Sushen, "record-and-enter") or (MouJiaXuRuntime.Rushi, "restore-recorded-values")) && ((IMouJiaXuFacts)this).History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.OwnerSeat == f.OwnerSeat && p.SkillId == f.SkillId && p.ActivationId == f.ActivationId) == 1;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return f.SkillId == MouJiaXuRuntime.Fumou && f.TriggerId == "opening-state" && f.ActivationId == f.TriggerId && f.WindowContext is { Window: SkillProgramTriggerWindow.GameStarting } w && w.OwnerSeat == f.OwnerSeat && at > 0 && _resolutionStackOperations._resolutionStack[at - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } parent && parent.Id == w.ParentFrameId;
    }

    IReadOnlyList<LegalAction> IMouJiaXuFacts.UseOptions(ProgramSkillFrame f) => MouJxUseOptions(f);
    CardKind IMouJiaXuFacts.UseKind(int actor, LegalAction a) => a.PlayedCardKind ?? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[actor], _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(a.CardId!.Value), a.ConversionSource));
    void IMouJiaXuNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouJiaXuNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouJiaXuNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouJiaXuNative.State(ProgramSkillFrame f, bool yin)
    {
        if (!MouJiaXu.Valid(f))
            throw new InvalidOperationException("Fumou's state requires its own paid source and frozen instance.");
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, MouJiaXuRuntime.Fumou, f.MouJiaXu!.FumouInstance, "yin", yin);
    }

    void IMouJiaXuNative.Grant(ProgramSkillFrame f)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.GrantChildren, Record: { } record } || record.FrameId != f.Id || _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiaXuDerivedGrantedEvent>().Any(g => g.FrameId == f.Id))
            throw new InvalidOperationException("Sushen must record real current values before actually granting derived Rushi once.");
        _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], $"sushen:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(f.Id)}", [MouJiaXuRuntime.Rushi]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiaXuDerivedGrantedEvent(f.Id, f.OwnerSeat));
    }

    void IMouJiaXuNative.Reveal(ProgramSkillFrame f)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.Use or MouJiaXuStage.Recipient } r || r.Frozen.Count > (r.View.Count + 1) / 2 || r.Frozen.Any(id => !r.View.Any(c => c.Id == id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.Holder)))
            throw new InvalidOperationException("Fumou may reveal only the exact chosen half of its privately viewed real Hand.");
        if (r.Frozen.Count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.ActivationId, f.OwnerSeat, "fumou-shown", Array.AsReadOnly(r.Frozen.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
        foreach (var id in r.Frozen)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiaXuShownEvent(f.Id, r.Holder, id));
    }

    void IMouJiaXuNative.Transfer(ProgramSkillFrame f)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.TransferChildren, Yin: false } r || r.Recipient == f.OwnerSeat || r.Recipient == r.Holder || !_coreMatchParticipantsPlayersAccess._players[r.Recipient].IsAlive)
            throw new InvalidOperationException("Fumou Yang needs its selected third living recipient and exact shown entities.");
        var ids = r.Frozen.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(r.Holder)).ToArray();
        if (ids.Length == 0)
            return;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(r.Recipient), new(MouJiaXuRuntime.TransferReason), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiaXu!;
            if (!MouJiaXu.Valid(parent) || moves.Count != ids.Length || moves.Any(m => !ids.Contains(m.CardId) || m.From != CardLocation.Hand(r.Holder) || m.Sequence <= r.Before || m.To != CardLocation.Hand(r.Recipient) && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Fumou lost the primary actual shown-card obtain receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJiaXu = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves.Where(m => m.To == CardLocation.Hand(r.Recipient)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiaXuTransferredEvent(f.Id, r.Holder, r.Recipient, m.CardId, m.Sequence));
        });
    }

    void IMouJiaXuNative.Use(ProgramSkillFrame f) => IssueMouJxUse(f);
    void IMouJiaXuNative.SetHealth(ProgramSkillFrame f)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { Operation: SkillProgramEffectOp.RestoreMouJiaXuRushi, Stage: MouJiaXuStage.HealthChildren, Record: { } record } r || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp != r.HpBefore || r.HpAfter != Math.Min(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, record.Hp))
            throw new InvalidOperationException("Rushi requires the actual recorded HP and current native maximum.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        owner.Hp = r.HpAfter;
        var change = _coreHpChangeProgramsOperations.RecordHpChange(f.Id, f.OwnerSeat, f.OwnerSeat, r.HpBefore, r.HpAfter, HpChangeKind.Setting);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiaXu = r with { HpChangeId = change?.Id } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiaXuHpSetEvent(f.Id, record.FrameId, r.HpBefore, r.HpAfter, change?.Id));
    }

    void IMouJiaXuNative.Discard(ProgramSkillFrame f)
    {
        if (!MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.DiscardChildren, Record: not null } r || r.Selected.Any(id => !_characterJiaXuMouOperations.CreateMouJiaXuFacts().Hand(f.OwnerSeat, true).Any(c => c.Id == id)))
            throw new InvalidOperationException("Rushi requires its selected real discardable Hand entities.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(MouJiaXuRuntime.DiscardReason), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiaXu!;
            if (!MouJiaXu.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Selected.Contains(m.CardId) || m.From != CardLocation.Hand(f.OwnerSeat) || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Rushi lost its actual logical Hand discard.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJiaXu = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiaXuDiscardPaidEvent(f.Id, m.CardId, m.Sequence));
        });
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 36500)]
    internal CardSnapshot[] MouJxPrivateHandView(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.MouJiaXu is { Stage: MouJiaXuStage.Show } && MouJiaXu.Valid(f)).SelectMany(f => f.MouJiaXu!.View.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Hand(c.Holder))).DistinctBy(c => c.Id).Select(c => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Id))).ToArray();
}
