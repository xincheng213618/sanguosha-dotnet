#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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
    int global::CardGame.Core.IMatchStepFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    DyingFrame? global::CardGame.Core.IMatchStepFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    bool global::CardGame.Core.IMatchStepFacts.HasDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not null;

    bool global::CardGame.Core.IMatchStepFacts.HasGroupRecovery => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.Recovery };

    bool global::CardGame.Core.IMatchStepFacts.SetupComplete => _coreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._setupComplete;

    bool global::CardGame.Core.IMatchStepFacts.GameStartingProgramsResolved => _coreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess._gameStartingProgramsResolved;

    TurnPhase global::CardGame.Core.IMatchStepFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    int global::CardGame.Core.IMatchStepFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IMatchStepFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IMatchStepFacts.StrategicEndPlayTurn => _coreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn;

    bool global::CardGame.Core.IMatchStepFacts.UseInteractiveDiscard => _coreMatchDefinitionOptionsAccess._options.UseInteractiveDiscard;

    int global::CardGame.Core.IMatchStepFacts.Seed => _coreMatchDefinitionOptionsAccess._options.Seed;

    Winner global::CardGame.Core.IMatchStepFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    bool global::CardGame.Core.IMatchStepFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    MatchStepPlayer global::CardGame.Core.IMatchStepFacts.Player(int seat) => new(_coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman);
    string global::CardGame.Core.IMatchStepFacts.GeneralId(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].General.Id;
    bool global::CardGame.Core.IMatchStepProtocols.TryAdvanceAutomatic(NativeFrameAutomaticPhase phase) => _nativeFrameAutomaticOperations.Get().TryAdvance(phase);
    bool global::CardGame.Core.IMatchStepProtocols.TryBeginOpening() => _modeRuntimeOperations._modeRuntime.TryBeginOpening();
    bool global::CardGame.Core.IMatchStepProtocols.TryAdvanceHook(ProgramRuntimePhase phase) => _coreProgramRuntimeHooksOperations.RuntimeHooks.TryAdvance(phase);
    bool global::CardGame.Core.IMatchStepProtocols.HasTypedResponseCompletionHealthCursor() => _coreTypedRecoveryDamageCursorOperations.ProjectTypedResponseCompletionHealthCursor()is { Owner: not null, IsMalformed: false };
    bool global::CardGame.Core.IMatchStepProtocols.HasTypedCommittedSlashPaymentCursor() => _coreTypedRecoveryDamageCursorOperations.ProjectTypedCommittedSlashPaymentCursor()is { Owner: not null, IsMalformed: false };
    bool global::CardGame.Core.IMatchStepProtocols.IsCurrentWenhePoisonDying() => _coreWenhePoisonOperations.IsCurrentWenhePoisonDying();
    bool global::CardGame.Core.IMatchStepProtocols.HasTypedRecoveryProgramDying() => _coreTypedRecoveryDamageCursorOperations.HasTypedRecoveryProgramDying();
    bool global::CardGame.Core.IMatchStepProtocols.HasRuntimeDyingProof() => _coreProgramRuntimeHooksOperations.RuntimeHooks.IsDying(ProgramDyingProofPhase.Runtime);
    void global::CardGame.Core.IMatchStepProtocols.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IMatchStepProtocols.RunDyingStep() => _coreNativeDyingOperations.RunOneDyingStep();
    void global::CardGame.Core.IMatchStepProtocols.RunDamageTriggerStep() => _coreNativeDamageWindowsOperations.RunOneDamageTriggerStep();
    void global::CardGame.Core.IMatchStepProtocols.RunGroupRecoveryStep() => _coreNativeGroupRecoveryOperations.RunOneGroupRecoveryStep();
    void global::CardGame.Core.IMatchStepProtocols.RunSetupStep() => _coreMatchParticipantSetupOperations.RunOneSetupStep();
    bool global::CardGame.Core.IMatchStepProtocols.BeginGameStartingPrograms() => _coreParticipantAndReserveProgramsOperations.BeginGameStartingPrograms();
    bool global::CardGame.Core.IMatchStepDecisions.Pending(MatchAutomaticDecision kind) => kind switch
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
    void global::CardGame.Core.IMatchStepDecisions.Resolve(MatchAutomaticDecision kind)
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

    bool global::CardGame.Core.IMatchStepDecisions.TryAdvanceRequest(MatchSpecialRequest kind) => kind switch
    {
        MatchSpecialRequest.Huanpei => _characterHuanShuJunOlOperations.TryAdvanceHuanpeiAi(),
        MatchSpecialRequest.Zonghu => _characterLiuZhangOlOperations.TryAdvanceZonghuAi(),
        MatchSpecialRequest.BoundaryLiuFeng => _characterLiuFengBoundaryOperations.TryAdvanceBoundaryLiuFengAi(),
        MatchSpecialRequest.Weijie => _characterGuoTuOlOperations.TryAdvanceWeijieAi(),
        MatchSpecialRequest.RequestedDeckBasic => _coreRequestedDeckBasicsOperations.TryAdvanceRequestedDeckBasicAi(),
        MatchSpecialRequest.DrawFundedDistinctBasic => _coreDrawFundedDistinctBasicResponsesOperations.TryAdvanceDrawFundedDistinctBasicAi(),
        MatchSpecialRequest.ChainedStateBasic => _coreChainedStateBasicConversionsOperations.TryAdvanceChainedStateBasicAi(),
        _ => throw new ArgumentOutOfRangeException("kind")};
    void global::CardGame.Core.IMatchStepDecisions.ContinueDeclaration(long frameId) => _coreCardDeclarationsOperations.ContinueCardDeclarationChallenge(frameId);
    void global::CardGame.Core.IMatchStepDecisions.AnswerDeclaration(PromptChoice choice) => _coreCardDeclarationsOperations.AnswerCardDeclaration(choice);
    void global::CardGame.Core.IMatchStepTurns.ClearStrategicEndPlay() => _coreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn = -1;
    void global::CardGame.Core.IMatchStepTurns.CompletePlayPhase() => _coreProgramLifecycleOperations.CompleteCurrentPlayPhase();
    void global::CardGame.Core.IMatchStepTurns.BeginTurn() => _coreActualTurnsOperations.BeginTurn();
    void global::CardGame.Core.IMatchStepTurns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void global::CardGame.Core.IMatchStepTurns.RequestHumanPlay() => _coreNativePromptBoundaryOperations.RequestHumanPlay();
    void global::CardGame.Core.IMatchStepTurns.RunAiPlayDecision(int seat) => _coreNativeAiDecisionsOperations.RunOneAiPlayDecision(_coreMatchParticipantsPlayersAccess._players[seat]);
    int global::CardGame.Core.IMatchStepTurns.RequiredDiscard(int seat) => _coreTurnHandCategoryRestrictionsOperations.RequiredSelfHandLimitDiscard(_coreMatchParticipantsPlayersAccess._players[seat]);
    void global::CardGame.Core.IMatchStepTurns.RequestHumanDiscard(int seat) => _coreDiscardOperations.RequestHumanDiscard(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IMatchStepTurns.AutoDiscardAndTryBeginEndedWindow(int seat)
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
    Winner global::CardGame.Core.IProgramCompletionFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus global::CardGame.Core.IProgramCompletionFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    int global::CardGame.Core.IProgramCompletionFacts.TriggerPriority(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority;
    bool global::CardGame.Core.IProgramCompletionFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IProgramCompletionFacts.HasSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    bool global::CardGame.Core.IProgramCompletionFacts.CanContinueEquipmentSuit(ProgramSkillFrame frame) => _characterHeQiOlOperations.CanContinueEquipmentSuitProgram(frame);
    bool global::CardGame.Core.IProgramCompletionFacts.CanContinueHpConditional(ProgramSkillFrame frame) => _coreHpConditionalSkillGrantsOperations.CanContinueHpConditionalProgram(frame);
    bool global::CardGame.Core.IProgramCompletionFacts.CanContinuePhaseAlcohol(ProgramSkillFrame frame) => _coreForcedPhaseAlcoholOperations.CanContinuePhaseAlcohol(frame);
    bool global::CardGame.Core.IProgramCompletionFacts.ValidDamageTargetObtain(ProgramSkillFrame frame) => _coreDamageTargetObtainProgramsOperations.IsValidDamageTargetObtain(frame);
    bool global::CardGame.Core.IProgramCompletionFacts.ValidDamageTargetMount(ProgramSkillFrame frame) => _coreDamageTargetMountProgramsOperations.IsValidDamageTargetMount(frame);
    int global::CardGame.Core.IProgramCompletionFacts.LivingInAttackRange(int seat) => _coreProgramCardPaymentsOperations.CountLivingInAttackRange(seat);
    bool global::CardGame.Core.IProgramCompletionProtocols.CompleteKeming(ProgramSkillFrame frame, bool completed) => _characterCaoPiAnniversaryGodOperations.TryCompleteKemingProgram(frame, completed);
    bool global::CardGame.Core.IProgramCompletionProtocols.CompleteMantianGift(ProgramSkillFrame frame, bool completed) => _coreMantianGuohaiOperations.TryCompleteMantianGuohaiGift(frame, completed);
    bool global::CardGame.Core.IProgramCompletionProtocols.CompleteShenwuTransplant(ProgramSkillFrame frame, bool completed) => _coreShenwuCardsOperations.TryCompleteShenwuTransplantProgram(frame, completed);
    bool global::CardGame.Core.IProgramCompletionProtocols.CompleteHellSoul(ProgramSkillFrame frame, bool completed) => _coreHellSoulCardsOperations.TryCompleteHellSoulProgram(frame, completed);
    bool global::CardGame.Core.IProgramCompletionProtocols.CompleteWenheChaos(ProgramSkillFrame frame, bool completed) => _coreWenheChaosOperations.TryCompleteWenheChaosProgram(frame, completed);
    void global::CardGame.Core.IProgramCompletionProtocols.CompleteGame() => _kernelCoreMainOperations.CompleteGame();
    void global::CardGame.Core.IProgramCompletionProtocols.FinishPaidOwnTarget(ProgramSkillFrame frame) => _corePaidOwnTargetObserversOperations.FinishPaidOwnTargetBeforeProgramCompletion(frame);
    void global::CardGame.Core.IProgramCompletionProtocols.FinishOwnedDeathBenefit(ProgramSkillFrame frame, bool completed) => _coreOwnedDeathBenefitReturnsOperations.FinishOwnedDeathBenefitReturn(frame, completed);
    bool global::CardGame.Core.IProgramCompletionProtocols.CompleteDeferredTurnEnd(ProgramSkillFrame frame, ProgramSkillWindowContext context) => _coreDeferredHandAlignmentsOperations.CompleteDeferredTurnEndBinding(frame, context);
    bool global::CardGame.Core.IProgramCompletionProtocols.ReturnCompletedBinding(ProgramSkillFrame frame, bool completed) => _programPipelineHooksOperations._programPipelineHooks.ReturnCompletedProgramBinding(frame, completed);
    void global::CardGame.Core.IProgramCompletionProtocols.CompleteFirstLegacyCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreFirstSlashDuelLegacyWindowsOperations.CompleteFirstLegacyBenefitCandidate(candidate, context);
    void global::CardGame.Core.IProgramCompletionProtocols.ReturnWindow(ProgramSkillFrame frame, bool completed) => _nativeCardGameCoreProgramWindowReturnCatalogOperations.Return(global::CardGame.Core.EngineOwners.CoreProgramWindowReturnsDefinitions.ProgramWindowReturns.Value, frame, completed);
    bool global::CardGame.Core.IProgramCompletionProtocols.TryReturnMovement(ProgramSkillFrame frame, bool afterPayment) => _kernelCoreGameEngineProgramContinuationsOperations.TryReturnMovement(global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsDefinitions.ProgramContinuations.Value, frame, afterPayment);
    void global::CardGame.Core.IProgramCompletionProtocols.CancelBindingAndCleanup(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    void global::CardGame.Core.IProgramCompletionProtocols.SetAttackRangeCoverage(long frameId, string bind, int subjectSeat, int before, int after) => _coreProgramCardPaymentsOperations.SetProgramAttackRangeCoverage(frameId, bind, subjectSeat, before, after);
    void global::CardGame.Core.IProgramCompletionProtocols.ContinueRepeatedJudgment(long frameId) => _coreProgramRepeatedJudgmentOperations.ContinueProgramRepeatedJudgmentAfterMovement(frameId);
}
