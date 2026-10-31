#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuaXiongMouOlRuntime))]
internal sealed partial class CharacterHuaXiongMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouOlRuntime> _resolve;
    private CharacterHuaXiongMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaXiongMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouOlRuntime> resolve) => new(resolve);
}
