#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDingYuanOlRuntime))]
internal sealed partial class CharacterDingYuanOlEntries
{
    private readonly Func<EngineOwners.CharacterDingYuanOlRuntime> _resolve;
    private CharacterDingYuanOlEntries(Func<EngineOwners.CharacterDingYuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDingYuanOlEntries Create(Func<EngineOwners.CharacterDingYuanOlRuntime> resolve) => new(resolve);
}
