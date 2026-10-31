#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed partial class CharacterWenChouMouOlEntries
{
    private readonly Func<EngineOwners.CharacterWenChouMouOlRuntime> _resolve;
    private CharacterWenChouMouOlEntries(Func<EngineOwners.CharacterWenChouMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWenChouMouOlEntries Create(Func<EngineOwners.CharacterWenChouMouOlRuntime> resolve) => new(resolve);
}
