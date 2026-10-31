#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXianOlEntries
{
    internal bool CaoXianAdvanceChildren() => _resolve().CaoXianAdvanceChildren();
    internal bool CaoXianDying() => _resolve().CaoXianDying();
    internal bool CaoXianEdge(int i) => _resolve().CaoXianEdge(i);
    internal bool CaoXianFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CaoXianFirstChild(f, child);
    internal bool CaoXianMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => _resolve().CaoXianMovement(f, op, p);
    internal bool CaoXianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().CaoXianRecoveryParent(f, recovery);
    internal ProgramSkillFrame? CaoXianRoot() => _resolve().CaoXianRoot();
    internal bool CaoXianSuffix(int n) => _resolve().CaoXianSuffix(n);
    internal CaoXianRuntime CaoXianRules { get => _resolve().CaoXianRules; }
    internal bool HasCaoXian { get => _resolve().HasCaoXian; }
}
