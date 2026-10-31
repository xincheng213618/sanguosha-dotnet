#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime))]
internal sealed partial class CoreGameEngineRevealedFireAttackPaymentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime> _resolve;
    private CoreGameEngineRevealedFireAttackPaymentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRevealedFireAttackPaymentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime> resolve) => new(resolve);
}
