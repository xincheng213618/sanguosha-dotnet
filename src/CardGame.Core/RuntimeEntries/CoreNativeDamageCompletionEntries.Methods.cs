#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageCompletionEntries
{
    internal void CompleteDamageAttack(GameEngine.IDamageAttempt attack) => _resolve().CompleteDamageAttack(attack);
}
