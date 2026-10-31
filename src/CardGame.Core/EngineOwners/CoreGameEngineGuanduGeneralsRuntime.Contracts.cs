#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGuanduGeneralsKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreGuanduGeneralsCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreGuanduGeneralsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreGuanduGeneralsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGuanduGeneralsCoreShanheSourceGrainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheSourceGrainContext(string skillId, global::CardGame.Core.SkillProgramEffectOp operation);
}

internal interface ICoreGuanduGeneralsCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduActivity { get; }

    bool GuanduAllies(int first, int second);
}

internal interface ICoreGuanduGeneralsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreGuanduGeneralsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreGuanduGeneralsCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreGuanduGeneralsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreGuanduGeneralsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreGuanduGeneralsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreGuanduGeneralsCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGuanduGeneralsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreGuanduGeneralsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreGuanduGeneralsCoreGuanduGeneralNativeCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuanduChoulue(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduYuanlve(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduMoushi(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGuanduGeneralsCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool LibraryBaseGrantLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreGuanduGeneralsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreGuanduGeneralsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreGuanduGeneralsCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreGuanduGeneralsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGuanduGeneralsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreGuanduGeneralsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreGuanduGeneralsCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreGuanduGeneralsCoreGuanduGeneralBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuanduShicai(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduZezhu(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduJieliang(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduJieliangClaim(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduCangchu(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduSushou(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduHengjiang(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduHengjiangReward(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduXiying(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduBeizhan(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduZhongjie(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGuanduGeneralsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgram> EnabledSkillPrograms(global::CardGame.Core.CharacterState player);
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreGuanduGeneralsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreGuanduGeneralsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuanduPolu(global::CardGame.Core.ProgramSkillFrame f);
    void ContinueGuanduThunderChariot(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGuanduGeneralsCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}
