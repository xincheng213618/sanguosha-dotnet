#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageCardColorHealthEntries
{
    internal bool AdvanceDamageCardColorHealthChildren() => _resolve().AdvanceDamageCardColorHealthChildren();
    internal bool CanContinueDamageCardColorHealth(ProgramSkillFrame f) => _resolve().CanContinueDamageCardColorHealth(f);
    internal bool CanOfferDamageCardColorHealth(ProgramTriggerLegalityContext c) => _resolve().CanOfferDamageCardColorHealth(c);
    internal bool DamageCardColorHealthFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().DamageCardColorHealthFirstChild(root, child);
    internal bool DamageCardColorHealthObserverEdge(int index) => _resolve().DamageCardColorHealthObserverEdge(index);
    internal ProgramSkillFrame? DamageCardColorHealthObserverRoot() => _resolve().DamageCardColorHealthObserverRoot();
    internal bool ExactDamageColorHealthParent(ProgramSkillFrame f, scoped out BeforeDamageProgramWindowFrame window, scoped out GameEngine.IDamageAttempt attack) => _resolve().ExactDamageColorHealthParent(f, out window, out attack);
    internal bool HasRecipientDamageCardColorHealth(int seat) => _resolve().HasRecipientDamageCardColorHealth(seat);
    internal bool IsDamageCardColorHealthRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsDamageCardColorHealthRecoveryParent(frame, recovery);
    internal SkillProgramStepOutcome ResolveDamageCardColorHealth(ProgramSkillFrame supplied) => _resolve().ResolveDamageCardColorHealth(supplied);
    internal bool ResumeDamageCardColorHealth(long id) => _resolve().ResumeDamageCardColorHealth(id);
    internal bool TryGetDamageHealthCard(GameEngine.IDamageAttempt attack, scoped out CardKind kind, scoped out Suit suit, scoped out bool red, scoped out CardUseFrame? use) => _resolve().TryGetDamageHealthCard(attack, out kind, out suit, out red, out use);
    internal bool ValidDamageCardColorHealth(ProgramSkillFrame f) => _resolve().ValidDamageCardColorHealth(f);
}
