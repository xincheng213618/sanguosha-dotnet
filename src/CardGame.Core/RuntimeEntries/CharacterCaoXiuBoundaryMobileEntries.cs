#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXiuBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaoXiuBoundaryMobileRuntime> _resolve;
    private CharacterCaoXiuBoundaryMobileEntries(Func<EngineOwners.CharacterCaoXiuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoXiuBoundaryMobileEntries Create(Func<EngineOwners.CharacterCaoXiuBoundaryMobileRuntime> resolve) => new(resolve);
}
