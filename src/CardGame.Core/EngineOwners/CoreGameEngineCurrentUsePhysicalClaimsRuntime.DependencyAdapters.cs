#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public bool IsXumingClaimedProcessingMaterial(long originalUseId, int cardId) => this._targetFactory().IsXumingClaimedProcessingMaterial(originalUseId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreExtraEffectMaterialClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreExtraEffectMaterialClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> _targetFactory = targetFactory;
    public bool HasExtraEffectMaterialClaim(long useId, int cardId) => this._targetFactory().HasExtraEffectMaterialClaim(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterXinPiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterXinPiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> _targetFactory = targetFactory;
    public bool IsXinPiPhysicalClaim(long useId, int cardId) => this._targetFactory().IsXinPiPhysicalClaim(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsExchangedUseCardClaim(long frameId, int cardId) => this._targetFactory().IsExchangedUseCardClaim(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterYanYanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterYanYanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryYanYanClaimedEntity(long useId, int card) => this._targetFactory().IsBoundaryYanYanClaimedEntity(useId, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterCuiLieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterCuiLieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryCuiLieClaimedUseMaterial(long useId, int cardId) => this._targetFactory().IsAnniversaryCuiLieClaimedUseMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterXiangChongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterXiangChongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiangChongOlRuntime> _targetFactory = targetFactory;
    public bool IsXiangChongClaimedUseMaterial(long useId, int cardId) => this._targetFactory().IsXiangChongClaimedUseMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool IsHuaManClaimedEntity(long useId, int cardId) => this._targetFactory().IsHuaManClaimedEntity(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreOverflowTargetCancellationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreOverflowTargetCancellationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> _targetFactory = targetFactory;
    public bool IsOverflowTargetCancellationPhysicalClaim(long useId, int cardId) => this._targetFactory().IsOverflowTargetCancellationPhysicalClaim(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterHuCheErAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterHuCheErAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId) => this._targetFactory().IsAnniversaryHuCheErClaimedUseMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterPangDeBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterPangDeBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasBoundaryPangDePaidDiscard(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HasBoundaryPangDePaidDiscard(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreShanheNullifiedTrickClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreShanheNullifiedTrickClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> _targetFactory = targetFactory;
    public bool IsShanheNullifiedTrickMaterialClaim(long useId, int cardId) => this._targetFactory().IsShanheNullifiedTrickMaterialClaim(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public bool IsMouDcClaimedEntity(long useId, int card) => this._targetFactory().IsMouDcClaimedEntity(useId, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterZhouCangBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterZhouCangBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryZhouCangPhysicalClaim(long useId, int cardId) => this._targetFactory().IsBoundaryZhouCangPhysicalClaim(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCharacterXuRongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCharacterXuRongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> _targetFactory = targetFactory;
    public bool IsXuRongCausalEntityClaim(long nativeOwnerId, int cardId) => this._targetFactory().IsXuRongCausalEntityClaim(nativeOwnerId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime))]
internal sealed class CoreCurrentUsePhysicalClaimsCoreSourceDingxiNativeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCoreSourceDingxiNativeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> _targetFactory = targetFactory;
    public bool IsSourceDingxiRetainedEntity(long useId, int cardId) => this._targetFactory().IsSourceDingxiRetainedEntity(useId, cardId);
}
