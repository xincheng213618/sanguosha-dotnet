#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunLuBanBoundaryMobileRuntime))]
internal sealed partial class CharacterSunLuBanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> _resolve;
    private CharacterSunLuBanBoundaryMobileEntries(Func<EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunLuBanBoundaryMobileEntries Create(Func<EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> resolve) => new(resolve);
}
