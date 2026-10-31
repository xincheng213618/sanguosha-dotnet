#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQuanCongBoundaryMobileRuntime))]
internal sealed partial class CharacterQuanCongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterQuanCongBoundaryMobileRuntime> _resolve;
    private CharacterQuanCongBoundaryMobileEntries(Func<EngineOwners.CharacterQuanCongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQuanCongBoundaryMobileEntries Create(Func<EngineOwners.CharacterQuanCongBoundaryMobileRuntime> resolve) => new(resolve);
}
