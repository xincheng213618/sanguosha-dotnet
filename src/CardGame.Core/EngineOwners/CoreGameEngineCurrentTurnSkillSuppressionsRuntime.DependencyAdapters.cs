#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreGuanduActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreGuanduActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _targetFactory = targetFactory;
    public long CaptureGuanduQualificationStamp(long underlying) => this._targetFactory().CaptureGuanduQualificationStamp(underlying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsDyingWindowGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsDyingWindowGrantQualified(owner, grant);
    public long CaptureDyingWindowQualificationStamp(long underlying) => this._targetFactory().CaptureDyingWindowQualificationStamp(underlying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreNativeFactionChangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreNativeFactionChangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> _targetFactory = targetFactory;
    public long CaptureNativeFactionQualificationStamp(long underlying) => this._targetFactory().CaptureNativeFactionQualificationStamp(underlying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouJieAnniversaryRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterXiahouJieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouJieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterXiahouJieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouJieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryXiahouJieGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsAnniversaryXiahouJieGrantQualified(owner, grant);
    public long CaptureAnniversaryXiahouJieQualificationStamp(long underlying) => this._targetFactory().CaptureAnniversaryXiahouJieQualificationStamp(underlying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsMobileDuoruiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsMobileDuoruiGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreWushuangOriginalSkillRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreWushuangOriginalSkillRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> _targetFactory = targetFactory;
    public bool WushuangYashiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().WushuangYashiGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreShenwuExpandedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreShenwuExpandedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> _targetFactory = targetFactory;
    public bool ShenwuFengdongQualifiesGrant(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().ShenwuFengdongQualifiesGrant(owner, grant);
    public long CaptureShenwuFengdongStamp() => this._targetFactory().CaptureShenwuFengdongStamp();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterZouShiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterZouShiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _targetFactory = targetFactory;
    public bool YunzhengNonLockedGrantQualified(global::CardGame.Core.CharacterState holder, global::CardGame.Core.SkillGrant grant) => this._targetFactory().YunzhengNonLockedGrantQualified(holder, grant);
    public long CaptureYunzhengQualificationStamp(long inner) => this._targetFactory().CaptureYunzhengQualificationStamp(inner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreAlcoholSlashSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreAlcoholSlashSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentTurnOwnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsCurrentTurnOwnSkillGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterCuiLieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterCuiLieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryCuiLieGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsAnniversaryCuiLieGrantQualified(owner, grant);
    public long CaptureAnniversaryCuiLieQualificationStamp(long underlying) => this._targetFactory().CaptureAnniversaryCuiLieQualificationStamp(underlying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreAnqiFeirenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreAnqiFeirenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime> _targetFactory = targetFactory;
    public bool IsAnqiFeirenGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsAnqiFeirenGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterZhangRenMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterZhangRenMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiZrFuanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().ShiZrFuanGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsYinzhiSkillSuppressed(int seat) => this._targetFactory().IsYinzhiSkillSuppressed(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterMaChaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterMaChaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsMobileMaChaoTingweiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsMobileMaChaoTingweiGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateProgramTurnEffectGrant(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsLishiPhaseSuppressed(int seat) => this._targetFactory().IsLishiPhaseSuppressed(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterCaoPiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterCaoPiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> _targetFactory = targetFactory;
    public bool MouCaoPiSkillsQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().MouCaoPiSkillsQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime))]
internal sealed class CoreCurrentTurnSkillSuppressionsCharacterWangGuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnSkillSuppressionsCharacterWangGuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> _targetFactory = targetFactory;
    public bool IsMiuyanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsMiuyanGrantQualified(owner, grant);
    public long CaptureMiuyanQualificationStamp(long underlying) => this._targetFactory().CaptureMiuyanQualificationStamp(underlying);
}
