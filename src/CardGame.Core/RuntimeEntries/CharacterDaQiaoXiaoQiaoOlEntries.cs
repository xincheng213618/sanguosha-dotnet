#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime))]
internal sealed partial class CharacterDaQiaoXiaoQiaoOlEntries
{
    private readonly Func<EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime> _resolve;
    private CharacterDaQiaoXiaoQiaoOlEntries(Func<EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoXiaoQiaoOlEntries Create(Func<EngineOwners.CharacterDaQiaoXiaoQiaoOlRuntime> resolve) => new(resolve);
}
