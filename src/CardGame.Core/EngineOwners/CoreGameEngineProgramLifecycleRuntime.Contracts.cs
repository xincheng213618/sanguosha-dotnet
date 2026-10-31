#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramLifecycleProgramPhaseSchedulingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseSchedulingRuntime Get();
}

internal interface ICoreProgramLifecycleCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreProgramLifecycleProgramBindingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramBindingRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTargetSelectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetSelectionRuntime Get();
}

internal interface ICoreProgramLifecycleProgramOwnedZoneDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramOwnedZoneDiscardRuntime Get();
}

internal interface ICoreProgramLifecycleCorePublicDamagePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateAiSkillContext(global::CardGame.Core.CharacterState player, int viewerSeat, global::CardGame.Core.GameSnapshot? snapshot = default);
    global::System.Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat);
    global::CardGame.Core.PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, global::CardGame.Core.LegalAction action, int targetSeat);
}

internal interface ICoreProgramLifecycleProgramTriggerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerRuntime Get();
}

internal interface ICoreProgramLifecycleProgramRandomCardAcquisitionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRandomCardAcquisitionRuntime Get();
}

internal interface ICoreProgramLifecycleProgramDamageCardClaimOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageCardClaimRuntime Get();
}

internal interface ICoreProgramLifecycleProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreProgramLifecycleProgramPindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPindianRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTurnWindowEntryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTurnWindowEntryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTriggerFactProjectorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerFactProjector Get();
}

internal interface ICoreProgramLifecycleProgramJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramJudgmentRuntime Get();
}

internal interface ICoreProgramLifecycleProgramDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDrawRuntime Get();
}

internal interface ICoreProgramLifecycleProgramPhaseBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseBoundaryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramDrawDiscardWindowEntryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramHealthStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramHealthStateRuntime Get();
}

internal interface ICoreProgramLifecycleCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramLifecycleProgramBoundCardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramBoundCardRuntime Get();
}

internal interface ICoreProgramLifecycleProgramActivationWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramActivationWindowRuntime Get();
}
