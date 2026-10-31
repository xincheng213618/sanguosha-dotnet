#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXunYuAnniversaryFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXunYuMouEntries> getCharacterXunYuMouRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries> getCoreGameEngineProgramDiscardOriginsRuntime) : IMouXunYuAnniversaryFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public bool OwnPlay(int s) => Alive(s) && s == actualTurnProgressState.ReferenceToCurrentSeat && actualTurnProgressState.ReferenceToPhase == TurnPhase.Play;
    public int HandCount(int s) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Count;
    public int MaximumHp(int s) => matchParticipantsState.ReferenceToPlayers[s].MaxHp;
    public int Hp(int s) => matchParticipantsState.ReferenceToPlayers[s].Hp;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public string Instance(int s, string skill) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[s], skill);
    public bool Yin(int owner, string instance) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(owner, MouXunYuAnniversaryRuntime.Shimou, instance, "yin");
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public SkillPromptPresentation Presentation(string id)
    {
        var s = getContentRegistry().GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }

    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<int> Living() => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    public bool SameChoice(PromptChoice a, PromptChoice b) => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([a], [b]);
    public IReadOnlyList<MouXyCard> Hand(int s) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Where(c => !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(s, c, CardLocation.Hand(s), OwnedCardMoveIntent.Discard)).Select(c => new MouXyCard(c.Id, c.Kind, c.DisplayName)).ToArray());
    public IReadOnlyList<MouXyPlan> Plans(ProgramSkillFrame f) => getCharacterXunYuMouRuntime().MouXyPlans(f);
    public bool Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouXunYuAnniversaryRuntime.Shimou && f.ActivationId == "adjust-and-command" && f.WindowContext is null && History<ProgramSkillStartedEvent>().Count(s => s.FrameId == f.Id && s.OwnerSeat == f.OwnerSeat && s.SkillId == f.SkillId && s.ActivationId == f.ActivationId) == 1;
        var at = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (at < 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        if (w.Window == SkillProgramTriggerWindow.GameStarting)
            return f.SkillId == MouXunYuAnniversaryRuntime.Shimou && f.TriggerId == "opening-state" && getFrameStore()[at - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } p && p.Id == w.ParentFrameId;
        return f.SkillId == MouXunYuAnniversaryRuntime.Bizuo && w.Window == SkillProgramTriggerWindow.AfterTurnEnded && getFrameStore()[at - 1] is DeferredTurnEndFrame p2 && p2.Id == w.ParentFrameId && getCoreGameEngineAfterTurnEndedProgramsRuntime().IsActualAfterTurnEndedParent(p2) && w.SourceSeat == p2.OwnerSeat && w.TargetSeat == p2.OwnerSeat && getCoreGameEngineAfterTurnEndedProgramsRuntime().AfterTurnEndedCandidate(p2)is { } c && c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.BindingId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash && (f.TriggerId is not ("allocate-own" or "allocate-otherLiving") || getCharacterXunYuMouRuntime().IsIssuedMouXyAllocationCandidate(c, w));
    }

    public MouXyBizuoGrant? PendingLease => actualTurnProgressState.ReferenceToTurnProgression.MouXyBizuoPending;

    public MouXyBizuoGrant? AllocationLease(ProgramSkillFrame f) => getCharacterXunYuMouRuntime().MouXyAllocationLease(f);
    public IReadOnlyList<MouXyAllocationCard> AllocationCards(ProgramSkillFrame f) => getCharacterXunYuMouRuntime().MouXyAllocationCards(f);
    public bool AllocationDue(int owner) => getCharacterXunYuMouRuntime().MouXyAllocationsDue().Any(x => x.Grant.Source.OwnerSeat == owner);
    public bool ExtraTurnStarted(TurnStartedEvent started, MouXyBizuoGrant g) => actualTurnProgressState.ReferenceToTurnProgression.Kind == ActualTurnKind.Extra && actualTurnProgressState.ReferenceToCurrentSeat == g.Beneficiary && started.ActorSeat == g.Beneficiary && started.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && g.IssuedTurn < started.TurnNumber && History<MouXyBizuoGrantedEvent>().Count(x => x.Grant == g) == 1 && History<ProgramExtraTurnPendedEvent>().LastOrDefault()is { } last && last.FrameId == g.FrameId && last.SkillId == g.Source.SkillId && last.Seat == g.Beneficiary && g.GameplayHash == Program(g.Source.SkillId)?.GameplayHash && !History<MouXyBizuoStartedEvent>().Any(x => x.Grant.FrameId == g.FrameId);
    public bool NonDiscardEntry(CardMovedEvent move, out int cardId, out long sequence)
    {
        cardId = 0;
        sequence = 0;
        if (move.To != CardLocation.DiscardPile)
            return false;
        var actual = cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(m => m.CardId == move.CardId && m.From == move.From && m.To == move.To && m.Reason == move.Reason);
        if (actual is null || getCoreGameEngineProgramDiscardOriginsRuntime().GetProgramDiscardSource(actual)is not null)
            return false;
        cardId = actual.CardId;
        sequence = actual.Sequence;
        return true;
    }
}
