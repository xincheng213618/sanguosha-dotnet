#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal bool IsPengbiTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolvePengbiFirstActualTurn }];
    internal int? FirstPengbiActualTurn(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Where(e => e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation && e.Phase.ActorSeat == owner && e.Phase.ActualTurnOwnerSeat == owner).Select(e => (int? )e.Phase.TurnNumber).Distinct().Order().FirstOrDefault();
}
