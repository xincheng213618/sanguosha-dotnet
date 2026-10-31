#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed partial class CharacterCaoChunOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoChunOlRuntime> _resolve;
    private CharacterCaoChunOlEntries(Func<EngineOwners.CharacterCaoChunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoChunOlEntries Create(Func<EngineOwners.CharacterCaoChunOlRuntime> resolve) => new(resolve);
}
