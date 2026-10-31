#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreNativeCardUseCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareZongluanSlashAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareZongluanSlashAction(action);
    public void BindZongluanSlashUse(long id) => this._targetFactory().BindZongluanSlashUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreNativeCardUseCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void IssueMouZhaJiangUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueMouZhaJiangUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed class CoreNativeCardUseCoreFinalizedCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreFinalizedCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetImplicitSelfCardUseTargets(global::CardGame.Core.CardKind kind, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().GetImplicitSelfCardUseTargets(kind, sourceSeat, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime))]
internal sealed class CoreNativeCardUseCharacterZhangXiuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterZhangXiuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouZhangXiuAnniversarySlashAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouZhangXiuAnniversarySlashAction(action);
    public void BindMouZhangXiuAnniversarySlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindMouZhangXiuAnniversarySlashUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime))]
internal sealed class CoreNativeCardUseCharacterKongShuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterKongShuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> _targetFactory = targetFactory;
    public void BindLeiluanNativeUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindLeiluanNativeUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreNativeCardUseCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public void BindJianjiNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindJianjiNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime))]
internal sealed class CoreNativeCardUseCoreNonBasicSlashProfileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreNonBasicSlashProfileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> _targetFactory = targetFactory;
    public void IssueNonBasicSlashPolicy(long useId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueNonBasicSlashPolicy(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreNativeCardUseCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public void BindZhangYanSujiUse(long id) => this._targetFactory().BindZhangYanSujiUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime))]
internal sealed class CoreNativeCardUseCharacterDuanJiongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterDuanJiongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> _targetFactory = targetFactory;
    public void BindDuanJiongSaoguNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindDuanJiongSaoguNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed class CoreNativeCardUseCoreChainedStateBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreChainedStateBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareChainedStateBasicAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ChainedStateBasicPayment? payment) => this._targetFactory().PrepareChainedStateBasicAction(action, payment);
    public void IssueChainedStateBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChainedStateBasicPayment? payment) => this._targetFactory().IssueChainedStateBasicUse(use, payment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed class CoreNativeCardUseCoreRoundOnceCardlessProgramRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreRoundOnceCardlessProgramRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _targetFactory = targetFactory;
    public void BindRoundOnceCardlessProgramRequestUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindRoundOnceCardlessProgramRequestUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreNativeCardUseCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareZonghuAction(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ZonghuPaidOrigin? p) => this._targetFactory().PrepareZonghuAction(a, p);
    public void IssueZonghuUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ZonghuPaidOrigin? p) => this._targetFactory().IssueZonghuUse(use, p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreNativeCardUseCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind) => this._targetFactory().RecordYingboCardUse(resolutionId, sourceSeat, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed class CoreNativeCardUseCoreProvenanceCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreProvenanceCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _targetFactory = targetFactory;
    public void IssueProvenanceUsePolicy(long useId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueProvenanceUsePolicy(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeCardUseCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CoreNativeCardUseCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasTurnRedSlashPolicyForColor(int actor, global::CardGame.Core.CardKind kind, bool? color) => this._targetFactory().HasTurnRedSlashPolicyForColor(actor, kind, color);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreNativeCardUseCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareWeijieAction(global::CardGame.Core.CardActionContext a, global::CardGame.Core.WeijiePaidOrigin? p) => this._targetFactory().PrepareWeijieAction(a, p);
    public void IssueWeijieUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WeijiePaidOrigin? p) => this._targetFactory().IssueWeijieUse(use, p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreNativeCardUseCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareHuanpeiAction(global::CardGame.Core.CardActionContext a, global::CardGame.Core.HuanpeiPaidOrigin? p) => this._targetFactory().PrepareHuanpeiAction(a, p);
    public void IssueHuanpeiUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HuanpeiPaidOrigin? p) => this._targetFactory().IssueHuanpeiUse(use, p);
    public void BindWenyiNativeUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindWenyiNativeUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreNativeCardUseCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public void CaptureMoQuanyuQuota(global::CardGame.Core.CardUseFrame use) => this._targetFactory().CaptureMoQuanyuQuota(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime))]
internal sealed class CoreNativeCardUseCharacterZhangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterZhangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> _targetFactory = targetFactory;
    public void BindZhangJiNativeUse(long useId) => this._targetFactory().BindZhangJiNativeUse(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed class CoreNativeCardUseCharacterZhouYuOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterZhouYuOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _targetFactory = targetFactory;
    public void BindMouZuifengAlcoholUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindMouZuifengAlcoholUse(supplied);
    public void IssueMouGuquUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().IssueMouGuquUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime))]
internal sealed class CoreNativeCardUseCharacterGuanYuRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterGuanYuRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> _targetFactory = targetFactory;
    public bool GuanFirstRedAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Suit? suit) => this._targetFactory().GuanFirstRedAvailable(owner, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CoreNativeCardUseCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public void BindBoundaryLiuFengNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindBoundaryLiuFengNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime))]
internal sealed class CoreNativeCardUseCharacterYaDanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterYaDanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> _targetFactory = targetFactory;
    public void BindQingyaNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindQingyaNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed class CoreNativeCardUseCharacterCaiWenJiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterCaiWenJiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _targetFactory = targetFactory;
    public void IssueBeifenUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueBeifenUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed class CoreNativeCardUseCharacterLiuChengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLiuChengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouLiuChengAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouLiuChengAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreNativeCardUseCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> MoGangquanNativeTargets(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<int> original) => this._targetFactory().MoGangquanNativeTargets(actor, card, kind, source, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime))]
internal sealed class CoreNativeCardUseCharacterHuBanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuBanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> _targetFactory = targetFactory;
    public void IssueHuiyunUse(long id, global::CardGame.Core.CardActionContext? a) => this._targetFactory().IssueHuiyunUse(id, a);
    public void BindHuiyunFollowupUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindHuiyunFollowupUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreNativeCardUseCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordActualPlayPhaseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreNativeCardUseCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public void BindSuyiEquipmentUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindSuyiEquipmentUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime))]
internal sealed class CoreNativeCardUseCharacterCaoAngMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterCaoAngMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> _targetFactory = targetFactory;
    public void BindMouCaNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouCaNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreNativeCardUseCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public void BindWuJingHejiUse(long id) => this._targetFactory().BindWuJingHejiUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime))]
internal sealed class CoreNativeCardUseCharacterXiaoQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterXiaoQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> _targetFactory = targetFactory;
    public void IssueWeiwanUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueWeiwanUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreNativeCardUseCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public void BeginMouSmSecretUse(long useId, long eventStart, long logStart) => this._targetFactory().BeginMouSmSecretUse(useId, eventStart, logStart);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime))]
internal sealed class CoreNativeCardUseCharacterPeiYuanShaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterPeiYuanShaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPeiYuanShaoOlRuntime> _targetFactory = targetFactory;
    public void IssueFulueCounterUse(long useId, global::CardGame.Core.CardActionContext action) => this._targetFactory().IssueFulueCounterUse(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreNativeCardUseCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouDcAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouDcAction(action);
    public void BindMouDcUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouDcUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime))]
internal sealed class CoreNativeCardUseCharacterLiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> _targetFactory = targetFactory;
    public void BindChanshuangSlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindChanshuangSlashUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime))]
internal sealed class CoreNativeCardUseCharacterJiaXuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterJiaXuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiaXuMouRuntime> _targetFactory = targetFactory;
    public void BindMouJxNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouJxNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreNativeCardUseCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public void BindSayingSlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindSayingSlashUse(supplied);
    public void BindDongxinEntityUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindDongxinEntityUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreNativeCardUseCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareDiouAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareDiouAction(action);
    public void IssueDiouUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueDiouUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed class CoreNativeCardUseCoreProgramFactionActionRewardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreProgramFactionActionRewardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureFactionAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime))]
internal sealed class CoreNativeCardUseCoreDrawFundedDistinctBasicUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreDrawFundedDistinctBasicUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareDrawFundedDistinctBasicAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.DrawFundedDistinctBasicPayment? payment) => this._targetFactory().PrepareDrawFundedDistinctBasicAction(action, payment);
    public void IssueDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DrawFundedDistinctBasicPayment? payment) => this._targetFactory().IssueDrawFundedDistinctBasicUse(use, payment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime))]
internal sealed class CoreNativeCardUseCharacterJuShouMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterJuShouMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> _targetFactory = targetFactory;
    public void BindMouJsNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouJsNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed class CoreNativeCardUseCharacterGuoSiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterGuoSiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _targetFactory = targetFactory;
    public void BindGuoSiAcceptedUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindGuoSiAcceptedUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed class CoreNativeCardUseCharacterZhouYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterZhouYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> _targetFactory = targetFactory;
    public void BindMouZyNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouZyNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime))]
internal sealed class CoreNativeCardUseCharacterXuHuangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterXuHuangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouXuHuangAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouXuHuangAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeCardUseCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed class CoreNativeCardUseCharacterChengYuMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterChengYuMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouDanchiAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouDanchiAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime))]
internal sealed class CoreNativeCardUseCoreGiftedSlashUseRewardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreGiftedSlashUseRewardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> _targetFactory = targetFactory;
    public void IssueGiftedSlashUseBenefit(long useId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueGiftedSlashUseBenefit(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed class CoreNativeCardUseCoreColorFireAttacksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreColorFireAttacksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ColorFireAttackReceipt? FreezeColorFireAttackPolicy(int sourceSeat, global::CardGame.Core.CardActionContext? action) => this._targetFactory().FreezeColorFireAttackPolicy(sourceSeat, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreNativeCardUseCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareShenDaCuixinAction(global::CardGame.Core.CardActionContext a) => this._targetFactory().PrepareShenDaCuixinAction(a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreNativeCardUseCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime))]
internal sealed class CoreNativeCardUseCoreNoDamageSkillDisableAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreNoDamageSkillDisableOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> _targetFactory = targetFactory;
    public void IssueNoDamageSkillDisablePolicy(long frameId) => this._targetFactory().IssueNoDamageSkillDisablePolicy(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreNativeCardUseCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment? TransferDeclarationToCardUse(long useId) => this._targetFactory().TransferDeclarationToCardUse(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime))]
internal sealed class CoreNativeCardUseCoreRoundGainedSourceUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreRoundGainedSourceUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedSourceUsesRuntime> _targetFactory = targetFactory;
    public void IssueRoundGainedUseQualifications(long useId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueRoundGainedUseQualifications(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime))]
internal sealed class CoreNativeCardUseCoreShownEntityTurnPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreShownEntityTurnPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _targetFactory = targetFactory;
    public void IssueShownEntityUseBenefits(long useId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueShownEntityUseBenefits(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime))]
internal sealed class CoreNativeCardUseCharacterFuRongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterFuRongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> _targetFactory = targetFactory;
    public void BindFuRongNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindFuRongNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed class CoreNativeCardUseCharacterWenChouMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterWenChouMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouWenChouAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouWenChouAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed class CoreNativeCardUseCharacterHuCheErMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuCheErMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouHuCheErSlashAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouHuCheErSlashAction(action);
    public void BindMouHuCheErSlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindMouHuCheErSlashUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreNativeCardUseCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareYouqueSlashAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareYouqueSlashAction(action);
    public global::CardGame.Core.CardActionContext PrepareXieweiAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareXieweiAction(action);
    public void BindYouqueSlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindYouqueSlashUse(supplied);
    public void PrepareXieweiNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().PrepareXieweiNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreNativeCardUseCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public void IssueWuyouUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueWuyouUse(use);
    public void IssueJuewuUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().IssueJuewuUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreNativeCardUseCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreNativeCardUseCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public void BindMouLixianNativeUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindMouLixianNativeUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed class CoreNativeCardUseCharacterLvKuangLvXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLvKuangLvXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _targetFactory = targetFactory;
    public void BindQigongNativeUse(long id) => this._targetFactory().BindQigongNativeUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime))]
internal sealed class CoreNativeCardUseCharacterHuLieMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterHuLieMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouHuLieSlashAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouHuLieSlashAction(action);
    public void BindMouHuLieSlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindMouHuLieSlashUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime))]
internal sealed class CoreNativeCardUseCoreSourceDingxiNativeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreSourceDingxiNativeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> _targetFactory = targetFactory;
    public void BindSourceDingxiNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindSourceDingxiNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreNativeCardUseCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareMouXqJueyanAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareMouXqJueyanAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed class CoreNativeCardUseCoreFriendEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreFriendEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> _targetFactory = targetFactory;
    public void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action) => this._targetFactory().CaptureFriendMinimumHandUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreNativeCardUseCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public void PrepareNativePublicPileUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().PrepareNativePublicPileUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime))]
internal sealed class CoreNativeCardUseCharacterChenLinMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterChenLinMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> _targetFactory = targetFactory;
    public void BindMouChenLinNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouChenLinNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreNativeCardUseCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasFirstActualPlayUseDistance(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasFirstActualPlayUseDistance(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreNativeCardUseCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public bool TracksPlayCardHistory { get => this._targetFactory().TracksPlayCardHistory; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime))]
internal sealed class CoreNativeCardUseCharacterWangYunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterWangYunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> _targetFactory = targetFactory;
    public void BindWangYunNativeUse(long useId) => this._targetFactory().BindWangYunNativeUse(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime))]
internal sealed class CoreNativeCardUseCoreGrantedEntityPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreGrantedEntityPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> _targetFactory = targetFactory;
    public void IssueGrantedPhaseEntityUseDistance(long frameId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueGrantedPhaseEntityUseDistance(frameId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime))]
internal sealed class CoreNativeCardUseCharacterWangGuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterWangGuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> _targetFactory = targetFactory;
    public void IssueMiuyanUse(long useId, global::CardGame.Core.CardActionContext? action) => this._targetFactory().IssueMiuyanUse(useId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreNativeCardUseCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext PrepareWuLuKangKegouSlashAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().PrepareWuLuKangKegouSlashAction(action);
    public void BindWuLuKangKegouSlashUse(global::CardGame.Core.CardUseFrame supplied) => this._targetFactory().BindWuLuKangKegouSlashUse(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreNativeCardUseCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public void IssueKenshangUse(long id, global::CardGame.Core.CardActionContext action) => this._targetFactory().IssueKenshangUse(id, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreNativeCardUseCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().RecordProgramUsedBasicCard(actorSeat, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreNativeCardUseCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public void IssueSiruiUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueSiruiUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeCardUseCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext? CaptureCardUseAction(global::CardGame.Core.Card card, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.CardConversionSource? explicitConversion = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversions = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false) => this._targetFactory().CaptureCardUseAction(card, actorSeat, targets, effectiveKind, physicalIds, explicitConversion, additionalConversions, designatedTargetSeats, isTrueZhangbaSlash);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime))]
internal sealed class CoreNativeCardUseCharacterJiangJiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterJiangJiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> _targetFactory = targetFactory;
    public void BindMouJjNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindMouJjNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed class CoreNativeCardUseCharacterKeBiNengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterKeBiNengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _targetFactory = targetFactory;
    public void BindKeBiNengPingduanNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindKeBiNengPingduanNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreNativeCardUseCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public void BindCaoChunNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().BindCaoChunNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreNativeCardUseCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public void IssueTieredRoundConversionUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IssueTieredRoundConversionUse(use);
}
