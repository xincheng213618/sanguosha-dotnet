#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed partial class CharacterSimaYiMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterSimaYiMobileGodRuntime> _resolve;
    private CharacterSimaYiMobileGodEntries(Func<EngineOwners.CharacterSimaYiMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiMobileGodEntries Create(Func<EngineOwners.CharacterSimaYiMobileGodRuntime> resolve) => new(resolve);
}
