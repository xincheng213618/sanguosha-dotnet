#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouTaiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhouTaiBoundaryMobileRuntime> _resolve;
    private CharacterZhouTaiBoundaryMobileEntries(Func<EngineOwners.CharacterZhouTaiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouTaiBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhouTaiBoundaryMobileRuntime> resolve) => new(resolve);
}
