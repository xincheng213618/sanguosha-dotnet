#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhenJiMouMobileEntries
{
    internal bool AdvanceMouZhenJiLuoshenChildren() => _resolve().AdvanceMouZhenJiLuoshenChildren();
    internal global::CardGame.Core.PromptChoice AiMouZhenJiLuoshen(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouZhenJiLuoshen(p, f);
    internal void AnswerMouZhenJiLuoshen(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouZhenJiLuoshen(selected);
    internal void AssertMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouZhenJiLuoshen(f);
    internal bool ContinueAcceptedMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedMouZhenJiLuoshen(f);
    internal void ContinueLuoshenAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueLuoshenAfterPublicReveal(f);
    internal void ContinueMouZhenJiLuoshen(long id) => _resolve().ContinueMouZhenJiLuoshen(id);
    internal bool DrainMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouZhenJiLuoshen(f);
    internal void FinishMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouZhenJiLuoshen(f);
    internal bool HasMouZhenJiLuoshenBeforeDamageObserver(long id) => _resolve().HasMouZhenJiLuoshenBeforeDamageObserver(id);
    internal bool HasMouZhenJiLuoshenDamageObserver(long id) => _resolve().HasMouZhenJiLuoshenDamageObserver(id);
    internal bool IsMouZhenJiLuoshenDying() => _resolve().IsMouZhenJiLuoshenDying();
    internal bool IsMouZhenJiLuoshenMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouZhenJiLuoshenMovement(f, e, p);
    internal bool IsMouZhenJiLuoshenRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouZhenJiLuoshenRecoveryParent(f, recovery);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouZhenJiLuoshenChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouZhenJiLuoshenChoices(f);
    internal bool MouZhenJiLuoshenFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouZhenJiLuoshenFirstChild(f, child);
    internal bool MouZhenJiLuoshenObserverEdge(int n) => _resolve().MouZhenJiLuoshenObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouZhenJiLuoshenObserverRoot() => _resolve().MouZhenJiLuoshenObserverRoot();
    internal bool MouZhenJiLuoshenObserverSuffix(int index) => _resolve().MouZhenJiLuoshenObserverSuffix(index);
    internal void PublishMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouZhenJiLuoshen(f);
    internal bool ResumeMouZhenJiLuoshen(long id) => _resolve().ResumeMouZhenJiLuoshen(id);
    internal bool ReturnMouZhenJiLuoshenMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouZhenJiLuoshenMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().StartMouZhenJiLuoshen(supplied);
    internal bool ValidMouZhenJiLuoshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouZhenJiLuoshen(f);
    internal bool HasMouZhenJiLuoshenCapability { get => _resolve().HasMouZhenJiLuoshenCapability; }
}
