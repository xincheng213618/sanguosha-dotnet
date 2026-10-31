#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed class CoreFinalizedCardTargetsCharacterHuangZhongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCharacterHuangZhongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryAnniversaryHzEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => this._targetFactory().TryAnniversaryHzEquipmentCommitted(id, equipment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime))]
internal sealed class CoreFinalizedCardTargetsCharacterCaoZhenBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCharacterCaoZhenBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TryBoundarySidiEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => this._targetFactory().TryBoundarySidiEquipmentCommitted(id, equipment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime))]
internal sealed class CoreFinalizedCardTargetsCharacterChengYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCharacterChengYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> _targetFactory = targetFactory;
    public bool TryMouCyEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => this._targetFactory().TryMouCyEquipmentCommitted(id, equipment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetWindowProtocols TargetWindowProtocols { get => this._targetFactory().TargetWindowProtocols; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreFinalizedCardTargetsCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryHappyWeiYanEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => this._targetFactory().TryHappyWeiYanEquipmentCommitted(id, equipment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreNativeEquipmentUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreNativeEquipmentUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> _targetFactory = targetFactory;
    public void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId) => this._targetFactory().CompleteEquipmentUse(source, equipment, resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreActivityOrdinaryTricksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreActivityOrdinaryTricksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> _targetFactory = targetFactory;
    public bool HasActivityOrdinaryTrickProjection { get => this._targetFactory().HasActivityOrdinaryTrickProjection; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreProgramCardUseNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreProgramCardUseNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _targetFactory = targetFactory;
    public bool HasBeforeTargetCardProgram(global::CardGame.Core.CardKind kind) => this._targetFactory().HasBeforeTargetCardProgram(kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFinalizedCardTargetsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public bool TryMarkFinalizedSimpleProgramsStarted(long frameId) => this._targetFactory().TryMarkFinalizedSimpleProgramsStarted(frameId);
}
