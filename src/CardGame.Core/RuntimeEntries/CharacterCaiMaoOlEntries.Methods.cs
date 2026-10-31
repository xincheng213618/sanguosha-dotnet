#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiMaoOlEntries
{
    internal bool AdvanceCaiMaoChildren() => _resolve().AdvanceCaiMaoChildren();
    internal void AnswerCaiMao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaiMao(choice);
    internal void AssertCaiMao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertCaiMao(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaiMao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginCaiMao(supplied, op);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaiMaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaiMaoChoices(f);
    internal bool CaiMaoDamageParentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CaiMaoDamageParentMatches(f, context);
    internal bool CaiMaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CaiMaoFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? CaiMaoObserverRoot() => _resolve().CaiMaoObserverRoot();
    internal bool CaiMaoSourceLive(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaiMaoSourceLive(f);
    internal double CaiMaoTargetScore(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().CaiMaoTargetScore(f, target);
    internal global::CardGame.Core.PromptChoice ChooseCaiMaoAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseCaiMaoAi(p, f);
    internal void ContinueCaiMao(long id) => _resolve().ContinueCaiMao(id);
    internal bool ContinuePaidCaiMao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidCaiMao(f);
    internal bool DrainCaiMao(long id) => _resolve().DrainCaiMao(id);
    internal void FinishCaiMao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCaiMao(f);
    internal bool HasCaiMaoBeforeDamageObserver(long id) => _resolve().HasCaiMaoBeforeDamageObserver(id);
    internal bool HasCaiMaoDamageObserver(long id) => _resolve().HasCaiMaoDamageObserver(id);
    internal bool IsCaiMaoDying() => _resolve().IsCaiMaoDying();
    internal void PayCaiMaoExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayCaiMaoExchange(f);
    internal void PublishCaiMao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaiMao(f);
    internal bool ResumeCaiMao(long id) => _resolve().ResumeCaiMao(id);
    internal bool ValidCaiMao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCaiMao(f);
    internal bool HasCaiMaoCapability { get => _resolve().HasCaiMaoCapability; }
}
