#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed partial class CharacterZuoCiHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterZuoCiHappyGodRuntime> _resolve;
    private CharacterZuoCiHappyGodEntries(Func<EngineOwners.CharacterZuoCiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZuoCiHappyGodEntries Create(Func<EngineOwners.CharacterZuoCiHappyGodRuntime> resolve) => new(resolve);
}
