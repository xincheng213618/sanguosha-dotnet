#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFuRongOlRuntime))]
internal sealed partial class CharacterFuRongOlEntries
{
    private readonly Func<EngineOwners.CharacterFuRongOlRuntime> _resolve;
    private CharacterFuRongOlEntries(Func<EngineOwners.CharacterFuRongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuRongOlEntries Create(Func<EngineOwners.CharacterFuRongOlRuntime> resolve) => new(resolve);
}
