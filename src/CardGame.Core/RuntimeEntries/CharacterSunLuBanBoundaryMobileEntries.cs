#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime))]
internal sealed partial class CharacterSunLuBanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> _resolve;
    private CharacterSunLuBanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunLuBanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunLuBanBoundaryMobileRuntime> resolve) => new(resolve);
}
