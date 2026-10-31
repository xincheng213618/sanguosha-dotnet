#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolsEntries
{
    internal bool AllowsShanheSchoolTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsShanheSchoolTrigger(gate);
    internal bool HasShanheSchoolEffect(int ownerSeat, int schoolId) => _resolve().HasShanheSchoolEffect(ownerSeat, schoolId);
    internal bool IsShanheSchoolComplete(int ownerSeat, int schoolId) => _resolve().IsShanheSchoolComplete(ownerSeat, schoolId);
    internal bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId) => _resolve().IsShanheSchoolSkillQualified(ownerSeat, skillId);
}
