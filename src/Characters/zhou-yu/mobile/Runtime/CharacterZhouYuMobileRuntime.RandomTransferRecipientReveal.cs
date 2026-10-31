#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMobileRuntime
{
    internal const string RandomRecipientRevealReturn = "random-owned-recipient-after-gain";
    internal SkillProgramRandomTransferRevealPolicy? RandomTransferRevealPolicy(ProgramSkillFrame frame, string bind)
    {
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        var index = frame.InstructionIndex - 1;
        if (index < 0 || index >= plan.Instructions.Count || plan.Instructions[index] is not { Op: SkillProgramEffectOp.TransferRandomOwnedCard } effect || effect.ResultBind != bind)
            throw new InvalidOperationException("A random transfer lost its owning instruction.");
        return effect.RandomTransferRevealPolicy;
    }

    internal SkillProgramStepOutcome BeginRecipientRandomTransfer(ProgramSkillFrame frame, int recipient, string bind, Card card)
    {
        if (frame.RandomOwnedTransfer is { Stage: not RandomOwnedTransferStage.Complete })
            throw new InvalidOperationException("A random transfer cannot replace an unfinished paid receipt.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var receipt = new ProgramRandomOwnedTransfer(frame.InstructionIndex, bind, recipient, card.Id, card.Kind, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { RandomOwnedTransfer = receipt, PendingMovementContinuation = new(recipient, 0, null) });
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.transferRandomOwnedCard");
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(frame.OwnerSeat), CardLocation.Processing, reason, disclosure: CardMovementDisclosure.FaceDown);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.Hand(recipient), reason, disclosure: CardMovementDisclosure.FaceDown);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { RandomOwnedTransfer = receipt with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueRecipientRandomTransfer(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueRecipientRandomTransfer(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.RandomOwnedTransfer is not { Stage: RandomOwnedTransferStage.GainChildren } receipt)
            return;
        AssertRandomOwnedTransfer(frame);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        var location = CardLocation.Hand(receipt.RecipientSeat);
        if (!_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat].IsAlive || _cardZonesOperations._cardZones.GetLocation(receipt.CardId) != location)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "受赠后的原实体或展示者已失效。");
            return;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(receipt.CardId);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat], card);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, RandomOwnedTransfer = receipt = receipt with { Stage = RandomOwnedTransferStage.RevealChildren, RevealedSuit = suit }, PublicHandRevealSelection = new(RandomRecipientRevealReturn, frame.InstructionIndex, 0, card.Id, card.Kind, location, location, receipt.RecipientSeat, receipt.RecipientSeat, receipt.ResultBind, suit) });
        _coreProgramLifecycleOperations.SetProgramCardSet(id, receipt.ResultBind, [card.Id], SkillProgramCardSetVisibility.Public, [location], suit);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(id, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), receipt.RecipientSeat, receipt.ResultBind, Array.AsReadOnly(new[] { _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat], card)with { Suit = suit } })));
        if (!_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(id, RandomRecipientRevealReturn))
            ReturnRecipientRandomTransferReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [PublicHandRevealReturn(RandomRecipientRevealReturn)]
    internal void ReturnRecipientRandomTransferReveal(ProgramSkillFrame frame)
    {
        AssertRandomOwnedTransfer(frame);
        if (frame.RandomOwnedTransfer is not { Stage: RandomOwnedTransferStage.RevealChildren } receipt || frame.PublicHandRevealSelection is not { } selected || selected.ReturnKey != RandomRecipientRevealReturn || selected.InstructionIndex != receipt.InstructionIndex || selected.CardId != receipt.CardId || selected.ActorSeat != receipt.RecipientSeat || selected.ChooserSeat != receipt.RecipientSeat || selected.From != CardLocation.Hand(receipt.RecipientSeat) || selected.To != selected.From || selected.ResultBind != receipt.ResultBind || selected.FrozenSuit != receipt.RevealedSuit)
            throw new InvalidOperationException("A recipient reveal lost its original paid entity and typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicHandRevealSelection = null, RandomOwnedTransfer = receipt with { Stage = RandomOwnedTransferStage.Complete } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 85800)]
    internal bool ResumeRecipientRandomTransfer(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RandomOwnedTransfer.Stage: RandomOwnedTransferStage.GainChildren } frame || frame.Id != id)
            return false;
        ContinueRecipientRandomTransfer(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 85800)]
    internal bool ReturnRecipientRandomTransferMovement(ProgramSkillFrame frame)
    {
        if (frame.RandomOwnedTransfer is not { Stage: RandomOwnedTransferStage.GainChildren } || frame.PendingMovementContinuation is null)
            return false;
        ContinueRecipientRandomTransfer(frame.Id);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 85800)]
    internal void AssertRandomOwnedTransfer(ProgramSkillFrame frame)
    {
        if (frame.RandomOwnedTransfer is not { } receipt)
            return;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        if (program.GameplayHash != frame.GameplayHash || receipt.InstructionIndex < 1 || receipt.InstructionIndex > plan.Instructions.Count || frame.InstructionIndex < receipt.InstructionIndex || plan.Instructions[receipt.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.TransferRandomOwnedCard, RandomTransferRevealPolicy: SkillProgramRandomTransferRevealPolicy.RecipientAfterGain } effect || effect.ResultBind != receipt.ResultBind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.RecipientSeat) || receipt.RecipientSeat == frame.OwnerSeat || (receipt.Stage != RandomOwnedTransferStage.Complete || frame.InstructionIndex == receipt.InstructionIndex) && (frame.SelectedTargetSeats is not [var selectedTarget] || selectedTarget != receipt.RecipientSeat) || receipt.SequenceBefore < 0 || receipt.SequenceAfter != receipt.SequenceBefore + 2 || receipt.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(receipt.Stage))
            throw new InvalidOperationException("A random transfer lost its original instruction, recipient or movement receipt.");
        var target = receipt.RecipientSeat;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= receipt.SequenceAfter).ToArray();
        var reason = $"skill-program.{(frame.SkillId)}.transferRandomOwnedCard";
        if (moves.Length != 2 || moves.Any(m => m.CardId != receipt.CardId || m.CardKind != receipt.PrintedKind || m.Reason.Value != reason) || moves[0].From != CardLocation.Hand(frame.OwnerSeat) || moves[0].To != CardLocation.Processing || moves[1].From != CardLocation.Processing || moves[1].To != CardLocation.Hand(target))
            throw new InvalidOperationException("A random transfer changed or repeated its committed original entity.");
        if (receipt.Stage == RandomOwnedTransferStage.GainChildren)
        {
            if (receipt.RevealedSuit is not null || frame.CardSetBindings.Any(b => b.Name == receipt.ResultBind) || frame.PendingMovementContinuation != new ProgramMovementContinuation(target, 0, null))
                throw new InvalidOperationException("A private gained card became public before its recipient's real reveal.");
            return;
        }

        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, receipt.ResultBind);
        var reveals = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Where(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.OwnerSeat == target && e.Bind == receipt.ResultBind).ToArray();
        if (receipt.RevealedSuit is not { } suit || !Enum.IsDefined(suit) || binding.Visibility != SkillProgramCardSetVisibility.Public || binding.CardIds is not [var card] || card != receipt.CardId || binding.FrozenRevealedSuit != suit || receipt.Stage == RandomOwnedTransferStage.RevealChildren && frame.PendingMovementContinuation is not null || reveals.Length == 0 || receipt.Stage == RandomOwnedTransferStage.RevealChildren && reveals.Length != 1 || reveals[0].Cards is not [var shown] || shown.Id != card || shown.Suit != suit)
            throw new InvalidOperationException("A recipient reveal changed its original public entity or frozen effective suit.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 85800)]
    internal bool KeepPaidRecipientRandomTransfer(ProgramSkillFrame frame)
    {
        if (frame.RandomOwnedTransfer is null)
            return false;
        AssertRandomOwnedTransfer(frame);
        return true;
    }
}
