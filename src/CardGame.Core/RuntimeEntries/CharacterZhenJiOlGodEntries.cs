#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhenJiOlGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime> _resolve;
    private CharacterZhenJiOlGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiOlGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime> resolve) => new(resolve);
}
