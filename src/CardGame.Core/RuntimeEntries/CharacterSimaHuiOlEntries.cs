#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaHuiOlRuntime))]
internal sealed partial class CharacterSimaHuiOlEntries
{
    private readonly Func<EngineOwners.CharacterSimaHuiOlRuntime> _resolve;
    private CharacterSimaHuiOlEntries(Func<EngineOwners.CharacterSimaHuiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaHuiOlEntries Create(Func<EngineOwners.CharacterSimaHuiOlRuntime> resolve) => new(resolve);
}
