#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreActualTurnsCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool TryBeginIdentityAppearanceForTurn(global::CardGame.Core.CharacterState player) => this._targetFactory().TryBeginIdentityAppearanceForTurn(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed class CoreActualTurnsCorePrepDiscardEndingPromisesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePrepDiscardEndingPromisesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _targetFactory = targetFactory;
    public void ExpirePrepDiscardEndingPromises() => this._targetFactory().ExpirePrepDiscardEndingPromises();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreActualTurnsCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public void ClearActualDiscardRecoveryPhase() => this._targetFactory().ClearActualDiscardRecoveryPhase();
    public void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor) => this._targetFactory().StartActualDiscardRecoveryPhase(kind, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreActualTurnsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetJudgment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreActualTurnsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void ResetProgramContributionUsesForPlayPhase(int providerSeat) => this._targetFactory().ResetProgramContributionUsesForPlayPhase(providerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanJiAnniversaryRuntime))]
internal sealed class CoreActualTurnsCharacterRuanJiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanJiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterRuanJiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanJiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool KeepsAnniversaryRuanJiAlcohol(int seat) => this._targetFactory().KeepsAnniversaryRuanJiAlcohol(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime))]
internal sealed class CoreActualTurnsCharacterHuaXiongMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterHuaXiongMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> _targetFactory = targetFactory;
    public void RecoverMouYangweiAtEnding(int owner) => this._targetFactory().RecoverMouYangweiAtEnding(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed class CoreActualTurnsCoreActualDiscardPhaseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreActualDiscardPhaseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _targetFactory = targetFactory;
    public void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false) => this._targetFactory().BeginDiscardPhase(beforeEntryPolicyCompleted);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime))]
internal sealed class CoreActualTurnsCoreEndRefillNextTurnAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreEndRefillNextTurnOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> _targetFactory = targetFactory;
    public void ConsumeEndRefillNextTurn(global::CardGame.Core.CharacterState current) => this._targetFactory().ConsumeEndRefillNextTurn(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime))]
internal sealed class CoreActualTurnsCoreShanheS5ShensuAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreShanheS5ShensuOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> _targetFactory = targetFactory;
    public void RecordShanheS5ActualPhaseSkip(int owner, global::CardGame.Core.SkillProgramTurnPhase phase, long extraDrawFrameId = 0, long? costProgramFrameId = default) => this._targetFactory().RecordShanheS5ActualPhaseSkip(owner, phase, extraDrawFrameId, costProgramFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreActualTurnsCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false) => this._targetFactory().FreezeAndCaptureCaoXiNativeDrawBase(player, replaced);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime))]
internal sealed class CoreActualTurnsCoreDrawPhaseObligationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreDrawPhaseObligationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> _targetFactory = targetFactory;
    public bool TryBeginActualDrawCompletion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects) => this._targetFactory().TryBeginActualDrawCompletion(owner, delayedEffects);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreActualTurnsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime))]
internal sealed class CoreActualTurnsCoreCharacterTurnedOverProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreCharacterTurnedOverProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => this._targetFactory().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime))]
internal sealed class CoreActualTurnsCoreProgramHoldCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreProgramHoldCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHoldCardsRuntime> _targetFactory = targetFactory;
    public void ReturnProgramPojunHoldsAtTurnEnd() => this._targetFactory().ReturnProgramPojunHoldsAtTurnEnd();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime))]
internal sealed class CoreActualTurnsCoreDeferredCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreDeferredCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> _targetFactory = targetFactory;
    public void ConsumeSkippedNextTurnDrawBenefits(global::CardGame.Core.CharacterState current) => this._targetFactory().ConsumeSkippedNextTurnDrawBenefits(current);
    public void CleanupLostDeferredPileSources() => this._targetFactory().CleanupLostDeferredPileSources();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreActualTurnsCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginBeiMiHuActualTurnPrelude(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginBeiMiHuActualTurnPrelude(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime))]
internal sealed class CoreActualTurnsCharacterGuoHuaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterGuoHuaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> _targetFactory = targetFactory;
    public bool TryBeginMouGuoHuaiActualStart(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginMouGuoHuaiActualStart(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime))]
internal sealed class CoreActualTurnsCoreCardUseDebitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreCardUseDebitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> _targetFactory = targetFactory;
    public void ResetCardUseDebitPhase() => this._targetFactory().ResetCardUseDebitPhase();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime))]
internal sealed class CoreActualTurnsCharacterLvQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterLvQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _targetFactory = targetFactory;
    public void ObserveLvQianActualPlayEntry(int ownerSeat, int phaseInstance) => this._targetFactory().ObserveLvQianActualPlayEntry(ownerSeat, phaseInstance);
    public bool TryBeginForcedWeiluPhaseEnd() => this._targetFactory().TryBeginForcedWeiluPhaseEnd();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreActualTurnsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool TryBeginRoundStartingProgramWindow(global::CardGame.Core.CharacterState turnOwner) => this._targetFactory().TryBeginRoundStartingProgramWindow(turnOwner);
    public bool TryBeginTurnStartProgramWindow(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginTurnStartProgramWindow(owner);
    public void BeginNormalTurnStartAfterProgramBindings(global::CardGame.Core.CharacterState current) => this._targetFactory().BeginNormalTurnStartAfterProgramBindings(current);
    public bool TryBeginDrawPhaseProgramWindow(global::CardGame.Core.CharacterState owner, bool skipPlayPhaseAfterDraw) => this._targetFactory().TryBeginDrawPhaseProgramWindow(owner, skipPlayPhaseAfterDraw);
    public bool TryBeginDrawPhaseEndedProgramWindow(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects) => this._targetFactory().TryBeginDrawPhaseEndedProgramWindow(owner, delayedEffects);
    public bool TryBeginAfterNormalDrawProgramWindow(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginAfterNormalDrawProgramWindow(owner);
    public bool ResumeScheduledProgramPhase() => this._targetFactory().ResumeScheduledProgramPhase();
    public bool TryBeginPlayPhaseStartingBoundary(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginPlayPhaseStartingBoundary(owner);
    public void CompletePlayPhaseAfterProgramWindow() => this._targetFactory().CompletePlayPhaseAfterProgramWindow();
    public void CancelScheduledProgramPhase() => this._targetFactory().CancelScheduledProgramPhase();
    public bool TryBeginTurnEndingBoundary(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginTurnEndingBoundary(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreActualTurnsCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public void CleanupDeferredHandAlignments() => this._targetFactory().CleanupDeferredHandAlignments();
    public bool TryBeginDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped) => this._targetFactory().TryBeginDeferredTurnEnd(previous, skipped);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CoreActualTurnsCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default) => this._targetFactory().RecordCharacterStateChange(targetSeat, window, parentFrameId, turnedOver);
    public bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
    public void CompleteFaceUpSkippedTurn(global::CardGame.Core.CharacterState current) => this._targetFactory().CompleteFaceUpSkippedTurn(current);
    public bool TryBeginJudgmentPhaseStartingPrograms(global::CardGame.Core.CharacterState turnOwner) => this._targetFactory().TryBeginJudgmentPhaseStartingPrograms(turnOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime))]
internal sealed class CoreActualTurnsCoreWholeActualTurnUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreWholeActualTurnUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _targetFactory = targetFactory;
    public bool TracksWholeActualTurnUseOrdinal { get => this._targetFactory().TracksWholeActualTurnUseOrdinal; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime))]
internal sealed class CoreActualTurnsCoreMouLuMengAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreMouLuMengOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> _targetFactory = targetFactory;
    public void BeginRoundForTurn(global::CardGame.Core.CharacterState current) => this._targetFactory().BeginRoundForTurn(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed class CoreActualTurnsCoreForeignTurnContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreForeignTurnContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _targetFactory = targetFactory;
    public bool TryBeginForeignActualTurnStart(global::CardGame.Core.CharacterState turnOwner) => this._targetFactory().TryBeginForeignActualTurnStart(turnOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime))]
internal sealed class CoreActualTurnsCharacterZhuHuanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterZhuHuanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public void RecordBoundaryZhuHuanSkippedPhase(int owner, global::CardGame.Core.SkillProgramTurnPhase phase, long extraDrawFrameId = 0) => this._targetFactory().RecordBoundaryZhuHuanSkippedPhase(owner, phase, extraDrawFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime))]
internal sealed class CoreActualTurnsCoreNianActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreNianActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> _targetFactory = targetFactory;
    public void RecordNianActualPhaseSkips(int owner, bool draw, bool play) => this._targetFactory().RecordNianActualPhaseSkips(owner, draw, play);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreActualTurnsCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public void BeginDelayedNullificationWindow(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().BeginDelayedNullificationWindow(owner, card, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime))]
internal sealed class CoreActualTurnsCorePrivateCardOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePrivateCardOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime> _targetFactory = targetFactory;
    public bool TryBeginDeferredPrivateOffers(global::CardGame.Core.CharacterState target) => this._targetFactory().TryBeginDeferredPrivateOffers(target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreActualTurnsCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.RuleQueryEvaluation EvaluateDrawCount(global::CardGame.Core.CharacterState player) => this._targetFactory().EvaluateDrawCount(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime))]
internal sealed class CoreActualTurnsCoreDeferredBenefitsProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreDeferredBenefitsProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> _targetFactory = targetFactory;
    public void ApplyQueuedNextTurnRuleModifiers(int targetSeat) => this._targetFactory().ApplyQueuedNextTurnRuleModifiers(targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime))]
internal sealed class CoreActualTurnsCoreStrategicProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreStrategicProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> _targetFactory = targetFactory;
    public void ResetPlayPhaseProgramStates(int ownerSeat) => this._targetFactory().ResetPlayPhaseProgramStates(ownerSeat);
    public void ExpireProgramSuppressions(int turnOwnerSeat) => this._targetFactory().ExpireProgramSuppressions(turnOwnerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime))]
internal sealed class CoreActualTurnsCorePlayPhaseSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePlayPhaseSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> _targetFactory = targetFactory;
    public bool HasPlayPhaseSkillGrantCapability { get => this._targetFactory().HasPlayPhaseSkillGrantCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime))]
internal sealed class CoreActualTurnsCharacterZhangLiaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterZhangLiaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public void ApplyAnniversaryCuixiDrawSkip(int currentSeat) => this._targetFactory().ApplyAnniversaryCuixiDrawSkip(currentSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreActualTurnsCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public bool TryBeginMoLuBuOpening() => this._targetFactory().TryBeginMoLuBuOpening();
    public bool TryStartMoLuBuOpeningPlay(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryStartMoLuBuOpeningPlay(owner);
    public bool TryEndMoLuBuOpeningEarly() => this._targetFactory().TryEndMoLuBuOpeningEarly();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreActualTurnsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void ExpireOwnerTurnStartDamageModifiers(int ownerSeat) => this._targetFactory().ExpireOwnerTurnStartDamageModifiers(ownerSeat);
    public void ExpireTurnCardUseEffects(int turnNumber, int turnSeat) => this._targetFactory().ExpireTurnCardUseEffects(turnNumber, turnSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed class CoreActualTurnsCoreRoundEndingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreRoundEndingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _targetFactory = targetFactory;
    public bool TryBeginRoundEndingBoundary(global::CardGame.Core.CharacterState previous, bool skipped) => this._targetFactory().TryBeginRoundEndingBoundary(previous, skipped);
    public bool TracksRoundEnding { get => this._targetFactory().TracksRoundEnding; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime))]
internal sealed class CoreActualTurnsCharacterChengGongYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterChengGongYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> _targetFactory = targetFactory;
    public void ConsumeChengGongYingDraw(int actorSeat) => this._targetFactory().ConsumeChengGongYingDraw(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime))]
internal sealed class CoreActualTurnsCoreMatchOutcomesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreMatchOutcomesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> _targetFactory = targetFactory;
    public void EndAsDraw(string reason) => this._targetFactory().EndAsDraw(reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime))]
internal sealed class CoreActualTurnsCoreActualTurnProgressionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreActualTurnProgressionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime> _targetFactory = targetFactory;
    public void AdvanceActualTurnProgression(global::CardGame.Core.CharacterState previous) => this._targetFactory().AdvanceActualTurnProgression(previous);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime))]
internal sealed class CoreActualTurnsCharacterSunQuanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterSunQuanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyLearningActualStart(global::CardGame.Core.CharacterState current) => this._targetFactory().TryBeginHappyLearningActualStart(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime))]
internal sealed class CoreActualTurnsCharacterLuBuHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterLuBuHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyLuBuActualStart(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginHappyLuBuActualStart(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime))]
internal sealed class CoreActualTurnsCoreNamedUseConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreNamedUseConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime> _targetFactory = targetFactory;
    public void ResetTurnProgramBooleanStates() => this._targetFactory().ResetTurnProgramBooleanStates();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime))]
internal sealed class CoreActualTurnsCoreActualPhaseEndsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreActualPhaseEndsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> _targetFactory = targetFactory;
    public bool TryBeginActualPhaseEnd(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, global::CardGame.Core.ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default) => this._targetFactory().TryBeginActualPhaseEnd(kind, continuation, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted, parentFrameId);
    public bool TracksActualPhaseEnds { get => this._targetFactory().TracksActualPhaseEnds; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime))]
internal sealed class CoreActualTurnsCoreConvertingGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreConvertingGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> _targetFactory = targetFactory;
    public void ExpireGiftRetentionObligations() => this._targetFactory().ExpireGiftRetentionObligations();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreActualTurnsCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public void ResolveHengyeTurnStart(global::CardGame.Core.CharacterState player) => this._targetFactory().ResolveHengyeTurnStart(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime))]
internal sealed class CoreActualTurnsCharacterQiaoZhouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterQiaoZhouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginQiaoZhouPreparation(global::CardGame.Core.CharacterState current) => this._targetFactory().TryBeginQiaoZhouPreparation(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreActualTurnsCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public bool TryBeginHeldOriginalUseLightning(global::CardGame.Core.CharacterState current, int? excludedCard) => this._targetFactory().TryBeginHeldOriginalUseLightning(current, excludedCard);
    public bool HasHeldOriginalUseLightning(global::CardGame.Core.CharacterState player) => this._targetFactory().HasHeldOriginalUseLightning(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime))]
internal sealed class CoreActualTurnsCoreGrantedEntityPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreGrantedEntityPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> _targetFactory = targetFactory;
    public void IssueGrantedEntityPhaseBoundary(global::CardGame.Core.CharacterState actor) => this._targetFactory().IssueGrantedEntityPhaseBoundary(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreActualTurnsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime))]
internal sealed class CoreActualTurnsCorePublicPersistentPilesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePublicPersistentPilesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> _targetFactory = targetFactory;
    public void CleanupLostPublicPersistentPiles() => this._targetFactory().CleanupLostPublicPersistentPiles();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed class CoreActualTurnsCharacterKeBiNengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterKeBiNengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _targetFactory = targetFactory;
    public void ExpireKeBiNengPingduanAlcohol(int actualTurn, int turnOwner) => this._targetFactory().ExpireKeBiNengPingduanAlcohol(actualTurn, turnOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime))]
internal sealed class CoreActualTurnsCorePrivateTurnHoldsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePrivateTurnHoldsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> _targetFactory = targetFactory;
    public void ReturnPrivateTurnHoldsAtTurnEnd() => this._targetFactory().ReturnPrivateTurnHoldsAtTurnEnd();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreActualTurnsCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public bool IsZixiQinJudgmentCard(int id) => this._targetFactory().IsZixiQinJudgmentCard(id);
    public global::CardGame.Core.CardKind? ZixiQinJudgmentSlot(int id) => this._targetFactory().ZixiQinJudgmentSlot(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime))]
internal sealed class CoreActualTurnsCoreDrawPhaseAttemptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreDrawPhaseAttemptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> _targetFactory = targetFactory;
    public bool TryBeginNormalDrawAttemptPhase(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayed, bool afterStartingWindow = false, bool replaced = false, int adjustment = 0, int? frozenBase = default) => this._targetFactory().TryBeginNormalDrawAttemptPhase(owner, delayed, afterStartingWindow, replaced, adjustment, frozenBase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed class CoreActualTurnsCharacterDuFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterDuFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _targetFactory = targetFactory;
    public void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat) => this._targetFactory().ConsumeDuFuRenPhase(kind, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime))]
internal sealed class CoreActualTurnsCharacterKanZeOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterKanZeOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime> _targetFactory = targetFactory;
    public bool ConsumeKuanshiDrawSkip(int ownerSeat) => this._targetFactory().ConsumeKuanshiDrawSkip(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime))]
internal sealed class CoreActualTurnsCharacterLiuQiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterLiuQiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> _targetFactory = targetFactory;
    public void ObserveLiuQiPlayBoundary(int ownerSeat, int phaseInstance, bool skipped) => this._targetFactory().ObserveLiuQiPlayBoundary(ownerSeat, phaseInstance, skipped);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreActualTurnsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreActualTurnsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreActualTurnsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryQueueHengyeRecovery(global::CardGame.Core.CharacterState player) => this._targetFactory().TryQueueHengyeRecovery(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreActualTurnsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public int FindNextAliveSeat(int fromSeat) => this._targetFactory().FindNextAliveSeat(fromSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime))]
internal sealed class CoreActualTurnsCoreMatchPresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreMatchPresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> _targetFactory = targetFactory;
    public string GetPublicGeneralName(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPublicGeneralName(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed class CoreActualTurnsCoreTreatedWoundedAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCoreTreatedWoundedOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _targetFactory = targetFactory;
    public bool UsesTreatedWounded { get => this._targetFactory().UsesTreatedWounded; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed class CoreActualTurnsCorePhaseHandSeizureAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCorePhaseHandSeizureOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _targetFactory = targetFactory;
    public bool TryBeginForcedPhaseHandDebtReturn() => this._targetFactory().TryBeginForcedPhaseHandDebtReturn();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreActualTurnsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public void ExpireWeifuActualTurnGrants() => this._targetFactory().ExpireWeifuActualTurnGrants();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime))]
internal sealed class CoreActualTurnsCharacterSunQuanAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualTurnsCharacterSunQuanAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginAnniversaryCangmingActualTurnStart(global::CardGame.Core.CharacterState current) => this._targetFactory().TryBeginAnniversaryCangmingActualTurnStart(current);
}
