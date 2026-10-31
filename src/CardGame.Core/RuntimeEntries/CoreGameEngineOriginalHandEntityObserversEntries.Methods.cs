#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandEntityObserversEntries
{
    internal bool ExactOriginalHandBenefitParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactOriginalHandBenefitParent(f);
    internal bool ExactOriginalHandInheritanceParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactOriginalHandInheritanceParent(f);
    internal bool ExactOriginalHandLifecycleParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().ExactOriginalHandLifecycleParent(f, window);
    internal bool IsOriginalHandEntityProgramDying() => _resolve().IsOriginalHandEntityProgramDying();
    internal bool OriginalHandBenefitFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OriginalHandBenefitFirstChild(f, child);
    internal bool OriginalHandBindingStarted(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().OriginalHandBindingStarted(f, window);
    internal bool OriginalHandEntityFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OriginalHandEntityFirstChild(f, child);
    internal bool OriginalHandEntityObserverEdge(int child) => _resolve().OriginalHandEntityObserverEdge(child);
    internal global::CardGame.Core.ProgramSkillFrame? OriginalHandEntityObserverRoot() => _resolve().OriginalHandEntityObserverRoot();
    internal bool OriginalHandEntityStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OriginalHandEntityStructuralEdge(parent, child);
    internal bool TryAdvanceOriginalHandEntitySubtree() => _resolve().TryAdvanceOriginalHandEntitySubtree();
    internal bool ValidOriginalHandBenefitReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidOriginalHandBenefitReceipt(f);
    internal bool ValidOriginalHandInheritanceReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidOriginalHandInheritanceReceipt(f);
}
