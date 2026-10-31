#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime))]
internal sealed partial class CharacterLiuQiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> _resolve;
    private CharacterLiuQiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuQiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> resolve) => new(resolve);
}
