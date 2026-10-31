#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime))]
internal sealed partial class CharacterWangGuanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> _resolve;
    private CharacterWangGuanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangGuanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> resolve) => new(resolve);
}
