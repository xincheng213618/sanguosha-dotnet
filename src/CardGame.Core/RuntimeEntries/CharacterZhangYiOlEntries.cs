#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangYiOlRuntime))]
internal sealed partial class CharacterZhangYiOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangYiOlRuntime> _resolve;
    private CharacterZhangYiOlEntries(Func<EngineOwners.CharacterZhangYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYiOlEntries Create(Func<EngineOwners.CharacterZhangYiOlRuntime> resolve) => new(resolve);
}
