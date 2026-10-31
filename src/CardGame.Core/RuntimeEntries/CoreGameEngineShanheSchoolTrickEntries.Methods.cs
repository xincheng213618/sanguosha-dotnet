#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolTrickEntries
{
    internal void AssertShanheSchoolTrickRounds() => _resolve().AssertShanheSchoolTrickRounds();
    internal bool CanOfferShanheSchoolTrick(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheSchoolTrick(candidate, trigger, context);
    internal void CompleteShanheSchoolTrick(long id) => _resolve().CompleteShanheSchoolTrick(id);
    internal bool HasUnstartedShanheSchoolTrick(long id) => _resolve().HasUnstartedShanheSchoolTrick(id);
    internal void IssueShanheSchoolTrick(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueShanheSchoolTrick(supplied);
    internal bool TryBeginShanheSchoolTrick(long id, bool afterAttack) => _resolve().TryBeginShanheSchoolTrick(id, afterAttack);
    internal bool TryGetShanheSchoolTrickUse(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetShanheSchoolTrickUse(owner, context, out use, out window);
}
