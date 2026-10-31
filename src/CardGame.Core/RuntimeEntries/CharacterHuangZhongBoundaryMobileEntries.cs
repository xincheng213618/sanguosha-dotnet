#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangZhongBoundaryMobileRuntime))]
internal sealed partial class CharacterHuangZhongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> _resolve;
    private CharacterHuangZhongBoundaryMobileEntries(Func<EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangZhongBoundaryMobileEntries Create(Func<EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> resolve) => new(resolve);
}
