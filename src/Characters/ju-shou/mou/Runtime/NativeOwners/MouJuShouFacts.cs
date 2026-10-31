#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJuShouFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CharacterJuShouMouEntries> getCharacterJuShouMouRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IMouJuShouFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.SkillId == MouJuShouRuntime.ZuoJun)
            return f.TriggerId is null && f.ActivationId == "draw-and-choose" && f.WindowContext is null && f.ActivatingSeat is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 1 && History<ProgramSkillStartedEvent>().Count(x => x.FrameId == f.Id && x.OwnerSeat == f.OwnerSeat && x.SkillId == f.SkillId && x.ActivationId == f.ActivationId) == 1;
        return getCharacterJuShouMouRuntime().MouJsMovementParent(f);
    }

    public bool InHand(int seat, int id) => getCardZoneStore().GetLocation(id) == CardLocation.Hand(seat);
    public bool InDiscard(int id) => getCardZoneStore().GetLocation(id) == CardLocation.DiscardPile;
    public bool Reclaimable(MouJsFirstLoss loss, CardMovementRecord discarded) => getCharacterJuShouMouRuntime().MouJsReclaimable(loss, discarded);
    public bool ReadyToReclaim(MouJsFirstLoss loss) => InDiscard(loss.CardId) && cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(m => m.CardId == loss.CardId)is { } last && getCharacterJuShouMouRuntime().MouJsReclaimable(loss, last);
    public MouJsMuwangQueuedEvent? MuwangOpportunity(ProgramSkillFrame f) => getCharacterJuShouMouRuntime().MouJsMuwangOpportunity(f);
    public IReadOnlyList<int> Discardable(int seat) => !Alive(seat) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, seat)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), OwnedCardMoveIntent.Discard) && !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(seat, c, new(z, seat), OwnedCardMoveIntent.Discard))).Select(c => c.Id).ToArray());
    public string Label(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).DisplayName;
    public IReadOnlyList<MouJsPlan> Plans(ProgramSkillFrame f) => getCharacterJuShouMouRuntime().MouJsPlans(f);
    public IReadOnlyList<MouJsZuoJunGrant> Grants(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MouJsZuoJunGrants ?? [];
    public IReadOnlyList<MouJsFirstLoss> Losses(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MouJsFirstLosses ?? [];
    public IReadOnlyList<int> Seats => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(p => p.Seat).ToArray());

    public IReadOnlyList<(CardConversionSource Source, string Hash)> MuwangSources(int seat) => !Alive(seat) ? [] : Array.AsReadOnly(getCoreGameEngineSkillProgramsRuntime().EnabledSkillPrograms(matchParticipantsState.ReferenceToPlayers[seat]).Where(p => p.Id == MouJuShouRuntime.Muwang).Select(p => (new CardConversionSource(p.Id, "first-loss", seat, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[seat], p.Id)), p.GameplayHash)).ToArray());
    public bool Eligible(int id)
    {
        var kind = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).Kind;
        return EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Basic || EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind);
    }
}
