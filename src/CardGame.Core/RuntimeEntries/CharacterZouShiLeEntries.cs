#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZouShiLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _resolve;
    private CharacterZouShiLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZouShiLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> resolve) => new(resolve);
}
