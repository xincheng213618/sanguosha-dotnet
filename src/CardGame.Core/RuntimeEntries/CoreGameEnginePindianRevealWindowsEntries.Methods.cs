#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianRevealWindowsEntries
{
    internal void AddRaoshePindianRankAndRestoreGushe(ProgramSkillFrame f, string skillId) => _resolve().AddRaoshePindianRankAndRestoreGushe(f, skillId);
    internal void AssertPindianRevealWindows() => _resolve().AssertPindianRevealWindows();
    internal void BeginPindianRevealWindow(long parent, bool simultaneous, IReadOnlyList<PindianRevealedCard> cards) => _resolve().BeginPindianRevealWindow(parent, simultaneous, cards);
    internal bool CanRunPindianReveal(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunPindianReveal(c, context);
    internal void CompletePindianRevealBinding(ProgramSkillFrame f) => _resolve().CompletePindianRevealBinding(f);
    internal void CompletePindianRevealBinding2(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CompletePindianRevealBinding(c, context);
    internal void CompletePindianRevealWindow(PindianRevealWindowFrame w) => _resolve().CompletePindianRevealWindow(w);
    internal void ContinuePindianRevealWindow() => _resolve().ContinuePindianRevealWindow();
    internal bool ExactPairRevealParent(PindianFrame pair, PindianRevealWindowFrame w) => _resolve().ExactPairRevealParent(pair, w);
    internal bool IsPairPindianRevealSubtree(PindianFrame pair) => _resolve().IsPairPindianRevealSubtree(pair);
    internal ProgramSkillWindowContext PindianRevealContext(PindianRevealWindowFrame w, ProgramTriggerCandidate c) => _resolve().PindianRevealContext(w, c);
    internal bool TryBeginPairPindianReveal(PindianFrame frame, PindianResult result) => _resolve().TryBeginPairPindianReveal(frame, result);
    internal bool TryGetPindianRevealTrigger(scoped out ProgramTriggerCandidate candidate, scoped out ProgramSkillWindowContext context) => _resolve().TryGetPindianRevealTrigger(out candidate, out context);
    internal bool HasPindianRevealCapability { get => _resolve().HasPindianRevealCapability; }
}
