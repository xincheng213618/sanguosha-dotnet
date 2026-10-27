namespace CardGame.Core;

public sealed partial class GameEngine
{
    private CardConversionSource? ProgramAllHandTrickSource(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } action ||
            action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat ||
            action.EffectiveKind != CardKind.Duel || use.CardKind != CardKind.Duel || action.PhysicalCards.Count == 0 ||
            action.ConversionChain.FirstOrDefault() is not { } source || source.OwnerSeat != use.SourceSeat ||
            !_contentRegistry.Skills.TryGetValue(source.SkillId, out var skill) || skill.Program is not { } program ||
            !program.Activations.Any(activation => activation.Effects.Any(effect =>
                effect.Op == SkillProgramEffectOp.UseAllHandCardsAsOrdinaryTrick && effect.SourceBind == source.BindingId &&
                (effect.OutputKind is null || effect.OutputKind == use.CardKind)))) return null;
        return source;
    }

    private bool ProgramAllHandTrickMaterialsMatch(CardUseFrame use, CardMovementBatchContext batch)
    {
        if (ProgramAllHandTrickSource(use) is not { } source || use.Action is not { } action ||
            batch.ParentFrameId != use.Id || batch.ParentBatchId is not null || batch.AwaitingProgramFrameId is not null ||
            batch.NativeDrawInvocation is not null || batch.NativeBuZhiDiscardInvocation is not null ||
            batch.OriginOwnerSeat != source.OwnerSeat || batch.OriginSkillId != source.SkillId ||
            batch.OriginSkillInstanceId != source.SkillInstanceId || batch.Movements.Count != action.PhysicalCards.Count ||
            batch.Movements.Select(move => move.CardId).Distinct().Count() != batch.Movements.Count ||
            batch.SourceCounts.SingleOrDefault(count => count.Location == CardLocation.Hand(source.OwnerSeat)) is not { } hand ||
            hand.CountBefore != action.PhysicalCards.Count || hand.CountAfter != 0 ||
            use.PhysicalCardIds is not { } physicalIds ||
            !physicalIds.Order().SequenceEqual(action.PhysicalCards.Select(cost => cost.CardId).Order())) return false;
        // Ordinary card.use records may retain Unspecified disclosure even
        // when the public Use requested FaceUp; concealed payments never match.
        return batch.Movements.All(move =>
            action.PhysicalCards.SingleOrDefault(cost => cost.CardId == move.CardId) is { } cost &&
            cost.From == CardLocation.Hand(source.OwnerSeat) && move.From == cost.From && move.CardKind == cost.CardKind &&
            move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use &&
            (move.Disclosure is CardMovementDisclosure.Unspecified or CardMovementDisclosure.FaceUp) && move.TurnNumber == batch.TurnNumber &&
            move.Sequence > 0 && move.Sequence <= _cardMovements.Count && _cardMovements[move.Sequence - 1] == move);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 50)]
    private void CaptureProgramAllHandTrickPayment(CardMovementBatchContext batch)
    {
        if (_resolutionStack.LastOrDefault() is not CardUseFrame use ||
            !ProgramAllHandTrickMaterialsMatch(use, batch)) return;
        var source = ProgramAllHandTrickSource(use)!;
        var index = _resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (index < 1 || _resolutionStack[index - 1] is not ProgramSkillFrame origin ||
            origin.TriggerId is not null || origin.OwnerSeat != source.OwnerSeat ||
            origin.SkillId != source.SkillId || origin.SkillInstanceId != source.SkillInstanceId ||
            ProgramInstructionResolver.Default.Resolve(origin, _contentRegistry.GetSkill(origin.SkillId).Program!)
                .GetPausedInstruction(origin.InstructionIndex).Effect is not
                { Op: SkillProgramEffectOp.UseAllHandCardsAsOrdinaryTrick } instruction ||
            instruction.SourceBind != source.BindingId)
            throw new InvalidOperationException("An all-hand trick payment lost its original activation instruction.");
        if (use.PaymentMovementReceipt is not null)
            throw new InvalidOperationException("An all-hand trick cannot issue its physical payment twice.");
        ReplaceRuntimeFrame(use.Id, use with
        {
            PaymentMovementReceipt = new(use.Id, use.Action!.ActionId,
                batch.Movements[0].Sequence - 1, batch.Movements[^1].Sequence) { Batch = batch }
        });
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MatchBatch, 90)]
    private bool IsProgramAllHandTrickPaymentBatch(CardUseFrame use, CardMovementBatchContext batch) =>
        CardUsePaymentBatchMatchesReceipt(use, batch) && ProgramAllHandTrickMaterialsMatch(use, batch);

    // BeginJizhiOrNullificationWindow already calls TryPauseRecoveryPaidCardUse.
    // Opt only an issued all-hand Duel activation into its original payment
    // tail, so last-hand children finish before the same Duel's first response.
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.LeadingTail, 30)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.HpObserverTail, 80)]
    private bool HasProgramAllHandTrickPaymentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) =>
        continuation is { Kind: RecoveryPaidCardUseKind.Trick, CardId: { } cardId,
            ActionKind: LegalActionKind.Duel, TargetSeats: { } targets } &&
        cardId == use.CardId && continuation.SourceSeat == use.SourceSeat &&
        (continuation.PlayedCardKind ?? use.CardKind) == use.CardKind &&
        targets.SequenceEqual(use.TargetSeats) &&
        use.PaymentMovementReceipt is { } receipt && IsProgramAllHandTrickPaymentBatch(use, receipt.Batch);

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.ProcessingAllowance, 40)]
    private bool HasProgramAllHandTrickPaymentProcessing(IReadOnlyList<Card> processing)
    {
        if (_resolutionStack.Count < 3 || _resolutionStack[^1] is not CardsMovedTriggerWindowFrame movement ||
            _resolutionStack[^2] is not CardUseFrame { RecoveryPaidContinuation: { } continuation,
                PaymentMovementReceipt: { } payment, Action: { } action } use ||
            !HasProgramAllHandTrickPaymentTail(use, continuation) || !IsPaidCardUseMovementReturn(use, movement)) return false;
        var allowed = payment.Batch.Movements.Select(move => move.CardId).ToHashSet();
        var visited = new HashSet<long>();
        for (var parentId = action.ParentActionId; parentId is { } id;)
        {
            if (!visited.Add(id) || _resolutionStack.OfType<CardUseFrame>()
                    .SingleOrDefault(parent => parent.Action?.ActionId == id) is not { Action: { } parentAction } parent ||
                parent.Id >= use.Id) return false;
            allowed.UnionWith(parent.PhysicalCardIds ?? [parent.CardId]);
            parentId = parentAction.ParentActionId;
        }
        return processing.All(card => allowed.Contains(card.Id));
    }
}
