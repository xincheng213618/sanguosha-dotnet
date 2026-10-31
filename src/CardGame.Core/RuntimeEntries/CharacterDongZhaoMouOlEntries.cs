#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongZhaoMouOlRuntime))]
internal sealed partial class CharacterDongZhaoMouOlEntries
{
    private readonly Func<EngineOwners.CharacterDongZhaoMouOlRuntime> _resolve;
    private CharacterDongZhaoMouOlEntries(Func<EngineOwners.CharacterDongZhaoMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhaoMouOlEntries Create(Func<EngineOwners.CharacterDongZhaoMouOlRuntime> resolve) => new(resolve);
}
