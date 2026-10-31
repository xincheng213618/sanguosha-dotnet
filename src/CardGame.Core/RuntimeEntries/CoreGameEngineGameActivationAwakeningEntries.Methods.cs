#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameActivationAwakeningEntries
{
    internal void AssertGameActivationAwakeningReceipt(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertGameActivationAwakeningReceipt(f, plan);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGameActivationAwakening(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginGameActivationAwakening(supplied, effect);
    internal bool CanContinueGameActivationAwakening(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueGameActivationAwakening(f);
    internal bool CanOfferGameActivationAwakening(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanOfferGameActivationAwakening(ownerSeat, skillId, trigger);
    internal bool ExactGameActivationAwakeningParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactGameActivationAwakeningParent(f);
    internal bool GameActivationAwakeningFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().GameActivationAwakeningFirstChild(f, child);
    internal int GameProgramActivationCount(int ownerSeat, string skillId, string activationId) => _resolve().GameProgramActivationCount(ownerSeat, skillId, activationId);
    internal long[] GameProgramActivationFrames(int owner, string skill, string activation, long beforeFrame = 9223372036854775807L) => _resolve().GameProgramActivationFrames(owner, skill, activation, beforeFrame);
    internal bool IsGameActivationAwakeningProgramDying() => _resolve().IsGameActivationAwakeningProgramDying();
    internal bool ResumeGameActivationAwakening(long id) => _resolve().ResumeGameActivationAwakening(id);
    internal bool ValidGameActivationAwakeningReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGameActivationAwakeningReceipt(f);
}
