#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignPublicPileSlashesEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildForeignPublicPileSlashActions(global::CardGame.Core.CharacterState actor) => _resolve().BuildForeignPublicPileSlashActions(actor);
    internal int GetForeignPublicPileSlashBaseDamage(long resolutionId) => _resolve().GetForeignPublicPileSlashBaseDamage(resolutionId);
    internal bool IsForeignPublicPileSlashUse(long resolutionId) => _resolve().IsForeignPublicPileSlashUse(resolutionId);
    internal bool TryExecuteForeignPublicPileSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction selected, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryExecuteForeignPublicPileSlash(actor, selected, cards, targets);
    internal bool TrySubmitForeignPublicPileSlash(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitForeignPublicPileSlash(command, out result);
}
