#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programUses { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses { get; }
}
