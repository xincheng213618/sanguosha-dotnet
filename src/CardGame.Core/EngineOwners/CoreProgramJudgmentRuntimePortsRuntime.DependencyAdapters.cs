#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterZhouQunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterZhouQunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> _targetFactory = targetFactory;
    public bool IsNingwuJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsNingwuJudgment(j, f);
    public bool CanBeginNingwuJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginNingwuJudgment(f, target, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterGuoJiaMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterGuoJiaMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsShenGuoJiaJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsShenGuoJiaJudgment(judgment, frame);
    public bool CanBeginShenGuoJiaJudgment(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginShenGuoJiaJudgment(frame, targetSeat, reason, resultBind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool IsWeifuJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsWeifuJudgment(j, f);
    public bool MatchesWeifuJudgmentInstruction(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().MatchesWeifuJudgmentInstruction(f, target, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCoreDamageJudgmentSuitPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCoreDamageJudgmentSuitPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> _targetFactory = targetFactory;
    public bool IsDamageJudgmentInstructionTarget(global::CardGame.Core.ProgramSkillFrame frame, int target) => this._targetFactory().IsDamageJudgmentInstructionTarget(frame, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterZhangFeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterZhangFeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsShenZfJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsShenZfJudgment(j, f);
    public bool CanBeginShenZfJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginShenZfJudgment(f, targetSeat, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsLishiJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsLishiJudgment(j, f);
    public bool CanBeginLishiJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginLishiJudgment(f, targetSeat, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public bool? BeginJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, global::CardGame.Core.CardKind? sourceCard, global::CardGame.Core.JudgmentContinuationKind continuation, global::CardGame.Core.Card? delayedCard = default, int? sourceSeat = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility? programResultVisibility = default, global::CardGame.Core.OriginalUseLightningAttachment? originalUseLightning = default, int? originalUseLightningExcludedCardId = default) => this._targetFactory().BeginJudgment(attack, targetSeat, reason, parentFrameId, sourceCard, continuation, delayedCard, sourceSeat, programResultBind, programResultVisibility, originalUseLightning, originalUseLightningExcludedCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterGuoJiaMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterGuoJiaMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> _targetFactory = targetFactory;
    public bool IsMouGjJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsMouGjJudgment(j, f);
    public bool MouGjJudgmentRequest(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().MouGjJudgmentRequest(f, target, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCoreWushuangCombatProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCoreWushuangCombatProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> _targetFactory = targetFactory;
    public bool IsWushuangLightningJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().IsWushuangLightningJudgment(f, target, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCoreSourceHuojieAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCoreSourceHuojieOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> _targetFactory = targetFactory;
    public bool IsSourceHuojieJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsSourceHuojieJudgment(j, f);
    public bool CanBeginSourceHuojieJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginSourceHuojieJudgment(f, target, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterMaYuanYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterMaYuanYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> _targetFactory = targetFactory;
    public bool IsBinghuoJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsBinghuoJudgment(j, f);
    public bool CanBeginBinghuoJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginBinghuoJudgment(f, targetSeat, reason, bind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime))]
internal sealed class CoreProgramJudgmentRuntimePortsCharacterZhangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramJudgmentRuntimePortsCharacterZhangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> _targetFactory = targetFactory;
    public bool IsZhangJiJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsZhangJiJudgment(j, f);
    public bool CanBeginZhangJiJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => this._targetFactory().CanBeginZhangJiJudgment(f, targetSeat, reason, bind, visibility);
}
