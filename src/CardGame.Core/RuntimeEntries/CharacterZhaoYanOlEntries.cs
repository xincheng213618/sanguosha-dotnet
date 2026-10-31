#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhaoYanOlRuntime))]
internal sealed partial class CharacterZhaoYanOlEntries
{
    private readonly Func<EngineOwners.CharacterZhaoYanOlRuntime> _resolve;
    private CharacterZhaoYanOlEntries(Func<EngineOwners.CharacterZhaoYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYanOlEntries Create(Func<EngineOwners.CharacterZhaoYanOlRuntime> resolve) => new(resolve);
}
