#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengYuMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _resolve;
    private CharacterChengYuMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengYuMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> resolve) => new(resolve);
}
