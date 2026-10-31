#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed partial class CharacterLiuPanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _resolve;
    private CharacterLiuPanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuPanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> resolve) => new(resolve);
}
