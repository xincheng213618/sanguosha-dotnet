#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingProgramsEntries
{
    internal void ResumeProgramVirtualDyingAlcoholRecovery(long resolutionId, long programParentId, long dyingId) => _resolve().ResumeProgramVirtualDyingAlcoholRecovery(resolutionId, programParentId, dyingId);
    internal void RevealProgramUniqueRankForDying(ProgramSkillFrame frame, CardZoneKind zone, int rescueHp) => _resolve().RevealProgramUniqueRankForDying(frame, zone, rescueHp);
    internal void UseProgramBoundCardAsDyingAlcohol(ProgramSkillFrame frame, string sourceBind, CardMoveReason reason) => _resolve().UseProgramBoundCardAsDyingAlcohol(frame, sourceBind, reason);
    internal void UseProgramVirtualDyingAlcohol(ProgramSkillFrame frame) => _resolve().UseProgramVirtualDyingAlcohol(frame);
}
