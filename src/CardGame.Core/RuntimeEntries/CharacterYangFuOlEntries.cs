#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYangFuOlRuntime))]
internal sealed partial class CharacterYangFuOlEntries
{
    private readonly Func<EngineOwners.CharacterYangFuOlRuntime> _resolve;
    private CharacterYangFuOlEntries(Func<EngineOwners.CharacterYangFuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangFuOlEntries Create(Func<EngineOwners.CharacterYangFuOlRuntime> resolve) => new(resolve);
}
