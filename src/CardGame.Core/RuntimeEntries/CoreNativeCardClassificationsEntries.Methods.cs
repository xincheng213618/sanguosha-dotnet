#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardClassificationsEntries
{
    internal DamageNature GetDamageNature(GameEngine.IDamageAttempt attack) => _resolve().GetDamageNature(attack);
}
