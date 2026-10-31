#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameDomainProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileEquipmentSequencesEntries> getCoreGameEnginePublicPileEquipmentSequencesRuntime) : IGameDomainProgramHost
{
    public SkillProgramStepOutcome ExecuteGameDomain(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEnginePublicPileEquipmentSequencesRuntime().ExecuteGameDomain(e, f);
}
