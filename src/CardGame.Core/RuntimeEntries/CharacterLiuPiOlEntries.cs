#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPiOlRuntime))]
internal sealed partial class CharacterLiuPiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPiOlRuntime> _resolve;
    private CharacterLiuPiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuPiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPiOlRuntime> resolve) => new(resolve);
}
