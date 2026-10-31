#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed partial class CharacterZhouYuOlMouEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuOlMouRuntime> _resolve;
    private CharacterZhouYuOlMouEntries(Func<EngineOwners.CharacterZhouYuOlMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuOlMouEntries Create(Func<EngineOwners.CharacterZhouYuOlMouRuntime> resolve) => new(resolve);
}
