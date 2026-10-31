#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundRankBonusProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundRankBonusProgramsEntries> getCoreGameEngineBoundRankBonusProgramsRuntime) : IBoundRankBonusProgramHost
{
    public void RevealTopCardsWithNextBooleanBonus(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineBoundRankBonusProgramsRuntime().RevealWithNextBooleanBonus(f, e);
    public SkillProgramStepOutcome ObtainBoundCardsAndArmNextRevealBonus(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineBoundRankBonusProgramsRuntime().ObtainAndArmNextRevealBonus(f, e);
    public void RecoverOtherDyingVictimTo(ProgramSkillFrame f, int hp) => getCoreGameEngineBoundRankBonusProgramsRuntime().RecoverOtherDyingVictim(f, hp);
}
