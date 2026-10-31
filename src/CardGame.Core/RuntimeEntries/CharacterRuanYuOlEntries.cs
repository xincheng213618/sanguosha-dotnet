#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed partial class CharacterRuanYuOlEntries
{
    private readonly Func<EngineOwners.CharacterRuanYuOlRuntime> _resolve;
    private CharacterRuanYuOlEntries(Func<EngineOwners.CharacterRuanYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanYuOlEntries Create(Func<EngineOwners.CharacterRuanYuOlRuntime> resolve) => new(resolve);
}
