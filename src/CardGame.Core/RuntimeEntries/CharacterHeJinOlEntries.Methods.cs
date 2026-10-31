#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeJinOlEntries
{
    internal bool AdvanceHeJinSubtree() => _resolve().AdvanceHeJinSubtree();
    internal void AnswerHeJinMouzhu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHeJinMouzhu(choice);
    internal void AnswerHeJinYanhuo(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHeJinYanhuo(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHeJinMouzhu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHeJinMouzhu(supplied);
    internal void BeginHeJinMouzhuCard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.HeJinMouzhuUseOrigin origin) => _resolve().BeginHeJinMouzhuCard(frame, origin);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHeJinYanhuo(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHeJinYanhuo(supplied);
    internal bool CanIssueHeJinMouzhuCard(int actor, int target, global::CardGame.Core.CardKind kind) => _resolve().CanIssueHeJinMouzhuCard(actor, target, kind);
    internal void CaptureHeJinYanhuoDeath(long deathId, global::CardGame.Core.CharacterState victim) => _resolve().CaptureHeJinYanhuoDeath(deathId, victim);
    internal global::CardGame.Core.PromptChoice ChooseHeJinAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseHeJinAi(p, f);
    internal bool ContinuePaidHeJin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHeJin(f);
    internal bool DrainHeJinChildren(long id) => _resolve().DrainHeJinChildren(id);
    internal bool ExactHeJinDeathEntry(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.DeathFrame death, scoped out global::CardGame.Core.ProgramDeathTriggerWindowFrame window) => _resolve().ExactHeJinDeathEntry(f, out death, out window);
    internal void FinishHeJinMouzhu(global::CardGame.Core.ProgramSkillFrame f, bool issued) => _resolve().FinishHeJinMouzhu(f, issued);
    internal bool HasHeJinDamageObserver(long id) => _resolve().HasHeJinDamageObserver(id);
    internal int HeJinDeathOwnedCardCount(global::CardGame.Core.DeathFrame death) => _resolve().HeJinDeathOwnedCardCount(death);
    internal bool HeJinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HeJinFirstChild(f, child);
    internal int[] HeJinHandAt(long sequence, int seat) => _resolve().HeJinHandAt(sequence, seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HeJinMouzhuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HeJinMouzhuChoices(f);
    internal global::CardGame.Core.ProgramSkillFrame? HeJinObserverRoot() => _resolve().HeJinObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HeJinYanhuoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HeJinYanhuoChoices(f);
    internal bool IsHeJinMouzhuDuelUse(long id) => _resolve().IsHeJinMouzhuDuelUse(id);
    internal bool IsHeJinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsHeJinRecoveryParent(f, recovery);
    internal bool IsOriginalDyingSuspendedByHeJinYanhuo(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByHeJinYanhuo(dying);
    internal bool MatchesHeJinMouzhu(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HeJinMouzhuUseOrigin origin) => _resolve().MatchesHeJinMouzhu(f, origin);
    internal void PublishHeJinPrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHeJinPrompt(f);
    internal bool ResumeHeJin(long id) => _resolve().ResumeHeJin(id);
    internal void ResumeHeJinYanhuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResumeHeJinYanhuo(f);
    internal bool ReturnHeJinMouzhuAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnHeJinMouzhuAttack(completion);
    internal void ReturnHeJinMouzhuUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnHeJinMouzhuUse(use);
    internal bool ValidHeJinMouzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHeJinMouzhu(f);
    internal bool ValidHeJinYanhuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHeJinYanhuo(f);
}
