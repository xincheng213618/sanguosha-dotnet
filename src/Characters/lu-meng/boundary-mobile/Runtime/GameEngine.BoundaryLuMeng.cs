namespace CardGame.Core;

public sealed partial class GameEngine
{
    // "Game player count" is the fixed roster, including dead seats. The shared
    // preparation comparison supplies the owner's actual hand count and HP.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 60700)]
    private bool QualifyBoundaryLuMengMobileQinxue(ProgramTriggerLegalityContext c) => c.SkillId != "boundary:qinxue-mobile" ||
        c.Context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.Owner.HasHpValue &&
        GetHand(c.Owner).Count - c.Owner.Hp >= (_players.Count >= 7 ? 2 : 3);
}
