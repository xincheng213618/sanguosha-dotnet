#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuMengHappyGodRuntime))]
internal sealed partial class CharacterLuMengHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterLuMengHappyGodRuntime> _resolve;
    private CharacterLuMengHappyGodEntries(Func<EngineOwners.CharacterLuMengHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengHappyGodEntries Create(Func<EngineOwners.CharacterLuMengHappyGodRuntime> resolve) => new(resolve);
}
