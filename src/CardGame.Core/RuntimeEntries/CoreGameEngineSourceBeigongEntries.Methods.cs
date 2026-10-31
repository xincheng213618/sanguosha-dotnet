#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceBeigongEntries
{
    internal void ContinueSourceBeigong(long id) => _resolve().ContinueSourceBeigong(id);
    internal bool DrainSourceBeigong(long id) => _resolve().DrainSourceBeigong(id);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawSourceDingxi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().DrawSourceDingxi(supplied);
    internal void FinishSourceBeigong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSourceBeigong(f);
    internal bool ResumeSourceBeigong(long id) => _resolve().ResumeSourceBeigong(id);
    internal bool ReturnSourceBeigongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSourceBeigongMovement(f);
    internal bool SourceBeigongCanOffer(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().SourceBeigongCanOffer(candidate, trigger, context);
    internal bool SourceBeigongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().SourceBeigongMovement(f, effect, pending);
    internal int SourceDingxiCount(int owner) => _resolve().SourceDingxiCount(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> SourceDingxiPiles(int owner) => _resolve().SourceDingxiPiles(owner);
}
