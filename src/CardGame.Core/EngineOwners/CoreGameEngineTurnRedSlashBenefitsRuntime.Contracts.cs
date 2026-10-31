#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTurnRedSlashBenefitsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreTurnRedSlashBenefitsCharacterMaMiDiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaMiDiCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterRuanYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuanYuCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreAdjacentRedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAdjacentRedTargetCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
    bool IsSpecificRankProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false);
}

internal interface ICoreTurnRedSlashBenefitsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterHanDangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouHanDangCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreTurnRedSlashBenefitsCoreTargetedOrdinaryTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetedOrdinaryTrickAdjustment { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreTurnRedSlashBenefitsCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreTurnRedSlashBenefitsCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksActionDiscardColor { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterLiuZanMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksAlternateActualUseColor { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuPanCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreTurnRedSlashBenefitsCharacterWenChouMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouWenChouOl { get; }
}

internal interface ICoreTurnRedSlashBenefitsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
}

internal interface ICoreTurnRedSlashBenefitsCoreSourceCursesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSourceCurseActionColors { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterLuZhiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouLuZhiCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuYanCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuLuKangDixianCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuJingLiubing { get; }

    bool HasWuJingHeji { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreDamageTargetBudgetOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksDamageTargetBudgetTargets { get; }
}

internal interface ICoreTurnRedSlashBenefitsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreTurnRedSlashBenefitsCoreComparedBlackSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasComparedBlackSlashPolicies { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterQuHuangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQuHuangCapability { get; }
}

internal interface ICoreTurnRedSlashBenefitsCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreTurnRedSlashBenefitsCoreCurrentTurnShownCardBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreTurnRedSlashBenefitsCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBlackTrickTargetPolicy { get; }
}

internal interface ICoreTurnRedSlashBenefitsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuyouFactionPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false);
}

internal interface ICoreTurnRedSlashBenefitsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasYuanchouCardPolicy { get; }
}
