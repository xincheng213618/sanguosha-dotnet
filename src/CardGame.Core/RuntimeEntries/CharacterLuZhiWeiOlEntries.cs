#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuZhiWeiOlRuntime))]
internal sealed partial class CharacterLuZhiWeiOlEntries
{
    private readonly Func<EngineOwners.CharacterLuZhiWeiOlRuntime> _resolve;
    private CharacterLuZhiWeiOlEntries(Func<EngineOwners.CharacterLuZhiWeiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiWeiOlEntries Create(Func<EngineOwners.CharacterLuZhiWeiOlRuntime> resolve) => new(resolve);
}
