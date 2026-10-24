using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class WuZhangFeiChecks
{
    private const string Zisheng = "wumiao:zisheng", Xianlue = "wumiao:xianlue", Haoxian = "wumiao:haoxian";
    private const string Owner = "fixture:wzf-owner", Ally = "fixture:wzf-ally", EnemyA = "fixture:wzf-enemy-a", EnemyB = "fixture:wzf-enemy-b";
    private const string Driver = "fixture:wzf-driver", Quiet = "fixture:wzf-quiet", Watch = "fixture:wzf-watch", Conditional = "fixture:wzf-conditional";
    private const string Deck = "fixture:wzf-deck", DyingGift = "fixture:wzf-dying-gift";
    private const string Xiewei = "ol:xiewei";
    private static ShanheStartingCard C(CardKind kind, int rank) => new(kind, Suit.Spade, rank);
    private static ShanheStartingCard[] Ranks(int first, int count) => Enumerable.Range(first, count).Select(r => C(CardKind.Slash, r)).ToArray();

    public static void TrueUseMaterialRanksPrivateSamplingAndCold()
    {
        ReservedLoaderRejectsTransplants();
        DeclaredResponsesKeepOriginalRanksAndOnePaymentCold();
        DeclaredCounterspellGroupAndProactiveRecoveryCold();
        ActualAiGroupMultiDodgeUsesItsCompletePublishedSelectionCold();
        foreach (var (left, right, eligible) in new[] { (1, 5, true), (9, 9, true), (1, 2, false), (1, 3, false) })
        {
            var s = Scene.Start(new("spear-" + left + "-" + right)
            { Hand = [C(CardKind.Slash, left), C(CardKind.Slash, right)], Equipment = [C(CardKind.ZhangbaSerpentSpear, 1)] });
            var materials = s.V().Hand.Select(c => c.Id).ToArray();
            s.Accept(new UseEquipmentEffectCommand(0, CardKind.ZhangbaSerpentSpear, materials, [2], s.G.Revision, s.P!.PromptId));
            if (eligible) { s.ReachSample(); s.SelectSample(); }
            s.Play();
            s.Need(materials.All(id => s.PaidOnce(id)) && s.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Slash) == 1 &&
                s.SampleObtains().Length == (eligible ? 1 : 0),
                "A genuine two-material Zhangba Slash pays each entity once and sums ranks without the native display-rank clamp: 9+9=18 qualifies, while 1+2=3 and 1+3=4 do not."); s.Cold();
        }

        foreach (var pool in new[] { 0, 1, 2, 5 })
        {
            var s = Scene.Start(new("pool-" + pool) { Hand = [C(CardKind.SilverLion, 6)], RankThreeStock = pool });
            var material = s.V().Hand.Single().Id; s.Native(CardKind.SilverLion, []);
            if (pool == 0)
            {
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Zisheng && s.P.Choices.Any(c => s.IsAction(c, "activate")));
                s.Answer(c => s.IsAction(c, "activate"));
            }
            else
            {
                s.ReachSample(); var choices = s.P!.Choices.Where(c => s.IsAction(c, "wu-zhang-fei-sample")).ToArray();
                s.Need(choices.Length == Math.Min(3, pool) && choices.All(c => c.Cards.Count == 1 && c.Targets.Count == 0) &&
                    choices.Select(c => c.Cards.Single()).Distinct().Count() == choices.Length &&
                    choices.All(c => s.G.CreateCardZoneDiagnostics().Any(z => z.CardId == c.Cards.Single() && z.Location == CardLocation.DrawPile)),
                    "Sampling publishes up to three distinct actual eligible deck entities and no invented card when fewer than three exist.");
                var sampleCards = s.G.CreateSnapshot(0).PrivateRevealedCards ?? [];
                s.Need(sampleCards.Count == choices.Length && sampleCards.All(c => c.Rank == 3) &&
                    sampleCards.Select(c => c.Id).ToHashSet().SetEquals(choices.Select(c => c.Cards.Single())),
                    "The real chooser sees the exact sampled rank-three faces while the sample remains private to that chooser.");
                s.Private(); s.Immutable(choices[0].Cards); s.RejectPrompt(); s.Cold(); s.SelectSample();
            }
            s.Play();
            s.Need(s.V().Equipment.Any(c => c.Id == material) && s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.SampleObtains().Length == (pool == 0 ? 0 : 1), "A rank-six native Equipment Use finishes after one real optional obtain; an empty pool fabricates neither sample nor draw."); s.Cold();
        }

        var dodge = Scene.Start(new("response-dodge") { Hand = [C(CardKind.Dodge, 6)] });
        var dodgeId = dodge.V().Hand.Single().Id; dodge.IncomingSlash(); dodge.Respond(dodgeId); dodge.ReachSample(); dodge.SelectSample(); dodge.Play();
        dodge.Need(dodge.PaidOnce(dodgeId) && dodge.SampleObtains().Length == 1 && dodge.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Slash) == 1,
            "An actual accepted native Dodge response-Use qualifies and returns through the incoming Slash once."); dodge.Cold();

        var multi = Scene.Start(new("multi-response") { Hand = [C(CardKind.Slash, 9), C(CardKind.Slash, 9)] });
        var pair = multi.V().Hand.Select(c => c.Id).ToArray(); multi.IncomingSlash(); multi.Private(); multi.Cold();
        multi.Answer(c => c.Cards.SequenceEqual(pair) && c.Parameters.GetValueOrDefault("conversion-binding-id") == "two-material-dodge");
        multi.ReachSample(); multi.SelectSample(); multi.Play();
        multi.Need(pair.All(multi.PaidOnce) && multi.SampleObtains().Length == 1 &&
            multi.E<WuZhangFeiUseRankCapturedEvent>().Any(e => e.ActorSeat == 0 && e.MaterialCount == 2 && e.RankSum == 18) &&
            multi.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Slash) == 1,
            "A true two-material Dodge response retains its original 9+9=18 invoice before payment, samples once and returns through the incoming native Slash once."); multi.Cold();

        var played = Scene.Start(new("pure-played") { Hand = [C(CardKind.Slash, 6)] }); var playedId = played.V().Hand.Single().Id;
        played.Use("duel", [1]); played.Reach(() => played.P is { Kind: DecisionKind.RespondSlash }); played.Respond(playedId); played.Play();
        played.Need(played.PaidOnce(playedId) && played.SampleObtains().Length == 0 &&
            !played.E<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Zisheng && e.BindingId == "rank-use-sample" && e.Activated),
            "A real Duel's pure Played Slash pays its entity without becoming a true Use or obtaining a rank-three sample."); played.Cold();

        var counter = Scene.Start(new("response-null") { Hand = [C(CardKind.Dismantlement, 1), C(CardKind.Nullification, 6)],
            PeerHands = [[], [C(CardKind.Slash, 2)], []] });
        var counterId = counter.V().Hand.Single(c => c.Kind == CardKind.Nullification).Id; counter.Native(CardKind.Dismantlement, [2]);
        counter.Reach(() => counter.P is { Kind: DecisionKind.Nullification }); counter.Respond(counterId); counter.ReachSample(); counter.SelectSample(); counter.Play();
        counter.Need(counter.PaidOnce(counterId) && counter.SampleObtains().Length == 1 && counter.V(2).HandCount == 1,
            "An actual paid Nullification response-Use samples once while the genuinely nullified parent preserves its target's card."); counter.Cold();

        foreach (var response in new[] { false, true })
        {
            var neutral = Scene.Start(new(response ? "xiewei-response" : "xiewei-use") { Hand = [C(CardKind.Slash, 9), C(CardKind.Slash, 9)], XieweiSource = true });
            var originals = neutral.V().Hand.Select(c => c.Id).ToArray();
            if (response)
            {
                neutral.IncomingSlash(); neutral.Private(); neutral.RejectPrompt(); neutral.Cold();
                neutral.Answer(c => c.Parameters.GetValueOrDefault("conversion-skill-id") == Xiewei && c.Cards.SequenceEqual(originals));
            }
            else
            {
                var action = neutral.G.GetHumanLegalActions().FirstOrDefault(a => a.ProgramSkillId == Xiewei && a.PlayedCardKind == CardKind.Slash && a.SelectableCardIds.Order().SequenceEqual(originals.Order()));
                neutral.Need(action is not null, "The actual full Xiewei program must publish its two real Hand cards for a native Slash.");
                neutral.Activate(Xiewei, action!.ProgramActivationId!, [1], originals);
            }
            neutral.Play(); var output = neutral.E<XieweiOutputBoundEvent>().Single();
            var native = neutral.E<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == output.ActionId).Select(e => e.Action).SingleOrDefault() ??
                neutral.E<CardUseAppearanceCapturedEvent>().Where(e => e.Action.ActionId == output.ActionId).Select(e => e.Action).SingleOrDefault();
            neutral.Need(native is { EffectiveRank: 0, EffectiveSuit: Suit.None, EffectiveIsRed: null } && native.PhysicalCards.Count == 2 &&
                native.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(originals.Order()) &&
                neutral.E<XieweiPaymentPreparedEvent>().Single().ResponseUse == response && neutral.E<XieweiBaitStoredEvent>().Select(e => e.CardId).Order().SequenceEqual(originals.Order()) &&
                neutral.E<XieweiBaitStoredEvent>().All(e => originals.Contains(e.CardId) && e.From == CardLocation.Hand(0) &&
                    neutral.G.CardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.Pile && m.Reason.Value == "skill-program.guan-hai.xiewei.store") == 1),
                "Actual Xiewei deposits both original nines once while its genuine immutable action receipt keeps the native Slash or true Dodge output colorless and rank zero; bait storage is not a second response payment."); neutral.Immutable(native!.PhysicalCards); neutral.Cold();
            if (response)
            {
                var admitted = neutral.E<MouZhaJiangResponseAdmittedEvent>().Single(e => e.ActionId == output.ActionId);
                var materials = neutral.E<MouZhaJiangResponseMaterialEvent>().Where(e => e.OwnerFrameId == admitted.OwnerFrameId && e.Index == admitted.Index).OrderBy(e => e.MaterialIndex).ToArray();
                neutral.Need(materials.Select(e => e.CardId).SequenceEqual(native.PhysicalCards.Select(c => c.CardId)) &&
                    materials.All(e => e.From == CardLocation.Hand(0)) && native.PhysicalCards.All(c => c.From == CardLocation.Hand(0)) &&
                    neutral.E<MouZhaJiangResponsePreparedEvent>().Single(e => e.OwnerFrameId == admitted.OwnerFrameId && e.Index == admitted.Index) is { ActorSeat: 0, Kind: CardKind.Dodge, MaterialCount: 2 },
                    "With the real ZhaJiang capability loaded, Xiewei freezes both original Hand materials before depositing its bait; its accepted Response adopts the same exact actor and materials without weakening admission validation.");
            }
            neutral.Need(neutral.SampleObtains().Length == 0 && neutral.E<WuZhangFeiUseQualifiedEvent>().Length == 0 && neutral.E<WuZhangFeiUseStartedEvent>().Length == 0 &&
                neutral.E<WuZhangFeiUseRankCapturedEvent>().Any(e => e.ActionId == output.ActionId && e.MaterialCount == 2 && e.RankSum == 0) &&
                (response ? neutral.E<CardResponseCompletedEvent>().Count(e => e.ActionId == output.ActionId && e.OriginalContinuation == ProgramCardContinuation.Dodge) == 1 :
                    neutral.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == output.UseId) == 1),
                "The source-specific neutral rank dominates its 9+9 provenance: neither active nor response Xiewei can qualify for a sample, and the real native continuation returns once without an invoice exception."); neutral.Cold();
        }
    }

    private static void DeclaredResponsesKeepOriginalRanksAndOnePaymentCold()
    {
        foreach (var kind in new[] { CardKind.Dodge, CardKind.Nullification })
        foreach (var alcoholIdentity in new[] { false, true })
        {
            var isCounter = kind == CardKind.Nullification;
            var s = Scene.Start(new("declared-" + kind + "-" + alcoholIdentity)
            {
                Declarations = true, DeclarationCostChild = !isCounter && !alcoholIdentity,
                AlcoholIdentity = alcoholIdentity,
                Hand = isCounter ? [C(CardKind.Dismantlement, 1), C(alcoholIdentity ? CardKind.Alcohol : CardKind.Slash, 6), C(CardKind.Duel, 1)]
                    : [C(alcoholIdentity ? CardKind.Alcohol : CardKind.Slash, 6)],
                PeerHands = [[], [C(CardKind.Slash, 2)], []]
            });
            var material = s.V().Hand.Single(card => card.Kind == (alcoholIdentity ? CardKind.Alcohol : CardKind.Slash)).Id;
            if (isCounter) { s.Native(CardKind.Dismantlement, [2]); s.Reach(() => s.P is { Kind: DecisionKind.Nullification }); }
            else s.IncomingSlash();
            s.Answer(choice => choice.Cards.SequenceEqual([material]) && choice.Parameters.GetValueOrDefault("conversion-skill-id") == "classic:guhuo");
            var declared = s.G.ResolutionStack.OfType<CardDeclarationFrame>().Single();
            var originalRank = alcoholIdentity ? 13 : 6;
            s.Need(declared.Payment is { OriginalRank: var rank, BianFuRenPaymentSequence: not null } && rank == originalRank &&
                declared.Payment.ReturnPurpose == (isCounter ? CardDeclarationPurpose.Counterspell : CardDeclarationPurpose.Dodge) &&
                declared.Payment.Cost.From == CardLocation.Hand(0) && declared.Payment.Cost.CardId == material,
                "A real declaration freezes its original owned rank and exact physical entry before leaving Hand; a king-identity Alcohol is thirteen, not its printed six.");
            if (!isCounter && !alcoholIdentity)
            {
                s.Reach(() => s.Hold("declaration-child")); s.Cold(); s.Continue();
            }
            s.Reach(() => s.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame);
            while (s.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame) s.AnswerDeclaration("pass");
            if (!alcoholIdentity) { s.ReachSample(); s.SelectSample(); }
            else s.Play();
            var action = s.E<CardActionAcceptedEvent>().Select(e => e.Action).Single(a => a.Type == CardActionType.Response &&
                a.ActorSeat == 0 && a.EffectiveKind == kind && a.PhysicalCards.Any(cost => cost.CardId == material));
            s.Need(action.ProviderSeat == 0 && action.ConversionChain.Contains(declared.Payment.Source) &&
                action.PhysicalCards.Single() == declared.Payment.Cost && action.EffectiveRank == originalRank &&
                s.E<WuZhangFeiUseRankCapturedEvent>().Single(e => e.ActionId == action.ActionId).RankSum == originalRank &&
                s.E<WuZhangFeiResponseRankBoundEvent>().Count(e => e.ActionId == action.ActionId) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay") == 1 &&
                !s.G.CardMovements.Any(m => m.CardId == material && (m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification)) &&
                s.PaidOnce(material) && s.SampleObtains().Length == (alcoholIdentity ? 0 : 1),
                "Declared Dodge/Nullification adopts exactly one real prepaid entity and original rank, binds once, and never manufactures a second native response payment.");
            s.Cold();
            if (isCounter && !alcoholIdentity)
            {
                var duelId = s.V().Hand.Single(card => card.Kind == CardKind.Duel).Id;
                s.Native(CardKind.Duel, [2]);
                var native = s.G.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == duelId);
                s.Need(native.Action is { Type: CardActionType.Use, ActorSeat: 0, ProviderSeat: 0 } used &&
                    used.EffectiveKind == CardKind.Duel && used.ConversionChain.Count == 0 &&
                    used.PhysicalCards is [var cost] && cost.CardId == duelId && cost.From == CardLocation.Hand(0),
                    "An explicitly selected native Duel after a declared Nullification has an empty source chain; it must not inherit the earlier Guhuo response source.");
                s.Play();
                s.Need(s.PaidOnce(duelId) && s.G.CardMovements.Count(move => move.CardId == duelId &&
                    move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                    s.E<CardUseFinishedEvent>().Count(finish => finish.ResolutionId == native.Id &&
                        finish.CardId == duelId && finish.CardKind == CardKind.Duel) == 1,
                    "The genuine native follow-up Duel pays its own entity once and completes its existing native use once.");
                s.Cold();
            }
        }

        var failed = Scene.Start(new("declared-failed") { Declarations = true, Hand = [C(CardKind.Slash, 6)] });
        var failedMaterial = failed.V().Hand.Single().Id;
        failed.IncomingSlash();
        failed.Answer(choice => choice.Cards.SequenceEqual([failedMaterial]) && choice.Parameters.GetValueOrDefault("conversion-skill-id") == "classic:guhuo");
        failed.Reach(() => failed.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame);
        failed.Cold(); failed.AnswerDeclaration("challenge"); failed.Play();
        failed.Need(failed.E<CardDeclarationRevealedEvent>() is [{ Succeeded: false }] &&
            failed.E<WuZhangFeiResponseRankBoundEvent>().Length == 0 && failed.SampleObtains().Length == 0 &&
            !failed.E<CardActionAcceptedEvent>().Any(e => e.Action.PhysicalCards.Any(cost => cost.CardId == failedMaterial)) &&
            failed.G.CardMovements.Count(m => m.CardId == failedMaterial && m.Reason.Value == "conversion.declaration.failed") == 1 &&
            failed.PaidOnce(failedMaterial),
            "A genuinely challenged false declaration consumes its existing material once and never binds a response rank or obtains a Zisheng sample.");
        failed.Cold();
    }

    private static void DeclaredCounterspellGroupAndProactiveRecoveryCold()
    {
        foreach(var trick in new[]{CardKind.DrawTwo,CardKind.ArrowBarrage})
        {
            var s=Scene.Start(new("declared-group-"+trick){Declarations=true,Hand=[C(trick,1),C(CardKind.Slash,6),C(CardKind.Peach,6)]});
            var material=s.V().Hand.Single(c=>c.Kind==CardKind.Slash).Id;
            s.Use("hp-loss",[0]);s.Play();var wounded=s.V().Hp;
            var use=s.G.GetHumanLegalActions().First(a=>a.CardId is{}id&&s.V().Hand.Any(c=>c.Id==id&&c.Kind==trick)&&a.ConversionSource is null&&a.Kind!=LegalActionKind.Recast);
            s.Accept(new PlayCardCommand(0,use.CardId!.Value,use.TargetSeats,s.G.Revision,s.P!.PromptId,use.PlayedCardKind));
            s.Reach(()=>s.P is{Kind:DecisionKind.Nullification});
            s.Answer(c=>c.Cards.SequenceEqual([material])&&c.Parameters.GetValueOrDefault("conversion-skill-id")=="classic:guhuo");
            s.Reach(()=>s.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame);s.Cold();
            while(s.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame)s.AnswerDeclaration("pass");
            s.ReachSample();s.SelectSample();s.Play();
            var response=s.E<CardActionAcceptedEvent>().Select(e=>e.Action).Single(a=>a.Type==CardActionType.Response&&a.PhysicalCards.Any(c=>c.CardId==material));
            s.Need(response.EffectiveKind==CardKind.Nullification&&response.EffectiveRank==6&&s.PaidOnce(material)&&
                s.E<WuZhangFeiResponseRankBoundEvent>().Count(e=>e.ActionId==response.ActionId)==1,
                "A declared counterspell retains its exact original rank and prepaid receipt when answering an implicit-self or multi-target group trick.");s.Cold();
            var nativePeach=s.G.GetHumanLegalActions().First(a=>a.Kind==LegalActionKind.Peach&&a.ConversionSource is null);var peachId=nativePeach.CardId!.Value;
            s.Accept(new PlayCardCommand(0,peachId,nativePeach.TargetSeats,s.G.Revision,s.P!.PromptId,nativePeach.PlayedCardKind));s.ReachSample();
            var usedPeach=s.G.ResolutionStack.OfType<CardUseFrame>().Single(f=>f.CardId==peachId).Action!;
            s.Need(usedPeach.Type==CardActionType.Use&&usedPeach.ActorSeat==0&&usedPeach.ProviderSeat==0&&usedPeach.ConversionChain.Count==0&&
                usedPeach.PhysicalCards is[{CardId:var paidPeach,From:var peachFrom}]&&paidPeach==peachId&&peachFrom==CardLocation.Hand(0),
                "The explicitly ordinary rank-six Peach pauses at its actual Zisheng Use boundary with its own physical invoice and an empty conversion chain.");s.Cold();s.SelectSample();s.Play();
            s.Need(s.V().Hp==wounded+1&&s.PaidOnce(peachId)&&s.E<CardDeclarationCommittedEvent>().Length==1,
                "The next explicitly ordinary Peach overrides the old Guhuo response source before recovery declaration lookup and native Use capture; it never invents another declaration or foreign conversion.");s.Cold();
        }
        var recovery=Scene.Start(new("declared-proactive-recovery"){Declarations=true,Hand=[C(CardKind.Slash,6)]});
        var cost=recovery.V().Hand.Single().Id;recovery.Use("hp-loss",[0]);recovery.Play();var before=recovery.V().Hp;
        var peach=recovery.G.GetHumanLegalActions().First(a=>a.CardId==cost&&a.PlayedCardKind==CardKind.Peach&&a.ConversionSource?.SkillId=="classic:guhuo");
        recovery.Accept(new PlayCardCommand(0,cost,peach.TargetSeats,recovery.G.Revision,recovery.P!.PromptId,peach.PlayedCardKind){ConversionSource=peach.ConversionSource});
        recovery.Reach(()=>recovery.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame);
        recovery.Need(recovery.G.ResolutionStack.OfType<CardDeclarationFrame>().Single().Return is{Purpose:CardDeclarationPurpose.Use,ParentFrameId:0,DyingResponse:null,UseAction.PlayedCardKind:CardKind.Peach},
            "The real proactive Peach declaration has no suspended response parent to borrow for its true Use.");recovery.Cold();
        while(recovery.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame)recovery.AnswerDeclaration("pass");
        recovery.ReachSample();recovery.SelectSample();recovery.Play();
        recovery.Need(recovery.V().Hp==before+1&&recovery.PaidOnce(cost)&&
            recovery.E<CardUseFinishedEvent>().Count(e=>e.CardId==cost&&e.CardKind==CardKind.Peach)==1&&
            !recovery.G.ResolutionStack.OfType<CardDeclarationFrame>().Any(),
            "An accepted proactive recovery transfers its original declaration directly to the genuine native Peach Use, restores one HP and finishes with one physical payment.");recovery.Cold();

        var foreign=Scene.Start(new("declared-foreign-native-counter"){Declarations=true,Hand=[C(CardKind.Dismantlement,1),C(CardKind.Slash,6)],
            PeerHands=[[C(CardKind.Nullification,1)],[C(CardKind.Slash,2)],[]]});
        var nullId=foreign.V(1).Hand.Single().Id;var ownMaterial=foreign.V().Hand.Single(c=>c.Kind==CardKind.Slash).Id;
        foreign.Native(CardKind.Dismantlement,[2]);foreign.Reach(()=>foreign.P is{Kind:DecisionKind.Nullification});
        foreign.Answer(c=>c.Cards.SequenceEqual([ownMaterial])&&c.Parameters.GetValueOrDefault("conversion-skill-id")=="classic:guhuo");
        foreign.Reach(()=>foreign.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame);
        while(foreign.G.ResolutionStack.LastOrDefault() is CardDeclarationChallengeFrame)foreign.AnswerDeclaration("pass");
        foreign.ReachSample();foreign.SelectSample();foreign.Play();
        var ordinary=foreign.E<CardActionAcceptedEvent>().Select(e=>e.Action).Single(a=>a.Type==CardActionType.Response&&a.ActorSeat==1&&a.PhysicalCards.Any(c=>c.CardId==nullId));
        foreign.Need(ordinary.ProviderSeat==1&&ordinary.EffectiveKind==CardKind.Nullification&&ordinary.ConversionChain.Count==0&&
            foreign.E<CardDeclarationCommittedEvent>().All(e=>e.OwnerSeat==0)&&foreign.G.CardMovements.Count(m=>m.CardId==nullId&&m.From==CardLocation.Hand(1)&&m.To==CardLocation.Processing&&m.Reason==CardMoveReasons.Nullification)==1&&
            foreign.G.CardMovements.Count(m=>m.CardId==nullId&&m.From==CardLocation.Processing&&m.To==CardLocation.DiscardPile&&m.Reason==CardMoveReasons.NullificationFinished)==1,
            "The next real AI responder's ordinary counterspell owns its own paid material and empty source chain, rather than inheriting the prior declarer's Guhuo skill instance.");foreign.Cold();
    }

    private static void ActualAiGroupMultiDodgeUsesItsCompletePublishedSelectionCold()
    {
        var s=Scene.Start(new("multi-group-ai"){Hand=[C(CardKind.ArrowBarrage,1)],PeerHands=[[],[C(CardKind.Slash,9),C(CardKind.Slash,9)],[]],PeerHp=[12,1,12]});
        var use=s.G.GetHumanLegalActions().First(a=>a.Kind==LegalActionKind.ArrowBarrage&&a.ConversionSource is null);
        s.Accept(new PlayCardCommand(0,use.CardId!.Value,use.TargetSeats,s.G.Revision,s.P!.PromptId,use.PlayedCardKind));
        s.Reach(()=>s.G.CreateSnapshot(2).PendingDecision is{Kind:DecisionKind.RespondDodge});
        var prompt=s.G.CreateSnapshot(2).PendingDecision!;var materials=s.V(2).Hand.Select(c=>c.Id).ToArray();
        s.Need(prompt.Choices.Any(c=>c.Parameters.GetValueOrDefault("response")=="extended-view-as"&&c.Cards.SequenceEqual(materials))&&
            !prompt.Choices.Any(c=>c.Cards.Count==1),"The actual NPC group defender publishes only its complete two-card Dodge conversion and no standalone material choice.");s.Cold();
        s.Accept(new AdvanceOneStepCommand(s.G.Revision));s.Play();
        var action=s.E<CardActionAcceptedEvent>().Select(e=>e.Action).Single(a=>a.Type==CardActionType.Response&&a.ActorSeat==2);
        s.Need(action.EffectiveKind==CardKind.Dodge&&action.PhysicalCards.Select(c=>c.CardId).SequenceEqual(materials)&&
            materials.All(id=>s.G.CardMovements.Count(m=>m.CardId==id&&m.From==CardLocation.Hand(2)&&m.To==CardLocation.Processing)==1&&
                s.G.CardMovements.Count(m=>m.CardId==id&&m.From==CardLocation.Processing&&m.To==CardLocation.DiscardPile)==1)&&s.V(2).Hp==1&&
            !s.G.AcceptedCommands.OfType<AnswerPromptCommand>().Any(c=>c.PromptId==prompt.PromptId),
            "One actual AI advance selects and pays the full published multi-Dodge against the group attack; it never parses its first material as an independent response or fabricates an NPC answer.");s.Cold();
    }

    public static void CausalDiscardObtainVictimDamageAndOxSpillCold()
    {
        foreach (var kind in new[] { CardKind.Snatch, CardKind.Dismantlement })
        {
            var s = Scene.Start(new("direct-" + kind) { Hand = [C(kind, 1)], PeerHands = [[C(CardKind.Slash, 2)], [], []] });
            var victimCard = s.V(1).Hand.Single().Id; var original = s.V().Hand.Single().Id; var hp = s.V(1).Hp;
            s.Native(kind, [1]); s.ReachMovement(); s.Private(); s.RejectPrompt(); s.Cold(); s.AcceptMovement(); s.Play();
            var destination = kind == CardKind.Snatch ? CardLocation.Hand(0) : CardLocation.DiscardPile;
            s.Need(s.V(1).Hp == hp - 1 && s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == victimCard && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == victimCard && m.From == CardLocation.Processing && m.To == destination) == 1 && s.PaidOnce(original),
                "The native actor's one genuine direct discard/obtain causes one optional real damage and returns to its once-paid parent Use."); s.Cold();
            s.Need(s.E<WuZishengMovementQualifiedEvent>().Single().Qualification is { VictimSeat: 1, Materials.Count: 1 } qualification &&
                qualification.Source.Source.OwnerSeat == 0 && qualification.Materials.Single().CardId == victimCard &&
                qualification.Materials.Single().OriginalFrom == CardLocation.Hand(1) && s.G.CardMovements.Single(m => m.Sequence == qualification.Materials.Single().TerminalSequence) is { } terminal && terminal.CardId == victimCard && terminal.To == destination,
                "The actual movement qualification retains the original direct entity and executor source rather than guessing from a movement reason.");
        }

        var ox = Scene.Start(new("ox-spill") { Hand = [C(CardKind.Dismantlement, 1), C(CardKind.Slash, 2), C(CardKind.Slash, 4)], Equipment = [C(CardKind.WoodenOx, 1)] });
        var weapon = ox.V().Equipment.Single().Id; var grain = ox.V().Hand.Where(c => c.Kind == CardKind.Slash).Select(c => c.Id).ToArray();
        ox.Use("store", cards: [grain[0]]); ox.Play();
        ox.Accept(new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grain[1]], [1], ox.G.Revision, ox.P!.PromptId)); ox.Play();
        ox.Need(ox.V(1).Equipment.Any(c => c.Id == weapon) && grain.All(id => ox.G.CreateCardZoneDiagnostics().Any(c => c.CardId == id && c.Location.Zone == CardZoneKind.WoodenOxGrain && c.Location.OwnerSeat == 1)),
            "The genuine equipment transfer carries both real stored Ox entities to the other owner before native removal.");
        ox.Native(CardKind.Dismantlement, [1], weapon); ox.ReachMovement(); ox.Cold(); ox.AcceptMovement(); ox.Play();
        ox.Need(ox.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
            ox.E<DamageAppliedEvent>().All(e => e.Amount != 3) && grain.All(id => ox.G.CardMovements.Count(m => m.CardId == id && m.From.Zone == CardZoneKind.WoodenOxGrain && m.To == CardLocation.DiscardPile) == 1) &&
            ox.G.CardMovements.Count(m => m.CardId == weapon && m.From == CardLocation.Equipment(1) && m.To == CardLocation.Processing) == 1 &&
            ox.G.CardMovements.Count(m => m.CardId == weapon && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "Only the owner's primary native Ox discard counts: both collateral grain spills still happen once without inflating retaliation to three."); ox.Cold();
        ox.Need(ox.E<WuZishengMovementQualifiedEvent>().Single().Qualification.Materials is [var primary] &&
            primary is { Discard: true, DiscardInvocationId: not null } && primary.CardId == weapon && primary.OriginalFrom == CardLocation.Equipment(1),
            "A genuine native logical-discard primary receipt attributes only the removed foreign Ox, never its spill children.");

        var unrelated = Scene.Start(new("unrelated-own-discard") { Hand = [C(CardKind.Slash, 1)] });
        unrelated.Use("discard", cards: unrelated.V().Hand.Select(c => c.Id).ToArray()); unrelated.Play();
        unrelated.Need(unrelated.E<DamageAppliedEvent>().Length == 0 && !unrelated.E<ProgramBindingResolvedEvent>().Any(e => e.SkillId == Zisheng && e.BindingId == "retaliate-foreign-cards" && e.Activated),
            "Discarding the owner's own real entity is not discarding another character's card and cannot fabricate a victim."); unrelated.Cold();

        var ancestor = Scene.Start(new("ancestor-dying-gift") { Hand = [C(CardKind.Slash, 1), C(CardKind.Peach, 1)], Equipment = [C(CardKind.ZhangbaSerpentSpear, 1)],
            PeerHands = [[C(CardKind.Slash, 2)], [], []], PeerHp = [12, 1, 12], AncestorGift = true });
        var attackCard = ancestor.V().Hand.Single(c => c.Kind == CardKind.Slash).Id; var gifted = ancestor.V(1).Hand.Single().Id;
        ancestor.Native(CardKind.Slash, [2]); ancestor.ReachMovement(); var originalDying = ancestor.G.ResolutionStack.OfType<DyingFrame>().Single(f => f.VictimSeat == 2);
        var originalUse = ancestor.E<CardUseDeclaredEvent>().Single(e => e.SourceSeat == 0 && e.CardId == attackCard).ResolutionId;
        var giftQualification = ancestor.E<WuZishengMovementQualifiedEvent>().Single().Qualification;
        var giftMaterial = giftQualification.Materials.Single();
        ancestor.Need(ancestor.V(1).Hp == 12 && ancestor.G.CardMovements.Count(m => m.CardId == gifted && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing) == 1 &&
            ancestor.G.CardMovements.Count(m => m.CardId == gifted && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0)) == 1 &&
            giftQualification.Source.Source.OwnerSeat == 0 && giftQualification.VictimSeat == 1 && giftMaterial.CardId == gifted && giftMaterial.OriginalFrom == CardLocation.Hand(1) && !giftMaterial.Discard &&
            ancestor.G.CardMovements.Single(m => m.Sequence == giftMaterial.TerminalSequence) is { } giftTerminal &&
            giftTerminal.CardId == gifted && giftTerminal.From == CardLocation.Processing && giftTerminal.To == CardLocation.Hand(0),
            "A living peer really gives its own entity in the original native DyingEntering window before Zisheng retaliation is accepted."); ancestor.Private(); ancestor.Cold(); ancestor.AcceptMovement();
        ancestor.Reach(() => ancestor.Hold("damage-child") && ancestor.G.ResolutionStack.OfType<DamageFrame>().Any(f => f.TargetSeat == 1));
        var suspension = ancestor.E<WuZishengSuspendedDyingIssuedEvent>().Single();
        ancestor.Need(suspension.Suspension.DyingFrameId == originalDying.Id && suspension.Suspension.AttackOwnerFrameId == originalUse &&
            ancestor.G.ResolutionStack.OfType<DyingFrame>().Single(f => f.Id == originalDying.Id).VictimSeat == 2 &&
            ancestor.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 && ancestor.E<DyingResolvedEvent>().Length == 0,
            "The exact original Dying token stays on its owning stack while one newly accepted movement damage resolves against the living giver."); ancestor.Cold(); ancestor.Continue();
        ancestor.Reach(() => ancestor.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        ancestor.Need(ancestor.E<WuZishengSuspendedDyingReturnedEvent>().Single() is { DyingFrameId: var returned, AttackOwnerFrameId: var attack } &&
            returned == originalDying.Id && attack == originalUse && ancestor.G.ResolutionStack.OfType<DyingFrame>().Any(f => f.Id == originalDying.Id && f.VictimSeat == 2),
            "Returning the retaliation restores the original native attack and rescue cursor, rather than resolving or replacing the ancestor Dying."); ancestor.Cold();
        var rescue = ancestor.V().Hand.Single(c => c.Kind == CardKind.Peach).Id; ancestor.Respond(rescue); ancestor.Play();
        ancestor.Need(ancestor.PaidOnce(attackCard) && ancestor.PaidOnce(rescue) && ancestor.V(2).IsAlive && ancestor.V(2).Hp == 1 &&
            ancestor.E<DyingResolvedEvent>().Count(e => e.ResolutionId == originalDying.Id && e.VictimSeat == 2 && e.Survived) == 1 &&
            ancestor.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == originalUse) == 1 && ancestor.E<WuZishengSuspendedDyingReturnedEvent>().Length == 1 &&
            ancestor.G.CardMovements.Count(m => m.CardId == gifted && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing) == 1 &&
            ancestor.G.CardMovements.Count(m => m.CardId == gifted && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0)) == 1 &&
            ancestor.E<WuZishengMovementQualifiedEvent>().Single().Qualification.Materials.Single() == giftMaterial &&
            ancestor.G.CardMovements.Count(m => m.Sequence == giftMaterial.TerminalSequence && m.CardId == gifted && m.To == CardLocation.Hand(0)) == 1,
            "The ancestor's actual Peach rescue and Slash completion finish once after the nested damage returns, without paying either original material or the gifted entity twice."); ancestor.Cold();
    }

    public static void PrivateActualTurnRankCyclesPhaseBudgetsAndResetCold()
    {
        foreach (var count in new[] { 3, 4, 6, 7, 9, 10 })
        {
            var s = Scene.Start(new("strict-gate-" + count) { PeerHands = [Ranks(1, count), [], []] });
            s.Use("damage", [1]); s.Play(); var before = s.V().HandCount; s.Observe(1); s.Play();
            s.Need(s.V().HandCount - before == new[] { 4, 7, 10 }.Count(g => count >= g) * 3 && s.CycleInt("CompletionTarget") == 13,
                "Distinct ranks pay each strict 4/7/10 gate once; stopping on 3/6/9 does not prematurely pay the next gate."); s.Cold();
        }

        var cycle = Scene.Start(new("cycle-reset") { Hand = [C(CardKind.Slash, 11), C(CardKind.Slash, 12), C(CardKind.Slash, 13)],
            PeerHands = [Ranks(1, 3), Ranks(4, 4), Ranks(8, 3)] });
        cycle.Activate(Haoxian, "collect-rank-three"); cycle.Play(); var three = cycle.V().Hand.Single(c => c.Rank == 3).Id;
        cycle.Use("give", [1], [three]); cycle.Play();
        cycle.Need(!cycle.Can(Haoxian, "collect-rank-three"), "The real first HaoXian consumes its stable game usage even after a gathered card is genuinely given back.");
        foreach (var seat in new[] { 1, 2, 3 }) { cycle.Use("damage", [seat]); cycle.Play(); cycle.Observe(seat); cycle.Play(); }
        cycle.Need(cycle.CycleInt("RankMask") == (1 << 10) - 1 && !cycle.Can(Xianlue, "observe-ranks", 1) && !cycle.Can(Xianlue, "observe-ranks", 2) &&
            cycle.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 9,
            "Three different actual phase targets accumulate ten distinct ranks and pay exactly three Draw3 gates without permitting a repeat target.");
        var extra = cycle.V().Hand.Where(c => c.Rank is 11 or 12 or 13).Select(c => c.Id).ToArray(); cycle.Use("give", [1], extra); cycle.Play();
        var turn = cycle.G.State.TurnNumber; cycle.End(); cycle.Reach(() => cycle.G.State.TurnNumber > turn && cycle.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        cycle.Need(!cycle.Can(Xianlue, "observe-ranks", 1), "A new turn does not retain last turn's target eligibility merely because its phase budget renewed.");
        cycle.Use("damage", [1]); cycle.Play(); cycle.Observe(1); cycle.Play();
        cycle.Need(cycle.CycleInt("RankMask") == 0 && cycle.CycleInt("PaidGates") == 0 && cycle.CycleInt("CompletionTarget") == 13 && cycle.Can(Haoxian, "collect-rank-three") &&
            cycle.NamedEvents("WuZhangFeiHaoxianResetEvent").Length == 1,
            "All thirteen genuinely observed ranks clear exactly the completed cycle and reset actual stable HaoXian usage once."); cycle.Cold();
        cycle.Activate(Haoxian, "collect-rank-three"); cycle.Play(); cycle.Need(!cycle.Can(Haoxian, "collect-rank-three"), "The reset enables exactly one new real limited activation.");

        var eligibility = Scene.Start(new("eligibility") { PeerHands = [Ranks(1, 2), Ranks(4, 2), Ranks(8, 2)] });
        eligibility.Use("hp-loss", [1]); eligibility.Play(); eligibility.Need(!eligibility.Can(Xianlue, "observe-ranks", 1), "Direct HP Loss alone is not damage or a card-loss eligibility fact.");
        eligibility.Use("armor", [2]); eligibility.Play(); eligibility.Use("damage", [2]); eligibility.Play();
        eligibility.Need(eligibility.V(2).Hp == 12 && eligibility.Can(Xianlue, "observe-ranks", 2) &&
            eligibility.E<DamageAppliedEvent>().Single(e => e.TargetSeat == 2) is { Amount: 1, ArmorPayment: { ArmorBefore: 2, ArmorAbsorbed: 1, ArmorAfter: 1, HpLossAmount: 0 } },
            "A fully Armor-absorbed actual damage still qualifies even though its target loses no HP."); eligibility.Observe(2); eligibility.Play(); eligibility.Cold();

        var lost = Scene.Start(new("loss-eligibility") { Hand = [C(CardKind.Snatch, 1)], PeerHands = [Ranks(1, 2), [], []] });
        lost.Native(CardKind.Snatch, [1]); lost.Play();
        lost.Need(lost.E<DamageAppliedEvent>().Length == 0 && lost.Can(Xianlue, "observe-ranks", 1),
            "A real foreign Hand loss qualifies independently of damage even when its separate Zisheng optional retaliation is declined."); lost.Observe(1); lost.Play(); lost.Cold();

        var dying = Scene.Start(new("dying-cycle") { Hand = [C(CardKind.Peach, 1)], PeerHands = [Ranks(1, 10), [], []], PeerHp = [12, 1, 12] });
        dying.Use("damage", [1]); dying.Play(); dying.Observe(1); dying.Play(); dying.Use("damage", [2]);
        dying.Reach(() => dying.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 }); dying.Cold(); var peach = dying.V().Hand.First(c => c.Kind == CardKind.Peach).Id; dying.Respond(peach); dying.Play();
        dying.Need(dying.E<PlayerDyingEvent>().Count(e => e.VictimSeat == 2 && e.KillerSeat == 0) == 1 && dying.PaidOnce(peach) && dying.CycleInt("RankMask") == 0 &&
            dying.CycleInt("CompletionTarget") == 13 && dying.NamedEvents("WuZhangFeiHaoxianResetEvent").Length == 1,
            "An exact native owner damage-to-Dying lowers the completion target from thirteen to ten and immediately clears the already-ten-rank cycle once, with actual rescue paid once."); dying.Cold();

        var nested = Scene.Start(new("draw-child-cycle") { Hand = [C(CardKind.Peach, 1)], PeerHands = [Ranks(1, 10), [], []], PeerHp = [12, 1, 12], DrawChildDamage = true });
        nested.Use("damage", [1]); nested.Play(); nested.Observe(1); nested.Reach(() => nested.Hold("draw-child"));
        var mother = nested.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.WuZhangFeiXianlue is not null);
        var origin = mother.WuZhangFeiXianlue!.OriginCycleId;
        nested.Need(mother.WuZhangFeiXianlue.EarnedGates == 7 && nested.CycleInt("RankMask") == 1023 && nested.CycleInt("PaidGates") == 7 &&
            nested.E<WuZhangFeiXianlueDrawIssuedEvent>().Length == 1 && nested.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 3,
            "All three gates are claimed on the original observation before the first genuine Draw3 gain child can cause another rule event."); nested.Cold(); nested.Continue();
        nested.Reach(() => nested.P?.Choices.Any(c => nested.IsAction(c, "select-target")) == true); nested.Answer(c => c.Targets.SequenceEqual([2]));
        nested.Reach(() => nested.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        nested.Need(nested.CycleInt("RankMask") == 0 && nested.E<WuZhangFeiHaoxianResetEvent>().Single().CompletedCycleId == origin &&
            nested.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == mother.Id).WuZhangFeiXianlue!.OriginCycleId == origin,
            "The first draw's real owner Damage-to-Dying independently clears the current cycle while its observing mother keeps the paid origin identity.");
        nested.Cold(); var rescue = nested.V().Hand.Single(c => c.Kind == CardKind.Peach).Id; nested.Respond(rescue); nested.Play();
        nested.Need(nested.E<WuZhangFeiXianlueDrawIssuedEvent>().Select(e => (e.OriginCycleId, e.Gate)).SequenceEqual(new[] { (origin, 0), (origin, 1), (origin, 2) }) &&
            nested.E<WuZhangFeiXianlueDrawReturnedEvent>().Count(e => e.OriginCycleId == origin && e.ActualCount == 3) == 3 &&
            nested.E<WuZhangFeiXianlueCompletedEvent>().Count(e => e.FrameId == mother.Id && e.OriginCycleId == origin) == 1 &&
            nested.E<WuZhangFeiHaoxianResetEvent>().Length == 1 && nested.E<WuZhangFeiRankCycleChangedEvent>().Last().CycleId == origin + 1 &&
            nested.CycleInt("RankMask") == 0 && nested.CycleInt("PaidGates") == 0 && nested.CycleInt("CompletionTarget") == 13 && nested.PaidOnce(rescue),
            "The original paid mother drains all three actual Draw3 awards after native rescue without clearing the new cycle, replaying a gate or resetting HaoXian twice."); nested.Cold();
    }

    public static void HaoxianRealShuffleMultiOwnerGainNativeDyingAndCold()
    {
        var s = Scene.Start(new("haoxian") { Hand = [C(CardKind.Dodge, 3), C(CardKind.Slash, 4), C(CardKind.Peach, 1)],
            PeerHands = [[C(CardKind.Slash, 3), C(CardKind.Slash, 3), C(CardKind.Slash, 5)], [C(CardKind.Slash, 3), C(CardKind.Slash, 6)], [C(CardKind.Slash, 3), C(CardKind.Slash, 7)]],
            PeerHp = [1, 12, 12], RankThreeStock = 0 });
        var three = s.V().Hand.Single(c => c.Rank == 3).Id; var nonThree = s.V().Hand.Single(c => c.Rank == 4).Id;
        var originalHands = Enumerable.Range(1, 3).ToDictionary(seat => seat, seat => s.V(seat).Hand.ToArray());
        s.Use("discard", cards: [three, nonThree]); s.Play(); var oldDeck = s.DeckIds(); var peach = s.V().Hand.Single().Id;
        s.Activate(Haoxian, "collect-rank-three"); s.ReachMovement(); s.Private(); s.RejectPrompt(); s.Cold();
        var shuffled = s.DeckIds();
        var shuffle = s.E<WuHaoxianShuffledEvent>().Single(); s.Immutable(shuffle.BeforeOrder); s.Immutable(shuffle.AfterOrder);
        s.Need(shuffled.Length == oldDeck.Length + 1 && shuffled.ToHashSet().SetEquals(oldDeck.Append(three)) &&
            !shuffled.Where(id => id != three).SequenceEqual(oldDeck) && s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == nonThree).Location == CardLocation.DiscardPile &&
            originalHands.All(pair => pair.Value.Where(c => c.Rank == 3).All(c => s.V().Hand.Any(h => h.Id == c.Id))),
            "HaoXian really merges only rank-three Discard into the whole existing deck and shuffles it before obtaining all other Hands' actual rank-three entities.");
        var paidPrompt = s.P!; s.AcceptMovement(); s.Reach(() => s.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 }); s.Cold(); s.Respond(peach);
        s.Reach(() => s.MovementOffer() || s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        while (s.MovementOffer()) { s.AcceptMovement(); s.Reach(() => s.MovementOffer() || s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }); }
        s.Play();
        s.Need(s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => (e.TargetSeat, e.Amount)).OrderBy(e => e.TargetSeat)
                .SequenceEqual(new[] { (1, 1), (2, 1), (3, 1) }) && s.V(1).IsAlive && s.V(1).Hp == 1 && s.PaidOnce(peach) &&
            originalHands.All(pair => pair.Value.All(c => s.G.CardMovements.Count(m => m.CardId == c.Id && m.From == CardLocation.Hand(pair.Key) && m.To == CardLocation.Hand(0)) == (c.Rank == 3 ? 1 : 0))) &&
            s.G.CardMovements.Count(m => m.CardId == three && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile) == 1 && !s.Can(Haoxian, "collect-rank-three"),
            "A true mixed-owner acquisition counts each victim separately and caps the two-card victim at current HP one; native damage/rescue and the parent limited activation return once without repeating collection.");
        s.RejectOld(paidPrompt); s.Cold();

        foreach (var fatal in new[] { false, true })
        {
            var paid = Scene.Start(new(fatal ? "paid-gain-death" : "paid-gain-source-loss") { ConditionalSources = true, GainChildLoss = fatal ? 4 : 3,
                PeerHands = [[C(CardKind.Slash, 3)], [], []], RankThreeStock = 0 });
            var entity = paid.V(1).Hand.Single().Id; paid.Activate(Haoxian, "collect-rank-three"); paid.Reach(() => paid.Hold("gain-child"));
            paid.Need(paid.V().Hand.Any(c => c.Id == entity), "The physical HaoXian gain has already paid before the genuine CardsGained source-change child."); paid.Cold(); paid.Continue();
            if (fatal) paid.Reach(() => !paid.V().IsAlive); else paid.Play();
            paid.Need(paid.G.CardMovements.Count(m => m.CardId == entity && m.From == CardLocation.Hand(1) && m.To == CardLocation.Hand(0)) == 1 &&
                paid.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Haoxian && e.UsageId == "collect-rank-three") == 1,
                "Source loss or native death after paid acquisition cannot replay a taken entity or restore the already-consumed limited usage."); paid.Cold();
            if (!fatal) { paid.Use("recover-two"); paid.Play(); paid.Need(!paid.Can(Haoxian, "collect-rank-three"), "A genuinely regranted source retains the stable game usage rather than gaining another HaoXian."); }
        }
    }

    private static string ReadBundle(string suffix)
    {
        using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.wumiao-zhang-fei." + suffix + ".json") ?? throw new InvalidOperationException("Missing complete Wu Zhang Fei bundle.");
        using var reader = new StreamReader(stream); return reader.ReadToEnd();
    }
    private static void ReservedLoaderRejectsTransplants()
    {
        foreach (var renameSource in new[] { true, false })
        {
            var rules = JsonNode.Parse(ReadBundle("rules"))!; var presentation = JsonNode.Parse(ReadBundle("presentation"))!;
            if (renameSource)
            {
                rules["skills"]!.AsArray().Single(n => n!["id"]!.GetValue<string>() == Zisheng)!["id"] = "fixture:transplanted-zisheng";
                var label = presentation["skills"]![Zisheng]!.DeepClone(); presentation["skills"]!.AsObject().Remove(Zisheng); presentation["skills"]!["fixture:transplanted-zisheng"] = label;
            }
            else rules["skills"]!.AsArray().Single(n => n!["id"]!.GetValue<string>() == Xianlue)!["activations"]![0]!["id"] = "incompatible-observation";
            var rejected = false;
            try { _ = SkillProgramCatalog.Load(rules.ToJsonString(), presentation.ToJsonString()); }
            catch (InvalidOperationException) { rejected = true; }
            if (!rejected) throw new InvalidOperationException("Reserved Wu Zhang Fei operations must reject a transplanted source or incompatible binding through their own composition guard.");
        }
    }

    private sealed record Spec(string Name)
    {
        public ShanheStartingCard[] Hand { get; init; } = [];
        public ShanheStartingCard[] Equipment { get; init; } = [];
        public ShanheStartingCard[][] PeerHands { get; init; } = [[], [], []];
        public int[] PeerHp { get; init; } = [12, 12, 12];
        public int RankThreeStock { get; init; } = 5;
        public bool ConditionalSources { get; init; }
        public int GainChildLoss { get; init; }
        public bool DrawChildDamage { get; init; }
        public bool XieweiSource { get; init; }
        public bool AncestorGift { get; init; }
        public bool Declarations { get; init; }
        public bool DeclarationCostChild { get; init; }
        public bool AlcoholIdentity { get; init; }
    }

    private sealed class Fixture(Spec spec) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wu-zhang-fei", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var production = SkillProgramCatalog.Load(ReadBundle("rules"), ReadBundle("presentation"));
            foreach (var (id, program) in production.Programs) b.AddSkill(new(id, production.Presentations[id].Name, production.Presentations[id].Description) { Program = program, ProgramPresentation = production.Presentations[id] });
            if (spec.Declarations) CharacterFixtureContent.Register(b, "classic:yu-ji", "ol:bian-fu-ren");
            if (spec.XieweiSource)
            {
                CharacterFixtureContent.Register(b, "mou:huang-gai-mobile");
                string ReadXiewei(string suffix) { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.ol-guan-hai." + suffix + ".json") ?? throw new InvalidOperationException("Missing real Xiewei source bundle."); using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
                var xiewei = SkillProgramCatalog.Load(ReadXiewei("rules"), ReadXiewei("presentation"));
                foreach (var (id, program) in xiewei.Programs) b.AddSkill(new(id, xiewei.Presentations[id].Name, xiewei.Presentations[id].Description) { Program = program, ProgramPresentation = xiewei.Presentations[id] });
            }
            foreach (var kind in new[] { CardKind.ZhangbaSerpentSpear, CardKind.SilverLion, CardKind.WoodenOx })
            { var card = CardCatalog.Get(kind); b.AddCard(new("fixture:wzf-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            static JsonNode N(string json) => JsonNode.Parse(json)!;
            static JsonObject A(string id, string effects, int targets = 0, int cards = 0, int? max = null) => new()
            { ["id"] = id, ["minCards"] = cards, ["maxCards"] = max ?? cards, ["minTargets"] = targets, ["maxTargets"] = targets, ["targetKind"] = "anyLiving", ["usesPerTurn"] = null, ["effects"] = N("[" + effects + "]") };
            const string capture = "{\"op\":\"captureSelectedCards\",\"target\":\"owner\",\"resultBind\":\"paid\"}";
            var acts = new JsonArray(A("damage", "{\"op\":\"damage\",\"target\":\"selectedTarget\",\"amount\":1}", 1),
                A("hp-loss", "{\"op\":\"loseHp\",\"target\":\"selectedTarget\",\"amount\":1}", 1),
                A("armor", "{\"op\":\"grantArmor\",\"target\":\"selectedTarget\",\"amount\":2}", 1),
                A("recover-two", "{\"op\":\"recover\",\"target\":\"owner\",\"amount\":2}"),
                A("discard", capture + ",{\"op\":\"moveBoundCards\",\"target\":\"owner\",\"sourceBind\":\"paid\",\"destination\":\"discardPile\",\"awaitMovementTriggers\":true}", cards: 1, max: 20),
                A("give", capture + ",{\"op\":\"moveBoundCards\",\"target\":\"owner\",\"sourceBind\":\"paid\",\"destination\":\"selectedTargetHand\"}", 1, 1, 20),
                A("store", capture + ",{\"op\":\"moveBoundCards\",\"target\":\"owner\",\"sourceBind\":\"paid\",\"destination\":\"ownerPersistentZone\",\"destinationZone\":\"woodenOxGrain\"}", cards: 1, max: 2),
                A("incoming", "{\"op\":\"offerVirtualSlashOrDraw\",\"target\":\"selectedTarget\"}", 1));
            var duel = A("duel", "{\"op\":\"useSelectedActorDuel\",\"target\":\"owner\"}", 1); duel["usesPerPhase"] = 2; duel["targetKind"] = "otherLiving"; acts.Add(duel);
            var defs = new JsonArray(new JsonObject { ["id"] = Driver, ["revision"] = 1, ["activations"] = acts,
                    ["modifiers"] = N("[{\"id\":\"hold-stock\",\"query\":\"handLimit\",\"operation\":\"add\",\"value\":40,\"priority\":0}]") },
                N("{\"id\":\"" + Quiet + "\",\"revision\":1,\"triggers\":[{\"id\":\"quiet\",\"window\":\"afterNormalDraw\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"skipTurnPhases\",\"target\":\"owner\",\"phases\":[\"play\"]}]}]}"));
            if (spec.AlcoholIdentity) defs[0]!["cardPolicies"] = N("[{\"id\":\"original-alcohol-rank\",\"kind\":\"alcoholKingIdentityRank\",\"cardKinds\":[\"alcohol\"]}]");
            if (spec.DeclarationCostChild) defs.Add(N("{\"id\":\"" + Watch + "\",\"revision\":1,\"triggers\":[{\"id\":\"declaration-child\",\"window\":\"cardsMoved\",\"subject\":\"owner\",\"sourceZones\":[\"hand\"],\"movementReasons\":[\"conversion.declaration.pay\"],\"movementOccurrence\":\"perBatch\",\"optional\":false,\"effects\":[{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"held\",\"options\":[{\"id\":\"declaration-child\"}]},{\"op\":\"draw\",\"target\":\"owner\",\"amount\":1}]}]}"));
            if (spec.Name == "multi-response") defs[0]!["viewAs"] = N("[{\"id\":\"two-material-dodge\",\"inputKinds\":[],\"inputSuits\":[],\"sourceZones\":[\"hand\"],\"inputCount\":2,\"extendedUse\":true,\"outputKind\":\"dodge\",\"forPlay\":false,\"forResponse\":true}]");
            if (spec.Name == "multi-group-ai") defs.Add(N("{\"id\":\"fixture:wzf-peer-multi-dodge\",\"revision\":1,\"viewAs\":[{\"id\":\"actual-pair\",\"inputKinds\":[],\"inputSuits\":[],\"sourceZones\":[\"hand\"],\"inputCount\":2,\"extendedUse\":true,\"outputKind\":\"dodge\",\"forPlay\":false,\"forResponse\":true}]}"));
            if (spec.ConditionalSources) defs.Add(N("{\"id\":\"" + Conditional + "\",\"revision\":1,\"hpSkillGrants\":[" + string.Join(",", new[] { Zisheng, Xianlue, Haoxian }.Select((id, i) => "{\"id\":\"living-" + i + "\",\"skillId\":\"" + id + "\",\"minimumHp\":2}")) + "]}"));
            if (spec.GainChildLoss != 0) defs.Add(N("{\"id\":\"" + Watch + "\",\"revision\":1,\"triggers\":[{\"id\":\"paid-gain-child\",\"window\":\"cardsGained\",\"subject\":\"owner\",\"destinationZones\":[\"hand\"],\"movementReasons\":[\"skill-program.wumiao-zhang-fei.haoxian.obtain\"],\"movementOccurrence\":\"perBatch\",\"priority\":1000,\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"effects\":[{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"held\",\"options\":[{\"id\":\"gain-child\"}]},{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":" + spec.GainChildLoss + "}]}]}"));
            if (spec.DrawChildDamage) defs.Add(N("{\"id\":\"" + Watch + "\",\"revision\":1,\"triggers\":[{\"id\":\"first-rank-award-child\",\"window\":\"cardsGained\",\"subject\":\"owner\",\"destinationZones\":[\"hand\"],\"movementReasons\":[\"skill-program.wu-zhang-fei.xianlue.draw\"],\"movementOccurrence\":\"perBatch\",\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"priority\":1000,\"effects\":[{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"held\",\"options\":[{\"id\":\"draw-child\"}]},{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"otherLiving\"},{\"op\":\"damage\",\"target\":\"selectedTarget\",\"amount\":1}]}]}"));
            if (spec.AncestorGift)
            {
                defs.Add(N("{\"id\":\"" + DyingGift + "\",\"revision\":1,\"triggers\":[{\"id\":\"real-dying-entry-gift\",\"window\":\"dyingEntering\",\"subject\":\"any\",\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"effects\":[{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"eventSource\"},{\"op\":\"selectAndMoveOwnedCard\",\"target\":\"owner\",\"chooserRef\":{\"kind\":\"owner\"},\"cardOwnerRef\":{\"kind\":\"owner\"},\"zones\":[\"hand\"],\"count\":1,\"destination\":\"selectedTargetHand\",\"targetRef\":{\"kind\":\"selectedTarget\"},\"awaitMovementTriggers\":true}]}]}"));
                defs.Add(N("{\"id\":\"" + Watch + "\",\"revision\":1,\"triggers\":[{\"id\":\"real-native-damage-child\",\"window\":\"afterDamageApplied\",\"subject\":\"damageSource\",\"damageOccurrence\":\"perDamage\",\"optional\":false,\"effects\":[{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"held\",\"options\":[{\"id\":\"damage-child\"}]}]}]}"));
            }
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "固定起始牌的武张飞真实行为夹具",
                optionLabels = new[] { "activations", "triggers" }.SelectMany(k => n[k]?.AsArray() ?? new JsonArray())
                    .SelectMany(binding => binding!["effects"]!.AsArray()).SelectMany(effect => effect!["options"]?.AsArray() ?? new JsonArray())
                    .Select(option => option!["id"]!.GetValue<string>()).Distinct(StringComparer.Ordinal).ToDictionary(id => id, id => id, StringComparer.Ordinal) });
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, labels[id].name, labels[id].description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var own = new List<string> { Driver }; if (spec.ConditionalSources) own.Add(Conditional); else own.AddRange([Zisheng, Xianlue, Haoxian]); if (spec.XieweiSource) own.Add(Xiewei); if (spec.GainChildLoss != 0 || spec.DrawChildDamage || spec.AncestorGift) own.Add(Watch);
            if (spec.Declarations) own.Add("classic:guhuo");
            if (spec.DeclarationCostChild) own.Add(Watch);
            b.AddGeneral(new(Owner, "武张飞真实本人", "", "standard:none", "shu", 4, own));
            foreach (var id in new[] { Ally, EnemyA, EnemyB }) b.AddGeneral(new(id, "实际其他角色", "", "standard:none", "qun", 12, spec.Name == "multi-group-ai" && id == EnemyA ? [Quiet, "fixture:wzf-peer-multi-dodge"] : spec.AncestorGift && id == Ally ? [Quiet, DyingGift] : [Quiet]));
            b.AddDeck(new(Deck, "四十八张固定实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, i < spec.RankThreeStock ? 3 : 4)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "武张飞真实小局", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Scene(Spec spec, GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public JsonElement[] NamedEvents(string type) => G.Events.Where(e => e.Payload.GetType().Name == type).Select(e => JsonSerializer.SerializeToElement(e.Payload, e.Payload.GetType())).ToArray();
        public int CycleInt(string field) { var e = NamedEvents("WuZhangFeiRankCycleChangedEvent").LastOrDefault(e => e.GetProperty("OwnerSeat").GetInt32() == 0); Need(e.ValueKind != JsonValueKind.Undefined && e.TryGetProperty(field, out _), "A real source-bound cycle event must expose " + field); return e.GetProperty(field).GetInt32(); }
        public bool IsAction(PromptChoice c, string action) => c.Parameters.GetValueOrDefault("program-action") == action;
        public bool Can(string skill, string activation, int? target = null) => G.GetHumanLegalActions().Any(a => a.ProgramSkillId == skill && a.ProgramActivationId == activation &&
            (target is null || (a.TargetSeats.Count == 0 ? a.SelectableTargetSeats.Contains(target.Value) : a.TargetSeats.SequenceEqual([target.Value]))));
        public bool Hold(string id) => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public bool MovementOffer() => P?.SkillPrompt?.SkillId == Zisheng && P.Choices.Any(c => IsAction(c, "activate"));
        public void ReachMovement() => Reach(MovementOffer);
        public void AcceptMovement() { Answer(c => IsAction(c, "activate")); }
        public CardMovementRecord[] SampleObtains() => G.CardMovements.Where(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value.Contains("zisheng", StringComparison.Ordinal)).ToArray();
        public int[] DeckIds() => G.CreateCardZoneDiagnostics().Where(c => c.Location == CardLocation.DrawPile).OrderBy(c => c.ZoneIndex).Select(c => c.CardId).ToArray();
        public bool PaidOnce(int id) => G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 && G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1;
        public void Use(string id, int[]? targets = null, int[]? cards = null) => Activate(Driver, id, targets, cards);
        public void Activate(string skill, string id, int[]? targets = null, int[]? cards = null) => Accept(new UseProgramSkillCommand(0, skill, id, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public void Native(CardKind kind, int[] targets, int? targetCard = null)
        { var a = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId is { } id && V().Hand.Any(c => c.Id == id && c.Kind == kind) && a.ConversionSource is null && a.Kind != LegalActionKind.Recast && a.TargetSeats.SequenceEqual(targets) && (targetCard is null || a.TargetCardId == targetCard)); Need(a is not null, "A real published native " + kind + " plan must exist."); Accept(new PlayCardCommand(0, a!.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)); }
        public void ReachSample()
        {
            Reach(() => P?.Choices.Any(c => IsAction(c, "wu-zhang-fei-sample")) == true || P?.SkillPrompt?.SkillId == Zisheng && P.Choices.Any(c => IsAction(c, "activate")));
            if (!P!.Choices.Any(c => IsAction(c, "wu-zhang-fei-sample"))) Answer(c => IsAction(c, "activate"));
            Reach(() => P?.Choices.Any(c => IsAction(c, "wu-zhang-fei-sample")) == true); Private(); Cold();
        }
        public void SelectSample()
        { var p = P!; var selected = p.Choices.First(c => IsAction(c, "wu-zhang-fei-sample")); var id = selected.Cards.Single(); Answer(c => c.Id == selected.Id); Play(); Need(G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 1, "The canonical selected deck entity really reaches Hand once."); RejectOld(p); }
        public void Observe(int seat)
        {
            var hand = V(seat).Hand.Select(c => c.Id).ToArray(); var moves = G.CardMovements.Count; Activate(Xianlue, "observe-ranks", [seat]);
            Reach(() => P?.Choices.Any(c => IsAction(c, "wu-zhang-fei-observed")) == true); Private();
            var viewed = G.CreateSnapshot(0).PrivateRevealedCards ?? [];
            Need(viewed.Select(c => c.Id).ToHashSet().SetEquals(hand) && Enumerable.Range(1, 3).All(v => G.CreateSnapshot(v).PrivateRevealedCards?.Any(c => hand.Contains(c.Id)) != true) && G.CardMovements.Count == moves,
                "Only the observer sees the actual complete target Hand; observation neither publicly reveals nor moves entities.");
            Immutable(viewed); RejectPrompt(); Cold(); Answer(c => IsAction(c, "wu-zhang-fei-observed") && c.Cards.Count == 0 && c.Targets.SequenceEqual([seat]));
        }
        public void IncomingSlash() { Use("incoming", [1]); Reach(() => P?.Choices.Any(c => IsAction(c, "virtual-slash-offer") && c.Targets.SequenceEqual([0])) == true); Answer(c => IsAction(c, "virtual-slash-offer") && c.Targets.SequenceEqual([0])); Reach(() => P is { Kind: DecisionKind.RespondDodge }); }
        public void Respond(int id) { Private(); RejectPrompt(); Cold(); Answer(c => c.Cards.SequenceEqual([id]) && c.Parameters.GetValueOrDefault("response") is not ("pass" or "take-damage" or "let-die")); }
        public void Continue() => Answer(c => c.Parameters.ContainsKey("option-id"));
        public void AnswerDeclaration(string answer)
        {
            for(var step=0;step<20&&G.PendingDecision is null&&!Enumerable.Range(0,4).Any(seat=>G.CreateSnapshot(seat).PendingDecision is not null);step++)
                Accept(new AdvanceOneStepCommand(G.Revision));
            var prompt = G.PendingDecision ?? Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).First(p => p is not null);
            var choice = prompt!.Choices.Single(c => c.Parameters.GetValueOrDefault("declaration-answer") == answer);
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; var c = p.Choices.FirstOrDefault(match); Need(p.PlayerSeat == 0 && c is not null, "The owner chooses an actually published canonical option."); Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 220; i++)
            {
                if (done()) return; Need(G.State.Winner == Winner.None, "The bounded scene ended before boundary " + line); var p = P;
                if (p is null || p.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Kind == DecisionKind.PlayCard) End();
                else if (p.SkillPrompt?.SkillId == Watch) Continue();
                else if (p.Choices.Any(c => IsAction(c, "skip"))) Answer(c => IsAction(c, "skip"));
                else if (p.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.Nullification or DecisionKind.RescueDying) Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                else if (p.Kind == DecisionKind.SelectTargetCard) Answer(c => c.Parameters.GetValueOrDefault("source-zone") is "Hand" or "Equipment" || c.Cards.Count == 1 || c.Parameters.ContainsKey("slot-index"));
                else throw new InvalidOperationException("Unexpected boundary " + line + " " + Diagnostic());
            }
            throw new InvalidOperationException("Missing bounded boundary " + line + " " + Diagnostic());
        }
        public void Private() => Need(P is { PlayerSeat: 0, IsPrivate: true } && Enumerable.Range(1, 3).All(v => G.CreateSnapshot(v).PendingDecision is null) && Enumerable.Range(0, 4).All(v => G.CreateSnapshot(v).Players.Where(p => p.Seat != v).All(p => p.Hand.Count == 0)), "Private decisions belong to their real chooser and every foreign Hand remains hidden.");
        public void Immutable<T>(IReadOnlyList<T> list) { if (list is IList mutable && mutable.Count > 0) { var before = JsonSerializer.Serialize(list); var rejected = false; try { mutable[0] = mutable[0]; } catch (NotSupportedException) { rejected = true; } Need(rejected && JsonSerializer.Serialize(list) == before, "Prepared nested collections reject observer mutation."); } }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(v => SnapshotJson.Serialize(G.CreateSnapshot(v))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold replay preserves exact owning frames, all private views, rank cycles, native children, paid entities and commands."); }
        public void Reject(GameCommand command) { var before = State(); Need(!G.Submit(command).Accepted && State() == before, "A stale, forged or foreign command cannot change rank claims, private views, payments or history."); }
        public void RejectPrompt() { var p = P!; Reject(new AnswerPromptCommand(1, p.PromptId, p.Choices[0].Id, G.Revision)); Reject(new AnswerPromptCommand(0, p.PromptId, new("forged-wu-zhang-fei"), G.Revision)); }
        public void RejectOld(PendingDecision p) => Reject(new AnswerPromptCommand(0, p.PromptId, p.Choices[0].Id, G.Revision));
        public void Accept(GameCommand command) { try { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, "Rejected " + command.GetType().Name + ": " + result.Error?.Code + "/" + result.Error?.Message); } catch (Exception e) { throw new InvalidOperationException(spec.Name + ": " + e.Message + " " + Diagnostic(), e); } }
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Diagnostic()); }
        private string Diagnostic() => JsonSerializer.Serialize(new { spec.Name, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner, Prompt = P, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).Armor, V(i).IsAlive, V(i).HandCount }), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind, f.Step, Native = f is CardUseFrame u ? new { u.SourceSeat, u.CardId, u.CardKind, u.PhysicalCardIds, u.TargetSeats } : null }), Damage = E<DamageAppliedEvent>(), HpLoss = E<ProgramSkillHpLostEvent>(), Cycle = NamedEvents("WuZhangFeiRankCycleChangedEvent").TakeLast(3), Bindings = E<ProgramBindingResolvedEvent>().Where(e => e.SkillId is Zisheng or Xianlue or Haoxian).TakeLast(8) });
        public static Scene Start(Spec spec)
        {
            ContentRegistry registry; try { registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(spec)); } catch (Exception e) { throw new InvalidOperationException(spec.Name + " fixture: " + e.Message, e); }
            var ally = new ShanheCombatantDefinition(Ally, Hp: spec.PeerHp[0]) { StartingHand = spec.PeerHands[0] };
            var enemies = new[] { new ShanheCombatantDefinition(EnemyA, Hp: spec.PeerHp[1]) { StartingHand = spec.PeerHands[1] }, new ShanheCombatantDefinition(EnemyB, Hp: spec.PeerHp[2]) { StartingHand = spec.PeerHands[2] } };
            var setup = new ShanheBattleSetup(83, 1, "wzf-" + spec.Name, Owner, 4, 0, 0, 0, 3, [], spec.Hand, spec.Equipment, enemies, [ally], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(spec, game, registry); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
}
