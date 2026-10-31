#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangZuMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> _resolve;
    private CharacterHuangZuMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangZuMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> resolve) => new(resolve);
}
