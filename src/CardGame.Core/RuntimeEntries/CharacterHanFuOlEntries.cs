#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHanFuOlRuntime))]
internal sealed partial class CharacterHanFuOlEntries
{
    private readonly Func<EngineOwners.CharacterHanFuOlRuntime> _resolve;
    private CharacterHanFuOlEntries(Func<EngineOwners.CharacterHanFuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanFuOlEntries Create(Func<EngineOwners.CharacterHanFuOlRuntime> resolve) => new(resolve);
}
