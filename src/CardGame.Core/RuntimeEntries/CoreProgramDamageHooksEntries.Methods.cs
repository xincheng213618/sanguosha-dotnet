#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramDamageHooksEntries
{
    internal global::CardGame.Core.ModeDamageFacts CaptureModeDamageFacts(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CaptureModeDamageFacts(attack);
}
