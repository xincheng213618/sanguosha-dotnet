#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentUsePhysicalClaimsCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXumingClaimedProcessingMaterial(long originalUseId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCurrentUsePhysicalClaimsCoreExtraEffectMaterialClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExtraEffectMaterialClaim(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXinPiPhysicalClaim(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExchangedUseCardClaim(long frameId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCurrentUsePhysicalClaimsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentUsePhysicalClaimsCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterYanYanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryYanYanClaimedEntity(long useId, int card);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryCuiLieClaimedUseMaterial(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterXiangChongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXiangChongClaimedUseMaterial(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuaManClaimedEntity(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreOverflowTargetCancellationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOverflowTargetCancellationPhysicalClaim(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterHuCheErAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterPangDeBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryPangDePaidDiscard(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreShanheNullifiedTrickClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheNullifiedTrickMaterialClaim(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDcClaimedEntity(long useId, int card);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterZhouCangBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryZhouCangPhysicalClaim(long useId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCharacterXuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXuRongCausalEntityClaim(long nativeOwnerId, int cardId);
}

internal interface ICoreCurrentUsePhysicalClaimsCoreSourceDingxiNativeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSourceDingxiRetainedEntity(long useId, int cardId);
}
