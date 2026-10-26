using CardGame.Core;

internal static partial class WushuangSkillsBChecks
{
    public static void DinganDamagesDuringExactSecondSlashAndReturnsCold()
    {
        const string watcher = "fixture:wushuang-b-dingan-damage";
        var (g, registry) = Create([3335], CardKind.Slash, initialHp: 8, dinganDamage: true);
        Play(g);
        Use(g, g.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1])));
        Play(g);
        var before = g.CreateSnapshot(0).Players[1].Hp;
        Use(g, g.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1])));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("step") == "draw-target"));
        var parent = g.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.SkillId == Skill(3335));
        var useId = parent.WindowContext!.CardUse!.ParentCardUseFrameId;
        var use = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == useId);
        var material = use.Action!.PhysicalCards.Single().CardId;
        Require(use.CardAttack is { Active: true, DamageWasApplied: false } && use.ProgramUseCommitted,
            "Dingan suspends the second real Slash before its original damage, with the physical cost already paid.");
        Answer(g, c => c.Targets.SequenceEqual([0]));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("step") == "punish"));
        Cold(g, registry);
        var punishment = P(g)!.Choices.First(c => c.Parameters.GetValueOrDefault("value")?.StartsWith("damage-", StringComparison.Ordinal) == true);
        var punishedSeat = punishment.Targets.Single();
        var punishedBefore = g.CreateSnapshot(0).Players[punishedSeat].Hp;
        var view = g.CreateSnapshot(0);
        Require(punishedSeat != 1 && view.Players[punishedSeat].HandCount ==
                view.Players.Where(p => p.IsAlive && p.Seat != 0).Max(p => p.HandCount),
            "The actual published punishment targets a largest-hand other, independently of the original Slash target.");
        Answer(g, c => c.Id == punishment.Id);
        Reach(g, p => p.SkillPrompt?.SkillId == watcher);
        var nested = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == parent.Id);
        Require(nested.AttackAttempt is { SourceSeat: 0, DamageWasApplied: true } hit && hit.TargetSeat == punishedSeat &&
                nested.AttackReturn?.ParentAttackOwnerFrameId == useId &&
                g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == useId).CardAttack is { DamageWasApplied: false },
            "The one-point punishment has its own attack and preserves the exact suspended Slash owner.");
        Cold(g, registry);
        Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        Play(g);
        Require(g.CreateSnapshot(0).Players[1].Hp == before - 1 &&
                g.CreateSnapshot(0).Players[punishedSeat].Hp == punishedBefore - 1 &&
                Facts<WushuangDinganDrawnEvent>(g).Count(e => e.ProgramFrameId == parent.Id) == 1 &&
                Facts<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == useId) == 1 &&
                g.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                g.CreateCardZoneDiagnostics().Single(c => c.CardId == material).Location == CardLocation.DiscardPile,
            "Punishment and original Slash each damage once, then the original material finishes without repeated payment.");
        Cold(g, registry);
    }
}
