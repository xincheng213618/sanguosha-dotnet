#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXinPiOlRuntime))]
internal sealed partial class CharacterXinPiOlEntries
{
    private readonly Func<EngineOwners.CharacterXinPiOlRuntime> _resolve;
    private CharacterXinPiOlEntries(Func<EngineOwners.CharacterXinPiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXinPiOlEntries Create(Func<EngineOwners.CharacterXinPiOlRuntime> resolve) => new(resolve);
}
