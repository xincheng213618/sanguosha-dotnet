#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceNamanEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceNamanRuntime> _resolve;
    private CoreGameEngineShanheSourceNamanEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceNamanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceNamanEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceNamanRuntime> resolve) => new(resolve);
}
