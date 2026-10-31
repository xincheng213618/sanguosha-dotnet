#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime))]
internal sealed partial class CharacterZhenJiOlGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime> _resolve;
    private CharacterZhenJiOlGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiOlGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiOlGodRuntime> resolve) => new(resolve);
}
