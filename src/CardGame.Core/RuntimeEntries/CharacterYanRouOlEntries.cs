#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYanRouOlRuntime))]
internal sealed partial class CharacterYanRouOlEntries
{
    private readonly Func<EngineOwners.CharacterYanRouOlRuntime> _resolve;
    private CharacterYanRouOlEntries(Func<EngineOwners.CharacterYanRouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanRouOlEntries Create(Func<EngineOwners.CharacterYanRouOlRuntime> resolve) => new(resolve);
}
