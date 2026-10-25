namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Cardless actual Uses still own a CardAttackHandle. Only a damage effect
    // stored on its actual skill owner's frame is direct skill damage; another
    // participant named as damage source cannot borrow that participant's tactic.
    private bool IsDirectSkillDamage(IDamageAttempt attack) =>
        attack is ProgramAttackHandle && !attack.IsSourceLess && !attack.IsChainPropagation &&
        _resolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == attack.ResolutionId &&
            frame.OwnerSeat == attack.SourceSeat && frame.AttackAttempt is not null);
}
