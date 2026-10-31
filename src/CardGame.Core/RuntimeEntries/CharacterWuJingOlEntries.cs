#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWuJingOlRuntime))]
internal sealed partial class CharacterWuJingOlEntries
{
    private readonly Func<EngineOwners.CharacterWuJingOlRuntime> _resolve;
    private CharacterWuJingOlEntries(Func<EngineOwners.CharacterWuJingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuJingOlEntries Create(Func<EngineOwners.CharacterWuJingOlRuntime> resolve) => new(resolve);
}
