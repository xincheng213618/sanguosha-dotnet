#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime))]
internal sealed partial class CharacterLuMengHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime> _resolve;
    private CharacterLuMengHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengHappyGodRuntime> resolve) => new(resolve);
}
