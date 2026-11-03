#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    internal MouXiaoQiaoOlRuntime MouXiaoQiaoOl => _mouXiaoQiaoOlOperations.Get();



    int IMouXiaoQiaoOlFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;


    int IMouXiaoQiaoOlFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    TurnPhase IMouXiaoQiaoOlFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    Winner IMouXiaoQiaoOlFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IMouXiaoQiaoOlFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    long IMouXiaoQiaoOlFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouXiaoQiaoOlFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouXiaoQiaoOlFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string IMouXiaoQiaoOlFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    SkillProgram IMouXiaoQiaoOlFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    IEnumerable<T> IMouXiaoQiaoOlFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    IReadOnlyList<Card> IMouXiaoQiaoOlFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<Card> IMouXiaoQiaoOlFacts.Equipment(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    Card IMouXiaoQiaoOlFacts.Card(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
    CardLocation IMouXiaoQiaoOlFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    CardKind IMouXiaoQiaoOlFacts.EffectiveKind(int seat, int id) => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id));
    Suit IMouXiaoQiaoOlFacts.EffectiveSuit(int seat, int id) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id));
    bool? IMouXiaoQiaoOlFacts.EffectiveRed(int seat, int id) => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)));
    IReadOnlyList<CardMovementRecord> IMouXiaoQiaoOlFacts.Movements(long before, long after) => Array.AsReadOnly(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray());
    IReadOnlyList<MouXiaoQiaoOlUsePlan> IMouXiaoQiaoOlFacts.NativePlans(int seat, CardKind kind, CardConversionSource source) => MouXqNativePlans(_coreMatchParticipantsPlayersAccess._players[seat], kind, source);
    IReadOnlyList<CardConversionSource> IMouXiaoQiaoOlFacts.RetainedSources(int seat) => MouXqRetainedSources(_coreMatchParticipantsPlayersAccess._players[seat]);
    SkillPromptPresentation IMouXiaoQiaoOlFacts.SkillPrompt(string id)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(id);
        return new(id, s.Name, s.Name, s.Description);
    }

    double IMouXiaoQiaoOlFacts.TargetValue(int owner, int target, bool helpful) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, helpful ? new(0, 0, 0, 0, 1, 0, true, false) : new(0, 0, 1, 0, 0, 0, false, false));
    bool IMouXiaoQiaoOlFacts.IsResponseRequest(ProgramSkillFrame f) => IsMouXqJueyanResponseRequest(f);
    bool IMouXiaoQiaoOlFacts.ActorLineage(long useId, long actionId, int originalActor, int actor, int provider, CardActionType type) => _characterZhouYuOlMouOperations.MouGuquActorLineageMatches(useId, actionId, originalActor, actor, provider, type);
    bool IMouXiaoQiaoOlFacts.GiftRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouXiaoQiaoOl is not { Stage: MouXiaoQiaoOlStage.GiftChildren, Paid: true } r || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiaoQiaoMiluoGiftEvent>().LastOrDefault(x => x.FrameId == f.Id && x.Kind == CardKind.SilverLion && x.From == CardLocation.Equipment(f.OwnerSeat))is not { } paid || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.CardKind == CardKind.SilverLion && m.From == paid.From && m.To == paid.To && m.Reason.Value == MouXiaoQiaoOlRuntime.GiveReason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouXiaoQiaoOlRuntime.GiveReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.ResolutionId > f.Id && x.ResolutionId < hp.Change.Id && x.PlayerSeat == f.OwnerSeat && x.Reason.Value == MouXiaoQiaoOlRuntime.GiveReason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter);
    }

    void IMouXiaoQiaoOlOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouXiaoQiaoOlOperations.Reveal(ProgramSkillFrame f, IReadOnlyList<int> ids) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, "mou-xiao-qiao-public-hand", Array.AsReadOnly(ids.Select(id => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
    bool IMouXiaoQiaoOlOperations.BeginRevealChildren(long id) => _publicHandRevealWindows.Begin(id, PublicHandRevealReturnKind.Program);
    void IMouXiaoQiaoOlOperations.Gift(ProgramSkillFrame f, int id, int target) => _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _cardZonesOperations._cardZones.GetLocation(id), CardLocation.Hand(target), new(MouXiaoQiaoOlRuntime.GiveReason), _nativeGivenCards.Completion(f.Id, f.OwnerSeat, target, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
    void IMouXiaoQiaoOlOperations.Recover(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
    void IMouXiaoQiaoOlOperations.LoseHp(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
    bool IMouXiaoQiaoOlOperations.BeginRecovery(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    bool IMouXiaoQiaoOlOperations.BeginCharacterState(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    bool IMouXiaoQiaoOlOperations.BeginHpChanged(long id) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    bool IMouXiaoQiaoOlOperations.BeginCardsMoved(long id) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    bool IMouXiaoQiaoOlOperations.BeginSkillsChanged(long id) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void IMouXiaoQiaoOlOperations.IssueNativeUse(ProgramSkillFrame f, MouXiaoQiaoOlUsePlan p) => IssueMouXqNativeUse(f, p);
    void IMouXiaoQiaoOlOperations.SignNativeBatches(ProgramSkillFrame f, CardUseFrame use) => SignMouXqNativeBatches(f, use);
    bool IMouXiaoQiaoOlOperations.BeginResponse(PromptChoice choice) => BeginMouXqJueyanResponse(choice);
    void IMouXiaoQiaoOlOperations.ReturnResponse(ProgramSkillFrame f) => ReturnMouXqJueyanResponse(f);
}
