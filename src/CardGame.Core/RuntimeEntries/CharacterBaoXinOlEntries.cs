#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime))]
internal sealed partial class CharacterBaoXinOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> _resolve;
    private CharacterBaoXinOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaoXinOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> resolve) => new(resolve);
}
