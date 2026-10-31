#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime))]
internal sealed partial class CharacterLuBuHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> _resolve;
    private CharacterLuBuHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> resolve) => new(resolve);
}
