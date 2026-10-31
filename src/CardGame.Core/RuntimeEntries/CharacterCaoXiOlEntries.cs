#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed partial class CharacterCaoXiOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoXiOlRuntime> _resolve;
    private CharacterCaoXiOlEntries(Func<EngineOwners.CharacterCaoXiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoXiOlEntries Create(Func<EngineOwners.CharacterCaoXiOlRuntime> resolve) => new(resolve);
}
