#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhugeJinFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreActualRoundProgressState actualRoundProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CharacterZhugeJinMouEntries> getCharacterZhugeJinMouRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime) : IMouZhugeJinFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public int Hp(int s) => matchParticipantsState.ReferenceToPlayers[s].Hp;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public bool Friendly(int owner, int other) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public SkillProgram? Program(string s) => getContentRegistry().Skills.GetValueOrDefault(s)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<int> Targets(int owner) => matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != owner && getCorePhysicalOwnedCardQueriesRuntime().GetHand(p).Count > 0 && !getCharacterZhugeJinMouRuntime().HasMouZhugeJinZijin(p.Seat)).Select(p => p.Seat).Order().ToArray();
    public IReadOnlyList<MouZhugeJinCard> Hand(int s) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Select(c => new MouZhugeJinCard(c.Id, CardLocation.Hand(s), c.DisplayName)).ToArray());
    public IReadOnlyList<MouZhugeJinCard> Costs(int chooser, int holder) => !Alive(chooser) || !Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(zone => getCardZoneStore().CardsAt(new(zone, holder)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(chooser, c, new(zone, holder), OwnedCardMoveIntent.Discard) && (chooser != holder || !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(chooser, c, new(zone, holder), OwnedCardMoveIntent.Discard))).Select(c => new MouZhugeJinCard(c.Id, new(zone, holder), c.DisplayName))).ToArray());
    private MouZhugeJinUse? NativeUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } card || getFrameStore().SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != card.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        var use = getFrameStore().OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == window.Action.ActionId);
        return new(window.Id, use?.Id ?? 0, window.Action, window.Action.EffectiveIsRed == true);
    }

    public MouZhugeJinUse? Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w, SkillProgramEffectOp op)
    {
        var result = NativeUse(c, w);
        if (result is null)
            return null;
        var action = result.Action;
        if (op == SkillProgramEffectOp.ResolveMouZhugeJinZijin)
            return w.Window == SkillProgramTriggerWindow.CardUseCompleted && action.ActorSeat == c.OwnerSeat && w.Facts?.CardUseCausedDamage == false && History<CurrentTurnCardUseKindsRecordedEvent>().Any(p => p.CardActionId == action.ActionId && p.ActorSeat == c.OwnerSeat && p.WholeActualTurnUseSequence is> 0) ? result : null;
        if (op != SkillProgramEffectOp.ResolveMouZhugeJinHoude || w.Window != SkillProgramTriggerWindow.CardUseBeforeTargetEffects || result.UseId == 0 || actualTurnProgressState.ReferenceToPhase != TurnPhase.Play || actualTurnProgressState.ReferenceToCurrentSeat == c.OwnerSeat || action.ActorSeat != actualTurnProgressState.ReferenceToCurrentSeat || !action.TargetSeats.Contains(c.OwnerSeat) || !HoudeKind(action, result.Red))
            return null;
        var history = getCoreGameEnginePlayCardHistoryRuntime().EventsSinceLastBoundary(p => p is PhaseChangedEvent or TurnStartedEvent or TurnEndedEvent);
        var first = history.Select(p => p switch
        {
            CardUseAppearanceCapturedEvent appearance => appearance.Action,
            CardActionAcceptedEvent accepted => accepted.Action,
            _ => null
        }).OfType<CardActionContext>().Append(action).Where(a => a.ActorSeat == action.ActorSeat && a.TargetSeats.Contains(c.OwnerSeat) && HoudeKind(a, result.Red)).DistinctBy(a => a.ActionId).MinBy(a => a.ActionId);
        return first?.ActionId == action.ActionId ? result : null;
    }

    internal static bool HoudeKind(CardActionContext a, bool red) => a.EffectiveIsRed == red && (red ? EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) : EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind));
    public bool Parent(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.ResolveMouZhugeJinTaozhou)
            return f.SkillId == MouZhugeJinComposition.Taozhou && f.ActivationId == "secret-number-and-give" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        if (f.WindowContext is not { } w || f.TriggerId != f.ActivationId || f.SkillId != (op == SkillProgramEffectOp.ResolveMouZhugeJinHoude ? MouZhugeJinComposition.Houde : MouZhugeJinComposition.Zijin) || w.Window != (op == SkillProgramEffectOp.ResolveMouZhugeJinHoude ? SkillProgramTriggerWindow.CardUseBeforeTargetEffects : SkillProgramTriggerWindow.CardUseCompleted))
            return false;
        var n = getFrameStore().FindIndex(x => x.Id == f.Id);
        return n > 0 && getFrameStore()[n - 1] is ProgramCardTriggerWindowFrame window && window.Id == w.ParentFrameId && NativeUse(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), w)is not null;
    }
}
