#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouMobileRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 4)]
    internal void IssueMouZhaJiangLegacyDuelPipeline(ProgramSkillFrame frame, int sourceSeat, int targetSeat) => IssueMouZhaJiangLegacyDuel(frame, sourceSeat);
}
