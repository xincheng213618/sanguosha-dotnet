#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanBoundaryMobileRuntime> _resolve;
    private CharacterSunQuanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanBoundaryMobileRuntime> resolve) => new(resolve);
}
