#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuHuangBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterXuHuangBoundaryMobileRuntime> _resolve;
    private CharacterXuHuangBoundaryMobileEntries(Func<EngineOwners.CharacterXuHuangBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuHuangBoundaryMobileEntries Create(Func<EngineOwners.CharacterXuHuangBoundaryMobileRuntime> resolve) => new(resolve);
}
