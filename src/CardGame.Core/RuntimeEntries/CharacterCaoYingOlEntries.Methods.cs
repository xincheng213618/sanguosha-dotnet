#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoYingOlEntries
{
    internal global::CardGame.Core.CaoYingGuessResolvedEvent[] ArmedCaoYingDamage(int source, int target, long? useId = default) => _resolve().ArmedCaoYingDamage(source, target, useId);
    internal void AssertRegisteredCaoYing(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredCaoYing(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoYing(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginCaoYing(supplied, effect);
    internal bool CanContinueCaoYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueCaoYing(f);
    internal bool CanOfferCaoYing(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferCaoYing(candidate, trigger, c);
    internal global::CardGame.Core.PromptChoice CaoYingChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().CaoYingChoice(f, branch, label, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoYingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoYingChoices(f);
    internal int CaoYingDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CaoYingDamage(attack);
    internal bool CaoYingFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoYingFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? CaoYingObserverRoot() => _resolve().CaoYingObserverRoot();
    internal global::CardGame.Core.CardConversionSource CaoYingSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoYingSource(f);
    internal bool CaoYingSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoYingSourceCurrent(f);
    internal bool DrainCaoYingChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainCaoYingChildren(f);
    internal void FinishCaoYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCaoYing(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> GetCaoYingPrivateCards(int viewer) => _resolve().GetCaoYingPrivateCards(viewer);
    internal bool HasCaoYingDamageObserver(long id) => _resolve().HasCaoYingDamageObserver(id);
    internal bool IsCaoYingDying() => _resolve().IsCaoYingDying();
    internal bool IsCaoYingMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsCaoYingMovement(f, e, p);
    internal bool IsMobileCaoYing(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsMobileCaoYing(frame);
    internal void ObserveCaoYingDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveCaoYingDamage(payload);
    internal void ObserveCaoYingExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveCaoYingExpiry(payload);
    internal void PublishCaoYingChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoYingChoice(f);
    internal void RequireCaoYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireCaoYing(f);
    internal void ResolveCaoYingChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveCaoYingChoice(choice);
    internal bool ResumeCaoYing(long id) => _resolve().ResumeCaoYing(id);
    internal bool ReturnCaoYingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCaoYingMovement(f);
    internal bool TryAdvanceCaoYingSubtree() => _resolve().TryAdvanceCaoYingSubtree();
    internal bool ValidCaoYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCaoYing(f);
    internal bool ValidMobileCaoYingGrantTail(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMobileCaoYingGrantTail(f);
}
