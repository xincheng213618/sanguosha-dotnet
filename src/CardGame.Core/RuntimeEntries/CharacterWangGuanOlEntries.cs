#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangGuanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> _resolve;
    private CharacterWangGuanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangGuanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> resolve) => new(resolve);
}
