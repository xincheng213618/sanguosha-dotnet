#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuBoundaryMobileRuntime> _resolve;
    private CharacterXuChuBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuChuBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuBoundaryMobileRuntime> resolve) => new(resolve);
}
