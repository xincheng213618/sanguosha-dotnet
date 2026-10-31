#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNextActualUseTargetAdjustmentsCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<int> GetDeclaredCardTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalActionKind actionKind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
    bool CanUseZhangbaSerpentSpear(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreGuanHaiBridgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreNextActualUseTargetAdjustmentsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoQuanyuCompleteZhangbaQuotaLegal(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualTurnLegacyVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNextActualUseTargetAdjustmentsCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ShenZfOrdinaryTrickOptions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ordinary, global::CardGame.Core.Suit? suit, bool? color, global::CardGame.Core.Suit? shield, bool excludeOwner);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreNextActualUseTargetAdjustmentsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId);
}

internal interface ICoreNextActualUseTargetAdjustmentsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeExtraNextCardTarget(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? effectiveUseSuit = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
}

internal interface ICoreNextActualUseTargetAdjustmentsCharacterJianYongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryJianYongAdjustmentCurrent(global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent grant);
    bool BoundaryJianYongAdjustmentConsumes(global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent grant, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeMaterialSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteNativeMaterialSlash(long id, global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> seats, global::CardGame.Core.CardConversionSource? conversion, bool armor);
    bool TryGetNativeMaterialSlashExpansion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets, out int ordinaryMaximum, out global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramSlashTargetExpansion> claims);
    void BeginNativeMaterialSlashExpansion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::System.Collections.Generic.IReadOnlyList<int> seats);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNextActualUseTargetAdjustmentsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId);
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreNextActualUseTargetAdjustmentsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> CardMovements { get; }
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreNextActualUseTargetAdjustmentsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder);
}

internal interface ICoreNextActualUseTargetAdjustmentsCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHpLossSlashTargets(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.CommandError? ValidateHpLossSlashZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets);
    void ResolveHpLossSlashZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNextActualUseTargetAdjustmentsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
