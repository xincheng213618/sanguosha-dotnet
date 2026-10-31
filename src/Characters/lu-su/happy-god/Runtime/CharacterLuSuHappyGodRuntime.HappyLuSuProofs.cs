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
}
