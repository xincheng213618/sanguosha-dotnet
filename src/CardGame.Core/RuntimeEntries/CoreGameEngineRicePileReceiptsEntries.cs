#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRicePileReceiptsRuntime))]
internal sealed partial class CoreGameEngineRicePileReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRicePileReceiptsRuntime> _resolve;
    private CoreGameEngineRicePileReceiptsEntries(Func<EngineOwners.CoreGameEngineRicePileReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileReceiptsEntries Create(Func<EngineOwners.CoreGameEngineRicePileReceiptsRuntime> resolve) => new(resolve);
}
