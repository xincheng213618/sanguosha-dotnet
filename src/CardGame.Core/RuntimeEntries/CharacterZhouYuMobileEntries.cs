#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuMobileRuntime))]
internal sealed partial class CharacterZhouYuMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMobileRuntime> _resolve;
    private CharacterZhouYuMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuMobileRuntime> resolve) => new(resolve);
}
