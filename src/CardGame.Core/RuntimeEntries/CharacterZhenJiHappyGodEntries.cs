#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime))]
internal sealed partial class CharacterZhenJiHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> _resolve;
    private CharacterZhenJiHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> resolve) => new(resolve);
}
