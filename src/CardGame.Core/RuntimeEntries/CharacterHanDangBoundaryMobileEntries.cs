#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHanDangBoundaryMobileRuntime))]
internal sealed partial class CharacterHanDangBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterHanDangBoundaryMobileRuntime> _resolve;
    private CharacterHanDangBoundaryMobileEntries(Func<EngineOwners.CharacterHanDangBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanDangBoundaryMobileEntries Create(Func<EngineOwners.CharacterHanDangBoundaryMobileRuntime> resolve) => new(resolve);
}
