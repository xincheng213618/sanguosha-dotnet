#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangYunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> _resolve;
    private CharacterWangYunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> resolve) => new(resolve);
}
