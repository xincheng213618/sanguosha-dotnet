#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetObtainProgramsEntries
{
    internal void AssertDamageTargetDuels() => _resolve().AssertDamageTargetDuels();
    internal void AssertDamageTargetObtainState(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDamageTargetObtainState(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageTargetObtain(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginDamageTargetObtain(input);
    internal bool CanContinuePaidDamageTargetObtain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePaidDamageTargetObtain(f);
    internal bool CanIssueDamageTargetDuel(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().CanIssueDamageTargetDuel(f, target);
    internal bool CanOfferDamageTargetObtain(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferDamageTargetObtain(owner, c);
    internal void CompleteDamageTargetDuelReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteDamageTargetDuelReturn(f);
    internal bool CompleteIssuedDamageTargetDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteIssuedDamageTargetDuelAttack(completion);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageTargetDuelChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DamageTargetDuelChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> DamageTargetDuelOuterProcessing(long useId) => _resolve().DamageTargetDuelOuterProcessing(useId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageTargetObtainChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DamageTargetObtainChoices(f);
    internal void FinishDamageTargetObtain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishDamageTargetObtain(f);
    internal bool IsDamageTargetDuelUse(long id) => _resolve().IsDamageTargetDuelUse(id);
    internal bool IsIssuedZeroEntityDuel(long id) => _resolve().IsIssuedZeroEntityDuel(id);
    internal bool IsValidDamageTargetObtain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsValidDamageTargetObtain(f);
    internal void IssueDamageTargetDuel(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().IssueDamageTargetDuel(f, target);
    internal bool MatchesDamageTargetDuelParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramDamageTargetDuelOrigin o) => _resolve().MatchesDamageTargetDuelParent(f, o);
    internal void PublishDamageTargetObtainPrompt(global::CardGame.Core.ProgramSkillFrame f, int chooser, string text, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, bool privateCards) => _resolve().PublishDamageTargetObtainPrompt(f, chooser, text, choices, privateCards);
    internal void ResolveDamageTargetObtainChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDamageTargetObtainChoice(selected);
    internal bool ResumeDamageTargetObtain(long id) => _resolve().ResumeDamageTargetObtain(id);
    internal void ReturnDamageTargetDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnDamageTargetDuel(use);
    internal global::CardGame.Core.PromptChoice SelectAiDamageTargetObtain(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDamageTargetObtain(decision, f);
}
