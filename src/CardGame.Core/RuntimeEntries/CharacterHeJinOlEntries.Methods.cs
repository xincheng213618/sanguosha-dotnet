#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeJinOlEntries
{
    internal bool AdvanceHeJinSubtree() => _resolve().AdvanceHeJinSubtree();
    internal void AnswerHeJinMouzhu(PromptChoice choice) => _resolve().AnswerHeJinMouzhu(choice);
    internal void AnswerHeJinYanhuo(PromptChoice choice) => _resolve().AnswerHeJinYanhuo(choice);
    internal SkillProgramStepOutcome BeginHeJinMouzhu(ProgramSkillFrame supplied) => _resolve().BeginHeJinMouzhu(supplied);
    internal void BeginHeJinMouzhuCard(ProgramSkillFrame frame, HeJinMouzhuUseOrigin origin) => _resolve().BeginHeJinMouzhuCard(frame, origin);
    internal SkillProgramStepOutcome BeginHeJinYanhuo(ProgramSkillFrame supplied) => _resolve().BeginHeJinYanhuo(supplied);
    internal bool CanIssueHeJinMouzhuCard(int actor, int target, CardKind kind) => _resolve().CanIssueHeJinMouzhuCard(actor, target, kind);
    internal void CaptureHeJinYanhuoDeath(long deathId, CharacterState victim) => _resolve().CaptureHeJinYanhuoDeath(deathId, victim);
    internal PromptChoice ChooseHeJinAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseHeJinAi(p, f);
    internal bool ContinuePaidHeJin(ProgramSkillFrame f) => _resolve().ContinuePaidHeJin(f);
    internal bool DrainHeJinChildren(long id) => _resolve().DrainHeJinChildren(id);
    internal bool ExactHeJinDeathEntry(ProgramSkillFrame f, scoped out DeathFrame death, scoped out ProgramDeathTriggerWindowFrame window) => _resolve().ExactHeJinDeathEntry(f, out death, out window);
    internal void FinishHeJinMouzhu(ProgramSkillFrame f, bool issued) => _resolve().FinishHeJinMouzhu(f, issued);
    internal int HeJinDeathOwnedCardCount(DeathFrame death) => _resolve().HeJinDeathOwnedCardCount(death);
    internal bool HeJinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HeJinFirstChild(f, child);
    internal int[] HeJinHandAt(long sequence, int seat) => _resolve().HeJinHandAt(sequence, seat);
    internal IReadOnlyList<PromptChoice> HeJinMouzhuChoices(ProgramSkillFrame f) => _resolve().HeJinMouzhuChoices(f);
    internal ProgramSkillFrame? HeJinObserverRoot() => _resolve().HeJinObserverRoot();
    internal IReadOnlyList<PromptChoice> HeJinYanhuoChoices(ProgramSkillFrame f) => _resolve().HeJinYanhuoChoices(f);
    internal bool IsHeJinMouzhuDuelUse(long id) => _resolve().IsHeJinMouzhuDuelUse(id);
    internal bool IsHeJinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsHeJinRecoveryParent(f, recovery);
    internal bool IsOriginalDyingSuspendedByHeJinYanhuo(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByHeJinYanhuo(dying);
    internal bool MatchesHeJinMouzhu(ProgramSkillFrame f, HeJinMouzhuUseOrigin origin) => _resolve().MatchesHeJinMouzhu(f, origin);
    internal void PublishHeJinPrompt(ProgramSkillFrame f) => _resolve().PublishHeJinPrompt(f);
    internal bool ResumeHeJin(long id) => _resolve().ResumeHeJin(id);
    internal void ResumeHeJinYanhuo(ProgramSkillFrame f) => _resolve().ResumeHeJinYanhuo(f);
    internal bool ReturnHeJinMouzhuAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnHeJinMouzhuAttack(completion);
    internal void ReturnHeJinMouzhuUse(CardUseFrame use) => _resolve().ReturnHeJinMouzhuUse(use);
    internal bool ValidHeJinMouzhu(ProgramSkillFrame f) => _resolve().ValidHeJinMouzhu(f);
    internal bool ValidHeJinYanhuo(ProgramSkillFrame f) => _resolve().ValidHeJinYanhuo(f);
}
