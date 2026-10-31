#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceLiuSandaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime> _resolve;
    private CoreGameEngineShanheSourceLiuSandaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceLiuSandaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime> resolve) => new(resolve);
}
