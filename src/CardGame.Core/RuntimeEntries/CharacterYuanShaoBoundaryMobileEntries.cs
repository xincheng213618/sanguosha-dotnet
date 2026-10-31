#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanShaoBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> _resolve;
    private CharacterYuanShaoBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanShaoBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoBoundaryMobileRuntime> resolve) => new(resolve);
}
