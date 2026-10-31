#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiAnniversaryGodRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 38600)]
    internal bool ShenZfMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.ShenZhangFei is not { Paid: true, CardId: > 0, From: { } from, To: { } to } r || effect?.Op != r.Operation || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Stage is not (ShenZhangFeiStage.GainChildren or ShenZhangFeiStage.TakeChildren or ShenZhangFeiStage.PenaltyChildren or ShenZhangFeiStage.JudgmentCleanupChildren))
            return false;
        var reason = ShenZfReason + (r.Stage == ShenZhangFeiStage.GainChildren ? "judgment-gain" : r.Stage == ShenZhangFeiStage.TakeChildren ? "take" : r.Stage == ShenZhangFeiStage.JudgmentCleanupChildren ? "judgment-unclaimed" : "tu-discard");
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiMovementPaidEvent>().Count(e => e == new ShenZhangFeiMovementPaidEvent(f.Id, r.Stage, r.CardId, from, to, r.Before, r.After)) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence == r.After && m.CardId == r.CardId && m.From == from && m.To == to && m.Reason.Value == reason) == 1 && (r.Stage == ShenZhangFeiStage.GainChildren ? f.TriggerId == "judgment-first-gain" && from == CardLocation.Judgment(r.TargetSeat) && to == CardLocation.Hand(f.OwnerSeat) : r.Stage == ShenZhangFeiStage.TakeChildren ? r.Operation == SkillProgramEffectOp.ResolveShenZhangFeiShencai && from.OwnerSeat == r.TargetSeat && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && to == (_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).IsGeneralWeapon && from.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat)) : r.Stage == ShenZhangFeiStage.JudgmentCleanupChildren ? from == CardLocation.Processing && to == CardLocation.DiscardPile && HasUnclaimedShenZfJudgmentResult(f, r.CardId, r.Before) : f.TriggerId == "tu-hand-loss" && from == CardLocation.Hand(f.OwnerSeat) && to == CardLocation.DiscardPile);
    }
}
