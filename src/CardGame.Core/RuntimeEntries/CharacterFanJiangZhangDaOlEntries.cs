#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFanJiangZhangDaOlRuntime))]
internal sealed partial class CharacterFanJiangZhangDaOlEntries
{
    private readonly Func<EngineOwners.CharacterFanJiangZhangDaOlRuntime> _resolve;
    private CharacterFanJiangZhangDaOlEntries(Func<EngineOwners.CharacterFanJiangZhangDaOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFanJiangZhangDaOlEntries Create(Func<EngineOwners.CharacterFanJiangZhangDaOlRuntime> resolve) => new(resolve);
}
