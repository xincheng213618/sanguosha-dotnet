#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuBuHappyGodRuntime))]
internal sealed partial class CharacterLuBuHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterLuBuHappyGodRuntime> _resolve;
    private CharacterLuBuHappyGodEntries(Func<EngineOwners.CharacterLuBuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuHappyGodEntries Create(Func<EngineOwners.CharacterLuBuHappyGodRuntime> resolve) => new(resolve);
}
