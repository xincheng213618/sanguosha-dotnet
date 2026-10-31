#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime))]
internal sealed partial class CoreGameEnginePrintedLordBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> _resolve;
    private CoreGameEnginePrintedLordBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrintedLordBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrintedLordBenefitsRuntime> resolve) => new(resolve);
}
