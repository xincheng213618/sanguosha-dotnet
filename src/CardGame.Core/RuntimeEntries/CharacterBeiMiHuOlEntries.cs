#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed partial class CharacterBeiMiHuOlEntries
{
    private readonly Func<EngineOwners.CharacterBeiMiHuOlRuntime> _resolve;
    private CharacterBeiMiHuOlEntries(Func<EngineOwners.CharacterBeiMiHuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBeiMiHuOlEntries Create(Func<EngineOwners.CharacterBeiMiHuOlRuntime> resolve) => new(resolve);
}
