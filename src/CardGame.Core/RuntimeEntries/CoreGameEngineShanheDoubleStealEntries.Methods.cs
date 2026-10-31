#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDoubleStealEntries
{
    internal bool CanOfferShanheDoubleSteal(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheDoubleSteal(candidate, trigger, context);
    internal void IssueShanheDoubleSteal(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueShanheDoubleSteal(supplied);
    internal bool TryGetShanheDoubleStealUse(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetShanheDoubleStealUse(owner, context, out use, out window);
}
