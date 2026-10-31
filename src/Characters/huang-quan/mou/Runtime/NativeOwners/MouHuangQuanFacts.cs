#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangQuanFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<CardZoneStore> getCardZoneStore, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime) : IMouHuangQuanFacts
{
    public bool GameOver => matchOutcomeState.ReferenceToWinner != Winner.None;
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public string CardName(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).DisplayName;
    public IReadOnlyList<int> Living => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    public ContentSkillDefinition Skill(string id) => getContentRegistry().GetSkill(id);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public bool ParentMatches(ProgramSkillFrame f) => getCharacterHuangQuanMouRuntime().MouHqParent(f);
    public MouHqOriginalUse? OriginalUse(int owner, ProgramSkillWindowContext? context) => getCharacterHuangQuanMouRuntime().TryMouHqOriginalUse(owner, context, out var u, out _) ? new(u.Id, u.Action!.ActionId, u.SourceSeat == owner) : null;
    public IReadOnlyList<MouHqGift> Gifts(int giver) => matchParticipantsState.ReferenceToPlayers[giver].MouHqActualGifts ?? [];
    public IReadOnlyList<MouHqEntity> TransferEntities(ProgramSkillFrame f, int actor) => Array.AsReadOnly(getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(f.Id, actor, actor, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.OwnerHand).Select(c => new MouHqEntity(c.Cards.Single(), getCardZoneStore().GetLocation(c.Cards.Single()))).ToArray());
    public double HelpValue(int actor, int target) => aiBrainsState.ReferenceToAiBrains[actor].ScoreProgramTarget(createSnapshot(actor), target, new(0, 0, 0, 0, 1, 0, true, false));
    public int KeepValue(int actor, int card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[actor]);
    public bool MovementRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouHuangQuan is not { Paid: true, Stage: MouHqStage.GiveChildren or MouHqStage.ReplyChildren } r)
            return false;
        foreach (var entity in r.Selected.Where(x => x.From.Zone == CardZoneKind.Equipment && getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(x.CardId).Kind == CardKind.SilverLion))
        {
            var seat = entity.From.OwnerSeat!.Value;
            var reason = r.Stage == MouHqStage.GiveChildren ? MouHuangQuanRuntime.MouHqGiveReason : MouHuangQuanRuntime.MouHqReplyReason;
            if (!cardMovementJournalState.ReferenceToCardMovements.Any(m => m.CardId == entity.CardId && m.From == entity.From && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == reason))
                continue;
            if (child is RecoveryReplacementFrame recovery && getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat)
                return true;
            if (child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.PlayerSeat == seat && x.Reason.Value == reason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter))
                return true;
        }

        return false;
    }
}
