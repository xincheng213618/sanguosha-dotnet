#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhenJiHappyGodRuntime))]
internal sealed partial class CharacterZhenJiHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterZhenJiHappyGodRuntime> _resolve;
    private CharacterZhenJiHappyGodEntries(Func<EngineOwners.CharacterZhenJiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiHappyGodEntries Create(Func<EngineOwners.CharacterZhenJiHappyGodRuntime> resolve) => new(resolve);
}
