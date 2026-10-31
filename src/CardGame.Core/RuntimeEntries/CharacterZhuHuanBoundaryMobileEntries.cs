#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhuHuanBoundaryMobileRuntime))]
internal sealed partial class CharacterZhuHuanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> _resolve;
    private CharacterZhuHuanBoundaryMobileEntries(Func<EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuHuanBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> resolve) => new(resolve);
}
