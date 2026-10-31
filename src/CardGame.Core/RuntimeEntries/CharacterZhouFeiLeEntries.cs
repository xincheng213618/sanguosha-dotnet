#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed partial class CharacterZhouFeiLeEntries
{
    private readonly Func<EngineOwners.CharacterZhouFeiLeRuntime> _resolve;
    private CharacterZhouFeiLeEntries(Func<EngineOwners.CharacterZhouFeiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFeiLeEntries Create(Func<EngineOwners.CharacterZhouFeiLeRuntime> resolve) => new(resolve);
}
