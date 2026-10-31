#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXianOlEntries
{
    internal bool CaoXianAdvanceChildren() => _resolve().CaoXianAdvanceChildren();
    internal bool CaoXianDying() => _resolve().CaoXianDying();
    internal bool CaoXianEdge(int i) => _resolve().CaoXianEdge(i);
    internal bool CaoXianFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoXianFirstChild(f, child);
    internal bool CaoXianMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().CaoXianMovement(f, op, p);
    internal bool CaoXianRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().CaoXianRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? CaoXianRoot() => _resolve().CaoXianRoot();
    internal bool CaoXianSuffix(int n) => _resolve().CaoXianSuffix(n);
    internal global::CardGame.Core.CaoXianRuntime CaoXianRules { get => _resolve().CaoXianRules; }
    internal bool HasCaoXian { get => _resolve().HasCaoXian; }
}
