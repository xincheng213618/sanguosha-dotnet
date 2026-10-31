#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuHappyGodRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 41280)]
    internal bool HappyDingzhouMovementProof(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (frame.HappyDingzhou is not { Paid: true, BatchId: > 0 } receipt || effect?.Op != SkillProgramEffectOp.ResolveHappyShenLuSuDingzhou || pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || receipt.Invoices.Count == 0 || receipt.After <= receipt.Before || receipt.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var paid = receipt.Invoices;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<HappyDingzhouMovementPaidEvent>().Count(e => e == new HappyDingzhouMovementPaidEvent(frame.Id, receipt.Stage, receipt.BatchId, paid.Count, receipt.Before, receipt.After)) != 1 || paid.Select(m => m.CardId).Distinct().Count() != paid.Count || paid.Max(m => m.Sequence) != receipt.After || paid.Any(m => m.Sequence <= receipt.Before || m.Sequence > receipt.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(actual => actual.Sequence == m.Sequence && actual.CardId == m.CardId && actual.From == m.From && actual.To == m.To && actual.Reason.Value == HappyDingzhouReason + receipt.Stage) != 1))
            return false;
        if (receipt.Stage is HappyDingzhouStage.FieldGiftChildren or HappyDingzhouStage.HandReturnChildren)
            return receipt.Branch == (receipt.Stage == HappyDingzhouStage.FieldGiftChildren ? 1 : 2) && paid.Count == (receipt.RequiredReturnCount ?? receipt.Amount) && receipt.Chosen.Count == paid.Count && paid.Select(m => m.CardId).Order().SequenceEqual(receipt.Chosen.Order()) && paid.All(m => m.From.OwnerSeat == frame.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && HappyDingzhouHandDestination(m, receipt.TargetSeat));
        if (receipt.Stage == HappyDingzhouStage.HandGainChildren)
            return receipt.Branch == 2 && paid.Count == receipt.Amount && paid.Select(m => m.CardId).Order().SequenceEqual(receipt.Obtained.Order()) && paid.All(m => m.From == CardLocation.Hand(receipt.TargetSeat) && HappyDingzhouHandDestination(m, frame.OwnerSeat));
        return receipt.Stage == HappyDingzhouStage.FieldGainChildren && receipt.Branch == 1 && paid.Select(m => m.CardId).Order().SequenceEqual(receipt.Obtained.Order()) && paid.All(m => m.From.OwnerSeat == receipt.TargetSeat && m.From.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && HappyDingzhouHandDestination(m, frame.OwnerSeat, allowGeneralWeapon: true));
    }

    internal bool HappyDingzhouHandDestination(ShenLuSuMoveInvoice invoice, int recipient, bool allowGeneralWeapon = false)
    {
        if (invoice.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(invoice.CardId).IsGeneralWeapon)
            return allowGeneralWeapon && invoice.To == CardLocation.OutsideGame;
        if (invoice.To == CardLocation.Hand(recipient))
            return true;
        // The exact paid record was already checked above. A generated field
        // entity can pay this movement while its native departure destroys it.
        // Require that producer's real destruction proof, never just OutsideGame.
        return invoice.To == CardLocation.OutsideGame && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == invoice.Sequence && m.CardId == invoice.CardId && m.From == invoice.From && m.To == invoice.To)is { } actual && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(actual);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 41280)]
    internal void AssertHappyDingzhou(ProgramSkillFrame frame)
    {
        if (frame.HappyDingzhou is not { } receipt)
            return;
        if (frame.SkillId != "happy:dingzhou" || frame.ActivationId != "exchange" || frame.TriggerId is not null || frame.InstructionIndex != 1 || frame.WindowContext is not null || frame.SelectedTargetSeats is not [var target] || target != receipt.TargetSeat || target == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || receipt.Source != global::CardGame.Core.EngineOwners.CharacterLuSuMobileGodRuntime.ShenLsSource(frame) || receipt.GameplayHash != frame.GameplayHash || !Enum.IsDefined(receipt.Stage) || receipt.Branch is < 0 or > 2 || receipt.Amount < 0 || receipt.Before < 0 || receipt.After < receipt.Before || receipt.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || receipt.Chosen.Count != receipt.Chosen.Distinct().Count() || receipt.Chosen.Count > receipt.Amount || receipt.Obtained.Count != receipt.Obtained.Distinct().Count() || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyDingzhouStartedEvent>().Count(e => e == new HappyDingzhouStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, target)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.ActivationId == frame.ActivationId) != 1)
            throw new InvalidOperationException("Happy Dingzhou lost its exact original activation, participant, source or frozen physical choices.");
        if (receipt.Stage == HappyDingzhouStage.BranchChoice)
        {
            if (receipt.Branch != 0 || receipt.Amount != 0 || receipt.Chosen.Count != 0 || receipt.Obtained.Count != 0 || receipt.Paid || receipt.Invoices.Count != 0)
                throw new InvalidOperationException("Unchosen Happy Dingzhou cannot carry paid exchange state.");
        }
        else if (_coreProgramEventHistoryOperations.ProgramEventHistory<HappyDingzhouBranchChosenEvent>().Count(e => e == new HappyDingzhouBranchChosenEvent(frame.Id, receipt.Branch, receipt.Amount)) != 1 || receipt.Branch == 1 && receipt.Stage is not (HappyDingzhouStage.FieldGiftChoice or HappyDingzhouStage.FieldGiftChildren or HappyDingzhouStage.FieldGainChildren) || receipt.Branch == 2 && (receipt.Stage is not (HappyDingzhouStage.HandGainChildren or HappyDingzhouStage.HandReturnChoice or HappyDingzhouStage.HandReturnChildren) || receipt.Obtained.Count != receipt.Amount) || receipt.Chosen.Select((card, index) => index).Any(index => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyDingzhouCardChosenEvent>().Count(e => e.FrameId == frame.Id && e.Index == index) != 1))
            throw new InvalidOperationException("Happy Dingzhou lost its issued branch, original exchange count or exact selected-card ordinals.");
        if (receipt.Stage is HappyDingzhouStage.HandReturnChoice or HappyDingzhouStage.HandReturnChildren)
        {
            if (receipt.RequiredReturnCount is not { } required || required < 0 || required > receipt.Amount || receipt.Chosen.Count > required || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyDingzhouReturnRequiredEvent>().Count(e => e == new HappyDingzhouReturnRequiredEvent(frame.Id, receipt.Amount, required)) != 1)
                throw new InvalidOperationException("Happy Dingzhou lost its original hand count or post-gain payable return obligation.");
        }
        else if (receipt.RequiredReturnCount is not null)
            throw new InvalidOperationException("Happy Dingzhou cannot settle a return obligation before its real gain children finish.");
        if (frame.PendingMovementContinuation is { } pending && !HappyDingzhouMovementProof(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions[0], pending))
            throw new InvalidOperationException("Happy Dingzhou lost its exact once-paid native movement invoice.");
    }
}
