#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineSkillProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess
{
    private global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> ReferenceToProgramContributionUses => ref _programContributionUses;

    private global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> ReferenceToProgramPhaseUses => ref _programPhaseUses;

    // The journal rebuilds these counters through accepted commands. They are
    // keyed by content identities, never by translated presentation text.
    private global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programUses = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> ReferenceToProgramUses => ref _programUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses => ref _programPhaseUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses => ref _programUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses => ref _programPhaseUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programUses => ref _programUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses => ref _programPhaseUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses => ref _programContributionUses;

    ref readonly global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses => ref _programContributionUses;
}
