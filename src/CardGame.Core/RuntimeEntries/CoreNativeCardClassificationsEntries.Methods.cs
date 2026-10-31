#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardClassificationsEntries
{
    internal global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetDamageNature(attack);
}
