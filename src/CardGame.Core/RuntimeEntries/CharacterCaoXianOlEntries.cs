#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoXianOlRuntime))]
internal sealed partial class CharacterCaoXianOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoXianOlRuntime> _resolve;
    private CharacterCaoXianOlEntries(Func<EngineOwners.CharacterCaoXianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoXianOlEntries Create(Func<EngineOwners.CharacterCaoXianOlRuntime> resolve) => new(resolve);
}
