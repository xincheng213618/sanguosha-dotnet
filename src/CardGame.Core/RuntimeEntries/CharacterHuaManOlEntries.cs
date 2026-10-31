#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaManOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _resolve;
    private CharacterHuaManOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaManOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> resolve) => new(resolve);
}
