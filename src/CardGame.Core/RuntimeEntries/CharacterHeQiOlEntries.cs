#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHeQiOlRuntime))]
internal sealed partial class CharacterHeQiOlEntries
{
    private readonly Func<EngineOwners.CharacterHeQiOlRuntime> _resolve;
    private CharacterHeQiOlEntries(Func<EngineOwners.CharacterHeQiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeQiOlEntries Create(Func<EngineOwners.CharacterHeQiOlRuntime> resolve) => new(resolve);
}
