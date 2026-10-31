#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime))]
internal sealed partial class CharacterGuanYuHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> _resolve;
    private CharacterGuanYuHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuHappyGodRuntime> resolve) => new(resolve);
}
