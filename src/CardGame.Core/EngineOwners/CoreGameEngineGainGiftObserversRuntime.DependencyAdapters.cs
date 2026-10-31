#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreGainGiftObserversCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreGainGiftObserversCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreGainGiftObserversCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying) => this._targetFactory().HuanpeiDyingRescueRide(index, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed class CoreGainGiftObserversCoreRecoveryReplacementDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreRecoveryReplacementDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _targetFactory = targetFactory;
    public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RecoveryReplacementFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CoreGainGiftObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreGainGiftObserversCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().IsRecordedActualDiscardRecoveryPhase(phase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreGainGiftObserversCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public int[] MatchingOwnerBatchMovementIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().MatchingOwnerBatchMovementIndexes(batch, candidate, trigger);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed class CoreGainGiftObserversCoreParticipantHandProgramRescueRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreParticipantHandProgramRescueRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentProgramAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreGainGiftObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
    public bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreGainGiftObserversCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().DyingSuitsStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreGainGiftObserversCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime))]
internal sealed class CoreGainGiftObserversCoreDelegatedJudgmentChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreDelegatedJudgmentChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _targetFactory = targetFactory;
    public bool ValidDelegatedJudgmentDraft(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.DelegatedJudgmentDraft d) => this._targetFactory().ValidDelegatedJudgmentDraft(j, d);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreGainGiftObserversCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreGainGiftObserversCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreGainGiftObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CoreGainGiftObserversCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime))]
internal sealed class CoreGainGiftObserversCoreGainGiftProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreGainGiftProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GainGiftPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GainGiftPausedEffect(f);
    public int[] MatchingActualGainGiftIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner, global::CardGame.Core.CardLocation location) => this._targetFactory().MatchingActualGainGiftIndexes(batch, owner, location);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GainGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GainGiftChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime))]
internal sealed class CoreGainGiftObserversCoreDrawFundedDistinctBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreDrawFundedDistinctBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> _targetFactory = targetFactory;
    public bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreGainGiftObserversCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreGainGiftObserversCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreGainGiftObserversCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public bool IsHappyWeiYanRetainedGainSettlement(global::CardGame.Core.HappyWeiYanRetainedGainSettlement settlement, long producerFrameId, int cardId, global::CardGame.Core.CardLocation requestedTo, long before) => this._targetFactory().IsHappyWeiYanRetainedGainSettlement(settlement, producerFrameId, cardId, requestedTo, before);
    public bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo) => this._targetFactory().IsHappyWeiYanRedirectedGain(movement, expectedTo);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreGainGiftObserversCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGainGiftObserversCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().TieredRoundZeroDyingRescueRide(dyingIndex, dying);
}
