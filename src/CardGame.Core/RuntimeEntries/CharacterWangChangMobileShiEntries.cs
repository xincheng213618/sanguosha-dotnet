#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangChangMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> _resolve;
    private CharacterWangChangMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangChangMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangMobileShiRuntime> resolve) => new(resolve);
}
