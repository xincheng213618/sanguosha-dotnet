#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime))]
internal sealed partial class CoreGameEngineFactionRequestPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> _resolve;
    private CoreGameEngineFactionRequestPaymentsEntries(Func<EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFactionRequestPaymentsEntries Create(Func<EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> resolve) => new(resolve);
}
