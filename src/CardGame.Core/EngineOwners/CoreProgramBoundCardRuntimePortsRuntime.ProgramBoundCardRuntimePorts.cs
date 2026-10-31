#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramBoundCardRuntimePortsRuntime
{
    bool global::CardGame.Core.IProgramBoundCardFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IProgramBoundCardFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IProgramBoundCardFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    bool global::CardGame.Core.IProgramBoundCardFacts.HasHpValue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    int global::CardGame.Core.IProgramBoundCardFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int global::CardGame.Core.IProgramBoundCardFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    IReadOnlyList<Card> global::CardGame.Core.IProgramBoundCardFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> global::CardGame.Core.IProgramBoundCardFacts.Equipment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> global::CardGame.Core.IProgramBoundCardFacts.CardsAt(CardLocation location) => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(location).ToArray());
    CardLocation global::CardGame.Core.IProgramBoundCardFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    CardMovementRecord? global::CardGame.Core.IProgramBoundCardFacts.LastMovement(int cardId) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move => move.CardId == cardId);
    EquipmentSlot global::CardGame.Core.IProgramBoundCardFacts.FieldEquipmentSlot(Card card) => _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
    Suit global::CardGame.Core.IProgramBoundCardFacts.EffectiveSuit(int seat, Card card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card);
    bool global::CardGame.Core.IProgramBoundCardFacts.ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, source, intent);
    ContentSkillDefinition global::CardGame.Core.IProgramBoundInstructionFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    bool global::CardGame.Core.IProgramBoundInstructionFacts.HasForeignDiscardCapability => _coreForeignEquipmentDiscardPoliciesOperations.HasForeignDiscardCapability;

    bool global::CardGame.Core.IProgramBoundInstructionFacts.HasAtomicDiscardCapability => _coreHellMengpoProgramsOperations.HasHellShiyouCapability;

    int? global::CardGame.Core.IProgramBoundInstructionFacts.SelectionActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => _characterWuGuoTaiBoundaryMobileOperations.BoundaryWuGuoTaiBuyiDiscardActor(frame, bind, ids, locations);
    int global::CardGame.Core.IProgramBoundInstructionFacts.Participant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    IReadOnlyList<int> global::CardGame.Core.IProgramBoundInstructionFacts.TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, kind, context);
    bool global::CardGame.Core.IProgramBoundInstructionFacts.IsAtomicDiscard(ProgramSkillFrame frame, string sourceBind) => _coreLiejieSourceDiscardsOperations.IsAtomicLiejieCostMove(frame, sourceBind);
    IReadOnlyList<PromptChoice> global::CardGame.Core.IProgramBoundInstructionFacts.PresentDistribution(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string sourceBind, SkillProgramTargetKind targetKind) => _coreCardDistributionPresentationOperations.PresentBoundCardDistribution(frame, choices, sourceBind, targetKind);
    long global::CardGame.Core.IProgramBoundCardMovementOperations.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    Card? global::CardGame.Core.IProgramBoundCardMovementOperations.DrawToProcessing(CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawOneToProcessing(reason);
    void global::CardGame.Core.IProgramBoundCardMovementOperations.Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason, ProgramBoundCardGive? give, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, source, destination, reason, give is { } bill ? _characterHuangQuanMouOperations.NativeGiveCompletion(bill.FrameId, bill.Giver, bill.Recipient, bill.CardIds, bill.SequenceBefore) : null, disclosure: disclosure);
    void global::CardGame.Core.IProgramBoundCardMovementOperations.MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason, ProgramBoundCardGive? give, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCards(cards, source, destination, reason, give is { } bill ? _characterHuangQuanMouOperations.NativeGiveCompletion(bill.FrameId, bill.Giver, bill.Recipient, bill.CardIds, bill.SequenceBefore) : null, disclosure: disclosure);
    void global::CardGame.Core.IProgramBoundCardMovementOperations.MoveMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, disclosure: disclosure);
    void global::CardGame.Core.IProgramBoundCardMovementOperations.PlaceAtBottom(IReadOnlyList<int> ids) => _cardZonesOperations._cardZones.PlaceDrawPileCardsAtBottom(ids);
    void global::CardGame.Core.IProgramBoundCardMovementOperations.PlaceAtTop(IReadOnlyList<int> ids) => _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(ids);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.CleanupRepeatedJudgment(ProgramSkillFrame frame) => _coreExactRepeatedJudgmentOperations.CleanupExactRepeatedJudgment(frame);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.CleanupDeckSlash(ProgramSkillFrame frame) => _coreDeckProgramsOperations.CleanupDeckSlashSequence(frame);
    ProgramSkillFrame global::CardGame.Core.IProgramBoundCardPaymentOperations.FreezeSequentialDiscardTop(ProgramSkillFrame frame, string bind, string? exceptBind, SkillProgramCardDestination destination) => _coreSequentialDiscardReceiptsOperations.FreezeSequentialDiscardTopPayment(frame, bind, exceptBind, destination);
    ProgramSkillFrame global::CardGame.Core.IProgramBoundCardPaymentOperations.FreezeLostHpChained(ProgramSkillFrame frame, string bind, string? exceptBind, SkillProgramCardDestination destination) => _coreProgramLostHpChainedPaymentsOperations.FreezeLostHpChainedPayment(frame, bind, exceptBind, destination);
    long? global::CardGame.Core.IProgramBoundCardPaymentOperations.BeginWholeHandAcquisition(ProgramSkillFrame frame, string bind, SkillProgramCardDestination destination) => _corePindianHandReturnsOperations.BeginPindianWholeHandAcquisition(frame, bind, destination);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.CommitWholeHandAcquisition(long frameId, string bind, long sequenceBefore) => _corePindianHandReturnsOperations.CommitPindianWholeHandAcquisition(frameId, bind, sequenceBefore);
    bool global::CardGame.Core.IProgramBoundCardPaymentOperations.PreparePhaseAlcohol(ProgramSkillFrame frame, string bind, CardMoveReason reason) => _coreForcedPhaseAlcoholOperations.PreparePhaseAlcoholPayment(frame, bind, reason);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.FreezeDiscardColors(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => _coreLiejieSourceDiscardsOperations.FreezeLiejieDiscardColors(frame, bind, ids, locations);
    SkillProgramStepOutcome global::CardGame.Core.IProgramBoundCardPaymentOperations.BeginParticipantGift(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, CardLocation target, CardMoveReason reason) => _coreParticipantHandProgramsOperations.BeginBoundParticipantGift(frame, bind, ids, target, reason);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.CommitLostHpChained(long frameId) => _coreProgramLostHpChainedPaymentsOperations.CommitLostHpChainedPayment(frameId);
    void global::CardGame.Core.IProgramBoundCardPaymentOperations.CommitPhaseAlcohol(long frameId) => _coreForcedPhaseAlcoholOperations.CommitPhaseAlcoholPayment(frameId);
}
