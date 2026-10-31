#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolTrickEntries
{
    internal bool CanOfferShanheSchoolTrick(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheSchoolTrick(candidate, trigger, context);
    internal void CompleteShanheSchoolTrick(long id) => _resolve().CompleteShanheSchoolTrick(id);
    internal bool HasUnstartedShanheSchoolTrick(long id) => _resolve().HasUnstartedShanheSchoolTrick(id);
    internal void IssueShanheSchoolTrick(ProgramSkillFrame supplied) => _resolve().IssueShanheSchoolTrick(supplied);
    internal bool TryBeginShanheSchoolTrick(long id, bool afterAttack) => _resolve().TryBeginShanheSchoolTrick(id, afterAttack);
    internal bool TryGetShanheSchoolTrickUse(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetShanheSchoolTrickUse(owner, context, out use, out window);
}
