#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengJiChengCuiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _resolve;
    private CharacterChengJiChengCuiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengJiChengCuiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> resolve) => new(resolve);
}
