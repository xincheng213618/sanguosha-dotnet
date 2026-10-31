#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianRevealWindowsEntries
{
    internal void AddRaoshePindianRankAndRestoreGushe(global::CardGame.Core.ProgramSkillFrame f, string skillId) => _resolve().AddRaoshePindianRankAndRestoreGushe(f, skillId);
    internal void AssertPindianRevealWindows() => _resolve().AssertPindianRevealWindows();
    internal void BeginPindianRevealWindow(long parent, bool simultaneous, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> cards) => _resolve().BeginPindianRevealWindow(parent, simultaneous, cards);
    internal bool CanRunPindianReveal(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunPindianReveal(c, context);
    internal void CompletePindianRevealBinding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletePindianRevealBinding(f);
    internal void CompletePindianRevealBinding2(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CompletePindianRevealBinding(c, context);
    internal void CompletePindianRevealWindow(global::CardGame.Core.PindianRevealWindowFrame w) => _resolve().CompletePindianRevealWindow(w);
    internal void ContinuePindianRevealWindow() => _resolve().ContinuePindianRevealWindow();
    internal bool ExactPairRevealParent(global::CardGame.Core.PindianFrame pair, global::CardGame.Core.PindianRevealWindowFrame w) => _resolve().ExactPairRevealParent(pair, w);
    internal bool IsPairPindianRevealSubtree(global::CardGame.Core.PindianFrame pair) => _resolve().IsPairPindianRevealSubtree(pair);
    internal global::CardGame.Core.ProgramSkillWindowContext PindianRevealContext(global::CardGame.Core.PindianRevealWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().PindianRevealContext(w, c);
    internal bool TryBeginPairPindianReveal(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PindianResult result) => _resolve().TryBeginPairPindianReveal(frame, result);
    internal bool TryGetPindianRevealTrigger(scoped out global::CardGame.Core.ProgramTriggerCandidate candidate, scoped out global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().TryGetPindianRevealTrigger(out candidate, out context);
    internal bool HasPindianRevealCapability { get => _resolve().HasPindianRevealCapability; }
}
