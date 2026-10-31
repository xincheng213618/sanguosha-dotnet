#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDoubleStealEntries
{
    internal bool CanOfferShanheDoubleSteal(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheDoubleSteal(candidate, trigger, context);
    internal void IssueShanheDoubleSteal(ProgramSkillFrame supplied) => _resolve().IssueShanheDoubleSteal(supplied);
    internal bool TryGetShanheDoubleStealUse(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetShanheDoubleStealUse(owner, context, out use, out window);
}
