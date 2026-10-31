#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaiFuRenOlRuntime))]
internal sealed partial class CharacterCaiFuRenOlEntries
{
    private readonly Func<EngineOwners.CharacterCaiFuRenOlRuntime> _resolve;
    private CharacterCaiFuRenOlEntries(Func<EngineOwners.CharacterCaiFuRenOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiFuRenOlEntries Create(Func<EngineOwners.CharacterCaiFuRenOlRuntime> resolve) => new(resolve);
}
