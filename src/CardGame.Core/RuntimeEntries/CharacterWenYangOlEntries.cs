#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime))]
internal sealed partial class CharacterWenYangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> _resolve;
    private CharacterWenYangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWenYangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenYangOlRuntime> resolve) => new(resolve);
}
