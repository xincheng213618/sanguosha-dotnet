#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuChuAnniversaryGodRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 38800)]
    internal bool IsZhuangpoPaidMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.ShenXuChu is not { Operation: SkillProgramEffectOp.ResolveZhuangpoTarget, Stage: ShenXuChuStage.DiscardChildren } r || e?.Op != r.Operation || p.SubjectSeat != r.TargetSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuangpoDiscardPaidEvent>().Where(x => x.FrameId == f.Id && x.TargetSeat == r.TargetSeat && x.Before == r.Before).ToArray()is not [var paid] || paid.After != r.After || paid.Count != r.RequiredDiscards || r.Selected.Count != paid.Count)
            return false;
        return r.Selected.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.CardId == id && m.From.OwnerSeat == r.TargetSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.To == CardLocation.DiscardPile && m.Reason.Value == ZhuangpoDiscardReason(f)) == 1);
    }
}
