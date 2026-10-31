#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDeathBenefitObserversEntries
{
    internal void AssertOwnedDeathBenefitReturns() => _resolve().AssertOwnedDeathBenefitReturns();
    internal bool IsOwnedDeathBenefitProgramDying() => _resolve().IsOwnedDeathBenefitProgramDying();
    internal bool OwnedDeathBenefitDeadDyingEdge(int index) => _resolve().OwnedDeathBenefitDeadDyingEdge(index);
    internal bool OwnedDeathBenefitObserverPrefix(global::CardGame.Core.ProgramSkillFrame root) => _resolve().OwnedDeathBenefitObserverPrefix(root);
    internal global::CardGame.Core.ProgramSkillFrame? OwnedDeathBenefitObserverRoot() => _resolve().OwnedDeathBenefitObserverRoot();
    internal bool TryAdvanceOwnedDeathBenefitSubtree() => _resolve().TryAdvanceOwnedDeathBenefitSubtree();
}
