#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuYuShengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengOlRuntime> _resolve;
    private CharacterLuYuShengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuYuShengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengOlRuntime> resolve) => new(resolve);
}
