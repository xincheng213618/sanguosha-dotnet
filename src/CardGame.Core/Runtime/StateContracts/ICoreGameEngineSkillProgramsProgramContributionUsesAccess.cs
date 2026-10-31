#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSkillProgramsProgramContributionUsesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses { get; }
}
