#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanHuanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanHuanOlRuntime> _resolve;
    private CharacterYuanHuanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanHuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanHuanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanHuanOlRuntime> resolve) => new(resolve);
}
