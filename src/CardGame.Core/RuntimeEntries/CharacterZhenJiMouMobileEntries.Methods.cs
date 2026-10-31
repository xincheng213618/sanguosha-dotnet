#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhenJiMouMobileEntries
{
    internal bool AdvanceMouZhenJiLuoshenChildren() => _resolve().AdvanceMouZhenJiLuoshenChildren();
    internal PromptChoice AiMouZhenJiLuoshen(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouZhenJiLuoshen(p, f);
    internal void AnswerMouZhenJiLuoshen(PromptChoice selected) => _resolve().AnswerMouZhenJiLuoshen(selected);
    internal bool ContinueAcceptedMouZhenJiLuoshen(ProgramSkillFrame f) => _resolve().ContinueAcceptedMouZhenJiLuoshen(f);
    internal void ContinueLuoshenAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueLuoshenAfterPublicReveal(f);
    internal void ContinueMouZhenJiLuoshen(long id) => _resolve().ContinueMouZhenJiLuoshen(id);
    internal bool DrainMouZhenJiLuoshen(ProgramSkillFrame f) => _resolve().DrainMouZhenJiLuoshen(f);
    internal void FinishMouZhenJiLuoshen(ProgramSkillFrame f) => _resolve().FinishMouZhenJiLuoshen(f);
    internal bool IsMouZhenJiLuoshenDying() => _resolve().IsMouZhenJiLuoshenDying();
    internal bool IsMouZhenJiLuoshenMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsMouZhenJiLuoshenMovement(f, e, p);
    internal bool IsMouZhenJiLuoshenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouZhenJiLuoshenRecoveryParent(f, recovery);
    internal IReadOnlyList<PromptChoice> MouZhenJiLuoshenChoices(ProgramSkillFrame f) => _resolve().MouZhenJiLuoshenChoices(f);
    internal bool MouZhenJiLuoshenFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouZhenJiLuoshenFirstChild(f, child);
    internal bool MouZhenJiLuoshenObserverEdge(int n) => _resolve().MouZhenJiLuoshenObserverEdge(n);
    internal ProgramSkillFrame? MouZhenJiLuoshenObserverRoot() => _resolve().MouZhenJiLuoshenObserverRoot();
    internal bool MouZhenJiLuoshenObserverSuffix(int index) => _resolve().MouZhenJiLuoshenObserverSuffix(index);
    internal void PublishMouZhenJiLuoshen(ProgramSkillFrame f) => _resolve().PublishMouZhenJiLuoshen(f);
    internal bool ResumeMouZhenJiLuoshen(long id) => _resolve().ResumeMouZhenJiLuoshen(id);
    internal bool ReturnMouZhenJiLuoshenMovement(ProgramSkillFrame f) => _resolve().ReturnMouZhenJiLuoshenMovement(f);
    internal SkillProgramStepOutcome StartMouZhenJiLuoshen(ProgramSkillFrame supplied) => _resolve().StartMouZhenJiLuoshen(supplied);
    internal bool ValidMouZhenJiLuoshen(ProgramSkillFrame f) => _resolve().ValidMouZhenJiLuoshen(f);
    internal bool HasMouZhenJiLuoshenCapability { get => _resolve().HasMouZhenJiLuoshenCapability; }
}
