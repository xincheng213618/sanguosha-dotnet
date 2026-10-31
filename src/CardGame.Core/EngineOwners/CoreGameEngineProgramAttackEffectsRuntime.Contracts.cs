#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramAttackEffectsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordWeifuRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int nextSeat);
}

internal interface ICoreProgramAttackEffectsCharacterDaQiaoMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? PostCostSlashRedirectFrame(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int owner);
    bool IsPostCostSlashRedirectTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int owner, int target, int? excludedEquipment);
    bool HasPostCostSlashRedirectPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target);
}

internal interface ICoreProgramAttackEffectsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramAttackEffectsCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBingzhongRedirect(global::CardGame.Core.ProgramSkillFrame frame, long useId, int previousSeat, int nextSeat);
}

internal interface ICoreProgramAttackEffectsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramAttackEffectsCoreCurrentEnhancementSlashRedirectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCurrentEnhancementSlashRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RedirectCardUseTarget(long frameId, int originalTargetSeat, int redirectedTargetSeat);
}

internal interface ICoreProgramAttackEffectsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ProgramCardAttack { get; }
}

internal interface ICoreProgramAttackEffectsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramAttackEffectsCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordDesignatedExtraTargetRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreProgramAttackEffectsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramAttackEffectsCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordShortRangeSlashRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramAttackEffectsCharacterMaWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordMaWanHunjiangRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordChenglieRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previousSeat, int nextSeat);
}

internal interface ICoreProgramAttackEffectsCoreSoleTargetRedirectOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSoleTargetAfterProtectionRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordKenshangRedirect(global::CardGame.Core.ProgramSkillFrame f, long id, int previous, int next);
}

internal interface ICoreProgramAttackEffectsCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordPijingRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previousSeat, int nextSeat);
}

internal interface ICoreProgramAttackEffectsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramAttackEffectsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }
}

internal interface ICoreProgramAttackEffectsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreProgramAttackEffectsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordZhangYanLangdaoRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramAttackEffectsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCurrentSlashFireRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat);
}

internal interface ICoreProgramAttackEffectsCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreProgramAttackEffectsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramAttackEffectsCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordZixiExtraEffectRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previous, int target);
}

internal interface ICoreProgramAttackEffectsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}
