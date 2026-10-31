#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed partial class CharacterBianFuRenOlEntries
{
    private readonly Func<EngineOwners.CharacterBianFuRenOlRuntime> _resolve;
    private CharacterBianFuRenOlEntries(Func<EngineOwners.CharacterBianFuRenOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBianFuRenOlEntries Create(Func<EngineOwners.CharacterBianFuRenOlRuntime> resolve) => new(resolve);
}
