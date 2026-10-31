#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiRuBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiRuBoundaryMobileRuntime> _resolve;
    private CharacterLiRuBoundaryMobileEntries(Func<EngineOwners.CharacterLiRuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiRuBoundaryMobileEntries Create(Func<EngineOwners.CharacterLiRuBoundaryMobileRuntime> resolve) => new(resolve);
}
