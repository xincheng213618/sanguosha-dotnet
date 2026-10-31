#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategorySlashTargetsEntries
{
    internal bool CanBeRecipientCategorySlashTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeRecipientCategorySlashTarget(use, target);
    internal void CommitRecipientCategorySlashTargets(ProgramSkillFrame f) => _resolve().CommitRecipientCategorySlashTargets(f);
    internal bool HasRecipientCategorySlashTargetTail(CardUseFrame use) => _resolve().HasRecipientCategorySlashTargetTail(use);
    internal bool IsRecipientCategorySlashTargetFact(CardUseFrame use, RecipientCategorySlashTargetsResolvedEvent fact, IReadOnlyList<int> before) => _resolve().IsRecipientCategorySlashTargetFact(use, fact, before);
    internal bool TryGetRecipientCategorySlashUse(int owner, ProgramSkillWindowContext c, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetRecipientCategorySlashUse(owner, c, out use, out window);
}
