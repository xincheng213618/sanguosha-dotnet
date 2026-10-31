#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardMovementProgramsRuntime
{
    internal CardMovementTiming? CaptureMovementTiming() => !(_coreProgramShanheSourceZhaoranOperations.HasShanheSourceZhaoranRules || _characterSimaYiOlGod2016Operations.Value.HasOl2016SimaRenjie || _characterZhouFeiLeOperations.HasLeZhouFeiLingkongCapability || _characterZhaoZhongOlOperations.HasZhaoZhongCapability || _characterFaZhengMouMobileOperations.Value.HasMouFaZhengCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesGainPhaseQualification || _coreGainGiftProgramsOperations.TracksRedOwnedLoss || _coreActualHandGainProgramsOperations.HasActualHandGainCapability || _coreShanheChanrangEdictOperations.Value.HasShanheChanrangCapability || _characterHuCheErAnniversaryOperations.HasAnniversaryHuCheErRules || _characterQiaoRuiAnniversaryOperations.Value.HasAnniversaryQiaoRuiCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveAfterBatchGain) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.BindOwnTurnEnemyHandGain) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GainHappyHuaTuoMedicine) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryMuShunShizhao) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGuyingAtomicLossChoice)) && !_coreJudgedRankSlashPoliciesOperations.HasOtherActualBasicDiscardCapability ? null : _programPhaseScheduleOperations._programPhaseSchedule is { Phase: TurnPhase.Draw } scheduled ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, TurnPhase.Draw, scheduled.Frame.ZhangShiPingHongji is { Stage: HongjiStage.ExtraPhase } hongji ? hongji.RecipientSeat : scheduled.Frame.OwnerSeat) : new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat);
    internal static bool IsGainPhaseQualified(SkillProgramTrigger trigger, int ownerSeat, CardMovementTiming? timing) => RuntimeSkills.RuntimeRules_Core_CardMovementPrograms.IsGainPhaseQualified(trigger, ownerSeat, timing);
    internal CardMovementBatchBuilder BeginCardMovementBatch(IEnumerable<CardLocation> sources, IEnumerable<CardLocation> destinations) => _cardMovementBatchCoordinatorOperations.Get().Begin(sources, destinations);
    internal void CompleteCardMovementBatch(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements, bool committed) => _cardMovementBatchCoordinatorOperations.Get().Complete(batch, movements, committed);
    int ICardMovementBatchFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    bool ICardMovementBatchFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    Winner ICardMovementBatchFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus ICardMovementBatchFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    ProgramSkillFrame? ICardMovementBatchFacts.LastProgram => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault();

    void ICardMovementBatchPipeline.Begin(CardMovementBatchBuilder batch) => _programPipelineHooksOperations._programPipelineHooks.BeginMovementBatch(batch);
    void ICardMovementBatchPipeline.ObserveCommitted(CardMovementBatchBuilder batch, CardMovementRecord movement) => _programPipelineHooksOperations._programPipelineHooks.ObserveCommittedMovement(batch, movement);
    void ICardMovementBatchPipeline.CaptureFacts(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _programPipelineHooksOperations._programPipelineHooks.CaptureMovementBatchFacts(batch, movements);
    CardMovementBatchContext ICardMovementBatchPipeline.Complete(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _programPipelineHooksOperations._programPipelineHooks.CompleteMovementBatch(batch, completed);
    internal bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = null, int? subjectSeat = null) => _cardsMovedWindowOperations.Get().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectCardsMovedProgramCandidates(CardMovementBatchContext batch) => _cardsMovedWindowOperations.Get().CollectCardsMovedProgramCandidates(batch);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectDiscardPileReceivedCandidates(CardMovementBatchContext batch) => _cardsMovedWindowOperations.Get().CollectDiscardPileReceivedCandidates(batch);
    internal int[] MatchingDiscardPileIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _cardsMovedWindowOperations.Get().MatchingDiscardPileIndexes(batch, candidate, trigger);
    internal void ClaimProgramMovedCards(ProgramSkillFrame frame) => _cardsMovedWindowOperations.Get().ClaimProgramMovedCards(frame);
    internal int[] MatchingMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _cardsMovedWindowOperations.Get().MatchingMovementIndexes(batch, candidate, trigger, location);
    internal int[] MatchingOwnerBatchMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _cardsMovedWindowOperations.Get().MatchingOwnerBatchMovementIndexes(batch, candidate, trigger);
    internal SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(CharacterState owner, int movedCardCount, CardMovementSourceCount sourceCount, SkillProgramTriggerWindow window, CardMovementTiming? timing = null, CardMovementBatchContext? nativeDrawBatch = null) => _cardsMovedWindowOperations.Get().CaptureCardsMovedTriggerFacts(owner.Seat, movedCardCount, sourceCount, window, timing, nativeDrawBatch);
    internal ProgramSkillWindowContext CreateCardsMovedProgramContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _cardsMovedWindowOperations.Get().CreateCardsMovedProgramContext(frame, candidate);
    [NativeFrameRoute(typeof(CardsMovedTriggerWindowFrame), 400)]
    internal void ContinueCardsMovedProgramWindowCore()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardsMovedTriggerWindowFrame frame && _coreWenhePoisonOperations.TryAdvanceWenhePoisonLoss(frame))
            return;
        ProgramTriggerWindowDriver.Run(_kernelCoreGameEngineProgramTriggerWindowsOperations.CreateCardsMovedTriggerWindowHost());
    }

    internal void CompleteCardsMovedProgramWindow(CardsMovedTriggerWindowFrame frame) => _cardsMovedWindowOperations.Get().CompleteCardsMovedProgramWindow(frame);
    internal void AdvanceCardsMovedProgramCandidate(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _cardsMovedWindowOperations.Get().AdvanceCardsMovedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceCardsMovedProgramCursor(CardsMovedTriggerWindowFrame frame) => _programWindowCursorOperations.Get().AdvanceCardsMovedProgramCursor(frame);
    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 700)]
    internal void CaptureMovementBatchStartMovementTiming(CardMovementBatchBuilder batch) => batch.MovementTiming = CaptureMovementTiming();
}
