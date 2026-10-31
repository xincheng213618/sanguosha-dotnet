#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuoXianOlRuntime))]
internal sealed partial class CharacterLuoXianOlEntries
{
    private readonly Func<EngineOwners.CharacterLuoXianOlRuntime> _resolve;
    private CharacterLuoXianOlEntries(Func<EngineOwners.CharacterLuoXianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuoXianOlEntries Create(Func<EngineOwners.CharacterLuoXianOlRuntime> resolve) => new(resolve);
}
