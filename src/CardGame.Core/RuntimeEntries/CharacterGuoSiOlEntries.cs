#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed partial class CharacterGuoSiOlEntries
{
    private readonly Func<EngineOwners.CharacterGuoSiOlRuntime> _resolve;
    private CharacterGuoSiOlEntries(Func<EngineOwners.CharacterGuoSiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoSiOlEntries Create(Func<EngineOwners.CharacterGuoSiOlRuntime> resolve) => new(resolve);
}
