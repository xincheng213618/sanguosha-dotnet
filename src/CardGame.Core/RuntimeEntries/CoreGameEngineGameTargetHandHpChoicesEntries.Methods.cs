#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameTargetHandHpChoicesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGameTargetHandHp(global::CardGame.Core.ProgramSkillFrame supplied, string state) => _resolve().BeginGameTargetHandHp(supplied, state);
    internal bool CanSelectGameHandHpTarget(int owner, string skill, string state, int target) => _resolve().CanSelectGameHandHpTarget(owner, skill, state, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GameHandHpChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GameHandHpChoices(f);
    internal global::CardGame.Core.Card[] GameHandHpDiscardCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GameHandHpDiscardCards(f);
    internal bool HasIssuedGameHandHpTarget(int owner, string skill, string state, int target) => _resolve().HasIssuedGameHandHpTarget(owner, skill, state, target);
    internal bool IsGameHandHpMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsGameHandHpMovement(f, effect, pending);
    internal void PublishGameHandHpDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishGameHandHpDiscard(f);
    internal void ResolveGameHandHpDiscard(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveGameHandHpDiscard(choice);
    internal bool ResumeGameHandHp(long id) => _resolve().ResumeGameHandHp(id);
    internal bool ReturnGameTargetHandHpMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnGameTargetHandHpMovement(f);
    internal bool ValidGameHandHpReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGameHandHpReceipt(f);
}
