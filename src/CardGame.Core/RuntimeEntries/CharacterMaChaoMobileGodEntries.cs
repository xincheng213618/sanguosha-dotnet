#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaChaoMobileGodRuntime))]
internal sealed partial class CharacterMaChaoMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterMaChaoMobileGodRuntime> _resolve;
    private CharacterMaChaoMobileGodEntries(Func<EngineOwners.CharacterMaChaoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChaoMobileGodEntries Create(Func<EngineOwners.CharacterMaChaoMobileGodRuntime> resolve) => new(resolve);
}
