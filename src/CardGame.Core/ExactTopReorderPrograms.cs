namespace CardGame.Core;

public interface IExactTopReorderProgramEffectHost
{
    SkillProgramStepOutcome ReorderTopCardsExactly(ProgramSkillFrame frame, int maximumCards,
        SkillProgramNumberExpression? numberExpression, int exactTopCount);
}

public sealed partial class GameEngine
{
}
