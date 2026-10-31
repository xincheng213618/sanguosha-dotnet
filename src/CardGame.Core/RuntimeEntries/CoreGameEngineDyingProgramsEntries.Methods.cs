#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingProgramsEntries
{
    internal void ResumeProgramVirtualDyingAlcoholRecovery(long resolutionId, long programParentId, long dyingId) => _resolve().ResumeProgramVirtualDyingAlcoholRecovery(resolutionId, programParentId, dyingId);
    internal void RevealProgramUniqueRankForDying(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardZoneKind zone, int rescueHp) => _resolve().RevealProgramUniqueRankForDying(frame, zone, rescueHp);
    internal void UseProgramBoundCardAsDyingAlcohol(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, global::CardGame.Core.CardMoveReason reason) => _resolve().UseProgramBoundCardAsDyingAlcohol(frame, sourceBind, reason);
    internal void UseProgramVirtualDyingAlcohol(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UseProgramVirtualDyingAlcohol(frame);
}
