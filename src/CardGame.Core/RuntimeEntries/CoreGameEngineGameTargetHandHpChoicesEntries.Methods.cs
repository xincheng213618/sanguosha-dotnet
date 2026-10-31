#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameTargetHandHpChoicesEntries
{
    internal SkillProgramStepOutcome BeginGameTargetHandHp(ProgramSkillFrame supplied, string state) => _resolve().BeginGameTargetHandHp(supplied, state);
    internal bool CanSelectGameHandHpTarget(int owner, string skill, string state, int target) => _resolve().CanSelectGameHandHpTarget(owner, skill, state, target);
    internal IReadOnlyList<PromptChoice> GameHandHpChoices(ProgramSkillFrame f) => _resolve().GameHandHpChoices(f);
    internal Card[] GameHandHpDiscardCards(ProgramSkillFrame f) => _resolve().GameHandHpDiscardCards(f);
    internal bool HasIssuedGameHandHpTarget(int owner, string skill, string state, int target) => _resolve().HasIssuedGameHandHpTarget(owner, skill, state, target);
    internal bool IsGameHandHpMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsGameHandHpMovement(f, effect, pending);
    internal void PublishGameHandHpDiscard(ProgramSkillFrame f) => _resolve().PublishGameHandHpDiscard(f);
    internal void ResolveGameHandHpDiscard(PromptChoice choice) => _resolve().ResolveGameHandHpDiscard(choice);
    internal bool ResumeGameHandHp(long id) => _resolve().ResumeGameHandHp(id);
    internal bool ReturnGameTargetHandHpMovement(ProgramSkillFrame f) => _resolve().ReturnGameTargetHandHpMovement(f);
    internal bool ValidGameHandHpReceipt(ProgramSkillFrame f) => _resolve().ValidGameHandHpReceipt(f);
}
