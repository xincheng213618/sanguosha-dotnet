#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouYuHappyGodRuntime))]
internal sealed partial class CharacterZhouYuHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuHappyGodRuntime> _resolve;
    private CharacterZhouYuHappyGodEntries(Func<EngineOwners.CharacterZhouYuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuHappyGodEntries Create(Func<EngineOwners.CharacterZhouYuHappyGodRuntime> resolve) => new(resolve);
}
