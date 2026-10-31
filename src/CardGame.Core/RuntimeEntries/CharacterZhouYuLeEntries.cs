#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouYuLeRuntime))]
internal sealed partial class CharacterZhouYuLeEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuLeRuntime> _resolve;
    private CharacterZhouYuLeEntries(Func<EngineOwners.CharacterZhouYuLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuLeEntries Create(Func<EngineOwners.CharacterZhouYuLeRuntime> resolve) => new(resolve);
}
