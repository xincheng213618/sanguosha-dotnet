#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaYuanYiOlRuntime))]
internal sealed partial class CharacterMaYuanYiOlEntries
{
    private readonly Func<EngineOwners.CharacterMaYuanYiOlRuntime> _resolve;
    private CharacterMaYuanYiOlEntries(Func<EngineOwners.CharacterMaYuanYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaYuanYiOlEntries Create(Func<EngineOwners.CharacterMaYuanYiOlRuntime> resolve) => new(resolve);
}
