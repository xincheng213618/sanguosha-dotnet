#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _resolve;
    private CharacterMaChengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> resolve) => new(resolve);
}
