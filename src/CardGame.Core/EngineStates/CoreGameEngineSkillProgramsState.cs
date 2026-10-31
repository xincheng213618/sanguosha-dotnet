#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineSkillProgramsState : Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess, Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess, Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess, Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess
{
    private Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses = new();
    internal ref readonly Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> ReferenceToProgramContributionUses => ref _programContributionUses;

    private Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses = new();
    internal ref readonly Dictionary<(int Seat, string Skill, string Activation), int> ReferenceToProgramPhaseUses => ref _programPhaseUses;

    // The journal rebuilds these counters through accepted commands. They are
    // keyed by content identities, never by translated presentation text.
    private Dictionary<(int Seat, string Skill, string Activation), int> _programUses = new();
    internal ref readonly Dictionary<(int Seat, string Skill, string Activation), int> ReferenceToProgramUses => ref _programUses;

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses => ref _programPhaseUses;

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses => ref _programUses;

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses => ref _programPhaseUses;

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programUses => ref _programUses;

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses => ref _programPhaseUses;

    ref readonly Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses => ref _programContributionUses;

    ref readonly Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses => ref _programContributionUses;
}
