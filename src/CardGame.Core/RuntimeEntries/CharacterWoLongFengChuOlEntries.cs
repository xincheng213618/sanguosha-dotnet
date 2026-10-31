#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWoLongFengChuOlRuntime))]
internal sealed partial class CharacterWoLongFengChuOlEntries
{
    private readonly Func<EngineOwners.CharacterWoLongFengChuOlRuntime> _resolve;
    private CharacterWoLongFengChuOlEntries(Func<EngineOwners.CharacterWoLongFengChuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWoLongFengChuOlEntries Create(Func<EngineOwners.CharacterWoLongFengChuOlRuntime> resolve) => new(resolve);
}
