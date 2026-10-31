#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJiaXuFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CharacterJiaXuMouEntries> getCharacterJiaXuMouRuntime, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime) : IMouJiaXuFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public bool ActualPlay(int s) => Alive(s) && actualTurnProgressState.ReferenceToCurrentSeat == s && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play && gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId > 0;
    public int Hp(int s) => matchParticipantsState.ReferenceToPlayers[s].Hp;
    public int MaximumHp(int s) => matchParticipantsState.ReferenceToPlayers[s].MaxHp;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public IReadOnlyList<MouJiaXuCard> Hand(int seat, bool discard = false) => !Alive(seat) ? [] : Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Where(c => !discard || !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard)).Select(c => new MouJiaXuCard(c.Id, seat, c.DisplayName, getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[seat], c))).ToArray());
    public string FumouInstance(int owner) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[owner], MouJiaXuRuntime.Fumou);
    public bool Yin(int owner, string instance) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(owner, MouJiaXuRuntime.Fumou, instance, "yin");
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((f.SkillId, f.ActivationId)is (MouJiaXuRuntime.Fumou, "view-show-and-use") or (MouJiaXuRuntime.Sushen, "record-and-enter") or (MouJiaXuRuntime.Rushi, "restore-recorded-values")) && History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.OwnerSeat == f.OwnerSeat && p.SkillId == f.SkillId && p.ActivationId == f.ActivationId) == 1;
        var at = getFrameStore().FindIndex(x => x.Id == f.Id);
        return f.SkillId == MouJiaXuRuntime.Fumou && f.TriggerId == "opening-state" && f.ActivationId == f.TriggerId && f.WindowContext is { Window: SkillProgramTriggerWindow.GameStarting } w && w.OwnerSeat == f.OwnerSeat && at > 0 && getFrameStore()[at - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } parent && parent.Id == w.ParentFrameId;
    }

    public IReadOnlyList<LegalAction> UseOptions(ProgramSkillFrame f) => getCharacterJiaXuMouRuntime().MouJxUseOptions(f);
    public CardKind UseKind(int actor, LegalAction a) => a.PlayedCardKind ?? getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[actor], getCoreGameEnginePlayCardHistoryRuntime().ApplyProgramUseAppearance(matchParticipantsState.ReferenceToPlayers[actor], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(a.CardId!.Value), a.ConversionSource));
}
