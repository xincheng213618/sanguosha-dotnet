#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXiaoQiaoOlFacts(global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangGuanOlEntries> getCharacterWangGuanOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries> getCharacterZhouYuOlMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime) : IMouXiaoQiaoOlFacts
{
    public bool Enabled => getContentRegistry().ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouXiaoQiaoJueyanReveal);
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public bool HasSource(CardConversionSource s) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[s.OwnerSeat], s.SkillId, s.SkillInstanceId!);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public IReadOnlyList<Card> Equipment(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]);
    public Card Card(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public CardKind EffectiveKind(int seat, int id) => getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[seat], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id));
    public Suit EffectiveSuit(int seat, int id) => getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id));
    public bool? EffectiveRed(int seat, int id) => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id)));
    public IReadOnlyList<CardMovementRecord> Movements(long before, long after) => Array.AsReadOnly(cardMovementJournalState.ReferenceToCardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray());
    public IReadOnlyList<MouXiaoQiaoOlUsePlan> NativePlans(int seat, CardKind kind, CardConversionSource source) => getCharacterXiaoQiaoMouOlRuntime().MouXqNativePlans(matchParticipantsState.ReferenceToPlayers[seat], kind, source);
    public IReadOnlyList<ProgramTriggerCandidate> Sources(int seat, string skillId) => Array.AsReadOnly(getCoreGameEngineProgramLifecycleRuntime().CollectProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[seat], SkillProgramTriggerWindow.PublicHandRevealed).Where(c => c.SkillId == skillId).ToArray());
    public SkillPromptPresentation SkillPrompt(string id)
    {
        var s = getContentRegistry().GetSkill(id);
        return new(id, s.Name, s.Name, s.Description);
    }

    public double KeepValue(int seat, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[seat]);
    public double TargetValue(int owner, int target, bool helpful) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, helpful ? new(0, 0, 0, 0, 1, 0, true, false) : new(0, 0, 1, 0, 0, 0, false, false));
    public (long FrameId, IReadOnlyList<int> CardIds) PublicReveal(IGameEvent p) => p switch
    {
        CardsRevealedEvent x => (x.ResolutionId, x.Cards.Select(c => c.Id).ToArray()),
        ProgramCardsRevealedEvent x => (x.FrameId, x.Cards.Select(c => c.Id).ToArray()),
        FireAttackCardRevealedEvent x => (x.ResolutionId, [x.CardId]),
        ProgramHandComparisonCardRevealedEvent x => (x.FrameId, [x.CardId]),
        ProgramHandComparisonSourceRevealedEvent x => (x.FrameId, [x.CardId]),
        ProgramHandDraftRevealedEvent x => (x.FrameId, getCharacterWangGuanOlRuntime().MiuyanDraftHandIds(x).ToArray()),
        ProgramQingliangRevealEvent x => (x.FrameId, getCharacterWangGuanOlRuntime().MiuyanQingliangHandIds(x).ToArray()),
        _ => (0, Array.Empty<int>())};
    public bool ActorLineage(long useId, long actionId, int originalActor, int actor, int provider, CardActionType type) => getCharacterZhouYuOlMouRuntime().MouGuquActorLineageMatches(useId, actionId, originalActor, actor, provider, type);
    public bool IsResponseRequest(ProgramSkillFrame f) => getCharacterXiaoQiaoMouOlRuntime().IsMouXqJueyanResponseRequest(f);
    public bool GiftRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouXiaoQiaoOl is not { Stage: MouXiaoQiaoOlStage.GiftChildren, Paid: true } r || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouXiaoQiaoMiluoGiftEvent>().LastOrDefault(x => x.FrameId == f.Id && x.Kind == CardKind.SilverLion && x.From == CardLocation.Equipment(f.OwnerSeat))is not { } paid || !cardMovementJournalState.ReferenceToCardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.CardKind == CardKind.SilverLion && m.From == paid.From && m.To == paid.To && m.Reason.Value == MouXiaoQiaoOlRuntime.GiveReason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouXiaoQiaoOlRuntime.GiveReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.ResolutionId > f.Id && x.ResolutionId < hp.Change.Id && x.PlayerSeat == f.OwnerSeat && x.Reason.Value == MouXiaoQiaoOlRuntime.GiveReason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter);
    }
}
