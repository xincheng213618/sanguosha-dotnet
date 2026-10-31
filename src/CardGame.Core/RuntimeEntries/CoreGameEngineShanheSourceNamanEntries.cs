#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceNamanEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceNamanRuntime> _resolve;
    private CoreGameEngineShanheSourceNamanEntries(Func<EngineOwners.CoreGameEngineShanheSourceNamanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceNamanEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceNamanRuntime> resolve) => new(resolve);
}
