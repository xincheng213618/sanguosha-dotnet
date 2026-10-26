using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheTeamGrowthCapability(GameEngine engine) : IShanheTeamGrowthHost
    {
        public void GrantOpeningMaximumHp(ProgramSkillFrame frame) => engine.GrantShanheTeamOpeningMaximumHp(frame);
    }

    private void GrantShanheTeamOpeningMaximumHp(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheTeamTactics.HealthSkillId || frame.InstructionIndex != 2 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } ||
            GetProgramTrigger(frame).Effects is not
            [{ Op: SkillProgramEffectOp.SelectAllPublicTeamTargets, TargetKind: SkillProgramTargetKind.OtherPublicTeamAllies },
             { Op: SkillProgramEffectOp.GrantShanheTeamOpeningMaximumHp }, { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.SelectedTargets, Amount: 3 }] ||
            frame.SelectedTargetSeats.Distinct().Count() != frame.SelectedTargetSeats.Count)
            throw new InvalidOperationException("Team opening health requires its actual frozen public-allied starting selection.");
        foreach (var seat in frame.SelectedTargetSeats)
        {
            if (!IsValidPlayerSeat(seat) || !OtherRevealedPublicTeamAlly(_players[frame.OwnerSeat], _players[seat])) continue;
            var target = _players[seat]; var previous = target.MaxHp;
            target.MaxHp = checked(previous + 3);
            AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(seat, 3, target.MaxHp, frame.SkillId));
        }
    }
}
