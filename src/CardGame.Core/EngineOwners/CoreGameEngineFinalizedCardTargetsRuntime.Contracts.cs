#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreFinalizedCardTargetsCharacterHuangZhongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAnniversaryHzEquipmentCommitted(long id, global::CardGame.Core.Card equipment);
}

internal interface ICoreFinalizedCardTargetsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreFinalizedCardTargetsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreFinalizedCardTargetsCharacterCaoZhenBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBoundarySidiEquipmentCommitted(long id, global::CardGame.Core.Card equipment);
}

internal interface ICoreFinalizedCardTargetsCharacterChengYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMouCyEquipmentCommitted(long id, global::CardGame.Core.Card equipment);
}

internal interface ICoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetWindowProtocols TargetWindowProtocols { get; }
}

internal interface ICoreFinalizedCardTargetsCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryHappyWeiYanEquipmentCommitted(long id, global::CardGame.Core.Card equipment);
}

internal interface ICoreFinalizedCardTargetsCoreNativeEquipmentUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId);
}

internal interface ICoreFinalizedCardTargetsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreFinalizedCardTargetsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreFinalizedCardTargetsCoreActivityOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActivityOrdinaryTrickProjection { get; }
}

internal interface ICoreFinalizedCardTargetsCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeforeTargetCardProgram(global::CardGame.Core.CardKind kind);
}

internal interface ICoreFinalizedCardTargetsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreFinalizedCardTargetsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMarkFinalizedSimpleProgramsStarted(long frameId);
}

internal interface ICoreFinalizedCardTargetsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
