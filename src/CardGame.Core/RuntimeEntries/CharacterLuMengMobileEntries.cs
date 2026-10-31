#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime))]
internal sealed partial class CharacterLuMengMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> _resolve;
    private CharacterLuMengMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> resolve) => new(resolve);
}
