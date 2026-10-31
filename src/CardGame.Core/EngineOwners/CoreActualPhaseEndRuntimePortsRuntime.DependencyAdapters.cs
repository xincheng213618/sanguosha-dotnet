#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPhaseSchedulingRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsProgramPhaseSchedulingAdapter(global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsProgramPhaseSchedulingOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPhaseSchedulingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void CompletePlayPhaseAfterProgramWindow() => this._targetFactory().CompletePlayPhaseAfterProgramWindow();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterHuangZuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterHuangZuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectShiHzPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().CollectShiHzPhaseEndCandidates(phase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterLuShiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterLuShiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuShiOlRuntime> _targetFactory = targetFactory;
    public bool HasOlLuShi { get => this._targetFactory().HasOlLuShi; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterSimaShiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterSimaShiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> _targetFactory = targetFactory;
    public bool HasMouSimaShi { get => this._targetFactory().HasMouSimaShi; }

    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectMouSimaShiPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().CollectMouSimaShiPhaseEndCandidates(phase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterYaDanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterYaDanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectQingyaPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().CollectQingyaPhaseEndCandidates(phase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void BeginTurnStartAfterProgramLifecycle(global::CardGame.Core.CharacterState current) => this._targetFactory().BeginTurnStartAfterProgramLifecycle(current);
    public void CompleteTurnStart(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool skippedActualJudgment = false) => this._targetFactory().CompleteTurnStart(current, delayedEffects, skippedActualJudgment);
    public void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false) => this._targetFactory().CompleteTurnStartAfterDraw(current, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    public void EndTurn() => this._targetFactory().EndTurn();
    public void FinalizeEndTurn(global::CardGame.Core.CharacterState previous) => this._targetFactory().FinalizeEndTurn(previous);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasLishiCapability { get => this._targetFactory().HasLishiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterLuYuShengMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterLuYuShengMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectShiLysPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => this._targetFactory().CollectShiLysPhaseEndCandidates(phase);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed class CoreActualPhaseEndRuntimePortsCharacterHuCheErMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPhaseEndRuntimePortsCharacterHuCheErMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _targetFactory = targetFactory;
    public bool HasMouHuCheEr { get => this._targetFactory().HasMouHuCheEr; }
}
