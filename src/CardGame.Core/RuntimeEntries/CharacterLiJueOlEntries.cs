#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiJueOlRuntime))]
internal sealed partial class CharacterLiJueOlEntries
{
    private readonly Func<EngineOwners.CharacterLiJueOlRuntime> _resolve;
    private CharacterLiJueOlEntries(Func<EngineOwners.CharacterLiJueOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiJueOlEntries Create(Func<EngineOwners.CharacterLiJueOlRuntime> resolve) => new(resolve);
}
