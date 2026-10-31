#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuanJiOlRuntime))]
internal sealed partial class CharacterYuanJiOlEntries
{
    private readonly Func<EngineOwners.CharacterYuanJiOlRuntime> _resolve;
    private CharacterYuanJiOlEntries(Func<EngineOwners.CharacterYuanJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanJiOlEntries Create(Func<EngineOwners.CharacterYuanJiOlRuntime> resolve) => new(resolve);
}
