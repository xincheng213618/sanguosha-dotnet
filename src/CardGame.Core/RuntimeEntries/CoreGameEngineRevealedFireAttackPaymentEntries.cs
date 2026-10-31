#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime))]
internal sealed partial class CoreGameEngineRevealedFireAttackPaymentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime> _resolve;
    private CoreGameEngineRevealedFireAttackPaymentEntries(Func<EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRevealedFireAttackPaymentEntries Create(Func<EngineOwners.CoreGameEngineRevealedFireAttackPaymentRuntime> resolve) => new(resolve);
}
