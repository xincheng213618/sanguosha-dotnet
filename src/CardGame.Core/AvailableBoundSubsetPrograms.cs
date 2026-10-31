namespace CardGame.Core;

public interface IAvailableBoundSubsetProgramEffectHost
{
    SkillProgramStepOutcome SelectAvailableBoundSubset(ProgramSkillFrame frame, SkillProgramEffect effect);
}
