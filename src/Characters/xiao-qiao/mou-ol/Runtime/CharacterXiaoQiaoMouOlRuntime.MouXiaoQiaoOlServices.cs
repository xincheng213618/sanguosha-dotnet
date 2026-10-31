#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    internal MouXiaoQiaoOlRuntime MouXiaoQiaoOl => _mouXiaoQiaoOlOperations.Get();

    internal bool TryBeginPublicHandRevealWindow(long parentId, PublicHandRevealReturnKind returned) => MouXiaoQiaoOl.BeginPublicReveal(parentId, returned);
    internal bool TryBeginPublicHandRevealWindow(long parentId, string key) => MouXiaoQiaoOl.BeginPublicReveal(parentId, PublicHandRevealReturnKind.Registered, key);
    internal bool IsPublicHandRevealProducerSuspended(long parentId, string key) => MouXiaoQiaoOl.RevealProducerSuspended(parentId, key);
    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.Enabled => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouXiaoQiaoJueyanReveal);

    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    int global::CardGame.Core.IMouXiaoQiaoOlFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int global::CardGame.Core.IMouXiaoQiaoOlFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IMouXiaoQiaoOlFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    TurnPhase global::CardGame.Core.IMouXiaoQiaoOlFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    Winner global::CardGame.Core.IMouXiaoQiaoOlFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IMouXiaoQiaoOlFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    long global::CardGame.Core.IMouXiaoQiaoOlFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IMouXiaoQiaoOlFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    SkillProgram global::CardGame.Core.IMouXiaoQiaoOlFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.HasSource(CardConversionSource s) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[s.OwnerSeat], s.SkillId, s.SkillInstanceId!);
    IEnumerable<T> global::CardGame.Core.IMouXiaoQiaoOlFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<Card> global::CardGame.Core.IMouXiaoQiaoOlFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<Card> global::CardGame.Core.IMouXiaoQiaoOlFacts.Equipment(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    Card global::CardGame.Core.IMouXiaoQiaoOlFacts.Card(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
    CardLocation global::CardGame.Core.IMouXiaoQiaoOlFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    CardKind global::CardGame.Core.IMouXiaoQiaoOlFacts.EffectiveKind(int seat, int id) => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id));
    Suit global::CardGame.Core.IMouXiaoQiaoOlFacts.EffectiveSuit(int seat, int id) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id));
    bool? global::CardGame.Core.IMouXiaoQiaoOlFacts.EffectiveRed(int seat, int id) => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)));
    IReadOnlyList<CardMovementRecord> global::CardGame.Core.IMouXiaoQiaoOlFacts.Movements(long before, long after) => Array.AsReadOnly(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray());
    IReadOnlyList<MouXiaoQiaoOlUsePlan> global::CardGame.Core.IMouXiaoQiaoOlFacts.NativePlans(int seat, CardKind kind, CardConversionSource source) => MouXqNativePlans(_coreMatchParticipantsPlayersAccess._players[seat], kind, source);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IMouXiaoQiaoOlFacts.Sources(int seat, string skillId) => Array.AsReadOnly(_coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramTriggerWindow.PublicHandRevealed).Where(c => c.SkillId == skillId).ToArray());
    SkillPromptPresentation global::CardGame.Core.IMouXiaoQiaoOlFacts.SkillPrompt(string id)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(id);
        return new(id, s.Name, s.Name, s.Description);
    }

    double global::CardGame.Core.IMouXiaoQiaoOlFacts.KeepValue(int seat, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[seat]);
    double global::CardGame.Core.IMouXiaoQiaoOlFacts.TargetValue(int owner, int target, bool helpful) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, helpful ? new(0, 0, 0, 0, 1, 0, true, false) : new(0, 0, 1, 0, 0, 0, false, false));
    (long FrameId, IReadOnlyList<int> CardIds) global::CardGame.Core.IMouXiaoQiaoOlFacts.PublicReveal(IGameEvent p) => p switch
    {
        CardsRevealedEvent x => (x.ResolutionId, x.Cards.Select(c => c.Id).ToArray()),
        ProgramCardsRevealedEvent x => (x.FrameId, x.Cards.Select(c => c.Id).ToArray()),
        FireAttackCardRevealedEvent x => (x.ResolutionId, [x.CardId]),
        ProgramHandComparisonCardRevealedEvent x => (x.FrameId, [x.CardId]),
        ProgramHandComparisonSourceRevealedEvent x => (x.FrameId, [x.CardId]),
        ProgramHandDraftRevealedEvent x => (x.FrameId, _characterWangGuanOlOperations.MiuyanDraftHandIds(x).ToArray()),
        ProgramQingliangRevealEvent x => (x.FrameId, _characterWangGuanOlOperations.MiuyanQingliangHandIds(x).ToArray()),
        _ => (0, Array.Empty<int>())};
    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.IsResponseRequest(ProgramSkillFrame f) => IsMouXqJueyanResponseRequest(f);
    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.ActorLineage(long useId, long actionId, int originalActor, int actor, int provider, CardActionType type) => _characterZhouYuOlMouOperations.MouGuquActorLineageMatches(useId, actionId, originalActor, actor, provider, type);
    bool global::CardGame.Core.IMouXiaoQiaoOlFacts.GiftRecoveryChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouXiaoQiaoOl is not { Stage: MouXiaoQiaoOlStage.GiftChildren, Paid: true } r || _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoMiluoGiftEvent>().LastOrDefault(x => x.FrameId == f.Id && x.Kind == CardKind.SilverLion && x.From == CardLocation.Equipment(f.OwnerSeat))is not { } paid || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.CardKind == CardKind.SilverLion && m.From == paid.From && m.To == paid.To && m.Reason.Value == MouXiaoQiaoOlRuntime.GiveReason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouXiaoQiaoOlRuntime.GiveReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(x => x.ResolutionId > f.Id && x.ResolutionId < hp.Change.Id && x.PlayerSeat == f.OwnerSeat && x.Reason.Value == MouXiaoQiaoOlRuntime.GiveReason && x.RecoveredAmount == hp.Change.Amount && x.RemainingHp == hp.Change.HpAfter);
    }

    void global::CardGame.Core.IMouXiaoQiaoOlOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.Reveal(ProgramSkillFrame f, IReadOnlyList<int> ids) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, "mou-xiao-qiao-public-hand", Array.AsReadOnly(ids.Select(id => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginRevealChildren(long id) => TryBeginPublicHandRevealWindow(id, PublicHandRevealReturnKind.Program);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.Gift(ProgramSkillFrame f, int id, int target) => _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _cardZonesOperations._cardZones.GetLocation(id), CardLocation.Hand(target), new(MouXiaoQiaoOlRuntime.GiveReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, target, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.Recover(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.LoseHp(ProgramSkillFrame f, int target) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginRecovery(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginCharacterState(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginHpChanged(long id) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginCardsMoved(long id) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginSkillsChanged(long id) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.IssueNativeUse(ProgramSkillFrame f, MouXiaoQiaoOlUsePlan p) => IssueMouXqNativeUse(f, p);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.SignNativeBatches(ProgramSkillFrame f, CardUseFrame use) => SignMouXqNativeBatches(f, use);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginRevealTrigger(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.BeginProgramBinding(c, context);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.ReturnRevealWindow(PublicHandRevealWindowFrame w)
    {
        if (w.ReturnKind == PublicHandRevealReturnKind.Program)
        {
            if (!_kernelCoreGameEngineProgramContinuationsOperations.TryResume(global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsDefinitions.ProgramContinuations.Value, w.ParentFrameId))
                _coreRuntimeOperations.AdvanceRuntimeProgram(w.ParentFrameId);
        }
        else if (w.ReturnKind == PublicHandRevealReturnKind.FireAttack)
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId) ?? throw new InvalidOperationException("Public reveal lost its actual FireAttack.");
            var id = use.FireAttackSelection?.RevealedCardId ?? throw new InvalidOperationException("FireAttack cannot replace its frozen revealed entity.");
            _coreNativeFireAttackOperations.BeginFireAttackDiscard(use, _coreAdvancedLifecycleOperations.GetAdvancedCard(id));
        }
        else if (w.ReturnKind == PublicHandRevealReturnKind.Registered)
            _nativeCardGameCorePublicHandRevealReturnsOperations.Return(PublicHandRevealReturns.Default, w.ReturnKey!, _resolutionStackOperations._resolutionStack.Single(x => x.Id == w.ParentFrameId));
        else
            _coreRuntimeOperations.AdvanceRuntimeFrame(w.ParentFrameId);
    }

    bool global::CardGame.Core.IMouXiaoQiaoOlOperations.BeginResponse(PromptChoice choice) => BeginMouXqJueyanResponse(choice);
    void global::CardGame.Core.IMouXiaoQiaoOlOperations.ReturnResponse(ProgramSkillFrame f) => ReturnMouXqJueyanResponse(f);
}
