#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineKuangfuReceiptsRuntime))]
internal sealed partial class CoreGameEngineKuangfuReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> _resolve;
    private CoreGameEngineKuangfuReceiptsEntries(Func<EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKuangfuReceiptsEntries Create(Func<EngineOwners.CoreGameEngineKuangfuReceiptsRuntime> resolve) => new(resolve);
}
