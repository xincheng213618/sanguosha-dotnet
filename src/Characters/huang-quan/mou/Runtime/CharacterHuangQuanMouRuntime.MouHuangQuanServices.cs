#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMouRuntime
{
    bool IMouHuangQuanFacts.GameOver => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    int IMouHuangQuanFacts.Turn => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int IMouHuangQuanFacts.TurnOwner => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long IMouHuangQuanFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouHuangQuanFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouHuangQuanFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string IMouHuangQuanFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string IMouHuangQuanFacts.CardName(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName;
    IReadOnlyList<int> IMouHuangQuanFacts.Living => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    ContentSkillDefinition IMouHuangQuanFacts.Skill(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id);
    IEnumerable<T> IMouHuangQuanFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool IMouHuangQuanFacts.ParentMatches(ProgramSkillFrame f) => MouHqParent(f);
    MouHqOriginalUse? IMouHuangQuanFacts.OriginalUse(int owner, ProgramSkillWindowContext? context) => TryMouHqOriginalUse(owner, context, out var u, out _) ? new(u.Id, u.Action!.ActionId, u.SourceSeat == owner) : null;
    IReadOnlyList<MouHqGift> IMouHuangQuanFacts.Gifts(int giver) => _coreMatchParticipantsPlayersAccess._players[giver].MouHqActualGifts ?? [];
    IReadOnlyList<MouHqEntity> IMouHuangQuanFacts.TransferEntities(ProgramSkillFrame f, int actor) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, actor, actor, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.OwnerHand).Select(c => new MouHqEntity(c.Cards.Single(), _cardZonesOperations._cardZones.GetLocation(c.Cards.Single()))).ToArray());
    double IMouHuangQuanFacts.HelpValue(int actor, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[actor].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(actor), target, new(0, 0, 0, 0, 1, 0, true, false));
    int IMouHuangQuanFacts.KeepValue(int actor, int card) => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[actor]);
    bool IMouHuangQuanFacts.MovementRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouHuangQuan is not { Paid: true, Stage: MouHqStage.GiveChildren or MouHqStage.ReplyChildren } r)
            return false;
        foreach (var entity in r.Selected.Where(x => x.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(x.CardId).Kind == CardKind.SilverLion))
        {
            var seat = entity.From.OwnerSeat!.Value;
            var reason = r.Stage == MouHqStage.GiveChildren ? MouHuangQuanRuntime.MouHqGiveReason : MouHuangQuanRuntime.MouHqReplyReason;
            if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == entity.CardId && m.From == entity.From && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == reason))
                continue;
            if (child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat)
                return true;
            if (child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.PlayerSeat == seat && x.Reason.Value == reason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter))
                return true;
        }

        return false;
    }

    void IMouHuangQuanNative.Give(ProgramSkillFrame f, int giver, int recipient, IReadOnlyList<int> ids, bool reply, long before)
    {
        if (ids.Count == 0)
            return;
        if (!ValidMouHq(f) || f.MouHuangQuan is not { Paid: true } r || r.Selected.Count != ids.Count || r.Selected.Any(x => !ids.Contains(x.CardId) || _cardZonesOperations._cardZones.GetLocation(x.CardId) != x.From) || (reply ? r.Recipient : f.OwnerSeat) != giver || (reply ? f.OwnerSeat : r.Recipient) != recipient || before != r.Before)
            throw new InvalidOperationException("Qiaodui/Tuicheng native payment lost its exact issued sources and payer.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(reply ? MouHuangQuanRuntime.MouHqReplyReason : MouHuangQuanRuntime.MouHqGiveReason), (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouHuangQuan = current.MouHuangQuan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            RecordNativeGivenCards(f.Id, giver, recipient, ids, before);
        }, disclosure: CardMovementDisclosure.FaceDown);
    }

    bool IMouHuangQuanNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    void IMouHuangQuanNative.OriginalEffect(ProgramSkillFrame f) => IssueMouHqOriginalEffect(f);
    void IMouHuangQuanNative.Finish(ProgramSkillFrame f, bool accepted) => _coreSkillProgramsOperations.FinishProgramSkill(f, accepted);
    void IMouHuangQuanNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
}
