#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime))]
internal sealed partial class CharacterHuangZhongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> _resolve;
    private CharacterHuangZhongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangZhongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> resolve) => new(resolve);
}
