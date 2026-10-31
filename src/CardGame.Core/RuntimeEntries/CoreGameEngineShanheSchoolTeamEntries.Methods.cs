#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolTeamEntries
{
    internal bool AllowsShanheSharedRebirth(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsShanheSharedRebirth(gate);
    internal bool AllowsShanheTeamHealth(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().AllowsShanheTeamHealth(candidate);
    internal void ApplyShanheAlliedHealth(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ApplyShanheAlliedHealth(supplied);
    internal void ApplyShanheAllyHealth(global::CardGame.Core.ProgramSkillFrame frame, int seat, int amount) => _resolve().ApplyShanheAllyHealth(frame, seat, amount);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheSchoolTeamRank(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheSchoolTeamRank(supplied);
    internal void ContinueShanheSchoolTeamRank(long id) => _resolve().ContinueShanheSchoolTeamRank(id);
    internal bool IsActualShanheAlly(int seat) => _resolve().IsActualShanheAlly(seat);
    internal global::CardGame.Core.ProgramSkillFrame RequireShanheTeamOpening(global::CardGame.Core.ProgramSkillFrame supplied, string skillId, string binding, bool requireActive = true) => _resolve().RequireShanheTeamOpening(supplied, skillId, binding, requireActive);
    internal bool ResumeShanheSchoolTeamRank(long frameId) => _resolve().ResumeShanheSchoolTeamRank(frameId);
    internal void ReturnShanheSchoolTeamRankDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheSchoolTeamRankDraw(supplied, outcome);
    internal bool ReturnShanheTeamRankMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnShanheTeamRankMovement(frame);
    internal bool ShanheAllyExpeditionRebirthAvailable(int seat) => _resolve().ShanheAllyExpeditionRebirthAvailable(seat);
    internal int ShanheSharedRankRuleBonus(int seat, global::CardGame.Core.SkillRuleQuery query) => _resolve().ShanheSharedRankRuleBonus(seat, query);
    internal bool HasShanheTeamHealthTactic { get => _resolve().HasShanheTeamHealthTactic; }
}
