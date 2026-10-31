#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceLiuSandaoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime> _resolve;
    private CoreGameEngineShanheSourceLiuSandaoEntries(Func<EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceLiuSandaoEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime> resolve) => new(resolve);
}
