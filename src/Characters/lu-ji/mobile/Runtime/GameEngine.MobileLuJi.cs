namespace CardGame.Core;

public sealed partial class GameEngine
{
    // This qualifies new invocations. An already-paid Yili continues on its
    // original program frame even if its HP-loss child removes Huaiju.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 80500)]
    private bool MobileLuJiRequiresHuaiju(ProgramTriggerLegalityContext gate) =>
        gate.SkillId is not ("mobile:yili" or "mobile:zhenglun") ||
        GetSkillBindingShard(gate.Owner).HasSkill("classic:huaiju");
}
