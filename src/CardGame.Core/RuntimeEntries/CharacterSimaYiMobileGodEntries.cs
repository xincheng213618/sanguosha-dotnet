#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed partial class CharacterSimaYiMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _resolve;
    private CharacterSimaYiMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> resolve) => new(resolve);
}
