#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuYuShengOlRuntime))]
internal sealed partial class CharacterLuYuShengOlEntries
{
    private readonly Func<EngineOwners.CharacterLuYuShengOlRuntime> _resolve;
    private CharacterLuYuShengOlEntries(Func<EngineOwners.CharacterLuYuShengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuYuShengOlEntries Create(Func<EngineOwners.CharacterLuYuShengOlRuntime> resolve) => new(resolve);
}
