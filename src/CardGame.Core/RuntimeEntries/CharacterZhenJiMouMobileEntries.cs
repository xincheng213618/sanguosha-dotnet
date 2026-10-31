#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime))]
internal sealed partial class CharacterZhenJiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime> _resolve;
    private CharacterZhenJiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiMouMobileRuntime> resolve) => new(resolve);
}
