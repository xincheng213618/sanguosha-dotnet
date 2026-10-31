#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime))]
internal sealed partial class CharacterXiZhiCaiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> _resolve;
    private CharacterXiZhiCaiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiZhiCaiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> resolve) => new(resolve);
}
