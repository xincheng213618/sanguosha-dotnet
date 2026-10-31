#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineKenshangPaymentsRuntime))]
internal sealed partial class CoreGameEngineKenshangPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineKenshangPaymentsRuntime> _resolve;
    private CoreGameEngineKenshangPaymentsEntries(Func<EngineOwners.CoreGameEngineKenshangPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKenshangPaymentsEntries Create(Func<EngineOwners.CoreGameEngineKenshangPaymentsRuntime> resolve) => new(resolve);
}
