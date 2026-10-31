#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengHuoBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime> _resolve;
    private CharacterMengHuoBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengHuoBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoBoundaryMobileRuntime> resolve) => new(resolve);
}
