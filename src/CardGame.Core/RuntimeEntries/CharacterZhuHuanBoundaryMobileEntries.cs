#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime))]
internal sealed partial class CharacterZhuHuanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> _resolve;
    private CharacterZhuHuanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuHuanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> resolve) => new(resolve);
}
