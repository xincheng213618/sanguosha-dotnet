#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamageJudgmentSuitPaymentObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreDamageJudgmentSuitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageJudgmentUseMatches(global::CardGame.Core.ProgramDamageJudgmentSuitPayment draft, global::CardGame.Core.CardKind? kind);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageJudgmentPaymentChoices(global::CardGame.Core.ProgramSkillFrame frame, bool sourceDiscard);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreZhenyiJudgmentWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactZhenyiBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame);
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDamageJudgmentSuitPaymentObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}
