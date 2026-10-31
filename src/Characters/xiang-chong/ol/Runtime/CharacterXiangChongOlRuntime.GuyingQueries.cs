#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiangChongOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27140)]
    internal bool QualifyGuying(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveGuyingAtomicLossChoice }] ? XiangLoss(c.OwnerSeat, context)is not null : t.Effects is not [{ Op: SkillProgramEffectOp.SettleGuyingPreparationDebt }] || context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && XiangDebtCount(c.OwnerSeat) > 0;
}
