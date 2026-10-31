#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed partial class CharacterZhangYanOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangYanOlRuntime> _resolve;
    private CharacterZhangYanOlEntries(Func<EngineOwners.CharacterZhangYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYanOlEntries Create(Func<EngineOwners.CharacterZhangYanOlRuntime> resolve) => new(resolve);
}
