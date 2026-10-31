#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed partial class CharacterZhangYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _resolve;
    private CharacterZhangYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> resolve) => new(resolve);
}
