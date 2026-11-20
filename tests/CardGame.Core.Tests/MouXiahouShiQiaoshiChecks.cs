namespace CardGame.Core.Tests;

internal static class MouXiahouShiQiaoshiChecks
{
    private const string Draw = "skill-program.mou-mobile:qiaoshi.Draw";

    public static void NativeRecoveryReplacementPaidChildrenSourceLossAndCold()
    {
        var loss = MouXiahouShiScene.Start(new("paid-recovery-child-source-loss")
            { HeroSeat = 1, Qiaoshi = true, ConditionalMinimum = 2, WatchRecovery = true, ChildLoss = 2 });
        loss.Use("damage-one", 1); loss.Reach(loss.QiaoshiChoice); loss.ChooseBranch("recover-and-draw"); loss.Reach(loss.Hold);
        var frame = loss.E<MouXiahouShiQiaoshiStartedEvent>().Single().FrameId;
        var normal = loss.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frame);
        var normalChange = loss.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(child => child.ResumeFrameId == frame);
        var application = loss.E<ProgramRecoveryAppliedEvent>().Single(fact => fact.ProgramFrameId == frame).Receipt;
        var normalRecovery = new ProgramOperationRecovery(new(frame, normal.InstructionIndex, normal.GameplayHash), 0, 1, 1)
        {
            NativeFrameId = application.RecoveryFrameId,
            Result = new(1, null)
        };
        loss.Need(loss.P!.PlayerSeat == 1 && loss.V(1).Hp == 3 && loss.E<RecoveryAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            !loss.E<MouXiahouShiDrawReturnedEvent>().Any() && normal.MouXiahouShiQiaoshi is
                { Branch: "recover-and-draw", Stage: MouXiahouShiQiaoshiStage.RecoveryChildren } &&
            application is { SourceSeat: 0, TargetSeat: 1, RequestedAmount: 1, HpBefore: 2, HpAfter: 3 } &&
            application.InstructionIndex == normal.InstructionIndex && application.GameplayHash == normal.GameplayHash &&
            application.HpChangeId == normalChange.Change.Id && normalChange.Change.ParentFrameId == frame &&
            normal.OperationRecovery == normalRecovery,
            "The true HP-recovery child pauses under the paid Qiaoshi frame before the source's draw, with the recipient independently controlling its own child.");
        loss.Private(); loss.Cold();
        loss.Need(loss.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frame).OperationRecovery == normalRecovery,
            "Cold restore keeps the actual one-point recovery result on the original operation before its HP child returns.");
        loss.Reach(() => loss.E<MouXiahouShiCompletedEvent>().Any(e => e.FrameId == frame)); loss.Play();
        loss.Need(loss.V(1).Hp == 1 && !loss.Has(MouXiahouShiScene.Qiaoshi) && loss.E<RecoveryAppliedEvent>().Count(e => e.TargetSeat == 1) == 1 &&
            loss.E<MouXiahouShiQiaoshiRecoveryReturnedEvent>().SequenceEqual([new(frame, 0, 1, 1) { OriginalRecovered = 1 }]),
            "The NPC's real child answer and subsequent HP Loss remove the conditional source without replaying or cancelling its accepted recovery/draw tail.");
        DrawOnce(loss, frame, 0); loss.Cold();

        var death = MouXiahouShiScene.Start(new("paid-draw-source-death")
            { HeroSeat = 1, Qiaoshi = true, WatchDraw = true, ChildLoss = 6, Hand = [MouXiahouShiScene.C(CardKind.Dodge)] });
        death.Use("damage-one", 1); death.Reach(death.QiaoshiChoice); death.ChooseBranch("recover-and-draw"); death.Reach(death.Hold);
        var paid = death.E<MouXiahouShiQiaoshiStartedEvent>().Single().FrameId;
        death.Need(death.P!.PlayerSeat == 0 && death.E<MouXiahouShiDrawReturnedEvent>().Single(e => e.FrameId == paid) is
            { RecipientSeat: 0, RequestedCount: 2, ActualCount: 2 } && !death.E<MouXiahouShiCompletedEvent>().Any(e => e.FrameId == paid),
            "Draw2's actual cards are committed before its genuine gain child; the parent remains pending until that child returns.");
        death.Private(); death.RejectPrompt(); death.Cold(); death.Continue();
        death.Reach(() => death.E<MouXiahouShiCompletedEvent>().Any(e => e.FrameId == paid));
        death.Need(!death.V().IsAlive && death.V(1).IsAlive && death.G.State.Winner == Winner.None &&
            death.E<PlayerDiedEvent>().Count(e => e.VictimSeat == 0) == 1 &&
            !death.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid),
            "Actual nonterminal death of the source in its already-paid draw child returns once without new draws or a stranded Qiaoshi parent.");
        DrawOnce(death, paid, 0); death.Cold();

        var replacement = MouXiahouShiScene.Start(new("native-faction-recovery-replacement") { Qiaoshi = true, RecoveryReplacement = true });
        var provider = Enumerable.Range(1, 3).Single(seat => replacement.V(seat).Role == Role.Loyalist);
        replacement.Need(replacement.V().Role == Role.Lord && replacement.G.CreateSnapshot(provider).Players[0].Role == Role.Lord &&
            replacement.V(provider).Hp == 1 && replacement.V(provider).Hp < replacement.V(provider).MaxHp &&
            replacement.E<LordSkillProjectionChangedEvent>().Any(e => e.Added && e.OwnerSeat == provider && e.SkillId == "fixture:mxhs-redirect" && e.Source.LordSeat == 0),
            "A genuine loyalist's native lord-skill projection qualifies its recovery policy. Its actual source decision sees the real public Lord as the Qiaoshi owner.");
        replacement.Use("foreign-one", provider);
        replacement.Reach(() => replacement.P is { PlayerSeat: 0, Kind: DecisionKind.RecoveryReplacement });
        var redirect = replacement.E<MouXiahouShiQiaoshiStartedEvent>().Single();
        var recovery = replacement.G.ResolutionStack.OfType<RecoveryReplacementFrame>().Single();
        var request = replacement.E<ProgramRecoveryRequestedEvent>().Single(fact => fact.ProgramFrameId == redirect.FrameId);
        var pendingRecovery = new ProgramOperationRecovery(new(redirect.FrameId, request.InstructionIndex, redirect.GameplayHash), provider, 0, 1)
        {
            NativeFrameId = request.RecoveryFrameId
        };
        replacement.Need(redirect.Source.OwnerSeat == 0 && redirect.DamageSourceSeat == provider &&
            replacement.E<MouXiahouShiQiaoshiBranchAcceptedEvent>().Single() is { Branch: "recover-and-draw" } &&
            recovery.Attempt is { TargetSeat: 0, Amount: 1 } && recovery.Attempt.SourceSeat == provider &&
            recovery.ParentFrameId == redirect.FrameId && recovery.Return.ResumeFrameId == redirect.FrameId &&
            request == new ProgramRecoveryRequestedEvent(redirect.FrameId, recovery.Attempt.Completion.InstructionIndex,
                redirect.GameplayHash, recovery.Id, provider, 0, 1, replacement.V().Hp) &&
            replacement.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == redirect.FrameId).OperationRecovery == pendingRecovery &&
            replacement.E<MouXiahouShiDrawReturnedEvent>().Length == 0,
            "The actual foreign NPC source accepts through its own AI, and Qiaoshi's real native recovery pauses for the human beneficiary's legal replacement.");
        var hp = replacement.V().Hp; var providerHp = replacement.V(provider).Hp;
        var hand = replacement.V().HandCount; replacement.Private(); replacement.RejectPrompt(); replacement.Cold();
        replacement.Need(replacement.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == redirect.FrameId).OperationRecovery == pendingRecovery,
            "The same native request remains pending on the original operation after restoring the replacement choice.");
        replacement.Answer(c => c.Parameters.GetValueOrDefault("recovery-action") == "redirect" && c.Targets.SequenceEqual([provider]));
        replacement.Play();
        replacement.Need(replacement.V().Hp == hp && hp == replacement.V().MaxHp - 1 && replacement.E<RecoveryReplacementChosenEvent>().Single() is
            { RecoveringSeat: 0, BeneficiarySeat: var beneficiary, ReplacedAmount: 1, RecoveryAmount: 1 } chosen && beneficiary == provider &&
            chosen.RecoveryFrameId == request.RecoveryFrameId &&
            replacement.E<MouXiahouShiQiaoshiRecoveryReturnedEvent>().SequenceEqual([
                new(redirect.FrameId, provider, 0, 1) { OriginalRecovered = 0, RedirectedSeat = provider, RedirectedRecovered = 1 }]) &&
            !replacement.E<RecoveryAppliedEvent>().Any(e => e.TargetSeat == 0) &&
            replacement.V(provider).Hp == providerHp + 1 && replacement.E<RecoveryAppliedEvent>().Count(e => e.TargetSeat == provider && e.Amount == 1) == 1 &&
            replacement.V().HandCount == hand + 1 && replacement.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) &&
                m.Reason.Value == "skill-program.recovery-replacement.reward") == 1,
            "Replacing the real recovery preserves zero actual HP recovered by the Qiaoshi owner and returns through its exact native receipt.");
        DrawOnce(replacement, redirect.FrameId, provider); replacement.Cold();

        var full = MouXiahouShiScene.Start(new("armor-absorbed-damage-full-recovery")
            { HeroSeat = 1, Qiaoshi = true, WatchDraw = true });
        full.Use("armor-one", 1); full.Play();
        var fullHp = full.V(1).MaxHp;
        full.Need(full.V(1).Hp == fullHp && full.V(1).Armor == 1, "The original Qiaoshi owner has full health and one real armor.");
        full.Use("damage-one", 1); full.Reach(full.QiaoshiChoice);
        var absorbed = full.E<MouXiahouShiQiaoshiStartedEvent>().Single();
        full.Need(full.V(1).Hp == fullHp && full.V(1).Armor == 0 && absorbed is { DamageSourceSeat: 0, DamageAmount: 1 } &&
            full.E<DamageAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1, ArmorPayment: { ArmorAbsorbed: 1, ActualHpLost: 0 } },
            "The real foreign damage opens Qiaoshi even though armor absorbed the entire amount and the recipient remains full.");
        full.ChooseBranch("recover-and-draw"); full.Reach(full.Hold);
        var fullFrame = full.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == absorbed.FrameId);
        var fullRecovery = new ProgramOperationRecovery(new(fullFrame.Id, fullFrame.InstructionIndex, fullFrame.GameplayHash), 0, 1, 1)
        {
            Result = new(0, null)
        };
        full.Need(full.V(1).Hp == fullHp && full.E<RecoveryAppliedEvent>().Length == 0 &&
            fullFrame.OperationRecovery == fullRecovery &&
            full.E<MouXiahouShiQiaoshiRecoveryReturnedEvent>().SequenceEqual([new(absorbed.FrameId, 0, 1, 1)]) &&
            full.E<MouXiahouShiDrawReturnedEvent>().Single(e => e.FrameId == absorbed.FrameId) is { RecipientSeat: 0, RequestedCount: 2, ActualCount: 2 } &&
            !full.E<MouXiahouShiCompletedEvent>().Any(e => e.FrameId == absorbed.FrameId),
            "The accepted one-point recovery is a real zero-healing result, and the original source has already paid Draw2 before its gain child returns.");
        full.Cold();
        full.Need(full.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == absorbed.FrameId).OperationRecovery == fullRecovery,
            "Cold restore keeps the original full-health request and its explicit zero result, with no native frame to repay.");
        full.Continue();
        full.Reach(() => full.E<MouXiahouShiCompletedEvent>().Any(e => e.FrameId == absorbed.FrameId));
        DrawOnce(full, absorbed.FrameId, 0);
    }

    private static void DrawOnce(MouXiahouShiScene s, long frame, int recipient)
    {
        var draw = s.E<MouXiahouShiDrawReturnedEvent>().Single(e => e.FrameId == frame);
        var moved = s.G.CardMovements.Where(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter &&
            m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason.Value == Draw).ToArray();
        s.Need(draw is { RequestedCount: 2, ActualCount: 2, ReplacementCount: 0 } && draw.RecipientSeat == recipient &&
            s.E<MouXiahouShiCompletedEvent>().Count(e => e.FrameId == frame && e.Accepted) == 1 &&
            moved.Length == 2 && moved.Select(m => m.CardId).Distinct().Count() == 2,
            "The original living source receives exactly the two native actual draws, and the owning Qiaoshi frame completes exactly once after its children.");
    }
}
