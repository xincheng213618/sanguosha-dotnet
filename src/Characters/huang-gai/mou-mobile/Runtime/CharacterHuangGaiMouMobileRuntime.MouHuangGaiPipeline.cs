#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouMobileRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 4)]
    internal void IssueMouZhaJiangLegacyDuelPipeline(ProgramSkillFrame frame, int sourceSeat, int targetSeat) => IssueMouZhaJiangLegacyDuel(frame, sourceSeat);
}
