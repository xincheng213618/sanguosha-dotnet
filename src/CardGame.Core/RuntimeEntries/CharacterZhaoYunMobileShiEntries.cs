#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> _resolve;
    private CharacterZhaoYunMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYunMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> resolve) => new(resolve);
}
