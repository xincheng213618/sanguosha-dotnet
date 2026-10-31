#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFuRongOlRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 90)]
    internal bool IsFuRongAwaitedMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => e?.Op == SkillProgramEffectOp.DiscardBasicPairAndReuse && f.FuRongPair is { Stage: FuRongPairStage.OwnerChildren or FuRongPairStage.TargetChildren or FuRongPairStage.DrawChildren or FuRongPairStage.NativeChildren } r && pending == f.PendingMovementContinuation && pending.BeforeCount == 0 && pending.CoverageResultBind is null && pending.SubjectSeat == (r.Stage == FuRongPairStage.TargetChildren ? r.TargetSeat : f.OwnerSeat);
}
