#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageCardColorHealthEntries
{
    internal bool AdvanceDamageCardColorHealthChildren() => _resolve().AdvanceDamageCardColorHealthChildren();
    internal void AssertDamageCardColorHealth(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDamageCardColorHealth(f);
    internal bool CanContinueDamageCardColorHealth(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueDamageCardColorHealth(f);
    internal bool CanOfferDamageCardColorHealth(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferDamageCardColorHealth(c);
    internal bool DamageCardColorHealthFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().DamageCardColorHealthFirstChild(root, child);
    internal bool DamageCardColorHealthObserverEdge(int index) => _resolve().DamageCardColorHealthObserverEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? DamageCardColorHealthObserverRoot() => _resolve().DamageCardColorHealthObserverRoot();
    internal bool ExactDamageColorHealthParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.BeforeDamageProgramWindowFrame window, scoped out global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ExactDamageColorHealthParent(f, out window, out attack);
    internal bool HasDamageCardColorHealthBeforeDamage(long id) => _resolve().HasDamageCardColorHealthBeforeDamage(id);
    internal bool HasDamageCardColorHealthDamageObserver(long id) => _resolve().HasDamageCardColorHealthDamageObserver(id);
    internal bool HasRecipientDamageCardColorHealth(int seat) => _resolve().HasRecipientDamageCardColorHealth(seat);
    internal bool IsDamageCardColorHealthDying() => _resolve().IsDamageCardColorHealthDying();
    internal bool IsDamageCardColorHealthRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsDamageCardColorHealthRecoveryParent(frame, recovery);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveDamageCardColorHealth(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveDamageCardColorHealth(supplied);
    internal bool ResumeDamageCardColorHealth(long id) => _resolve().ResumeDamageCardColorHealth(id);
    internal bool TryGetDamageHealthCard(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out global::CardGame.Core.CardKind kind, scoped out global::CardGame.Core.Suit suit, scoped out bool red, scoped out global::CardGame.Core.CardUseFrame? use) => _resolve().TryGetDamageHealthCard(attack, out kind, out suit, out red, out use);
    internal bool ValidDamageCardColorHealth(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDamageCardColorHealth(f);
}
