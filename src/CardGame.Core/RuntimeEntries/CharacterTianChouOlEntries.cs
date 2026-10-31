#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTianChouOlRuntime))]
internal sealed partial class CharacterTianChouOlEntries
{
    private readonly Func<EngineOwners.CharacterTianChouOlRuntime> _resolve;
    private CharacterTianChouOlEntries(Func<EngineOwners.CharacterTianChouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianChouOlEntries Create(Func<EngineOwners.CharacterTianChouOlRuntime> resolve) => new(resolve);
}
