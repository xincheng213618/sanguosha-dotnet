#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGainGiftObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreGainGiftObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreGainGiftObserversCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreGainGiftObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreGainGiftObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreGainGiftObserversCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreGainGiftObserversCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] MatchingOwnerBatchMovementIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger);
}

internal interface ICoreGainGiftObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreGainGiftObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreGainGiftObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreGainGiftObserversCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreGainGiftObserversCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreGainGiftObserversCoreDelegatedJudgmentChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidDelegatedJudgmentDraft(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.DelegatedJudgmentDraft d);
}

internal interface ICoreGainGiftObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreGainGiftObserversCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreGainGiftObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreGainGiftObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreGainGiftObserversCoreGainGiftProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GainGiftPausedEffect(global::CardGame.Core.ProgramSkillFrame f);
    int[] MatchingActualGainGiftIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner, global::CardGame.Core.CardLocation location);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GainGiftChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGainGiftObserversCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreGainGiftObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreGainGiftObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreGainGiftObserversCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanRetainedGainSettlement(global::CardGame.Core.HappyWeiYanRetainedGainSettlement settlement, long producerFrameId, int cardId, global::CardGame.Core.CardLocation requestedTo, long before);
    bool IsHappyWeiYanRedirectedGain(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation expectedTo);
}

internal interface ICoreGainGiftObserversCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}
