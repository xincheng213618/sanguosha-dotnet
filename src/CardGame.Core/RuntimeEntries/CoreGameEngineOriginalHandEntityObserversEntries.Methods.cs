#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandEntityObserversEntries
{
    internal bool ExactOriginalHandBenefitParent(ProgramSkillFrame f) => _resolve().ExactOriginalHandBenefitParent(f);
    internal bool ExactOriginalHandInheritanceParent(ProgramSkillFrame f) => _resolve().ExactOriginalHandInheritanceParent(f);
    internal bool ExactOriginalHandLifecycleParent(ProgramSkillFrame f, SkillProgramTriggerWindow window) => _resolve().ExactOriginalHandLifecycleParent(f, window);
    internal bool IsOriginalHandEntityProgramDying() => _resolve().IsOriginalHandEntityProgramDying();
    internal bool OriginalHandBenefitFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OriginalHandBenefitFirstChild(f, child);
    internal bool OriginalHandBindingStarted(ProgramSkillFrame f, SkillProgramTriggerWindow window) => _resolve().OriginalHandBindingStarted(f, window);
    internal bool OriginalHandEntityFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OriginalHandEntityFirstChild(f, child);
    internal bool OriginalHandEntityObserverEdge(int child) => _resolve().OriginalHandEntityObserverEdge(child);
    internal ProgramSkillFrame? OriginalHandEntityObserverRoot() => _resolve().OriginalHandEntityObserverRoot();
    internal bool OriginalHandEntityStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OriginalHandEntityStructuralEdge(parent, child);
    internal bool TryAdvanceOriginalHandEntitySubtree() => _resolve().TryAdvanceOriginalHandEntitySubtree();
    internal bool ValidOriginalHandBenefitReceipt(ProgramSkillFrame f) => _resolve().ValidOriginalHandBenefitReceipt(f);
    internal bool ValidOriginalHandInheritanceReceipt(ProgramSkillFrame f) => _resolve().ValidOriginalHandInheritanceReceipt(f);
}
