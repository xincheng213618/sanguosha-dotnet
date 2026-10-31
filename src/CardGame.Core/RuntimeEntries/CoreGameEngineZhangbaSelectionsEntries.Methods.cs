#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhangbaSelectionsEntries
{
    internal IReadOnlyList<ActiveSkillCardGroupTarget> GetZhangbaSlashGroupTargets(CharacterState actor) => _resolve().GetZhangbaSlashGroupTargets(actor);
}
