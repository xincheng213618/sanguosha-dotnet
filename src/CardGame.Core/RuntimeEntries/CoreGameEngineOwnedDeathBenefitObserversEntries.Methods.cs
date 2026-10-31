#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDeathBenefitObserversEntries
{
    internal bool IsOwnedDeathBenefitProgramDying() => _resolve().IsOwnedDeathBenefitProgramDying();
    internal bool OwnedDeathBenefitDeadDyingEdge(int index) => _resolve().OwnedDeathBenefitDeadDyingEdge(index);
    internal bool OwnedDeathBenefitObserverPrefix(ProgramSkillFrame root) => _resolve().OwnedDeathBenefitObserverPrefix(root);
    internal ProgramSkillFrame? OwnedDeathBenefitObserverRoot() => _resolve().OwnedDeathBenefitObserverRoot();
    internal bool TryAdvanceOwnedDeathBenefitSubtree() => _resolve().TryAdvanceOwnedDeathBenefitSubtree();
}
