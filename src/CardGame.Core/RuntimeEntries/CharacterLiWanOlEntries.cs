#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiWanOlRuntime))]
internal sealed partial class CharacterLiWanOlEntries
{
    private readonly Func<EngineOwners.CharacterLiWanOlRuntime> _resolve;
    private CharacterLiWanOlEntries(Func<EngineOwners.CharacterLiWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiWanOlEntries Create(Func<EngineOwners.CharacterLiWanOlRuntime> resolve) => new(resolve);
}
