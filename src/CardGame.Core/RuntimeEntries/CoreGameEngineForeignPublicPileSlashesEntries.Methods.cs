#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignPublicPileSlashesEntries
{
    internal IEnumerable<LegalAction> BuildForeignPublicPileSlashActions(CharacterState actor) => _resolve().BuildForeignPublicPileSlashActions(actor);
    internal int GetForeignPublicPileSlashBaseDamage(long resolutionId) => _resolve().GetForeignPublicPileSlashBaseDamage(resolutionId);
    internal bool IsForeignPublicPileSlashUse(long resolutionId) => _resolve().IsForeignPublicPileSlashUse(resolutionId);
    internal bool TryExecuteForeignPublicPileSlash(CharacterState actor, LegalAction selected, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().TryExecuteForeignPublicPileSlash(actor, selected, cards, targets);
    internal bool TrySubmitForeignPublicPileSlash(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().TrySubmitForeignPublicPileSlash(command, out result);
}
