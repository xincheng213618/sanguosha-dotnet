#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageColorDuelProgramCapability(Func<RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries> getCoreGameEngineDamageColorDuelProgramsRuntime) : IDamageColorDuelProgramHost
{
    public SkillProgramStepOutcome DrawByDamageCardColor(ProgramSkillFrame frame) => getCoreGameEngineDamageColorDuelProgramsRuntime().BeginDamageAppearanceDraw(frame);
    public SkillProgramStepOutcome UseSelectedActorDuel(ProgramSkillFrame frame) => getCoreGameEngineDamageColorDuelProgramsRuntime().BeginSelectedActorDuel(frame);
}
