#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiAnniversaryGodRuntime
{
    internal bool HasShenJwPaidPingxiang(ProgramSkillFrame f) => f.ShenJiangWei is { Operation: SkillProgramEffectOp.ResolveShenJiangWeiPingxiang } r && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenJiangWeiPingxiangPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.MaximumBefore == r.MaximumBefore && e.MaximumAfter == r.MaximumAfter && e.MaximumBefore > 9 && e.MaximumBefore - e.MaximumAfter == 9) == 1;
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 38400)]
    internal bool ProveShenJwMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending)
    {
        if (f.ShenJiangWei is not { Operation: SkillProgramEffectOp.ResolveShenJiangWeiJiufa } r || e?.Op != r.Operation || r.Stage is not (ShenJiangWeiStage.RevealChildren or ShenJiangWeiStage.GainChildren or ShenJiangWeiStage.DiscardChildren) || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ShenJiangWeiMovementPaidEvent>().Where(p => p.FrameId == f.Id && p.Stage == r.Stage).ToArray();
        if (paid is not [var p] || p.Before != r.Before || p.After != r.After || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || p.Count < 0)
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > p.Before && m.Sequence <= p.After).ToArray();
        var allowed = r.Stage == ShenJiangWeiStage.GainChildren ? r.Selected : r.Stage == ShenJiangWeiStage.DiscardChildren ? r.Revealed.Except(r.Selected).ToArray() : r.Revealed;
        var exact = moves.Where(m => allowed.Contains(m.CardId) && (r.Stage == ShenJiangWeiStage.RevealChildren ? m.From == CardLocation.DrawPile && m.To == CardLocation.Processing : m.From == CardLocation.Processing && m.To == (r.Stage == ShenJiangWeiStage.GainChildren ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile)) && m.Reason.Value == (r.Stage == ShenJiangWeiStage.RevealChildren ? "skill-program.anniversary:jiufa." + f.ActivationId + ".reveal" : r.Stage == ShenJiangWeiStage.GainChildren ? "skill-program.anniversary:jiufa.obtain" : "skill-program.anniversary:jiufa.discard")).ToArray();
        return exact.Length == p.Count && exact.Select(m => m.CardId).Distinct().Count() == p.Count;
    }
}
