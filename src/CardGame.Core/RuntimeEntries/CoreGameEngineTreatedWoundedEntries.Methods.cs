#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTreatedWoundedEntries
{
    internal bool IsRuleWounded(global::CardGame.Core.CharacterState player) => _resolve().IsRuleWounded(player);
    internal bool UsesTreatedWounded { get => _resolve().UsesTreatedWounded; }
}
