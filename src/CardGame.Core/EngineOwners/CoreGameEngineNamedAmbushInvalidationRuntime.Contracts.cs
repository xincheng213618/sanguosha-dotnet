#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNamedAmbushInvalidationContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNamedAmbushInvalidationCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNamedAmbushInvalidationCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreNamedAmbushInvalidationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNamedAmbushInvalidationCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreNamedAmbushInvalidationCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat);
}

internal interface ICoreNamedAmbushInvalidationCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreNamedAmbushInvalidationCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNamedAmbushInvalidationCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNamedAmbushInvalidationCoreNamedAmbushOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetAmbushUse(global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.NamedAmbushUseIdentity identity, out global::CardGame.Core.CardActionContext action);
    bool HasNamedAmbushCapability { get; }

    global::CardGame.Core.NamedAmbushDeposit[] LiveAmbushDeposits(int owner, string? skill = default, string? instance = default);
}

internal interface ICoreNamedAmbushInvalidationCoreDrawFundedDistinctBasicResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearDrawFundedDistinctBasicDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNamedAmbushInvalidationCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreNamedAmbushInvalidationCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated);
}

internal interface ICoreNamedAmbushInvalidationCoreRoundOnceCardlessNativeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearRoundOnceCardlessDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNamedAmbushInvalidationCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearTieredRoundDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNamedAmbushInvalidationCharacterLiJueOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureLiJueCompletedNullificationUse(long windowId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNamedAmbushInvalidationCharacterHuangZhongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryHzUseIneffective(global::CardGame.Core.CardUseFrame use);
    bool ContinueAnniversaryHzIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    bool HasAnniversaryHuangZhongCapability { get; }

    bool AnniversaryHzCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNamedAmbushInvalidationCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanUseIneffective(global::CardGame.Core.CardUseFrame use);
    bool ContinueHappyWeiYanIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    bool HasHappyWeiYanEzhiCapability { get; }

    bool HappyWeiYanCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNamedAmbushInvalidationCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNamedAmbushInvalidationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNamedAmbushInvalidationCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next);
}

internal interface ICoreNamedAmbushInvalidationCharacterChengYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouCyUseIneffective(global::CardGame.Core.CardUseFrame use);
    bool ContinueMouCyIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    bool HasMouChengYuAnniversary { get; }

    bool MouCyCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreNamedAmbushInvalidationCoreCompletedCategoryUseProofOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext completed);
    bool RebuildAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext accepted, global::CardGame.Core.CardActionContext completed, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int acceptedIndex);
}

internal interface ICoreNamedAmbushInvalidationCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearChainedStateBasicDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNamedAmbushInvalidationCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}
