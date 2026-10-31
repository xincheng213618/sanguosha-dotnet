#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIdentityAppearanceInvariantsEntries
{
    internal bool HasIdentityAppearanceAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasIdentityAppearanceAttack(attack);
}
