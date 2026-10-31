#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed partial class CharacterXunYuMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterXunYuMobileGodRuntime> _resolve;
    private CharacterXunYuMobileGodEntries(Func<EngineOwners.CharacterXunYuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXunYuMobileGodEntries Create(Func<EngineOwners.CharacterXunYuMobileGodRuntime> resolve) => new(resolve);
}
