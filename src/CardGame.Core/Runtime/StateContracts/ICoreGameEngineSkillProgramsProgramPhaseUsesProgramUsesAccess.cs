#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programUses { get; }

    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses { get; }
}
