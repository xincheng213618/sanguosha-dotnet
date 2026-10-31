#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed partial class CharacterZhaoZhongOlEntries
{
    private readonly Func<EngineOwners.CharacterZhaoZhongOlRuntime> _resolve;
    private CharacterZhaoZhongOlEntries(Func<EngineOwners.CharacterZhaoZhongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoZhongOlEntries Create(Func<EngineOwners.CharacterZhaoZhongOlRuntime> resolve) => new(resolve);
}
