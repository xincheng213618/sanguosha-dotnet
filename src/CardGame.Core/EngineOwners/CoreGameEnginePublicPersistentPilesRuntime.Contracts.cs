#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePublicPersistentPilesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void ReturnRuntimeProgramMovement(long frameId);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICorePublicPersistentPilesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePublicPersistentPilesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICorePublicPersistentPilesCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default);
}

internal interface ICorePublicPersistentPilesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICorePublicPersistentPilesCoreCardDistributionPresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PresentCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string stage, int maximum = 2147483647, global::System.Func<global::CardGame.Core.PromptChoice, string>? destination = default, global::System.Func<global::CardGame.Core.PromptChoice, string>? label = default, int? scopeInstruction = default);
}

internal interface ICorePublicPersistentPilesCoreBoundPublicPileProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> PublicPileDistributionTargets(global::CardGame.Core.ProgramSkillFrame frame);
    void BindSelectedPublicPile(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PublicPersistentPileSource? source);
}

internal interface ICorePublicPersistentPilesCharacterBaoXinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedBaoXinMutaoPile(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICorePublicPersistentPilesCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDurableZengdaoPublicPile(global::CardGame.Core.PublicPersistentPileSource pile);
}

internal interface ICorePublicPersistentPilesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
}

internal interface ICorePublicPersistentPilesCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RetainIssuedJiyunPile(global::CardGame.Core.PublicPersistentPileSource pile);
    bool IsIssuedJiyunRetainedEntity(global::CardGame.Core.PublicPersistentPileSource pile, int cardId);
}

internal interface ICorePublicPersistentPilesCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICorePublicPersistentPilesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePublicPersistentPilesCharacterSunShangxiangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedMouSsxZhuang(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICorePublicPersistentPilesCharacterGanNingMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedMouGanNingPile(global::CardGame.Core.PublicPersistentPileSource p);
}

internal interface ICorePublicPersistentPilesCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedMouGszHuPile(global::CardGame.Core.PublicPersistentPileSource p);
}

internal interface ICorePublicPersistentPilesCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICorePublicPersistentPilesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICorePublicPersistentPilesCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SupportsMultiplePublicPiles { get; }

    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> PublicPileSources(int seat);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
    global::CardGame.Core.PublicPersistentPileSource EnsurePublicPileSource(global::CardGame.Core.ProgramSkillFrame frame, int capacity);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileDraftCards(global::CardGame.Core.ProgramPublicPileDraft draft);
    global::CardGame.Core.CardLocation PublicPileDraftLocation(global::CardGame.Core.ProgramPublicPileDraft draft);
    global::CardGame.Core.PublicPersistentPileSource? PublicPileDraftSource(global::CardGame.Core.ProgramPublicPileDraft draft);
}

internal interface ICorePublicPersistentPilesCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICorePublicPersistentPilesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePublicPersistentPilesCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MoveProcessingCardUnlessDestroyed(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default);
}

internal interface ICorePublicPersistentPilesCharacterHuangZuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedShiHzPile(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICorePublicPersistentPilesCharacterXiahouDunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedMouXiahouDunPile(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICorePublicPersistentPilesCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason);
}
