#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoYingOlEntries
{
    internal CaoYingGuessResolvedEvent[] ArmedCaoYingDamage(int source, int target, long? useId = default) => _resolve().ArmedCaoYingDamage(source, target, useId);
    internal SkillProgramStepOutcome BeginCaoYing(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginCaoYing(supplied, effect);
    internal bool CanContinueCaoYing(ProgramSkillFrame f) => _resolve().CanContinueCaoYing(f);
    internal bool CanOfferCaoYing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferCaoYing(candidate, trigger, c);
    internal PromptChoice CaoYingChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? targets = default) => _resolve().CaoYingChoice(f, branch, label, targets);
    internal IReadOnlyList<PromptChoice> CaoYingChoices(ProgramSkillFrame f) => _resolve().CaoYingChoices(f);
    internal int CaoYingDamage(GameEngine.IDamageAttempt attack) => _resolve().CaoYingDamage(attack);
    internal bool CaoYingFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().CaoYingFirstChild(root, child);
    internal ProgramSkillFrame? CaoYingObserverRoot() => _resolve().CaoYingObserverRoot();
    internal CardConversionSource CaoYingSource(ProgramSkillFrame f) => _resolve().CaoYingSource(f);
    internal bool CaoYingSourceCurrent(ProgramSkillFrame f) => _resolve().CaoYingSourceCurrent(f);
    internal bool DrainCaoYingChildren(ProgramSkillFrame f) => _resolve().DrainCaoYingChildren(f);
    internal void FinishCaoYing(ProgramSkillFrame f) => _resolve().FinishCaoYing(f);
    internal IEnumerable<CardSnapshot> GetCaoYingPrivateCards(int viewer) => _resolve().GetCaoYingPrivateCards(viewer);
    internal bool IsMobileCaoYing(ProgramSkillFrame frame) => _resolve().IsMobileCaoYing(frame);
    internal void ObserveCaoYingDamage(IGameEvent payload) => _resolve().ObserveCaoYingDamage(payload);
    internal void ObserveCaoYingExpiry(IGameEvent payload) => _resolve().ObserveCaoYingExpiry(payload);
    internal void PublishCaoYingChoice(ProgramSkillFrame f) => _resolve().PublishCaoYingChoice(f);
    internal void RequireCaoYing(ProgramSkillFrame f) => _resolve().RequireCaoYing(f);
    internal void ResolveCaoYingChoice(PromptChoice choice) => _resolve().ResolveCaoYingChoice(choice);
    internal bool ResumeCaoYing(long id) => _resolve().ResumeCaoYing(id);
    internal bool ReturnCaoYingMovement(ProgramSkillFrame f) => _resolve().ReturnCaoYingMovement(f);
    internal bool TryAdvanceCaoYingSubtree() => _resolve().TryAdvanceCaoYingSubtree();
    internal bool ValidCaoYing(ProgramSkillFrame f) => _resolve().ValidCaoYing(f);
    internal bool ValidMobileCaoYingGrantTail(ProgramSkillFrame f) => _resolve().ValidMobileCaoYingGrantTail(f);
}
