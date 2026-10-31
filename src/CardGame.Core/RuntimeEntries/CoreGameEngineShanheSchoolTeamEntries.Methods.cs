#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolTeamEntries
{
    internal bool AllowsShanheSharedRebirth(ProgramTriggerLegalityContext gate) => _resolve().AllowsShanheSharedRebirth(gate);
    internal bool AllowsShanheTeamHealth(ProgramTriggerCandidate candidate) => _resolve().AllowsShanheTeamHealth(candidate);
    internal void ApplyShanheAlliedHealth(ProgramSkillFrame supplied) => _resolve().ApplyShanheAlliedHealth(supplied);
    internal void ApplyShanheAllyHealth(ProgramSkillFrame frame, int seat, int amount) => _resolve().ApplyShanheAllyHealth(frame, seat, amount);
    internal SkillProgramStepOutcome BeginShanheSchoolTeamRank(ProgramSkillFrame supplied) => _resolve().BeginShanheSchoolTeamRank(supplied);
    internal void ContinueShanheSchoolTeamRank(long id) => _resolve().ContinueShanheSchoolTeamRank(id);
    internal bool IsActualShanheAlly(int seat) => _resolve().IsActualShanheAlly(seat);
    internal ProgramSkillFrame RequireShanheTeamOpening(ProgramSkillFrame supplied, string skillId, string binding, bool requireActive = true) => _resolve().RequireShanheTeamOpening(supplied, skillId, binding, requireActive);
    internal bool ResumeShanheSchoolTeamRank(long frameId) => _resolve().ResumeShanheSchoolTeamRank(frameId);
    internal void ReturnShanheSchoolTeamRankDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheSchoolTeamRankDraw(supplied, outcome);
    internal bool ReturnShanheTeamRankMovement(ProgramSkillFrame frame) => _resolve().ReturnShanheTeamRankMovement(frame);
    internal bool ShanheAllyExpeditionRebirthAvailable(int seat) => _resolve().ShanheAllyExpeditionRebirthAvailable(seat);
    internal int ShanheSharedRankRuleBonus(int seat, SkillRuleQuery query) => _resolve().ShanheSharedRankRuleBonus(seat, query);
    internal bool HasShanheTeamHealthTactic { get => _resolve().HasShanheTeamHealthTactic; }
}
