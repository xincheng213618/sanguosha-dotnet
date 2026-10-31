#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSkillProgramsProgramPhaseUsesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses { get; }
}
