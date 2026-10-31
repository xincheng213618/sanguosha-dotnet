#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> _resolve;
    private CharacterXiaoQiaoBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> resolve) => new(resolve);
}
