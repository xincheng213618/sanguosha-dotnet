#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed partial class CharacterLiuZhangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _resolve;
    private CharacterLiuZhangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuZhangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> resolve) => new(resolve);
}
