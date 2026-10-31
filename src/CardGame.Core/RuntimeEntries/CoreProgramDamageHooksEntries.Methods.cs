#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramDamageHooksEntries
{
    internal ModeDamageFacts CaptureModeDamageFacts(GameEngine.IDamageAttempt attack) => _resolve().CaptureModeDamageFacts(attack);
}
