#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime))]
internal sealed partial class CoreGameEnginePrintedLordBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> _resolve;
    private CoreGameEnginePrintedLordBenefitsEntries(Func<EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrintedLordBenefitsEntries Create(Func<EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> resolve) => new(resolve);
}
