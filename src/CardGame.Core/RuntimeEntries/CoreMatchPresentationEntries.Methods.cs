#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchPresentationEntries
{
    internal string GetPublicGeneralName(CharacterState player) => _resolve().GetPublicGeneralName(player);
    internal string GetRoleName(Role role) => _resolve().GetRoleName(role);
}
