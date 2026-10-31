#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuZhiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> _resolve;
    private CharacterZhuZhiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuZhiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiBoundaryMobileRuntime> resolve) => new(resolve);
}
