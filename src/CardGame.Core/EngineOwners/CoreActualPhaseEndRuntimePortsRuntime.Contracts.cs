#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseSchedulingRuntime Get();
}

internal interface ICoreActualPhaseEndRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompletePlayPhaseAfterProgramWindow();
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterHuangZuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectShiHzPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterLuShiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOlLuShi { get; }
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterSimaShiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouSimaShi { get; }

    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectMouSimaShiPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterYaDanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectQingyaPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreActualPhaseEndRuntimePortsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginTurnStartAfterProgramLifecycle(global::CardGame.Core.CharacterState current);
    void CompleteTurnStart(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool skippedActualJudgment = false);
    void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false);
    void EndTurn();
    void FinalizeEndTurn(global::CardGame.Core.CharacterState previous);
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLishiCapability { get; }
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterLuYuShengMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectShiLysPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreActualPhaseEndRuntimePortsCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouHuCheEr { get; }
}

internal interface ICoreActualPhaseEndRuntimePortsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreActualPhaseEndRuntimePortsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}
