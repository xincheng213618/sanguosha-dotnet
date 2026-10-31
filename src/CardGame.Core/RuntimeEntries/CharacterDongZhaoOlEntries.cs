#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime))]
internal sealed partial class CharacterDongZhaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> _resolve;
    private CharacterDongZhaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoOlRuntime> resolve) => new(resolve);
}
