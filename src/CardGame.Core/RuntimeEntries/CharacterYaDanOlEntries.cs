#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime))]
internal sealed partial class CharacterYaDanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> _resolve;
    private CharacterYaDanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYaDanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> resolve) => new(resolve);
}
