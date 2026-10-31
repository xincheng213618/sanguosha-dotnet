#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJianYongBoundaryMobileRuntime))]
internal sealed partial class CharacterJianYongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterJianYongBoundaryMobileRuntime> _resolve;
    private CharacterJianYongBoundaryMobileEntries(Func<EngineOwners.CharacterJianYongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJianYongBoundaryMobileEntries Create(Func<EngineOwners.CharacterJianYongBoundaryMobileRuntime> resolve) => new(resolve);
}
