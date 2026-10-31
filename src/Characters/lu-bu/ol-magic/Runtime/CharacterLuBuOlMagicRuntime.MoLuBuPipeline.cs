#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 2)]
    internal void CaptureMoLuBuLegacyDuelPipeline(ProgramSkillFrame frame, int sourceSeat, int targetSeat) => CaptureMoLuBuLegacyDuel(frame, sourceSeat);
}
