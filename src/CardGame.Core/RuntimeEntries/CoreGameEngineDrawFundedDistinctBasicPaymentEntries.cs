#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicPaymentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicPaymentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicPaymentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> resolve) => new(resolve);
}
