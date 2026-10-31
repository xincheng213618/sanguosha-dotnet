#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programUses { get; }

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses { get; }

    ref readonly Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses { get; }
}
