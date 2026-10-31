#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDongZhaoMouOlRuntime))]
internal sealed partial class CharacterDongZhaoMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMouOlRuntime> _resolve;
    private CharacterDongZhaoMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhaoMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMouOlRuntime> resolve) => new(resolve);
}
