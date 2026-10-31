#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuZhiMouOlRuntime))]
internal sealed partial class CharacterLuZhiMouOlEntries
{
    private readonly Func<EngineOwners.CharacterLuZhiMouOlRuntime> _resolve;
    private CharacterLuZhiMouOlEntries(Func<EngineOwners.CharacterLuZhiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMouOlEntries Create(Func<EngineOwners.CharacterLuZhiMouOlRuntime> resolve) => new(resolve);
}
