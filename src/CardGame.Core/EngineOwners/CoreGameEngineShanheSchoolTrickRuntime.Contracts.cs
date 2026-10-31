#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSchoolTrickCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreShanheSchoolTrickCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingAdjustedSimpleTargets(long frameId);
}

internal interface ICoreShanheSchoolTrickCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreShanheSchoolTrickCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreShanheSchoolTrickContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShanheSchoolTrickCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedFeiYiExtraEffect(long id);
    global::CardGame.Core.ExtraCardEffectState? NativeExtraEffectState(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreShanheSchoolTrickCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreShanheSchoolTrickCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreShanheSchoolTrickKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheSchoolTrickCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingSequentialTrickTargets(long frameId);
}

internal interface ICoreShanheSchoolTrickResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheSchoolTrickCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedZixiExtraEffect(long id);
}

internal interface ICoreShanheSchoolTrickCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreShanheSchoolTrickCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheSchoolTrickCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreShanheSchoolTrickCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreShanheSchoolTrickCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedMouBingcaiExtraEffect(long id);
}

internal interface ICoreShanheSchoolTrickCoreShanheSchoolsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheSchoolEffect(int ownerSeat, int schoolId);
}

internal interface ICoreShanheSchoolTrickCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedMouHqRound(long id);
}

internal interface ICoreShanheSchoolTrickCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreShanheSchoolTrickCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreShanheSchoolTrickCharacterJiangJiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedMouJjEffect(long id);
}

internal interface ICoreShanheSchoolTrickCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedMoQuanyuExtraRound(long id);
}

internal interface ICoreShanheSchoolTrickCoreSoleIncomingEffectRepeatOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedSoleIncomingEffect(long id);
}
