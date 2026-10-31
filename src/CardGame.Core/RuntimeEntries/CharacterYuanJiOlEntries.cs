#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime))]
internal sealed partial class CharacterYuanJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> _resolve;
    private CharacterYuanJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> resolve) => new(resolve);
}
