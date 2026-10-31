#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterRuanHuiOlRuntime))]
internal sealed partial class CharacterRuanHuiOlEntries
{
    private readonly Func<EngineOwners.CharacterRuanHuiOlRuntime> _resolve;
    private CharacterRuanHuiOlEntries(Func<EngineOwners.CharacterRuanHuiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanHuiOlEntries Create(Func<EngineOwners.CharacterRuanHuiOlRuntime> resolve) => new(resolve);
}
