#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangQiYingOlRuntime))]
internal sealed partial class CharacterZhangQiYingOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangQiYingOlRuntime> _resolve;
    private CharacterZhangQiYingOlEntries(Func<EngineOwners.CharacterZhangQiYingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangQiYingOlEntries Create(Func<EngineOwners.CharacterZhangQiYingOlRuntime> resolve) => new(resolve);
}
