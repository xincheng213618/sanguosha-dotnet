#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenLinMouRuntime
{
    internal MouChenLinRuntime MouChenLinRules => _mouChenLinOperations.Get();
    internal bool HasMouChenLin => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouChenLinZhuanwen);

    internal bool ValidMouChenLin(ProgramSkillFrame f) => MouChenLinRules.Valid(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 37000, SkillProgramEffectOp.ResolveMouChenLinYaozuo)]
    internal bool CanStartMouChenLinInvitation(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 37000)]
    internal bool MouChenLinDamageTargets(ProgramTargetCardWindowQuery q) => HasMouChenLin && MouChenLinRuntime.DamageCard(q.Kind) && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(q.Kind);
    int IMouChenLinFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long IMouChenLinFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouChenLinFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouChenLinFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouChenLinFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool IMouChenLinFacts.Friendly(int owner, int other) => owner == other || (_coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(owner, out var brain) ? brain.ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0 : _coreMatchParticipantsPlayersAccess._players[owner].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[other].TeamRevealed && _coreMatchParticipantsPlayersAccess._players[owner].TeamId is { } team && _coreMatchParticipantsPlayersAccess._players[other].TeamId == team);
    string IMouChenLinFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    int IMouChenLinFacts.HandCount(int s) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count;
    SkillProgram IMouChenLinFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    bool IMouChenLinFacts.Parent(ProgramSkillFrame f)
    {
        if (f.SkillId == MouChenLinRuntime.Yaozuo)
            return f.TriggerId is null && f.ActivationId == "invite-authors" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((IMouChenLinFacts)this).History<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId && e.OwnerSeat == f.OwnerSeat) == 1;
        if (f.SkillId != MouChenLinRuntime.Zhuanwen || f.TriggerId != "ending-top-written-cards" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return n >= 1 && _resolutionStackOperations._resolutionStack[n - 1] is TurnEndingBoundaryFrame p && p.Id == w.ParentFrameId && p.OwnerSeat == f.OwnerSeat && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Items[p.ItemIndex].Candidate);
    }

    IEnumerable<T> IMouChenLinFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    ActualDiscardRecoveryPhaseKey? IMouChenLinFacts.Phase() => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
    bool IMouChenLinFacts.Recorded(ActualDiscardRecoveryPhaseKey phase) => _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase);
    string IMouChenLinFacts.SkillName(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Name;
    string IMouChenLinFacts.SkillDescription(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Description;
    IReadOnlyList<int> IMouChenLinFacts.Living() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
    IReadOnlyList<MouChenLinCard> IMouChenLinFacts.Revealed(ProgramSkillFrame f) => ((IMouChenLinFacts)this).Current(f.CardSetBindings.Single(b => b.Name == MouChenLinRuntime.RevealBind).CardIds);
    IReadOnlyList<MouChenLinCard> IMouChenLinFacts.Current(IEnumerable<int> ids) => Array.AsReadOnly(ids.Select(id =>
    {
        var c = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return new MouChenLinCard(id, c.Kind, _cardZonesOperations._cardZones.GetLocation(id), c.DisplayName);
    }).ToArray());
    IReadOnlyList<MouChenLinCard> IMouChenLinFacts.He(int actor) => !((IMouChenLinFacts)this).Alive(actor) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, actor)).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(actor, c, new(z, actor), OwnedCardMoveIntent.Transfer)).Select(c => new MouChenLinCard(c.Id, c.Kind, new(z, actor), c.DisplayName))).ToArray());
    MouChenLinUsePlan? IMouChenLinFacts.Plan(ProgramSkillFrame f, int id, int target) => MouChenLinPlan(f, id, target);
    void IMouChenLinNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMouChenLinNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool IMouChenLinNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouChenLinNative.Reveal(ProgramSkillFrame f)
    {
        if (!ValidMouChenLin(f) || f.MouChenLin is not { Stage: MouChenLinStage.Revealing } r || f.CardSetBindings.Any(b => b.Name == MouChenLinRuntime.RevealBind))
            throw new InvalidOperationException("Zhuanwen needs its original unspent native top reveal.");
        _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, r.Requested, null, MouChenLinRuntime.RevealBind, SkillProgramCardSetVisibility.Public);
    }

    void IMouChenLinNative.Move(ProgramSkillFrame f)
    {
        if (!ValidMouChenLin(f) || f.MouChenLin is not { Stage: MouChenLinStage.GiveChildren or MouChenLinStage.ReturningChildren } r)
            throw new InvalidOperationException("Zhuanwen needs its own real public-card transfer segment.");
        var give = r.Stage == MouChenLinStage.GiveChildren;
        var ids = r.Cards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing && (!give || !MouChenLinRuntime.DamageCard(c.Kind))).Select(c => c.Id).ToArray();
        if (ids.Length == 0)
            return;
        if (give && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            return;
        var destination = give ? CardLocation.Hand(r.Target) : CardLocation.DrawPile;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(give ? MouChenLinRuntime.GiveReason : MouChenLinRuntime.ReturnReason), (_, moves) =>
        {
            if (moves.Any(m => m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Zhuanwen changed the destination of its retained physical entities.");
            if (!give)
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(moves.Where(m => m.To == CardLocation.DrawPile).Select(m => m.CardId).ToArray());
            MouChenLinRules.MovesPaid(f.Id, moves);
            if (give && r.Target != f.OwnerSeat)
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, ids, r.Before);
        }, disclosure: CardMovementDisclosure.FaceUp);
    }

    void IMouChenLinNative.Use(ProgramSkillFrame f) => IssueMouChenLinUse(f);
    void IMouChenLinNative.Give(ProgramSkillFrame f)
    {
        if (!MouChenLinRules.ValidInvitation(f) || f.MouChenLinInvitation is not { Stage: MouChenLinInvitationStage.MovementChildren, Closed: false, PendingCard: { } card } r || !r.Eligible.Contains(r.PendingGiver) || r.Answered.Contains(r.PendingGiver) || !_characterChenLinMouOperations.CreateMouChenLinFacts().He(r.PendingGiver).Contains(card) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Yaozuo needs this actual participant's current original HE entity.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new("skill-program.mou:yaozuo.give"), (_, moves) =>
        {
            if (moves is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Yaozuo lost its original native Give payment movement.");
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouChenLinInvitation = parent.MouChenLinInvitation!with { After = m.Sequence } });
            if (m.To == CardLocation.Hand(f.OwnerSeat))
                RecordMouChenLinGift(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.PendingGiver, card.Id, r.Before, moves);
            _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, r.PendingGiver, f.OwnerSeat, [card.Id], r.Before);
        });
    }

    void IMouChenLinNative.CloseInvitation(ProgramSkillFrame f) => IssueMouChenLinNonHandDonorTickets(f);
}
