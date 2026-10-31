#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangJiOlRuntime))]
internal sealed partial class CharacterZhangJiOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangJiOlRuntime> _resolve;
    private CharacterZhangJiOlEntries(Func<EngineOwners.CharacterZhangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangJiOlEntries Create(Func<EngineOwners.CharacterZhangJiOlRuntime> resolve) => new(resolve);
}
