#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhangBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaoZhangBoundaryMobileRuntime> _resolve;
    private CharacterCaoZhangBoundaryMobileEntries(Func<EngineOwners.CharacterCaoZhangBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhangBoundaryMobileEntries Create(Func<EngineOwners.CharacterCaoZhangBoundaryMobileRuntime> resolve) => new(resolve);
}
