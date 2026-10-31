#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBeiMiHuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _resolve;
    private CharacterBeiMiHuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBeiMiHuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> resolve) => new(resolve);
}
