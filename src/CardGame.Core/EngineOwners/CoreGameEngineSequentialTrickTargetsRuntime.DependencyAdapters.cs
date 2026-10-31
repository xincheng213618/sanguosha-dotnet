#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsSiruiOutsideGeneralWeaponMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreExtraCardEffectRoundsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreExtraCardEffectRoundsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _targetFactory = targetFactory;
    public bool HasUnstartedExtraCardEffect(long id) => this._targetFactory().HasUnstartedExtraCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool KeepLeChengyanBorrowedMaterial(long id) => this._targetFactory().KeepLeChengyanBorrowedMaterial(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsDiouZeroUse(long id) => this._targetFactory().IsDiouZeroUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterFanJiangZhangDaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterFanJiangZhangDaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> _targetFactory = targetFactory;
    public bool IsJueshengDuelUse(long id) => this._targetFactory().IsJueshengDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterCuiLieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterCuiLieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryCuiLieDelayedTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasAnniversaryCuiLieDelayedTail(use);
    public bool ContinueAnniversaryCuiLieDelayedPlacement(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ContinueAnniversaryCuiLieDelayedPlacement(use);
    public bool IsAnniversaryCuiLieDelayedMaterialPlaced(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsAnniversaryCuiLieDelayedMaterialPlaced(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreDamageTargetObtainProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreDamageTargetObtainProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _targetFactory = targetFactory;
    public bool IsIssuedZeroEntityDuel(long id) => this._targetFactory().IsIssuedZeroEntityDuel(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreVirtualOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreVirtualOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public bool IsProgramVirtualOrdinaryTrickUse(long frameId) => this._targetFactory().IsProgramVirtualOrdinaryTrickUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreTargetedOrdinaryTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreTargetedOrdinaryTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _targetFactory = targetFactory;
    public bool HasTargetedOrdinaryTrickTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasTargetedOrdinaryTrickTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card EntityAtCurrentLocation(int id) => this._targetFactory().EntityAtCurrentLocation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreProgramMultiTargetDuelsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreProgramMultiTargetDuelsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> _targetFactory = targetFactory;
    public bool HasProgramIronChainTargetExpansion(int sourceSeat) => this._targetFactory().HasProgramIronChainTargetExpansion(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public bool IsEphemeralCardDestroyedMaterial(long useId, int cardId) => this._targetFactory().IsEphemeralCardDestroyedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreNextCardTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreNextCardTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public bool HasRemainingAdjustedBorrowedSwordTargets(long frameId) => this._targetFactory().HasRemainingAdjustedBorrowedSwordTargets(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public bool KeepMantianGuohaiMaterial(long id) => this._targetFactory().KeepMantianGuohaiMaterial(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterXuHuangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterXuHuangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouXuHuangZeroUse(long id) => this._targetFactory().IsMouXuHuangZeroUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterHuBanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterHuBanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> _targetFactory = targetFactory;
    public bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsHuiyunOutsideGeneralWeaponMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool IsZeroEntityUse(long id) => this._targetFactory().IsZeroEntityUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsClaimedUseCardEntity(long frameId, int cardId) => this._targetFactory().IsClaimedUseCardEntity(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterWenChouMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterWenChouMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouWenChouZeroDuel(long id) => this._targetFactory().IsMouWenChouZeroDuel(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool IsTieredRoundZeroUse(long id) => this._targetFactory().IsTieredRoundZeroUse(id);
    public global::CardGame.Core.Card TieredRoundZeroRepresentation(long id) => this._targetFactory().TieredRoundZeroRepresentation(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public bool IsHellDestroyedSoulMaterial(long useId, int cardId) => this._targetFactory().IsHellDestroyedSoulMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreAdjacentRedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreAdjacentRedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _targetFactory = targetFactory;
    public bool HasAdjacentRedTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasAdjacentRedTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public bool IsOriginalUseLightning(long id) => this._targetFactory().IsOriginalUseLightning(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreDamageTargetBudgetAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreDamageTargetBudgetOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _targetFactory = targetFactory;
    public bool HasDamageTargetBudgetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasDamageTargetBudgetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card) => this._targetFactory().SkipTieredRoundZeroFinishedMovement(id, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreUniqueLeaderTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreUniqueLeaderTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> _targetFactory = targetFactory;
    public bool HasUniqueLeaderTrickTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasUniqueLeaderTrickTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public bool IsMouDcZeroUse(long id) => this._targetFactory().IsMouDcZeroUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreGuanduEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreGuanduEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime> _targetFactory = targetFactory;
    public bool IsGuanduDestroyedChariotMaterial(long useId, int cardId) => this._targetFactory().IsGuanduDestroyedChariotMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouXqJueyanUse(long id) => this._targetFactory().IsMouXqJueyanUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreSequentialTrickTargetsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterXunXuAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterXunXuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryFinishAnniversaryXunXuUseMaterials(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryFinishAnniversaryXunXuUseMaterials(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime))]
internal sealed class CoreSequentialTrickTargetsCoreSameTypeActualUseAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCoreSameTypeActualUseAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _targetFactory = targetFactory;
    public bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasSameTypeAidTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime))]
internal sealed class CoreSequentialTrickTargetsCharacterZhouYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSequentialTrickTargetsCharacterZhouYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouZyZeroFireUse(long id) => this._targetFactory().IsMouZyZeroFireUse(id);
}
