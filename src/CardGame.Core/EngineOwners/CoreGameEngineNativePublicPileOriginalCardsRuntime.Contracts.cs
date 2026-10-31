#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativePublicPileOriginalCardsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativePublicPileOriginalCardsCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiFengCapability { get; }

    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> ShuliangOriginalGrainChoices(global::CardGame.Core.NativePublicPileCardRequest request);
    bool IsShuliangOriginalGrainChoice(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection selection);
    void RecordShuliangGrainPayment(global::CardGame.Core.NativePublicPileCardPayment payment);
}

internal interface ICoreNativePublicPileOriginalCardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativePublicPileOriginalCardsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativePublicPileOriginalCardsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativePublicPileOriginalCardsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreNativePublicPileOriginalCardsCharacterPangTongFriendOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendYangmingOriginalUseParent(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativePublicPileOriginalCardsCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativePublicPileCardRequest? MouSmPublicPileRequest(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativePublicPileOriginalCardsCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActionsCore(global::CardGame.Core.CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, global::CardGame.Core.Card? nativeOriginalCard, bool ignoreForcedHandSelection = false);
}

internal interface ICoreNativePublicPileOriginalCardsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativePublicPileOriginalCardsCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouGszMobile { get; }

    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> MouGszHuOriginalChoices(global::CardGame.Core.NativePublicPileCardRequest request);
    bool IsMouGszHuOriginalChoice(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection choice);
    bool PrepareMouGszHuResponse(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion, int? actor = default);
    void PrepareMouGszHuUse(global::CardGame.Core.CardUseFrame use);
    bool IsMouGszHuOriginalPayment(global::CardGame.Core.NativePublicPileCardPayment p);
}

internal interface ICoreNativePublicPileOriginalCardsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativePublicPileOriginalCardsCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICoreNativePublicPileOriginalCardsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreNativePublicPileOriginalCardsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativePublicPileOriginalCardsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativePublicPileOriginalCardsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreNativePublicPileOriginalCardsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreNativePublicPileOriginalCardsCharacterHuangZuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiHzOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    bool IsShiHzOriginalUseParent(global::CardGame.Core.CardUseFrame use);
}
