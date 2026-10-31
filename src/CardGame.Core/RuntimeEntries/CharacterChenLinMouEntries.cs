#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChenLinMouRuntime))]
internal sealed partial class CharacterChenLinMouEntries
{
    private readonly Func<EngineOwners.CharacterChenLinMouRuntime> _resolve;
    private CharacterChenLinMouEntries(Func<EngineOwners.CharacterChenLinMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenLinMouEntries Create(Func<EngineOwners.CharacterChenLinMouRuntime> resolve) => new(resolve);
}
