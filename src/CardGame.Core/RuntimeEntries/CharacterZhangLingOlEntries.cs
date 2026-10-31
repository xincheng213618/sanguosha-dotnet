#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangLingOlRuntime))]
internal sealed partial class CharacterZhangLingOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangLingOlRuntime> _resolve;
    private CharacterZhangLingOlEntries(Func<EngineOwners.CharacterZhangLingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLingOlEntries Create(Func<EngineOwners.CharacterZhangLingOlRuntime> resolve) => new(resolve);
}
