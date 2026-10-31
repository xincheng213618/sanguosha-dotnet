#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFanChouOlRuntime))]
internal sealed partial class CharacterFanChouOlEntries
{
    private readonly Func<EngineOwners.CharacterFanChouOlRuntime> _resolve;
    private CharacterFanChouOlEntries(Func<EngineOwners.CharacterFanChouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFanChouOlEntries Create(Func<EngineOwners.CharacterFanChouOlRuntime> resolve) => new(resolve);
}
