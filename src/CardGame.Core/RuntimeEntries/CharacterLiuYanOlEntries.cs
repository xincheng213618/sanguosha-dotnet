#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime))]
internal sealed partial class CharacterLiuYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> _resolve;
    private CharacterLiuYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> resolve) => new(resolve);
}
