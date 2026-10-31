#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuYongOlRuntime))]
internal sealed partial class CharacterGuYongOlEntries
{
    private readonly Func<EngineOwners.CharacterGuYongOlRuntime> _resolve;
    private CharacterGuYongOlEntries(Func<EngineOwners.CharacterGuYongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuYongOlEntries Create(Func<EngineOwners.CharacterGuYongOlRuntime> resolve) => new(resolve);
}
