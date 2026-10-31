#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBaoXinOlRuntime))]
internal sealed partial class CharacterBaoXinOlEntries
{
    private readonly Func<EngineOwners.CharacterBaoXinOlRuntime> _resolve;
    private CharacterBaoXinOlEntries(Func<EngineOwners.CharacterBaoXinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaoXinOlEntries Create(Func<EngineOwners.CharacterBaoXinOlRuntime> resolve) => new(resolve);
}
