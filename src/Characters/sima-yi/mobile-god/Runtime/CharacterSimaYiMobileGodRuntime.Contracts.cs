#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterSimaYiMobileGodCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterSimaYiMobileGodCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQizhengAfterResponse(long id);
}

internal interface ICharacterSimaYiMobileGodMobileSimaRenjieOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MobileSimaRenjieRuntime Get();
}

internal interface ICharacterSimaYiMobileGodCoreProgramMarkerPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId);
}

internal interface ICharacterSimaYiMobileGodCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterSimaYiMobileGodCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICharacterSimaYiMobileGodCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICharacterSimaYiMobileGodCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> skillIds);
}

internal interface ICharacterSimaYiMobileGodKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterSimaYiMobileGodCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICharacterSimaYiMobileGodContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterSimaYiMobileGodCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICharacterSimaYiMobileGodCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next);
}

internal interface ICharacterSimaYiMobileGodResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterSimaYiMobileGodCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICharacterSimaYiMobileGodCharacterLiuQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenjiNullificationResponseForbidden(global::CardGame.Core.NullificationWindowFrame window, int responderSeat);
    bool IsWenjiResponseForbidden(long useId, int responderSeat);
}

internal interface ICharacterSimaYiMobileGodCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYingboUnrespondable(long resolutionId);
}

internal interface ICharacterSimaYiMobileGodCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterSimaYiMobileGodCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkillInvocationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool HasSkillInvocationObserverSuffix(int rootIndex);
    bool TryBeginSkillInvocation(long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string? skillInstanceId = default, global::CardGame.Core.SkillInvocationReturnKind returnKind = (global::CardGame.Core.SkillInvocationReturnKind)0);
}

internal interface ICharacterSimaYiMobileGodCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PendProgramExtraTurn(global::CardGame.Core.ProgramSkillFrame frame, int? targetSeat);
}

internal interface ICharacterSimaYiMobileGodCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICharacterSimaYiMobileGodCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICharacterSimaYiMobileGodCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat);
}

internal interface ICharacterSimaYiMobileGodCoreWushuangSkillsBAuditOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangFuqiNullificationBanned(int responder);
}

internal interface ICharacterSimaYiMobileGodCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
    void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon);
}

internal interface ICharacterSimaYiMobileGodKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICharacterSimaYiMobileGodCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICharacterSimaYiMobileGodCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int NativeExtraEffectRoundOrdinal(long id);
}

internal interface ICharacterSimaYiMobileGodCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedCardUnrespondable(long frameId);
}

internal interface ICharacterSimaYiMobileGodCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}
