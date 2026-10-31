#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLingTongBoundaryMobileRuntime))]
internal sealed partial class CharacterLingTongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterLingTongBoundaryMobileRuntime> _resolve;
    private CharacterLingTongBoundaryMobileEntries(Func<EngineOwners.CharacterLingTongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLingTongBoundaryMobileEntries Create(Func<EngineOwners.CharacterLingTongBoundaryMobileRuntime> resolve) => new(resolve);
}
