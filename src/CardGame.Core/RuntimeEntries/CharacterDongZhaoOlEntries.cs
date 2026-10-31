#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongZhaoOlRuntime))]
internal sealed partial class CharacterDongZhaoOlEntries
{
    private readonly Func<EngineOwners.CharacterDongZhaoOlRuntime> _resolve;
    private CharacterDongZhaoOlEntries(Func<EngineOwners.CharacterDongZhaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhaoOlEntries Create(Func<EngineOwners.CharacterDongZhaoOlRuntime> resolve) => new(resolve);
}
