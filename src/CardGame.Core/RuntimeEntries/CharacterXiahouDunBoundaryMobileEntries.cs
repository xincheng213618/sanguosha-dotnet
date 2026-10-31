#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouDunBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> _resolve;
    private CharacterXiahouDunBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouDunBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> resolve) => new(resolve);
}
