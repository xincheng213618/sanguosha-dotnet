#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed partial class CharacterAHuiNanOlEntries
{
    private readonly Func<EngineOwners.CharacterAHuiNanOlRuntime> _resolve;
    private CharacterAHuiNanOlEntries(Func<EngineOwners.CharacterAHuiNanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterAHuiNanOlEntries Create(Func<EngineOwners.CharacterAHuiNanOlRuntime> resolve) => new(resolve);
}
