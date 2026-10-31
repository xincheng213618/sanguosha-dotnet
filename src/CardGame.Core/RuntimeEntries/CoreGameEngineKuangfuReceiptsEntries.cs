#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKuangfuReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> _resolve;
    private CoreGameEngineKuangfuReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKuangfuReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> resolve) => new(resolve);
}
