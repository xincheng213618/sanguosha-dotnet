#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXuRongOlRuntime))]
internal sealed partial class CharacterXuRongOlEntries
{
    private readonly Func<EngineOwners.CharacterXuRongOlRuntime> _resolve;
    private CharacterXuRongOlEntries(Func<EngineOwners.CharacterXuRongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuRongOlEntries Create(Func<EngineOwners.CharacterXuRongOlRuntime> resolve) => new(resolve);
}
