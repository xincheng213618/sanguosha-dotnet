#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterHuCheErMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterHuCheErMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _targetFactory = targetFactory;
    public bool IsMouHuCheErUse(long id) => this._targetFactory().IsMouHuCheErUse(id);
    public global::CardGame.Core.Card MouHuCheErRepresentation(long id) => this._targetFactory().MouHuCheErRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterHuLieMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterHuLieMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> _targetFactory = targetFactory;
    public bool IsMouHuLieUse(long id) => this._targetFactory().IsMouHuLieUse(id);
    public global::CardGame.Core.Card MouHuLieRepresentation(long id) => this._targetFactory().MouHuLieRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterLiuYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterLiuYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> _targetFactory = targetFactory;
    public bool IsLiuYanLegacyAlcoholUse(long id) => this._targetFactory().IsLiuYanLegacyAlcoholUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool IsIssuedTieredRoundUse(global::CardGame.Core.CardUseFrame use, bool zeroOnly = false) => this._targetFactory().IsIssuedTieredRoundUse(use, zeroOnly);
    public bool IsTieredRoundZeroUse(long id) => this._targetFactory().IsTieredRoundZeroUse(id);
    public global::CardGame.Core.Card TieredRoundZeroRepresentation(long id) => this._targetFactory().TieredRoundZeroRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool IsQinLangXianyingUse(long id) => this._targetFactory().IsQinLangXianyingUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public bool IsZongluanUse(long id) => this._targetFactory().IsZongluanUse(id);
    public global::CardGame.Core.Card ZongluanRepresentation(long id) => this._targetFactory().ZongluanRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsYouqueUse(long id) => this._targetFactory().IsYouqueUse(id);
    public global::CardGame.Core.Card YouqueRepresentation(long id) => this._targetFactory().YouqueRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreNativeTrickUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreNativeTrickUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId) => this._targetFactory().GetCardUsePhysicalCards(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool IsHuanpeiUse(long id) => this._targetFactory().IsHuanpeiUse(id);
    public global::CardGame.Core.Card HuanpeiRepresentation(long id) => this._targetFactory().HuanpeiRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get => this._targetFactory().ActiveQinglongFollowup; set => this._targetFactory().ActiveQinglongFollowup = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuLuKangKegouUse(long id) => this._targetFactory().IsWuLuKangKegouUse(id);
    public global::CardGame.Core.Card WuLuKangKegouRepresentation(long id) => this._targetFactory().WuLuKangKegouRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ReadCardAppearance(global::CardGame.Core.CardAppearanceReference appearance) => this._targetFactory().ReadCardAppearance(appearance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public bool IsRoundOnceCardlessBasicUse(long id) => this._targetFactory().IsRoundOnceCardlessBasicUse(id);
    public global::CardGame.Core.Card RoundOnceCardlessNativeRepresentation(long id) => this._targetFactory().RoundOnceCardlessNativeRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public bool IsMouBingcaiLegacyAlcoholUse(long id) => this._targetFactory().IsMouBingcaiLegacyAlcoholUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool IsWeijieUse(long id) => this._targetFactory().IsWeijieUse(id);
    public global::CardGame.Core.Card WeijieRepresentation(long id) => this._targetFactory().WeijieRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool IsMouZhijueZeroUse(long id) => this._targetFactory().IsMouZhijueZeroUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterCuiYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterCuiYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _targetFactory = targetFactory;
    public bool IsCuiYanLegacyAlcoholUse(long id) => this._targetFactory().IsCuiYanLegacyAlcoholUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue) => this._targetFactory().ParticipantHandRescueObserverRide(frame, parent, rescue);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterBaoSanNiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterBaoSanNiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> _targetFactory = targetFactory;
    public bool IsChosenAssaultUse(long id) => this._targetFactory().IsChosenAssaultUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool IsZonghuUse(long id) => this._targetFactory().IsZonghuUse(id);
    public global::CardGame.Core.Card ZonghuRepresentation(long id) => this._targetFactory().ZonghuRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterZhangXiuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterZhangXiuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouZhangXiuAnniversaryUse(long id) => this._targetFactory().IsMouZhangXiuAnniversaryUse(id);
    public global::CardGame.Core.Card MouZhangXiuAnniversaryRepresentation(long id) => this._targetFactory().MouZhangXiuAnniversaryRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterAHuiNanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterAHuiNanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> _targetFactory = targetFactory;
    public bool IsAHuiNanJuemanUse(long id) => this._targetFactory().IsAHuiNanJuemanUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => this._targetFactory().IsCurrentUsePhysicalCardClaim(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterTianChouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterTianChouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> _targetFactory = targetFactory;
    public bool IsTianChouShandaoUse(long id) => this._targetFactory().IsTianChouShandaoUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCharacterZhouYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCharacterZhouYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouZyZeroFireUse(long id) => this._targetFactory().IsMouZyZeroFireUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime))]
internal sealed class CoreTieredRoundZeroReturnsCoreCappedConversionBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTieredRoundZeroReturnsCoreCappedConversionBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> _targetFactory = targetFactory;
    public bool ValidCappedConversionBenefit(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidCappedConversionBenefit(f);
}
