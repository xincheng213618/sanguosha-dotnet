#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime))]
internal sealed partial class CharacterTangTangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> _resolve;
    private CharacterTangTangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangTangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> resolve) => new(resolve);
}
