#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime))]
internal sealed partial class CharacterLuBuMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> _resolve;
    private CharacterLuBuMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> resolve) => new(resolve);
}
