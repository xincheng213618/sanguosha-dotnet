#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameActivationAwakeningEntries
{
    internal SkillProgramStepOutcome BeginGameActivationAwakening(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginGameActivationAwakening(supplied, effect);
    internal bool CanContinueGameActivationAwakening(ProgramSkillFrame f) => _resolve().CanContinueGameActivationAwakening(f);
    internal bool CanOfferGameActivationAwakening(int ownerSeat, string skillId, SkillProgramTrigger trigger) => _resolve().CanOfferGameActivationAwakening(ownerSeat, skillId, trigger);
    internal bool ExactGameActivationAwakeningParent(ProgramSkillFrame f) => _resolve().ExactGameActivationAwakeningParent(f);
    internal bool GameActivationAwakeningFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().GameActivationAwakeningFirstChild(f, child);
    internal int GameProgramActivationCount(int ownerSeat, string skillId, string activationId) => _resolve().GameProgramActivationCount(ownerSeat, skillId, activationId);
    internal long[] GameProgramActivationFrames(int owner, string skill, string activation, long beforeFrame = 9223372036854775807L) => _resolve().GameProgramActivationFrames(owner, skill, activation, beforeFrame);
    internal bool IsGameActivationAwakeningProgramDying() => _resolve().IsGameActivationAwakeningProgramDying();
    internal bool ResumeGameActivationAwakening(long id) => _resolve().ResumeGameActivationAwakening(id);
    internal bool ValidGameActivationAwakeningReceipt(ProgramSkillFrame f) => _resolve().ValidGameActivationAwakeningReceipt(f);
}
