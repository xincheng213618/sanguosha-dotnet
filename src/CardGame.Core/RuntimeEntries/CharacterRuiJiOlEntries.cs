#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterRuiJiOlRuntime))]
internal sealed partial class CharacterRuiJiOlEntries
{
    private readonly Func<EngineOwners.CharacterRuiJiOlRuntime> _resolve;
    private CharacterRuiJiOlEntries(Func<EngineOwners.CharacterRuiJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuiJiOlEntries Create(Func<EngineOwners.CharacterRuiJiOlRuntime> resolve) => new(resolve);
}
