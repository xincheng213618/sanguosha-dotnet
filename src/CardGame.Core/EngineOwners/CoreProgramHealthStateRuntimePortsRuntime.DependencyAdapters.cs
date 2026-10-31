#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetLivingFactionCount() => this._targetFactory().GetLivingFactionCount();
    public int GetLivingPlayersMinHp() => this._targetFactory().GetLivingPlayersMinHp();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default) => this._targetFactory().TryQueueRecoveryReplacement(parentFrameId, sourceSeat, targetSeat, amount, completion, happyQiaoRecoverToOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default) => this._targetFactory().RecordCharacterStateChange(targetSeat, window, parentFrameId, turnedOver);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreCharacterTurnedOverProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreCharacterTurnedOverProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => this._targetFactory().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryHandleHappyQiaoRecoverTo(long frameId, int ownerSeat, int targetSeat, int value) => this._targetFactory().TryHandleHappyQiaoRecoverTo(frameId, ownerSeat, targetSeat, value);
    public bool TryApplyHappyQiaoMaximumHp(long parentFrameId, int targetSeat, int newMaximum, string sourceId) => this._targetFactory().TryApplyHappyQiaoMaximumHp(parentFrameId, targetSeat, newMaximum, sourceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreQimeiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreQimeiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> _targetFactory = targetFactory;
    public bool HasQimeiHealthSubject(int seat) => this._targetFactory().HasQimeiHealthSubject(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreWushuangTacticRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreWushuangTacticRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> _targetFactory = targetFactory;
    public bool HasWushuangTactic(int seat, int id) => this._targetFactory().HasWushuangTactic(seat, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreShenwuNpcProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreShenwuNpcProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> _targetFactory = targetFactory;
    public bool ShenwuCannotTurnOver(global::CardGame.Core.CharacterState target) => this._targetFactory().ShenwuCannotTurnOver(target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCoreNativeRecoveryFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCoreNativeRecoveryFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> _targetFactory = targetFactory;
    public long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount) => this._targetFactory().BeginRecovery(parentFrameId, sourceSeat, targetSeat, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed class CoreProgramHealthStateRuntimePortsCharacterSunJianMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramHealthStateRuntimePortsCharacterSunJianMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouSunJianOlHpSubject(int seat) => this._targetFactory().HasMouSunJianOlHpSubject(seat);
}
