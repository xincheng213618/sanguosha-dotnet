#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 2)]
    internal void CaptureMoLuBuLegacyDuelPipeline(ProgramSkillFrame frame, int sourceSeat, int targetSeat) => CaptureMoLuBuLegacyDuel(frame, sourceSeat);
}
