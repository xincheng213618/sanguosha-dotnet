#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed partial class CharacterZhouQunOlEntries
{
    private readonly Func<EngineOwners.CharacterZhouQunOlRuntime> _resolve;
    private CharacterZhouQunOlEntries(Func<EngineOwners.CharacterZhouQunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouQunOlEntries Create(Func<EngineOwners.CharacterZhouQunOlRuntime> resolve) => new(resolve);
}
