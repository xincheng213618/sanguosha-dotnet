#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongBaiOlRuntime))]
internal sealed partial class CharacterDongBaiOlEntries
{
    private readonly Func<EngineOwners.CharacterDongBaiOlRuntime> _resolve;
    private CharacterDongBaiOlEntries(Func<EngineOwners.CharacterDongBaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongBaiOlEntries Create(Func<EngineOwners.CharacterDongBaiOlRuntime> resolve) => new(resolve);
}
