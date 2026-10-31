#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFaZhengBoundaryMobileRuntime
{
    internal int[] FaZhengMobileAssistedTargets(ProgramSkillFrame frame, int actor)
    {
        if (_characterJiangWeiBoundaryMobileOperations.MobileJiangWeiAssistedTargets(frame, actor)is { } mobileTargets)
            return mobileTargets;
        if (frame.SkillId != "boundary:xuanhuo-mobile")
            return _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashTargets(actor);
        if (frame.TriggerId != "replace-draw-with-request" || frame.ActivationId != frame.TriggerId || frame.InstructionIndex != 3 || frame.WindowContext?.Window != SkillProgramTriggerWindow.DrawPhaseStarting || frame.SelectedTargetSeats is not [var selected] || selected != actor || actor == frame.OwnerSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)is not { Op: SkillProgramEffectOp.RequestSlashAgainstChosenTarget, Target: SkillProgramEffectTarget.SelectedTarget, ResultBind: "command-answer", ChooserRef: null } || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("Mobile Xuanhuo must retain its original draw-replacement frame and exact commanded actor.");
        // A privilege to use a particular Slash without distance does not enlarge
        // the attack range named by this skill's target-selection instruction.
        return _coreMatchParticipantsPlayersAccess._players.Where(target => _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && target.IsAlive && target.Seat != actor && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(actor, target.Seat)).Select(target => target.Seat).ToArray();
    }
}
