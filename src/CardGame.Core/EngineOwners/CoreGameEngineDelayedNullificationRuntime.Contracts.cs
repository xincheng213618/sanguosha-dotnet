#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDelayedNullificationCoreShanheDelayedNullifiedTrickClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheClaimedDelayedOwner(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed);
    bool TryCompleteShanheClaimedDelayedNullification(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed);
}

internal interface ICoreDelayedNullificationCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreDelayedNullificationCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
    void BeginDelayedJudgmentOrTurnStart(global::CardGame.Core.CharacterState current, int? excludedCardId = default);
}

internal interface ICoreDelayedNullificationCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreDelayedNullificationCoreCardEffectProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDelayedCardEffectPrograms(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    void BeginActualDelayedJudgment(global::CardGame.Core.CharacterState current, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreDelayedNullificationCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreDelayedNullificationCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreDelayedNullificationCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDelayedNullificationCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreDelayedNullificationCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDelayedNullificationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDelayedNullificationCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat);
    void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreDelayedNullificationCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreDelayedNullificationCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidOriginalUseLightningDelayed(global::CardGame.Core.DelayedEffectContinuation d);
    void CompleteOriginalUseLightningNullification(global::CardGame.Core.DelayedEffectContinuation delayed, bool nullified, long windowId);
}

internal interface ICoreDelayedNullificationCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDelayedNullificationCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreDelayedNullificationCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteLightningWithoutHit(long resolutionId, global::CardGame.Core.Card lightning, global::CardGame.Core.CharacterState target, int? judgmentCardId);
}
