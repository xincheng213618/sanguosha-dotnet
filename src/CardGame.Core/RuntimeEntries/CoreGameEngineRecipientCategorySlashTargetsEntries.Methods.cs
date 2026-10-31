#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategorySlashTargetsEntries
{
    internal bool CanBeRecipientCategorySlashTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeRecipientCategorySlashTarget(use, target);
    internal void CommitRecipientCategorySlashTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CommitRecipientCategorySlashTargets(f);
    internal bool HasRecipientCategorySlashTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasRecipientCategorySlashTargetTail(use);
    internal bool IsRecipientCategorySlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecipientCategorySlashTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => _resolve().IsRecipientCategorySlashTargetFact(use, fact, before);
    internal bool TryGetRecipientCategorySlashUse(int owner, global::CardGame.Core.ProgramSkillWindowContext c, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetRecipientCategorySlashUse(owner, c, out use, out window);
}
