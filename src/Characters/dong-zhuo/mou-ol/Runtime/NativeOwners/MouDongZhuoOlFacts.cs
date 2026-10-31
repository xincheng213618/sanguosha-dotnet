#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDongZhuoOlFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDongZhuoMouOlEntries> getCharacterDongZhuoMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime) : IMouDongZhuoOlFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int Round => actualRoundProgressState.ReferenceToRoundNumber;
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;
    public int PhaseId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public bool Owns(int seat, string skill) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(matchParticipantsState.ReferenceToPlayers[seat], skill);
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public int KeepValue(int seat, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[seat]);
    public bool Friendly(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    public IReadOnlyList<MouDongZhuoOlCard> Discardable(int seat) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => getCardZoneStore().CardsAt(new(z, seat)).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), OwnedCardMoveIntent.Discard)).Select(c => new MouDongZhuoOlCard(c.Id, new(z, seat), getCoreGameEngineProgramCardPoliciesRuntime().GetProgramEffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], c)))).ToArray());
    public IReadOnlyList<MouDongZhuoOlCard> CurrentDiscard() => Array.AsReadOnly(getCardZoneStore().CardsAt(CardLocation.DiscardPile).Where(c => cardMovementJournalState.ReferenceToCardMovements.Any(m => m.CardId == c.Id && m.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && m.To == CardLocation.DiscardPile)).Select(c => new MouDongZhuoOlCard(c.Id, CardLocation.DiscardPile, c.Suit)).ToArray());
    public bool ParentMatches(ProgramSkillFrame f) => getCharacterDongZhuoMouOlRuntime().MouDongZhuoOlParent(f);
    public bool ExactStart(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (f.InstructionIndex != 1 || getCoreGameEnginePrivateOfferReturnsRuntime().GetPausedPrivateOfferEffect(f)?.Op != op || Program(f.SkillId).GameplayHash != f.GameplayHash || !getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            return false;
        if (f.TriggerId is null)
            return op == SkillProgramEffectOp.ResolveMouDongZhuoFengshang && f.SkillId == MouDongZhuoOlRuntime.Fengshang && f.ActivationId == "distribute-pair" && f.WindowContext is null && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == f.OwnerSeat && getCoreGameEngineActualDiscardRecoveryPhasesRuntime().IsRecordedActualDiscardRecoveryPhase(p) && getCharacterDongZhuoMouOlRuntime().MouDongZhuoOl.CanFengshang(f.OwnerSeat, false);
        return getCharacterDongZhuoMouOlRuntime().MouDongZhuoOlParent(f) && (op switch
        {
            SkillProgramEffectOp.InitializeMouDongZhuoGuanbian => f.SkillId == MouDongZhuoOlRuntime.Guanbian && f.WindowContext!.Window == SkillProgramTriggerWindow.GameStarting,
            SkillProgramEffectOp.RemoveMouDongZhuoGuanbian => f.SkillId == MouDongZhuoOlRuntime.Guanbian && f.WindowContext!.Window == SkillProgramTriggerWindow.RoundEnding && actualRoundProgressState.ReferenceToRoundNumber == 1,
            SkillProgramEffectOp.ResolveMouDongZhuoXiongni => f.SkillId == MouDongZhuoOlRuntime.Xiongni && f.WindowContext!.Window == SkillProgramTriggerWindow.PlayPhaseStarting && f.OwnerSeat == actualTurnProgressState.ReferenceToCurrentSeat,
            SkillProgramEffectOp.ResolveMouDongZhuoFengshang => f.SkillId == MouDongZhuoOlRuntime.Fengshang && f.WindowContext!.Window == SkillProgramTriggerWindow.DyingEntering && getCharacterDongZhuoMouOlRuntime().MouDongZhuoOl.CanFengshang(f.OwnerSeat, true),
            _ => f.SkillId == MouDongZhuoOlRuntime.Zhibing && f.WindowContext!.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && f.OwnerSeat == actualTurnProgressState.ReferenceToCurrentSeat
        });
    }
}
