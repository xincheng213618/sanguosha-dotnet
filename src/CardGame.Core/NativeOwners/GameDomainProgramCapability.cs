#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameDomainProgramCapability(Func<RuntimeEntries.CoreGameEnginePublicPileEquipmentSequencesEntries> getCoreGameEnginePublicPileEquipmentSequencesRuntime) : IGameDomainProgramHost
{
    public SkillProgramStepOutcome ExecuteGameDomain(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePublicPileEquipmentSequencesRuntime().ExecuteGameDomain(e, f);
}
