#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoYuOlRuntime))]
internal sealed partial class CharacterCaoYuOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoYuOlRuntime> _resolve;
    private CharacterCaoYuOlEntries(Func<EngineOwners.CharacterCaoYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoYuOlEntries Create(Func<EngineOwners.CharacterCaoYuOlRuntime> resolve) => new(resolve);
}
