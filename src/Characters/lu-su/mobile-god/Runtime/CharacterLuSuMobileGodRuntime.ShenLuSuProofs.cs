#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileGodRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 38900)]
    internal bool ShenLsMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.ShenLuSu is not { Paid: true, BatchId: > 0, MovementCount: > 0 } r || effect?.Op != r.Operation || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Stage is not (ShenLuSuStage.GiftChildren or ShenLuSuStage.ObtainChildren or ShenLuSuStage.DivideChildren))
            return false;
        var paid = r.MovementInvoices;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShenLuSuMovementPaidEvent>().Count(e => e == new ShenLuSuMovementPaidEvent(f.Id, r.Stage, r.BatchId, r.MovementCount, r.Before, r.After)) != 1)
            return false;
        if (paid.Count != r.MovementCount || paid.Select(e => e.CardId).Distinct().Count() != paid.Count || paid.Max(e => e.Sequence) != r.After || paid.Any(e => e.Sequence <= r.Before || e.Sequence > r.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == ShenLsReason + r.Stage) != 1))
            return false;
        if (r.Stage == ShenLuSuStage.GiftChildren)
            return r.Operation == SkillProgramEffectOp.ResolveShenLuSuDingzhou && paid.Select(e => e.CardId).Order().SequenceEqual(r.Cards.Order()) && r.Cards.Count == r.Amount && paid.All(e => e.From.OwnerSeat == f.OwnerSeat && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && e.To == CardLocation.Hand(r.Target));
        if (r.Stage == ShenLuSuStage.ObtainChildren)
            return r.Operation == SkillProgramEffectOp.ResolveShenLuSuDingzhou && paid.All(e => e.From.OwnerSeat == r.Target && e.From.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && e.To == (_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).IsGeneralWeapon && e.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat)));
        var division = r.Division;
        return r.Operation == SkillProgramEffectOp.ResolveShenLuSuZhimeng && division.Count == r.Cards.Count && division.Count(e => e.ToSeat == f.OwnerSeat) == (r.Amount + 1) / 2 && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenLuSuDivisionEvent>().Count(e => e == new ShenLuSuDivisionEvent(f.Id, r.Target, r.Amount, r.OwnerShare.Count)) == 1 && division.Select(e => e.CardId).Order().SequenceEqual(r.Cards.Order()) && division.All(e => e.FromSeat == f.OwnerSeat || e.FromSeat == r.Target) && division.All(e => e.ToSeat == (r.OwnerShare.Contains(e.CardId) ? f.OwnerSeat : r.Target)) && division.Where(e => e.FromSeat != e.ToSeat).All(e => paid.Count(p => p.CardId == e.CardId && p.From == CardLocation.Hand(e.FromSeat) && p.To == CardLocation.Hand(e.ToSeat)) == 1) && paid.Count == division.Count(e => e.FromSeat != e.ToSeat);
    }
}
