#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunQuanHappyGodRuntime))]
internal sealed partial class CharacterSunQuanHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterSunQuanHappyGodRuntime> _resolve;
    private CharacterSunQuanHappyGodEntries(Func<EngineOwners.CharacterSunQuanHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanHappyGodEntries Create(Func<EngineOwners.CharacterSunQuanHappyGodRuntime> resolve) => new(resolve);
}
