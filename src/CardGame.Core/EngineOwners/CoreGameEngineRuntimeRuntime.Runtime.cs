#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRuntimeRuntime
{
    internal void AdvanceRuntimeTop<TFrame>()
        where TFrame : ResolutionFrame => _nativeFrameRoutingOperations.Get().AdvanceTop<TFrame>();
    internal void AdvanceRuntimeProgram(long frameId) => _nativeFrameRoutingOperations.Get().AdvanceProgram(frameId);
    internal void AdvanceRuntimeFrame(long frameId) => _nativeFrameRoutingOperations.Get().AdvanceFrame(frameId);
    internal void PushRuntimeFrame(ResolutionFrame frame) => _resolutionSchedulerOperations._resolutionScheduler.Push(frame);
    internal void ReplaceRuntimeFrame(long expectedFrameId, ResolutionFrame next) => _resolutionSchedulerOperations._resolutionScheduler.Replace(expectedFrameId, next);
    internal void ReplaceRuntimeTop(ResolutionFrame next) => _resolutionSchedulerOperations._resolutionScheduler.ReplaceTop(next);
    internal void PopResolutionFrame(long frameId, ResolutionFrameKind expectedKind) => _resolutionSchedulerOperations._resolutionScheduler.Complete(frameId, expectedKind);
    internal void AdvanceOneStepCore() => _matchExecutionLoopOperations._matchExecutionLoop.AdvanceOneStep();
    internal void RunOneEngineStep() => _matchStepDispatcherOperations.Get().Run();
    int IMatchStepFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    DyingFrame? IMatchStepFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    bool IMatchStepFacts.HasDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null;

    bool IMatchStepFacts.HasGroupRecovery => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.Recovery };

    bool IMatchStepFacts.SetupComplete => _coreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._setupComplete;

    bool IMatchStepFacts.GameStartingProgramsResolved => _coreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._gameStartingProgramsResolved;

    TurnPhase IMatchStepFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    int IMatchStepFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int IMatchStepFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int IMatchStepFacts.StrategicEndPlayTurn => _coreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn;

    bool IMatchStepFacts.UseInteractiveDiscard => _coreMatchDefinitionOptionsAccess._options.UseInteractiveDiscard;

    int IMatchStepFacts.Seed => _coreMatchDefinitionOptionsAccess._options.Seed;

    Winner IMatchStepFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    bool IMatchStepFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    MatchStepPlayer IMatchStepFacts.Player(int seat) => new(_coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman);
    string IMatchStepFacts.GeneralId(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].General.Id;
    bool IMatchStepProtocols.TryAdvanceAutomatic(NativeFrameAutomaticPhase phase) => _nativeFrameAutomaticOperations.Get().TryAdvance(phase);
    bool IMatchStepProtocols.TryBeginOpening() => _modeRuntimeOperations._modeRuntime.TryBeginOpening();
    bool IMatchStepProtocols.TryAdvanceHook(ProgramRuntimePhase phase) => _coreProgramRuntimeHooksOperations.RuntimeHooks.TryAdvance(phase);
    bool IMatchStepProtocols.HasTypedResponseCompletionHealthCursor() => _coreTypedRecoveryDamageCursorOperations.ProjectTypedResponseCompletionHealthCursor()is { Owner: not null, IsMalformed: false };
    bool IMatchStepProtocols.HasTypedCommittedSlashPaymentCursor() => _coreTypedRecoveryDamageCursorOperations.ProjectTypedCommittedSlashPaymentCursor()is { Owner: not null, IsMalformed: false };
    bool IMatchStepProtocols.IsCurrentWenhePoisonDying() => _coreWenhePoisonOperations.IsCurrentWenhePoisonDying();
    bool IMatchStepProtocols.HasTypedRecoveryProgramDying() => _coreTypedRecoveryDamageCursorOperations.HasTypedRecoveryProgramDying();
    bool IMatchStepProtocols.HasRuntimeDyingProof() => _coreProgramRuntimeHooksOperations.RuntimeHooks.IsDying(ProgramDyingProofPhase.Runtime);
    void IMatchStepProtocols.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMatchStepProtocols.RunDyingStep() => _coreNativeDyingOperations.RunOneDyingStep();
    void IMatchStepProtocols.RunDamageTriggerStep() => _coreNativeDamageWindowsOperations.RunOneDamageTriggerStep();
    void IMatchStepProtocols.RunGroupRecoveryStep() => _coreNativeGroupRecoveryOperations.RunOneGroupRecoveryStep();
    void IMatchStepProtocols.RunSetupStep() => _coreMatchParticipantSetupOperations.RunOneSetupStep();
    bool IMatchStepProtocols.BeginGameStartingPrograms() => _coreParticipantAndReserveProgramsOperations.BeginGameStartingPrograms();
    bool IMatchStepDecisions.Pending(MatchAutomaticDecision kind) => kind switch
    {
        MatchAutomaticDecision.RecoveryReplacement => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.RecoveryReplacement } recovery && !_coreMatchParticipantsPlayersAccess._players[recovery.PlayerSeat].IsHuman,
        MatchAutomaticDecision.ProgramTrigger => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } trigger && !_coreMatchParticipantsPlayersAccess._players[trigger.PlayerSeat].IsHuman,
        MatchAutomaticDecision.ProgramTopReorder => _coreProgramTopReorderOperations.IsAiProgramTopReorderPending(),
        MatchAutomaticDecision.ProgramJudgment => _coreJudgmentProgramsOperations.IsAiProgramJudgmentPending(),
        MatchAutomaticDecision.ProgramRepeatJudgment => _coreProgramRepeatedJudgmentOperations.IsAiProgramRepeatJudgmentPending(),
        MatchAutomaticDecision.ProgramJudgmentReplacement => _coreJudgmentReplacementProgramsOperations.IsAiProgramJudgmentReplacementPending(),
        MatchAutomaticDecision.Yingbo => _characterLuMengMouOperations.IsAiYingboPending(),
        MatchAutomaticDecision.SkipDiscardPolicy => _coreNativePromptBoundaryOperations.IsAiSkipDiscardPolicyPending(),
        MatchAutomaticDecision.Pindian => _coreSkillPromptsOperations.IsAiPindianPending(),
        MatchAutomaticDecision.TargetCardSelection => _coreNativePromptBoundaryOperations.IsAiTargetCardSelectionPending(),
        MatchAutomaticDecision.FireAttack => _coreNativePromptBoundaryOperations.IsAiFireAttackPending(),
        MatchAutomaticDecision.Nullification => _coreNativePromptBoundaryOperations.IsAiNullificationPending(),
        MatchAutomaticDecision.Response => _coreNativePromptBoundaryOperations.IsAiResponsePending(),
        MatchAutomaticDecision.DyingResponse => _coreNativePromptBoundaryOperations.IsAiDyingResponsePending(),
        MatchAutomaticDecision.Harvest => _coreNativePromptBoundaryOperations.IsAiHarvestPending(),
        _ => throw new ArgumentOutOfRangeException("kind")};
    void IMatchStepDecisions.Resolve(MatchAutomaticDecision kind)
    {
        switch (kind)
        {
            case MatchAutomaticDecision.RecoveryReplacement:
                _coreRecoveryReplacementOperations.ResolveAiRecoveryReplacement();
                break;
            case MatchAutomaticDecision.ProgramTrigger:
                _coreProgramLifecycleOperations.ResolvePendingAiProgramTrigger();
                break;
            case MatchAutomaticDecision.ProgramTopReorder:
                _coreProgramTopReorderOperations.ResolvePendingAiProgramTopReorder();
                break;
            case MatchAutomaticDecision.ProgramJudgment:
                _coreJudgmentProgramsOperations.ResolvePendingAiProgramJudgment();
                break;
            case MatchAutomaticDecision.ProgramRepeatJudgment:
                _coreProgramRepeatedJudgmentOperations.ResolvePendingAiProgramRepeatJudgment();
                break;
            case MatchAutomaticDecision.ProgramJudgmentReplacement:
                _coreJudgmentReplacementProgramsOperations.ResolvePendingAiProgramJudgmentReplacement();
                break;
            case MatchAutomaticDecision.Yingbo:
                _characterLuMengMouOperations.ResolvePendingAiYingbo();
                break;
            case MatchAutomaticDecision.SkipDiscardPolicy:
                _coreNativeAiDecisionsOperations.ResolvePendingAiSkipDiscardPolicy();
                break;
            case MatchAutomaticDecision.Pindian:
                _corePindianOperations.ResolvePendingAiPindian();
                break;
            case MatchAutomaticDecision.TargetCardSelection:
                _coreNativeAiDecisionsOperations.ResolvePendingAiTargetCardSelection();
                break;
            case MatchAutomaticDecision.FireAttack:
                _coreNativeAiDecisionsOperations.ResolvePendingAiFireAttack();
                break;
            case MatchAutomaticDecision.Nullification:
                _coreNativeAiDecisionsOperations.ResolvePendingAiNullification();
                break;
            case MatchAutomaticDecision.Response:
                _coreNativeAiDecisionsOperations.ResolvePendingAiResponse();
                break;
            case MatchAutomaticDecision.Harvest:
                _coreNativeAiDecisionsOperations.ResolvePendingAiHarvest();
                break;
            default:
                throw new ArgumentOutOfRangeException("kind");
        }
    }

    bool IMatchStepDecisions.TryAdvanceRequest(MatchSpecialRequest kind) => kind switch
    {
        MatchSpecialRequest.Huanpei => _characterHuanShuJunOlOperations.TryAdvanceHuanpeiAi(),
        MatchSpecialRequest.Zonghu => _characterLiuZhangOlOperations.TryAdvanceZonghuAi(),
        MatchSpecialRequest.BoundaryLiuFeng => _characterLiuFengBoundaryOperations.TryAdvanceBoundaryLiuFengAi(),
        MatchSpecialRequest.Weijie => _characterGuoTuOlOperations.TryAdvanceWeijieAi(),
        MatchSpecialRequest.RequestedDeckBasic => _coreRequestedDeckBasicsOperations.TryAdvanceRequestedDeckBasicAi(),
        MatchSpecialRequest.DrawFundedDistinctBasic => _coreDrawFundedDistinctBasicResponsesOperations.TryAdvanceDrawFundedDistinctBasicAi(),
        MatchSpecialRequest.ChainedStateBasic => _coreChainedStateBasicConversionsOperations.TryAdvanceChainedStateBasicAi(),
        _ => throw new ArgumentOutOfRangeException("kind")};
    void IMatchStepDecisions.ContinueDeclaration(long frameId) => _coreCardDeclarationsOperations.ContinueCardDeclarationChallenge(frameId);
    void IMatchStepDecisions.AnswerDeclaration(PromptChoice choice) => _coreCardDeclarationsOperations.AnswerCardDeclaration(choice);
    void IMatchStepTurns.ClearStrategicEndPlay() => _coreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn = -1;
    void IMatchStepTurns.CompletePlayPhase() => _coreProgramLifecycleOperations.CompleteCurrentPlayPhase();
    void IMatchStepTurns.BeginTurn() => _coreActualTurnsOperations.BeginTurn();
    void IMatchStepTurns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void IMatchStepTurns.RequestHumanPlay() => _coreNativePromptBoundaryOperations.RequestHumanPlay();
    void IMatchStepTurns.RunAiPlayDecision(int seat) => _coreNativeAiDecisionsOperations.RunOneAiPlayDecision(_coreMatchParticipantsPlayersAccess._players[seat]);
    int IMatchStepTurns.RequiredDiscard(int seat) => _coreTurnHandCategoryRestrictionsOperations.RequiredSelfHandLimitDiscard(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IMatchStepTurns.RequestHumanDiscard(int seat) => _coreDiscardOperations.RequestHumanDiscard(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IMatchStepTurns.AutoDiscardAndTryBeginEndedWindow(int seat)
    {
        // A native encounter may replace a seat while discard effects run.
        // The ended window still belongs to this exact original participant.
        var current = _coreMatchParticipantsPlayersAccess._players[seat];
        _coreActualDiscardPhaseOperations.AutoDiscard(current);
        return _coreProgramLifecycleOperations.TryBeginDiscardPhaseEndedProgramWindow(current);
    }

    internal ProgramCompletionRuntime ProgramCompletion => _programCompletionOperations.Get();

    internal void CompleteRuntimeProgramBinding(ProgramSkillFrame frame, bool completed) => ProgramCompletion.CompleteBinding(frame, completed);
    internal void ReturnRuntimeProgramMovement(long frameId) => ProgramCompletion.ReturnMovement(frameId);
    Winner IProgramCompletionFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus IProgramCompletionFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    int IProgramCompletionFacts.TriggerPriority(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority;
    bool IProgramCompletionFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IProgramCompletionFacts.HasSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    bool IProgramCompletionFacts.CanContinueEquipmentSuit(ProgramSkillFrame frame) => _characterHeQiOlOperations.CanContinueEquipmentSuitProgram(frame);
    bool IProgramCompletionFacts.CanContinueHpConditional(ProgramSkillFrame frame) => _coreHpConditionalSkillGrantsOperations.CanContinueHpConditionalProgram(frame);
    bool IProgramCompletionFacts.CanContinuePhaseAlcohol(ProgramSkillFrame frame) => _coreForcedPhaseAlcoholOperations.CanContinuePhaseAlcohol(frame);
    bool IProgramCompletionFacts.ValidDamageTargetObtain(ProgramSkillFrame frame) => _coreDamageTargetObtainProgramsOperations.IsValidDamageTargetObtain(frame);
    bool IProgramCompletionFacts.ValidDamageTargetMount(ProgramSkillFrame frame) => _coreDamageTargetMountProgramsOperations.IsValidDamageTargetMount(frame);
    int IProgramCompletionFacts.LivingInAttackRange(int seat) => _coreProgramCardPaymentsOperations.CountLivingInAttackRange(seat);
    bool IProgramCompletionProtocols.CompleteKeming(ProgramSkillFrame frame, bool completed) => _characterCaoPiAnniversaryGodOperations.TryCompleteKemingProgram(frame, completed);
    bool IProgramCompletionProtocols.CompleteMantianGift(ProgramSkillFrame frame, bool completed) => _coreMantianGuohaiOperations.TryCompleteMantianGuohaiGift(frame, completed);
    bool IProgramCompletionProtocols.CompleteShenwuTransplant(ProgramSkillFrame frame, bool completed) => _coreShenwuCardsOperations.TryCompleteShenwuTransplantProgram(frame, completed);
    bool IProgramCompletionProtocols.CompleteHellSoul(ProgramSkillFrame frame, bool completed) => _coreHellSoulCardsOperations.TryCompleteHellSoulProgram(frame, completed);
    bool IProgramCompletionProtocols.CompleteWenheChaos(ProgramSkillFrame frame, bool completed) => _coreWenheChaosOperations.TryCompleteWenheChaosProgram(frame, completed);
    void IProgramCompletionProtocols.CompleteGame() => _kernelCoreMainOperations.CompleteGame();
    void IProgramCompletionProtocols.FinishPaidOwnTarget(ProgramSkillFrame frame) => _corePaidOwnTargetObserversOperations.FinishPaidOwnTargetBeforeProgramCompletion(frame);
    void IProgramCompletionProtocols.FinishOwnedDeathBenefit(ProgramSkillFrame frame, bool completed) => _coreOwnedDeathBenefitReturnsOperations.FinishOwnedDeathBenefitReturn(frame, completed);
    bool IProgramCompletionProtocols.CompleteDeferredTurnEnd(ProgramSkillFrame frame, ProgramSkillWindowContext context) => _coreDeferredHandAlignmentsOperations.CompleteDeferredTurnEndBinding(frame, context);
    bool IProgramCompletionProtocols.ReturnCompletedBinding(ProgramSkillFrame frame, bool completed) => _programPipelineHooksOperations._programPipelineHooks.ReturnCompletedProgramBinding(frame, completed);
    void IProgramCompletionProtocols.CompleteFirstLegacyCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreFirstSlashDuelLegacyWindowsOperations.CompleteFirstLegacyBenefitCandidate(candidate, context);
    void IProgramCompletionProtocols.ReturnWindow(ProgramSkillFrame frame, bool completed) => _nativeCardGameCoreProgramWindowReturnCatalogOperations.Return(CoreProgramWindowReturnsDefinitions.ProgramWindowReturns.Value, frame, completed);
    bool IProgramCompletionProtocols.TryReturnMovement(ProgramSkillFrame frame, bool afterPayment) => _kernelCoreGameEngineProgramContinuationsOperations.TryReturnMovement(CoreGameEngineProgramContinuationsDefinitions.ProgramContinuations.Value, frame, afterPayment);
    void IProgramCompletionProtocols.CancelBindingAndCleanup(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    void IProgramCompletionProtocols.SetAttackRangeCoverage(long frameId, string bind, int subjectSeat, int before, int after) => _coreProgramCardPaymentsOperations.SetProgramAttackRangeCoverage(frameId, bind, subjectSeat, before, after);
    void IProgramCompletionProtocols.ContinueRepeatedJudgment(long frameId) => _coreProgramRepeatedJudgmentOperations.ContinueProgramRepeatedJudgmentAfterMovement(frameId);
}
