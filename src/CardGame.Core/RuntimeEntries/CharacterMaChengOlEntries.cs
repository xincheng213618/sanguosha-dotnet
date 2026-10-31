#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed partial class CharacterMaChengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _resolve;
    private CharacterMaChengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> resolve) => new(resolve);
}
