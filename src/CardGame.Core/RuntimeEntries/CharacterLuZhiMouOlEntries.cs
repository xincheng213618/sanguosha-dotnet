#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime))]
internal sealed partial class CharacterLuZhiMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> _resolve;
    private CharacterLuZhiMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouOlRuntime> resolve) => new(resolve);
}
