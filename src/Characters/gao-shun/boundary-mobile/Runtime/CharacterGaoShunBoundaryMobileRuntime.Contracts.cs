#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterGaoShunBoundaryMobileCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterGaoShunBoundaryMobileCoreTurnHandLimitKindExemptionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramTurnHandLimitCardKindExemption(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0);
}

internal interface ICharacterGaoShunBoundaryMobileCoreProgramDamageReductionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReduceProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program, int amount);
}

internal interface ICharacterGaoShunBoundaryMobileCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
    bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICharacterGaoShunBoundaryMobileResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterGaoShunBoundaryMobileCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICharacterGaoShunBoundaryMobileCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICharacterGaoShunBoundaryMobileCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterGaoShunBoundaryMobileCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICharacterGaoShunBoundaryMobileKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
