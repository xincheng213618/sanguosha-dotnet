#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programUses { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses { get; }
}
