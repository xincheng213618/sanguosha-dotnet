#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuanYuHappyGodRuntime))]
internal sealed partial class CharacterGuanYuHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterGuanYuHappyGodRuntime> _resolve;
    private CharacterGuanYuHappyGodEntries(Func<EngineOwners.CharacterGuanYuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuHappyGodEntries Create(Func<EngineOwners.CharacterGuanYuHappyGodRuntime> resolve) => new(resolve);
}
