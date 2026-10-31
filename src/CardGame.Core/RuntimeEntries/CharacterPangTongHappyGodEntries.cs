#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPangTongHappyGodRuntime))]
internal sealed partial class CharacterPangTongHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterPangTongHappyGodRuntime> _resolve;
    private CharacterPangTongHappyGodEntries(Func<EngineOwners.CharacterPangTongHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongHappyGodEntries Create(Func<EngineOwners.CharacterPangTongHappyGodRuntime> resolve) => new(resolve);
}
