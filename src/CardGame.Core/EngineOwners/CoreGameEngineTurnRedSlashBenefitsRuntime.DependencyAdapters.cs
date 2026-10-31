#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().CanUseProvidedSlashTarget(owner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterMaMiDiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterMaMiDiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> _targetFactory = targetFactory;
    public bool HasMaMiDiCapability { get => this._targetFactory().HasMaMiDiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterRuanYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterRuanYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _targetFactory = targetFactory;
    public bool HasRuanYuCapability { get => this._targetFactory().HasRuanYuCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreAdjacentRedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreAdjacentRedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _targetFactory = targetFactory;
    public bool HasAdjacentRedTargetCapability { get => this._targetFactory().HasAdjacentRedTargetCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreFinalTargetSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreFinalTargetSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetSlashRuntime> _targetFactory = targetFactory;
    public bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HasRankSlashRange(actor, kind);
    public bool IsSpecificRankProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false) => this._targetFactory().IsSpecificRankProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterHanDangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterHanDangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouHanDangCapability { get => this._targetFactory().HasMouHanDangCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreTargetedOrdinaryTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreTargetedOrdinaryTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _targetFactory = targetFactory;
    public bool HasTargetedOrdinaryTrickAdjustment { get => this._targetFactory().HasTargetedOrdinaryTrickAdjustment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateProgramTurnEffectGrant(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreNativeQinglongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreNativeQinglongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _targetFactory = targetFactory;
    public bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => this._targetFactory().CanUseQinglongCrescentBladeTarget(source, target, effectiveKind, effectiveColor, allowAnyColor, physicalSuit, effectiveRank, physicalCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool TracksActionDiscardColor { get => this._targetFactory().TracksActionDiscardColor; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterLiuZanMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterLiuZanMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> _targetFactory = targetFactory;
    public bool TracksAlternateActualUseColor { get => this._targetFactory().TracksAlternateActualUseColor; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasLiuPanCapability { get => this._targetFactory().HasLiuPanCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterWenChouMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterWenChouMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouWenChouOl { get => this._targetFactory().HasMouWenChouOl; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false) => this._targetFactory().GetSlashUseCards(owner, ignoreSuitUseProhibition);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreSourceCursesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreSourceCursesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> _targetFactory = targetFactory;
    public bool HasSourceCurseActionColors { get => this._targetFactory().HasSourceCurseActionColors; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiMouMobileRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterLuZhiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterLuZhiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouLuZhiCapability { get => this._targetFactory().HasMouLuZhiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterLiuYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterLiuYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> _targetFactory = targetFactory;
    public bool HasLiuYanCapability { get => this._targetFactory().HasLiuYanCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuLuKangDixianCapability { get => this._targetFactory().HasWuLuKangDixianCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool HasWuJingLiubing { get => this._targetFactory().HasWuJingLiubing; }
    public bool HasWuJingHeji { get => this._targetFactory().HasWuJingHeji; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreSoleTargetProtectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreSoleTargetProtectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _targetFactory = targetFactory;
    public bool HasSoleTargetProtectionCapability { get => this._targetFactory().HasSoleTargetProtectionCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreDamageTargetBudgetAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreDamageTargetBudgetOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _targetFactory = targetFactory;
    public bool TracksDamageTargetBudgetTargets { get => this._targetFactory().TracksDamageTargetBudgetTargets; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreComparedBlackSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreComparedBlackSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineComparedBlackSlashPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasComparedBlackSlashPolicies { get => this._targetFactory().HasComparedBlackSlashPolicies; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterQuHuangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterQuHuangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> _targetFactory = targetFactory;
    public bool HasQuHuangCapability { get => this._targetFactory().HasQuHuangCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PhysicalGroupSuit(owner, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreCurrentTurnShownCardBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreCurrentTurnShownCardBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit) => this._targetFactory().HasSlashUseDistanceBySuit(owner, kind, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasBlackTrickTargetPolicy { get => this._targetFactory().HasBlackTrickTargetPolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuyouFactionPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false) => this._targetFactory().IsWuyouFactionPaymentLegal(pending, kind, cards, isTrueZhangba);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed class CoreTurnRedSlashBenefitsCoreDirectedCardPolicyQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTurnRedSlashBenefitsCoreDirectedCardPolicyQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _targetFactory = targetFactory;
    public bool HasYuanchouCardPolicy { get => this._targetFactory().HasYuanchouCardPolicy; }
}
