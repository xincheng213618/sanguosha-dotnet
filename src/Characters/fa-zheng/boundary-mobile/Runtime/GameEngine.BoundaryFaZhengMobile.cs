namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int[] FaZhengMobileAssistedTargets(ProgramSkillFrame frame, int actor)
    {
        if (frame.SkillId != "boundary:xuanhuo-mobile") return AssistedPhysicalSlashTargets(actor);
        if (frame.TriggerId != "replace-draw-with-request" || frame.ActivationId != frame.TriggerId || frame.InstructionIndex != 3 ||
            frame.WindowContext?.Window != SkillProgramTriggerWindow.DrawPhaseStarting || frame.SelectedTargetSeats is not [var selected] || selected != actor || actor == frame.OwnerSeat ||
            GetPausedPrivateOfferEffect(frame) is not { Op: SkillProgramEffectOp.RequestSlashAgainstChosenTarget, Target: SkillProgramEffectTarget.SelectedTarget, ResultBind: "command-answer", ChooserRef: null } ||
            _contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("Mobile Xuanhuo must retain its original draw-replacement frame and exact commanded actor.");
        // A privilege to use a particular Slash without distance does not enlarge
        // the attack range named by this skill's target-selection instruction.
        return _players.Where(target => _players[actor].IsAlive && target.IsAlive && target.Seat != actor && IsWithinAttackRange(actor, target.Seat))
            .Select(target => target.Seat).ToArray();
    }
}
