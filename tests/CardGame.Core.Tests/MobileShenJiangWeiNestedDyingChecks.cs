using CardGame.Core;

internal static partial class MobileShenJiangWeiChecks
{
    public static void ShenpeiThunderOwnsNestedDyingAndReturnsOriginalEntryCold()
    {
        foreach (var rescue in new[] { false, true })
        {
            var (g, registry) = Start(nestedDying: true);
            Play(g);
            var peach = V(g).Hand.Single(c => c.Kind == CardKind.Peach).Id;
            var initialHp = V(g).Hp;
            for (var n = 0; n < initialHp; n++) { Use(g, "hurt"); if (n < initialHp - 1) Play(g); }
            Offer(g, "mobile:shenpei"); Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip"); Play(g);
            Use(g, "hurt"); Offer(g, "mobile:shenpei");
            Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Menu(g, MobileJiangWeiStage.ThunderTarget);
            var root = Root(g)!;
            var original = root.MobileJiangWei!.DyingId;
            var entry = g.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
                .Single(f => f.ResumeDyingFrameId == original);
            Need(V(g).Hp == 2 && V(g, 1).Hp == 2 && root.MobileJiangWei.Amount == 2,
                "A real second Shenpei entry recovers its owner before threatening a living two-HP target.");
            g = Cold(g, registry);
            Answer(g, c => c.Targets.SequenceEqual([1]));
            Reach(g, p => p.Kind == DecisionKind.RescueDying && p.PlayerSeat == 0);
            var entries = g.ResolutionStack.OfType<DyingFrame>().ToArray();
            var nested = entries.Single(d => d.Id != original);
            var damage = g.ResolutionStack.OfType<DamageFrame>().Single(d => d.Id == nested.ParentFrameId);
            var program = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == root.Id);
            var retainedEntry = g.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(f => f.Id == entry.Id);
            Need(entries.Length == 2 && entries.Any(d => d.Id == original && d.VictimSeat == 0) &&
                    nested.VictimSeat == 1 && damage.ParentFrameId == root.Id && damage.Nature == DamageNature.Thunder &&
                    retainedEntry.ResumeDyingFrameId == original && retainedEntry.Window == SkillProgramTriggerWindow.DyingEntering &&
                    retainedEntry.Continuation == ProgramLifecycleContinuation.ResumeDyingEntry &&
                    retainedEntry.CandidateIndex == entry.CandidateIndex && program.ParentFrameId == retainedEntry.Id &&
                    retainedEntry.Candidates[retainedEntry.CandidateIndex].SkillInstanceId == root.SkillInstanceId &&
                    program.MobileJiangWei!.DyingId == original && program.AttackAttempt is { TargetSeat: 1, DamageWasApplied: true } &&
                    P(g)!.Choices.Any(c => c.Cards.SequenceEqual([peach]) && c.Parameters.GetValueOrDefault("response") == "peach"),
                "Only the exact thunder child exposes its new victim; the recovered original Dying and owning program remain suspended.");
            g = Cold(g, registry);
            if (rescue) Answer(g, c => c.Cards.SequenceEqual([peach]) && c.Parameters.GetValueOrDefault("response") == "peach");
            else Answer(g, c => c.Parameters.GetValueOrDefault("response") == "let-die");
            Play(g);
            Need(V(g).IsAlive && V(g).Hp == 2 && V(g, 1).IsAlive == rescue &&
                    E<DyingResolvedEvent>(g).Count(e => e.ResolutionId == nested.Id && e.VictimSeat == 1 && e.Survived == rescue) == 1 &&
                    E<DyingResolvedEvent>(g).Count(e => e.ResolutionId == original && e.VictimSeat == 0 && e.Survived) == 1 &&
                    E<MobileJiangWeiCompletedEvent>(g).Count(e => e.FrameId == root.Id) == 1 &&
                    E<DamageAppliedEvent>(g).Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 2 && e.Nature == DamageNature.Thunder) == 1 &&
                    V(g).Skills!.Any(s => s.ContentId == "mobile:huitian") && g.ResolutionStack.Count == 0,
                "Either real rescue or death returns the thunder once, grants Huitian once and completes the original surviving entry.");
            Need(rescue ? Zone(g, peach) == CardLocation.DiscardPile : Zone(g, peach) == CardLocation.Hand(0),
                "The actual published rescue pays its Peach only when selected.");
            _ = Cold(g, registry);
        }
    }
}
