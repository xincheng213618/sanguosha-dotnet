namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string RandomRecipientRevealReturn = "random-owned-recipient-after-gain";

    private SkillProgramRandomTransferRevealPolicy? RandomTransferRevealPolicy(ProgramSkillFrame frame, string bind)
    {
        var program = _contentRegistry.GetSkill(frame.SkillId).Program!;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        var index = frame.InstructionIndex - 1;
        if (index < 0 || index >= plan.Instructions.Count ||
            plan.Instructions[index] is not { Op: SkillProgramEffectOp.TransferRandomOwnedCard } effect || effect.ResultBind != bind)
            throw new InvalidOperationException("A random transfer lost its owning instruction.");
        return effect.RandomTransferRevealPolicy;
    }

    private SkillProgramStepOutcome BeginRecipientRandomTransfer(ProgramSkillFrame frame, int recipient, string bind, Card card)
    {
        if (frame.RandomOwnedTransfer is { Stage: not RandomOwnedTransferStage.Complete })
            throw new InvalidOperationException("A random transfer cannot replace an unfinished paid receipt.");
        var before = _movementSequence;
        var receipt = new ProgramRandomOwnedTransfer(frame.InstructionIndex, bind, recipient, card.Id, card.Kind, before, before);
        ReplaceRuntimeTop(frame = frame with { RandomOwnedTransfer = receipt,
            PendingMovementContinuation = new(recipient, 0, null) });
        var reason = new CardMoveReason($"skill-program.{frame.SkillId}.transferRandomOwnedCard");
        MoveCard(card, CardLocation.Hand(frame.OwnerSeat), CardLocation.Processing, reason, disclosure: CardMovementDisclosure.FaceDown);
        MoveCard(card, CardLocation.Processing, CardLocation.Hand(recipient), reason, disclosure: CardMovementDisclosure.FaceDown);
        ReplaceRuntimeTop(frame = GetActiveProgramFrame(frame.Id) with
        { RandomOwnedTransfer = receipt with { SequenceAfter = _movementSequence } });
        ContinueRecipientRandomTransfer(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void ContinueRecipientRandomTransfer(long id)
    {
        if (_pendingDecision is not null || _resolutionStack.LastOrDefault() is not ProgramSkillFrame frame || frame.Id != id ||
            frame.RandomOwnedTransfer is not { Stage: RandomOwnedTransferStage.GainChildren } receipt) return;
        AssertRandomOwnedTransfer(frame);
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return;
        var location = CardLocation.Hand(receipt.RecipientSeat);
        if (!_players[receipt.RecipientSeat].IsAlive || _cardZones.GetLocation(receipt.CardId) != location)
        {
            ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
            CancelProgramBindingAndCleanup(GetActiveProgramFrame(id), "受赠后的原实体或展示者已失效。");
            return;
        }
        var card = GetAdvancedCard(receipt.CardId);
        var suit = EffectiveSuit(_players[receipt.RecipientSeat], card);
        ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null,
            RandomOwnedTransfer = receipt = receipt with { Stage = RandomOwnedTransferStage.RevealChildren, RevealedSuit = suit },
            PublicHandRevealSelection = new(RandomRecipientRevealReturn, frame.InstructionIndex, 0, card.Id, card.Kind,
                location, location, receipt.RecipientSeat, receipt.RecipientSeat, receipt.ResultBind, suit) });
        SetProgramCardSet(id, receipt.ResultBind, [card.Id], SkillProgramCardSetVisibility.Public, [location], suit);
        AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(id, frame.SkillId, GetProgramBindingId(frame),
            receipt.RecipientSeat, receipt.ResultBind, Array.AsReadOnly(new[] { ProjectHandCard(_players[receipt.RecipientSeat], card) with { Suit = suit } })));
        if (!TryBeginPublicHandRevealWindow(id, RandomRecipientRevealReturn)) ReturnRecipientRandomTransferReveal(GetActiveProgramFrame(id));
    }

    [PublicHandRevealReturn(RandomRecipientRevealReturn)]
    private void ReturnRecipientRandomTransferReveal(ProgramSkillFrame frame)
    {
        AssertRandomOwnedTransfer(frame);
        if (frame.RandomOwnedTransfer is not { Stage: RandomOwnedTransferStage.RevealChildren } receipt ||
            frame.PublicHandRevealSelection is not { } selected || selected.ReturnKey != RandomRecipientRevealReturn ||
            selected.InstructionIndex != receipt.InstructionIndex || selected.CardId != receipt.CardId ||
            selected.ActorSeat != receipt.RecipientSeat || selected.ChooserSeat != receipt.RecipientSeat ||
            selected.From != CardLocation.Hand(receipt.RecipientSeat) || selected.To != selected.From ||
            selected.ResultBind != receipt.ResultBind || selected.FrozenSuit != receipt.RevealedSuit)
            throw new InvalidOperationException("A recipient reveal lost its original paid entity and typed return.");
        ReplaceRuntimeTop(frame with { PublicHandRevealSelection = null,
            RandomOwnedTransfer = receipt with { Stage = RandomOwnedTransferStage.Complete } });
        AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 85800)]
    private bool ResumeRecipientRandomTransfer(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { RandomOwnedTransfer.Stage: RandomOwnedTransferStage.GainChildren } frame || frame.Id != id) return false;
        ContinueRecipientRandomTransfer(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 85800)]
    private bool ReturnRecipientRandomTransferMovement(ProgramSkillFrame frame)
    {
        if (frame.RandomOwnedTransfer is not { Stage: RandomOwnedTransferStage.GainChildren } || frame.PendingMovementContinuation is null) return false;
        ContinueRecipientRandomTransfer(frame.Id); return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 85800)]
    private void AssertRandomOwnedTransfer(ProgramSkillFrame frame)
    {
        if (frame.RandomOwnedTransfer is not { } receipt) return;
        var program = _contentRegistry.GetSkill(frame.SkillId).Program!;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        if (program.GameplayHash != frame.GameplayHash || receipt.InstructionIndex < 1 ||
            receipt.InstructionIndex > plan.Instructions.Count || frame.InstructionIndex < receipt.InstructionIndex ||
            plan.Instructions[receipt.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.TransferRandomOwnedCard,
                RandomTransferRevealPolicy: SkillProgramRandomTransferRevealPolicy.RecipientAfterGain } effect ||
            effect.ResultBind != receipt.ResultBind || !IsValidPlayerSeat(receipt.RecipientSeat) || receipt.RecipientSeat == frame.OwnerSeat ||
            (receipt.Stage != RandomOwnedTransferStage.Complete || frame.InstructionIndex == receipt.InstructionIndex) &&
                (frame.SelectedTargetSeats is not [var selectedTarget] || selectedTarget != receipt.RecipientSeat) || receipt.SequenceBefore < 0 ||
            receipt.SequenceAfter != receipt.SequenceBefore + 2 || receipt.SequenceAfter > _movementSequence ||
            !Enum.IsDefined(receipt.Stage))
            throw new InvalidOperationException("A random transfer lost its original instruction, recipient or movement receipt.");
        var target = receipt.RecipientSeat;
        var moves = _cardMovements.Where(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= receipt.SequenceAfter).ToArray();
        var reason = $"skill-program.{frame.SkillId}.transferRandomOwnedCard";
        if (moves.Length != 2 || moves.Any(m => m.CardId != receipt.CardId || m.CardKind != receipt.PrintedKind || m.Reason.Value != reason) ||
            moves[0].From != CardLocation.Hand(frame.OwnerSeat) || moves[0].To != CardLocation.Processing ||
            moves[1].From != CardLocation.Processing || moves[1].To != CardLocation.Hand(target))
            throw new InvalidOperationException("A random transfer changed or repeated its committed original entity.");
        if (receipt.Stage == RandomOwnedTransferStage.GainChildren)
        {
            if (receipt.RevealedSuit is not null || frame.CardSetBindings.Any(b => b.Name == receipt.ResultBind) ||
                frame.PendingMovementContinuation != new ProgramMovementContinuation(target, 0, null))
                throw new InvalidOperationException("A private gained card became public before its recipient's real reveal.");
            return;
        }
        var binding = GetProgramCardSet(frame, receipt.ResultBind);
        var reveals = ProgramEventHistory<ProgramCardsRevealedEvent>().Where(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId &&
            e.OwnerSeat == target && e.Bind == receipt.ResultBind).ToArray();
        if (receipt.RevealedSuit is not { } suit || !Enum.IsDefined(suit) || binding.Visibility != SkillProgramCardSetVisibility.Public ||
            binding.CardIds is not [var card] || card != receipt.CardId || binding.FrozenRevealedSuit != suit ||
            receipt.Stage == RandomOwnedTransferStage.RevealChildren && frame.PendingMovementContinuation is not null ||
            reveals.Length == 0 || receipt.Stage == RandomOwnedTransferStage.RevealChildren && reveals.Length != 1 ||
            reveals[0].Cards is not [var shown] || shown.Id != card || shown.Suit != suit)
            throw new InvalidOperationException("A recipient reveal changed its original public entity or frozen effective suit.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 85800)]
    private bool KeepPaidRecipientRandomTransfer(ProgramSkillFrame frame)
    {
        if (frame.RandomOwnedTransfer is null) return false;
        AssertRandomOwnedTransfer(frame); return true;
    }
}
