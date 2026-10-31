#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoHongFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, KernelGetCombatDistanceCall getCombatDistance, Func<RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries> getCoreGameEngineProgramDodgeResponsesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CharacterCaoHongMouEntries> getCharacterCaoHongMouRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime) : IMouCaoHongFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public int PhaseId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public int Keep(int owner, int card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card), matchParticipantsState.ReferenceToPlayers[owner]);
    public SkillProgram? Program(string skill) => getContentRegistry().Skills.GetValueOrDefault(skill)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool ActualPlay(int owner) => owner == actualTurnProgressState.ReferenceToCurrentSeat && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p);
    public bool AllDistanceOne(int owner) => matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != owner).All(p => getCombatDistance(owner, p.Seat) == 1 && getCombatDistance(p.Seat, owner) == 1);
    public IReadOnlyList<int> InRange(int owner) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != owner && getCoreGameEngineProgramDodgeResponsesRuntime().IsWithinAttackRange(owner, p.Seat)).Select(p => p.Seat).Order().ToArray());
    public int OutsideCount(int owner) => matchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive && p.Seat != owner && !getCoreGameEngineProgramDodgeResponsesRuntime().IsWithinAttackRange(owner, p.Seat));
    public IReadOnlyList<int> ExchangeTargets(int owner) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != owner && getCorePhysicalOwnedCardQueriesRuntime().GetHand(p).Count > 0 && !History<MouCaoHongExchangeIssuedEvent>().Any(x => x.Owner == owner && x.Turn == Turn && x.Target == p.Seat)).Select(p => p.Seat).Order().ToArray());
    public IReadOnlyList<MouCaoHongCard> Hand(int owner) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[owner]).Select(c => new MouCaoHongCard(c.Id, c.Kind, CardLocation.Hand(owner), c.DisplayName)).ToArray());
    public IReadOnlyList<MouCaoHongCard> Cards(int actor, int holder, bool discard) => !Alive(actor) || !Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, holder)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(actor, c, new(z, holder), discard ? OwnedCardMoveIntent.Discard : actor == holder ? OwnedCardMoveIntent.Transfer : OwnedCardMoveIntent.Obtain)).Select(c => new MouCaoHongCard(c.Id, c.Kind, new(z, holder), c.DisplayName))).ToArray());
    public MouCaoHongXianjuIssuedEvent? Debt(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => getCharacterCaoHongMouRuntime().CurrentMouCaoHongDebt(c, w);
    public MouCaoHongUse? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || getFrameStore().SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.ActionId != info.CardActionId || p.Action.Type != CardActionType.Use || p.Action.ActorSeat != c.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return null;
        var current = p.Candidates[p.CandidateIndex];
        if (current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash || getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(info.ParentCardUseFrameId)is not { } use || use.Action?.ActionId != p.Action.ActionId || use.SourceSeat != c.OwnerSeat)
            return null;
        return new(p.Id, use.Id, p.Action.ActionId, Array.AsReadOnly(use.TargetSeats.Where(s => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && s != c.OwnerSeat).Distinct().Order().ToArray()));
    }

    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouCaoHongComposition.Xianju && f.ActivationId == "obtain-in-range" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (n < 1 || getFrameStore()[n - 1].Id != w.ParentFrameId)
            return false;
        if (f.SkillId == MouCaoHongComposition.Yingjia && f.TriggerId == "used-other-targets")
            return Use(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), w)is not null;
        return f.SkillId == MouCaoHongComposition.Xianju && f.TriggerId == "issued-play-end" && this.CurrentMouCaoHongDebt(f, w);
    }

    internal bool CurrentMouCaoHongDebt(ProgramSkillFrame f, ProgramSkillWindowContext w) => getCharacterCaoHongMouRuntime().CurrentMouCaoHongDebt(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex), w)is not null;
}
