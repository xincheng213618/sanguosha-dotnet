#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageCompletionEntries
{
    internal void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CompleteDamageAttack(attack);
}
