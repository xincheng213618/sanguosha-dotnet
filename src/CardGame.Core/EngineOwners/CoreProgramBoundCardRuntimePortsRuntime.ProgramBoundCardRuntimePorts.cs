#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramBoundCardRuntimePortsRuntime
{
    bool IProgramBoundCardFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProgramBoundCardFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string IProgramBoundCardFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    bool IProgramBoundCardFacts.HasHpValue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue;
    int IProgramBoundCardFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int IProgramBoundCardFacts.MaxHp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    IReadOnlyList<Card> IProgramBoundCardFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> IProgramBoundCardFacts.Equipment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> IProgramBoundCardFacts.CardsAt(CardLocation location) => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(location).ToArray());
    CardLocation IProgramBoundCardFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    CardMovementRecord? IProgramBoundCardFacts.LastMovement(int cardId) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move => move.CardId == cardId);
    EquipmentSlot IProgramBoundCardFacts.FieldEquipmentSlot(Card card) => _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
    Suit IProgramBoundCardFacts.EffectiveSuit(int seat, Card card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card);
    bool IProgramBoundCardFacts.ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, source, intent);
    ContentSkillDefinition IProgramBoundInstructionFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    bool IProgramBoundInstructionFacts.HasForeignDiscardCapability => _coreForeignEquipmentDiscardPoliciesOperations.HasForeignDiscardCapability;

    bool IProgramBoundInstructionFacts.HasAtomicDiscardCapability => _coreHellMengpoProgramsOperations.HasHellShiyouCapability;

    int? IProgramBoundInstructionFacts.SelectionActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => _characterWuGuoTaiBoundaryMobileOperations.BoundaryWuGuoTaiBuyiDiscardActor(frame, bind, ids, locations);
    int IProgramBoundInstructionFacts.Participant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    IReadOnlyList<int> IProgramBoundInstructionFacts.TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, kind, context);
    bool IProgramBoundInstructionFacts.IsAtomicDiscard(ProgramSkillFrame frame, string sourceBind) => _coreLiejieSourceDiscardsOperations.Value.IsAtomicLiejieCostMove(frame, sourceBind);
    IReadOnlyList<PromptChoice> IProgramBoundInstructionFacts.PresentDistribution(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string sourceBind, SkillProgramTargetKind targetKind) => _coreCardDistributionPresentationOperations.PresentBoundCardDistribution(frame, choices, sourceBind, targetKind);
    long IProgramBoundCardMovementOperations.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    Card? IProgramBoundCardMovementOperations.DrawToProcessing(CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawOneToProcessing(reason);
    void IProgramBoundCardMovementOperations.Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason, ProgramBoundCardGive? give, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, source, destination, reason, give is { } bill ? _characterHuangQuanMouOperations.NativeGiveCompletion(bill.FrameId, bill.Giver, bill.Recipient, bill.CardIds, bill.SequenceBefore) : null, disclosure: disclosure);
    void IProgramBoundCardMovementOperations.MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason, ProgramBoundCardGive? give, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCards(cards, source, destination, reason, give is { } bill ? _characterHuangQuanMouOperations.NativeGiveCompletion(bill.FrameId, bill.Giver, bill.Recipient, bill.CardIds, bill.SequenceBefore) : null, disclosure: disclosure);
    void IProgramBoundCardMovementOperations.MoveMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, disclosure: disclosure);
    void IProgramBoundCardMovementOperations.PlaceAtBottom(IReadOnlyList<int> ids) => _cardZonesOperations._cardZones.PlaceDrawPileCardsAtBottom(ids);
    void IProgramBoundCardMovementOperations.PlaceAtTop(IReadOnlyList<int> ids) => _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(ids);
    void IProgramBoundCardPaymentOperations.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    void IProgramBoundCardPaymentOperations.CleanupRepeatedJudgment(ProgramSkillFrame frame) => _coreExactRepeatedJudgmentOperations.Value.CleanupExactRepeatedJudgment(frame);
    void IProgramBoundCardPaymentOperations.CleanupDeckSlash(ProgramSkillFrame frame) => _coreDeckProgramsOperations.CleanupDeckSlashSequence(frame);
    ProgramSkillFrame IProgramBoundCardPaymentOperations.FreezeSequentialDiscardTop(ProgramSkillFrame frame, string bind, string? exceptBind, SkillProgramCardDestination destination) => _coreSequentialDiscardReceiptsOperations.FreezeSequentialDiscardTopPayment(frame, bind, exceptBind, destination);
    ProgramSkillFrame IProgramBoundCardPaymentOperations.FreezeLostHpChained(ProgramSkillFrame frame, string bind, string? exceptBind, SkillProgramCardDestination destination) => _coreProgramLostHpChainedPaymentsOperations.Value.FreezeLostHpChainedPayment(frame, bind, exceptBind, destination);
    long? IProgramBoundCardPaymentOperations.BeginWholeHandAcquisition(ProgramSkillFrame frame, string bind, SkillProgramCardDestination destination) => _corePindianHandReturnsOperations.BeginPindianWholeHandAcquisition(frame, bind, destination);
    void IProgramBoundCardPaymentOperations.CommitWholeHandAcquisition(long frameId, string bind, long sequenceBefore) => _corePindianHandReturnsOperations.CommitPindianWholeHandAcquisition(frameId, bind, sequenceBefore);
    bool IProgramBoundCardPaymentOperations.PreparePhaseAlcohol(ProgramSkillFrame frame, string bind, CardMoveReason reason) => _coreForcedPhaseAlcoholOperations.PreparePhaseAlcoholPayment(frame, bind, reason);
    void IProgramBoundCardPaymentOperations.FreezeDiscardColors(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations) => _coreLiejieSourceDiscardsOperations.Value.FreezeLiejieDiscardColors(frame, bind, ids, locations);
    SkillProgramStepOutcome IProgramBoundCardPaymentOperations.BeginParticipantGift(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, CardLocation target, CardMoveReason reason) => _coreParticipantHandProgramsOperations.BeginBoundParticipantGift(frame, bind, ids, target, reason);
    void IProgramBoundCardPaymentOperations.CommitLostHpChained(long frameId) => _coreProgramLostHpChainedPaymentsOperations.Value.CommitLostHpChainedPayment(frameId);
    void IProgramBoundCardPaymentOperations.CommitPhaseAlcohol(long frameId) => _coreForcedPhaseAlcoholOperations.CommitPhaseAlcoholPayment(frameId);
}
