#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchPresentationEntries
{
    internal string GetPublicGeneralName(global::CardGame.Core.CharacterState player) => _resolve().GetPublicGeneralName(player);
    internal string GetRoleName(global::CardGame.Core.Role role) => _resolve().GetRoleName(role);
}
