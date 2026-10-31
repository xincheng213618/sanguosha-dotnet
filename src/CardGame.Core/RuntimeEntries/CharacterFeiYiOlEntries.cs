#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed partial class CharacterFeiYiOlEntries
{
    private readonly Func<EngineOwners.CharacterFeiYiOlRuntime> _resolve;
    private CharacterFeiYiOlEntries(Func<EngineOwners.CharacterFeiYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFeiYiOlEntries Create(Func<EngineOwners.CharacterFeiYiOlRuntime> resolve) => new(resolve);
}
