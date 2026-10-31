#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _resolve;
    private CharacterZhugeGuoLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> resolve) => new(resolve);
}
