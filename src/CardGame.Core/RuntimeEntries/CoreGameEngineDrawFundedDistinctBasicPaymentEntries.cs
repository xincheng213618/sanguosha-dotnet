#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicPaymentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicPaymentEntries(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicPaymentEntries Create(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> resolve) => new(resolve);
}
