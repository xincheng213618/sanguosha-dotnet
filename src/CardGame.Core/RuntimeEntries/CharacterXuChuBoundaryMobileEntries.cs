#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXuChuBoundaryMobileRuntime))]
internal sealed partial class CharacterXuChuBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterXuChuBoundaryMobileRuntime> _resolve;
    private CharacterXuChuBoundaryMobileEntries(Func<EngineOwners.CharacterXuChuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuChuBoundaryMobileEntries Create(Func<EngineOwners.CharacterXuChuBoundaryMobileRuntime> resolve) => new(resolve);
}
