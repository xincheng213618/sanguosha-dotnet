#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRanMouMobileRuntime
{
    internal MouZhuRanRuntime MouZhuRan => _mouZhuRanOperations.Get();

    int IMouZhuRanFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    bool IMouZhuRanFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    ActualDiscardRecoveryPhaseKey? IMouZhuRanFacts.Phase => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();

    IReadOnlyList<int> IMouZhuRanFacts.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(p => p.Seat).ToArray());

    IReadOnlyList<int> IMouZhuRanFacts.ActionOrder(int from, IEnumerable<int> seats) => Array.AsReadOnly(seats.Distinct().OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(from, s)).ToArray());
    MouZhuRanPlayer IMouZhuRanFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[seat]));
    IEnumerable<T> IMouZhuRanFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    SkillProgram? IMouZhuRanFacts.Program(string skill) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(skill)?.Program;
    bool IMouZhuRanFacts.ExactStart(ProgramSkillFrame f, SkillProgramEffectOp operation)
    {
        if (f.InstructionIndex < 1 || ((IMouZhuRanFacts)this).Program(f.SkillId)?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != operation || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return false;
        if (operation == SkillProgramEffectOp.ResolveMouZhuRanZhenwei)
            return f.SkillId == MouZhuRanRuntime.Zhenwei && f.ActivationId == "sealed-discard" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats is [var target] && target != f.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && ((IMouZhuRanFacts)this).Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && !((IMouZhuRanFacts)this).History<MouZhuRanStartedEvent>().Any(x => x.Source.OwnerSeat == f.OwnerSeat && x.Phase == p && x.Operation == operation);
        return operation == SkillProgramEffectOp.ResolveMouZhuRanHeyuan && f.SkillId == MouZhuRanRuntime.Heyuan && f.TriggerId == "repeat-last" && f.ActivationId == f.TriggerId && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((IMouZhuRanFacts)this).NativeParent(f);
    }

    bool IMouZhuRanFacts.NativeParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w || w.OwnerSeat != f.OwnerSeat || w.SourceSeat != f.OwnerSeat || w.TargetSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        return n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is TurnEndingBoundaryFrame parent && parent.Id == w.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } c && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    IReadOnlyList<MouZhuRanCard> IMouZhuRanFacts.Discardable(int seat) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, seat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), OwnedCardMoveIntent.Discard)).Select(c => new MouZhuRanCard(c.Id, new(z, seat), _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], c)))).ToArray());
    int IMouZhuRanFacts.KeepValue(int seat, int id) => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[seat]);
    string IMouZhuRanFacts.CardLabel(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName;
    bool IMouZhuRanFacts.Friendly(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false)) > 0;
    long IMouZhuRanFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    void IMouZhuRanNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouZhuRanNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouZhuRanNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouZhuRanNative.Discard(ProgramSkillFrame f, int payer, IReadOnlyList<MouZhuRanCard> cards)
    {
        var reason = f.MouZhuRan!.Operation == SkillProgramEffectOp.ResolveMouZhuRanZhenwei ? MouZhuRanRuntime.CostReason : MouZhuRanRuntime.HeyuanCostReason;
        if (!MouZhuRan.Valid(f) || f.MouZhuRan is not { Stage: MouZhuRanStage.CostChildren } r || r.Payer != payer || !r.Paying.SequenceEqual(cards) || cards.Count == 0 || cards.Any(c => !_characterZhuRanMouMobileOperations.CreateMouZhuRanFacts().Discardable(payer).Any(l => l.CardId == c.CardId && l.From == c.From)))
            throw new InvalidOperationException("Mou Zhu Ran's native discard must pay precisely this participant's sealed real HE materials.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, payer, cards.Select(c => c.CardId).ToArray(), new(reason), (_, records) =>
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
            if (parent?.MouZhuRan is not { Stage: MouZhuRanStage.CostChildren } paid || !MouZhuRan.Valid(parent) || paid.Payer != payer || !paid.Paying.SequenceEqual(cards) || records.Count != cards.Count || records.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= paid.Before || m.Reason.Value != reason || !cards.Any(c => c.CardId == m.CardId && c.From == m.From) || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Zhu Ran's discard callback must seal its exact ancestor and native primary movement records.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent = parent with { MouZhuRan = paid with { After = records.Max(m => m.Sequence) } });
            foreach (var m in records)
            {
                var material = cards.Single(c => c.CardId == m.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhuRanCostPaidEvent(f.Id, payer, m.CardId, m.From, material.Suit, m.Sequence));
            }
        });
    }

    void IMouZhuRanNative.Damage(ProgramSkillFrame f, int target) => _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
}
