#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPeiYuanShaoOlRuntime))]
internal sealed partial class CharacterPeiYuanShaoOlEntries
{
    private readonly Func<EngineOwners.CharacterPeiYuanShaoOlRuntime> _resolve;
    private CharacterPeiYuanShaoOlEntries(Func<EngineOwners.CharacterPeiYuanShaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPeiYuanShaoOlEntries Create(Func<EngineOwners.CharacterPeiYuanShaoOlRuntime> resolve) => new(resolve);
}
