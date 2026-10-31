#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetObtainProgramsEntries
{
    internal SkillProgramStepOutcome BeginDamageTargetObtain(ProgramSkillFrame input) => _resolve().BeginDamageTargetObtain(input);
    internal bool CanContinuePaidDamageTargetObtain(ProgramSkillFrame f) => _resolve().CanContinuePaidDamageTargetObtain(f);
    internal bool CanIssueDamageTargetDuel(ProgramSkillFrame f, int target) => _resolve().CanIssueDamageTargetDuel(f, target);
    internal bool CanOfferDamageTargetObtain(int owner, ProgramSkillWindowContext c) => _resolve().CanOfferDamageTargetObtain(owner, c);
    internal void CompleteDamageTargetDuelReturn(ProgramSkillFrame f) => _resolve().CompleteDamageTargetDuelReturn(f);
    internal bool CompleteIssuedDamageTargetDuelAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteIssuedDamageTargetDuelAttack(completion);
    internal IReadOnlyList<PromptChoice> DamageTargetDuelChoices(ProgramSkillFrame f) => _resolve().DamageTargetDuelChoices(f);
    internal IReadOnlyList<PromptChoice> DamageTargetObtainChoices(ProgramSkillFrame f) => _resolve().DamageTargetObtainChoices(f);
    internal void FinishDamageTargetObtain(ProgramSkillFrame f) => _resolve().FinishDamageTargetObtain(f);
    internal bool IsDamageTargetDuelUse(long id) => _resolve().IsDamageTargetDuelUse(id);
    internal bool IsIssuedZeroEntityDuel(long id) => _resolve().IsIssuedZeroEntityDuel(id);
    internal bool IsValidDamageTargetObtain(ProgramSkillFrame f) => _resolve().IsValidDamageTargetObtain(f);
    internal void IssueDamageTargetDuel(ProgramSkillFrame f, int target) => _resolve().IssueDamageTargetDuel(f, target);
    internal bool MatchesDamageTargetDuelParent(ProgramSkillFrame f, ProgramDamageTargetDuelOrigin o) => _resolve().MatchesDamageTargetDuelParent(f, o);
    internal void PublishDamageTargetObtainPrompt(ProgramSkillFrame f, int chooser, string text, IReadOnlyList<PromptChoice> choices, bool privateCards) => _resolve().PublishDamageTargetObtainPrompt(f, chooser, text, choices, privateCards);
    internal void ResolveDamageTargetObtainChoice(PromptChoice selected) => _resolve().ResolveDamageTargetObtainChoice(selected);
    internal bool ResumeDamageTargetObtain(long id) => _resolve().ResumeDamageTargetObtain(id);
    internal void ReturnDamageTargetDuel(CardUseFrame use) => _resolve().ReturnDamageTargetDuel(use);
    internal PromptChoice SelectAiDamageTargetObtain(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiDamageTargetObtain(decision, f);
}
