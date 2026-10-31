#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceBeigongEntries
{
    internal void ContinueSourceBeigong(long id) => _resolve().ContinueSourceBeigong(id);
    internal bool DrainSourceBeigong(long id) => _resolve().DrainSourceBeigong(id);
    internal SkillProgramStepOutcome DrawSourceDingxi(ProgramSkillFrame supplied) => _resolve().DrawSourceDingxi(supplied);
    internal void FinishSourceBeigong(ProgramSkillFrame f) => _resolve().FinishSourceBeigong(f);
    internal bool ResumeSourceBeigong(long id) => _resolve().ResumeSourceBeigong(id);
    internal bool ReturnSourceBeigongMovement(ProgramSkillFrame f) => _resolve().ReturnSourceBeigongMovement(f);
    internal bool SourceBeigongCanOffer(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().SourceBeigongCanOffer(candidate, trigger, context);
    internal bool SourceBeigongMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().SourceBeigongMovement(f, effect, pending);
    internal int SourceDingxiCount(int owner) => _resolve().SourceDingxiCount(owner);
    internal IReadOnlyList<PublicPersistentPileSource> SourceDingxiPiles(int owner) => _resolve().SourceDingxiPiles(owner);
}
