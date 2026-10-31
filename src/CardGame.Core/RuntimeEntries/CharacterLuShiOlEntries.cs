#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuShiOlRuntime))]
internal sealed partial class CharacterLuShiOlEntries
{
    private readonly Func<EngineOwners.CharacterLuShiOlRuntime> _resolve;
    private CharacterLuShiOlEntries(Func<EngineOwners.CharacterLuShiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuShiOlEntries Create(Func<EngineOwners.CharacterLuShiOlRuntime> resolve) => new(resolve);
}
