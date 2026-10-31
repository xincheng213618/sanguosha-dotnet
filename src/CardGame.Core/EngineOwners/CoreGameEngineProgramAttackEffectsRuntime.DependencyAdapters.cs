#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public void RecordWeifuRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int nextSeat) => this._targetFactory().RecordWeifuRedirect(redirector, useId, previousSeat, nextSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterDaQiaoMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterDaQiaoMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? PostCostSlashRedirectFrame(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int owner) => this._targetFactory().PostCostSlashRedirectFrame(attack, owner);
    public bool IsPostCostSlashRedirectTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int owner, int target, int? excludedEquipment) => this._targetFactory().IsPostCostSlashRedirectTarget(attack, owner, target, excludedEquipment);
    public bool HasPostCostSlashRedirectPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target) => this._targetFactory().HasPostCostSlashRedirectPayment(frame, attack, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public void RecordBingzhongRedirect(global::CardGame.Core.ProgramSkillFrame frame, long useId, int previousSeat, int nextSeat) => this._targetFactory().RecordBingzhongRedirect(frame, useId, previousSeat, nextSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasArmorBypass(global::CardGame.Core.CharacterState player) => this._targetFactory().HasArmorBypass(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime))]
internal sealed class CoreProgramAttackEffectsCoreCurrentEnhancementSlashRedirectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreCurrentEnhancementSlashRedirectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> _targetFactory = targetFactory;
    public void RecordCurrentEnhancementSlashRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => this._targetFactory().RecordCurrentEnhancementSlashRedirect(redirector, useId, previousSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void RedirectCardUseTarget(long frameId, int originalTargetSeat, int redirectedTargetSeat) => this._targetFactory().RedirectCardUseTarget(frameId, originalTargetSeat, redirectedTargetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreProgramAttackEffectsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ProgramCardAttack { get => this._targetFactory().ProgramCardAttack; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramAttackEffectsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed class CoreProgramAttackEffectsCoreDesignatedExtraTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreDesignatedExtraTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _targetFactory = targetFactory;
    public void RecordDesignatedExtraTargetRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => this._targetFactory().RecordDesignatedExtraTargetRedirect(redirector, useId, originalSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramAttackEffectsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime))]
internal sealed class CoreProgramAttackEffectsCoreShortRangeSlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreShortRangeSlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> _targetFactory = targetFactory;
    public void RecordShortRangeSlashRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => this._targetFactory().RecordShortRangeSlashRedirect(redirector, useId, originalSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterMaWanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterMaWanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> _targetFactory = targetFactory;
    public void RecordMaWanHunjiangRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => this._targetFactory().RecordMaWanHunjiangRedirect(redirector, useId, originalSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public void RecordChenglieRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previousSeat, int nextSeat) => this._targetFactory().RecordChenglieRedirect(f, useId, previousSeat, nextSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed class CoreProgramAttackEffectsCoreSoleTargetRedirectAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreSoleTargetRedirectOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _targetFactory = targetFactory;
    public void RecordSoleTargetAfterProtectionRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => this._targetFactory().RecordSoleTargetAfterProtectionRedirect(redirector, useId, previousSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public void RecordKenshangRedirect(global::CardGame.Core.ProgramSkillFrame f, long id, int previous, int next) => this._targetFactory().RecordKenshangRedirect(f, id, previous, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public void RecordPijingRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previousSeat, int nextSeat) => this._targetFactory().RecordPijingRedirect(f, useId, previousSeat, nextSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    public bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => this._targetFactory().HasCardArmorBypass(actor, target, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreProgramAttackEffectsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get => this._targetFactory().ActiveFangtianHalberd; set => this._targetFactory().ActiveFangtianHalberd = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramAttackEffectsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public void RecordZhangYanLangdaoRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => this._targetFactory().RecordZhangYanLangdaoRedirect(redirector, useId, originalSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsSlashProhibited(global::CardGame.Core.CharacterState target) => this._targetFactory().IsSlashProhibited(target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime))]
internal sealed class CoreProgramAttackEffectsCoreCurrentSlashFireOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreCurrentSlashFireOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentSlashFireOffersRuntime> _targetFactory = targetFactory;
    public void RecordCurrentSlashFireRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => this._targetFactory().RecordCurrentSlashFireRedirect(redirector, useId, originalSeat, newSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreProgramAttackEffectsCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public void RecordZixiExtraEffectRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previous, int target) => this._targetFactory().RecordZixiExtraEffectRedirect(f, useId, previous, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed class CoreProgramAttackEffectsCoreDirectedTurnCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackEffectsCoreDirectedTurnCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default) => this._targetFactory().IsDirectedCardTargetProhibited(actorSeat, targetSeat, cardKind, cardUseFrameId);
}
