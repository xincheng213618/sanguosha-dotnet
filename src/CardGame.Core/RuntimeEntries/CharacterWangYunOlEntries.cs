#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime))]
internal sealed partial class CharacterWangYunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> _resolve;
    private CharacterWangYunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYunOlRuntime> resolve) => new(resolve);
}
