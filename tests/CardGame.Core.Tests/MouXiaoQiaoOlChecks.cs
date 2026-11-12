using System.Runtime.CompilerServices;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class MouXiaoQiaoOlChecks
{
    private const string Hero="mou:xiao-qiao-ol",Driver="fixture:mouxq-driver",Hold="fixture:mouxq-reveal-draw",Quiet="fixture:mouxq-quiet",PlainReward="fixture:public-reveal-plain";
    private const string Jueyan="mou-ol:jueyan";
    private static bool FirstSuitBinding(string id) => id is "public-hand-first-heart" or "public-hand-first-diamond" or "public-hand-first-spade" or "public-hand-first-club";
    private static SkillUsageConsumedEvent[] FirstSuitUsages(Scene s, int owner = 0) => s.E<SkillUsageConsumedEvent>()
        .Where(e => e.SkillOwnerSeat == owner && e.SkillId == Jueyan && e.Scope == SkillUsageScope.Round && FirstSuitBinding(e.UsageId)).ToArray();
    private static ProgramBindingStartedEvent[] FirstSuitStarts(Scene s, int owner = 0) => s.E<ProgramBindingStartedEvent>()
        .Where(e => e.OwnerSeat == owner && e.SkillId == Jueyan && FirstSuitBinding(e.BindingId)).ToArray();
    private static ProgramDrawCompletedEvent[] FirstSuitDraws(Scene s, int owner = 0)
    {
        var frames = FirstSuitStarts(s, owner).Select(e => e.FrameId).ToHashSet();
        return s.E<ProgramDrawCompletedEvent>().Where(e => frames.Contains(e.FrameId)).ToArray();
    }
    public static void RealRetainedZeroCounterspellKeepsItsHandEntityAndNativeNodeCold()
    {
        var s = Scene.Start("zero-counterspell"); s.Play();
        var retained = s.V().Hand.Single(c => c.Kind == CardKind.Nullification).Id;
        s.Show([retained]); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        s.Need(s.E<ProgramCardsRevealedEvent>().Any(e => e.SkillId == Driver && e.Cards.Select(c => c.Id).SequenceEqual([retained])) &&
            s.V().Hand.Any(c => c.Id == retained), "The existing real Show instruction publicly reveals the future retained response without moving its entity.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        var before = s.V().HandCount;
        var material = s.V().Hand.Single(c => c.Kind == CardKind.DrawTwo).Id;
        var play = s.G.GetHumanLegalActions().First(a => a.CardId == material && a.ConversionSource is null);
        s.Accept(new PlayCardCommand(0, material, play.TargetSeats, s.G.Revision, s.P!.PromptId, play.PlayedCardKind, play.TargetCardId));
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.Nullification } && s.P.Choices.Any(c =>
            c.Parameters.GetValueOrDefault("response") == "mou-xiao-qiao-jueyan" && c.Cards.SequenceEqual([retained])));
        var original = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        var windowId = s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single().Id;
        s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "mou-xiao-qiao-jueyan" && c.Cards.SequenceEqual([retained]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold && s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>()
            .Any(w => w.Action is { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification }));
        var accepted = s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last().Action;
        var counter = s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single(f => f.Id == windowId);
        var issued = s.E<MouXiaoQiaoJueyanResponseIssuedEvent>().Single(e => e.ParentFrameId == windowId);
        s.Need(accepted is { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification, ActorSeat: 0, ProviderSeat: 0,
                ResponderSeat: 0, PhysicalCards.Count: 0 } && accepted.ParentActionId == original.Action!.ActionId &&
            accepted.ConversionChain is [var source] && source.SkillId == Jueyan &&
            counter is { ChainDepth: 1, EffectNullified: false, ResponseRuleNode.ChainDepth: 1, NamedAmbushResponse.CardId: 0 } &&
            counter.ResponseRuleNode.Action.ActionId == accepted.ActionId && counter.NamedAmbushResponse.ActionId == accepted.ActionId &&
            issued.ActionId == accepted.ActionId && issued.ActorSeat == 0 && issued.Kind == CardKind.Nullification &&
            s.V().Hand.Any(c => c.Id == retained) && s.E<NullificationRespondedEvent>().All(e => e.WindowFrameId != windowId),
            "The real Jueyan producer binds one zero-material accepted action and depth-one native node before offset, retaining the shown physical Nullification in Hand.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard or DecisionKind.Nullification });
        if (s.P!.Kind == DecisionKind.Nullification)
            s.Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
        s.Play();
        s.Need(s.E<NullificationRespondedEvent>().Where(e => e.WindowFrameId == windowId).ToArray() is
                [{ ResponderSeat: 0, NullificationCardId: 0, ChainDepth: 1, EffectNullified: true }] &&
            s.E<CardActionAcceptedEvent>().Count(e => e.Action.ActionId == accepted.ActionId) == 1 &&
            s.E<CardRespondedEvent>().Count(e => e.CardId == 0 && e.ResponderSeat == 0 && e.EffectiveCardKind == CardKind.Nullification) == 1 &&
            s.E<MouXiaoQiaoJueyanResponseGrantedEvent>().Count(e => e.Grant.ProgramFrameId == issued.ProgramFrameId) == 1 &&
            s.E<MouXiaoQiaoJueyanResponseIssuedEvent>().Count(e => e.ProgramFrameId == issued.ProgramFrameId) == 1 &&
            s.E<MouXiaoQiaoOlCompletedEvent>().Count(e => e.FrameId == issued.ProgramFrameId && e.Operation == SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan) == 1 &&
            !s.G.ResolutionStack.Any(f => f.Id == issued.ProgramFrameId || f.Id == windowId || f.Id == original.Id) &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.Id) == 1 && s.V().HandCount == before - 1 &&
            s.V().Hand.Any(c => c.Id == retained) && s.G.CardMovements.All(m => m.CardId != retained || m.From != CardLocation.Hand(0)) &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "One retained zero counterspell cancels the original DrawTwo without drawing two, consuming its displayed entity or repeating the producer, native return, response notification or physical trick cleanup.");
        s.Cold();
    }
    public static void RealMiluoRetainedUsesAndExternalRevealReturnCold()
    {
        PublicHandRevealSourcesRejectInvalidDeclarations();
        var s=Scene.Start();s.Play();var dodge=s.V().Hand.First(c=>c.Kind==CardKind.Dodge&&c.Suit==Suit.Heart).Id;
        var giftSlash=s.V().Hand.First(c=>c.Kind==CardKind.Slash&&c.Suit==Suit.Club).Id;
        var retained=s.V().Hand.First(c=>c.Kind==CardKind.DrawTwo).Id;var red=s.V().Hand.First(c=>c.Kind==CardKind.Slash&&c.Suit==Suit.Diamond).Id;
        s.Skill("mou-ol:miluo","reveal-and-give",[]);s.Answer(c=>c.Cards.SequenceEqual([dodge]));s.Branch("recipient",1);
        s.Answer(c=>c.Cards.SequenceEqual([giftSlash]));s.Branch("recipient",2);s.Branch("confirm");
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Hold);s.Need(s.E<MouXiaoQiaoMiluoGiftEvent>().Length==0&&s.G.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Any(),"The first actual public Hand-suit Draw child resolves before either original gift moves.");
        var issuedReveal = s.G.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Single();
        s.Need(issuedReveal.Claims.Count > 0 && issuedReveal.Claims.All(claim => claim.RetainOwnerDeath && claim.UsageReserved) &&
            issuedReveal.InvocationAuthorizations is { } retainedAuthorizations && retainedAuthorizations.Count == issuedReveal.Claims.Count &&
            retainedAuthorizations.All(authorization => authorization.RetainOwnerDeath && authorization.UsageReserved),
            "Jueyan explicitly retains its already issued reward's owner-death policy and reserved usage in both claims and invocation authorizations.");
        s.Cold();s.Answer(_=>true);s.Play();
        var suitRewards = FirstSuitStarts(s).Select(e => e.FrameId).ToHashSet();
        s.Need(s.E<MouXiaoQiaoMiluoGiftEvent>()is[{CardId:var giftA},{CardId:var giftB}]&&giftA==dodge&&giftB==giftSlash&&
            FirstSuitUsages(s).Select(e => e.UsageId).Order().SequenceEqual(new[] { "public-hand-first-heart", "public-hand-first-club" }.Order())&&
            s.E<ProgramDrawCompletedEvent>().Count(e=>suitRewards.Contains(e.FrameId)&&e.RequestedCount==1&&e.ActualDrawCount==1)==2,"Miluo's two genuine Hand gifts follow two independent first-suit Draw1 returns exactly once after Cold.");
        s.Skill("mou-ol:jueyan","retained-hand-use",[retained]);s.Reach(()=>s.HasBranch("use"));s.Branch("use");s.Play();
        var drawnUse=s.E<CardUseDeclaredEvent>().Last(e=>e.CardKind==CardKind.DrawTwo);
        s.Need(drawnUse.CardId==0&&s.V().Hand.Any(c=>c.Id==retained)&&s.E<CardUseFinishedEvent>().Count(e=>e.ResolutionId==drawnUse.ResolutionId)==1&&
            s.E<MouXiaoQiaoJueyanUsedEvent>().Count(e=>e.Source.OwnerSeat==0&&!e.RedBasic)==1,"The ordinary-trick branch retains its shown entity and completes one genuine zero-material native DrawTwo.");
        s.Skill("mou-ol:jueyan","retained-hand-use",[red]);s.Reach(()=>s.HasBranch("use"));s.Answer(c=>c.Parameters.GetValueOrDefault("branch")=="use"&&c.Targets.SequenceEqual([3]));s.Play();
        s.Need(s.V().Hand.Any(c=>c.Id==red)&&s.E<DamageAppliedEvent>().Any(e=>e.SourceSeat==0&&e.TargetSeat==3&&e.Amount==1)&&s.E<MouXiaoQiaoJueyanUsedEvent>().Count(e=>e.Source.OwnerSeat==0&&e.RedBasic)==1&&
            !s.G.GetHumanLegalActions().Any(a=>a.SkillId=="mou-ol:jueyan"),"The red-basic allowance is separate, uses a neutral native Slash, retains its original entity, and both Round branches are now spent.");
        var fire=s.V().Hand.First(c=>c.Kind==CardKind.FireAttack).Id;s.Native(fire,1);s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.FireAttackDiscard});
        var reveals=s.G.Events.Select(e=>e.Payload).ToList();var shown=reveals.FindLastIndex(e=>e is FireAttackCardRevealedEvent);
        var targetRewards = FirstSuitStarts(s, 1).Select(e => e.FrameId).ToHashSet();
        var reward=reveals.FindLastIndex(e=>e is ProgramDrawCompletedEvent d&&d.RequestedCount==1&&targetRewards.Contains(d.FrameId));
        s.Need(shown>=0&&reward>shown&&FirstSuitUsages(s, 1).Length==1&&!s.E<FireAttackResolvedEvent>().Any(),"An external real FireAttack Hand reveal pays its owner's first-suit Draw1 before returning to the original source's discard prompt.");
        s.Answer(c=>c.Cards.Count==0);s.Play();s.Skill(Driver,"strip",[],[2]);
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Driver);s.Answer(c=>c.Cards.Count==0&&c.Parameters.GetValueOrDefault("source-zone")=="Hand"&&c.Parameters.GetValueOrDefault("slot-index")=="0");
        s.Play();s.End();s.Reach(()=>s.HasBranch("lose"));s.Branch("lose",2);
        s.Reach(()=>s.E<RoundEndedEvent>().Any(e=>e.RoundNumber==1));s.Need(s.E<MouXiaoQiaoMiluoConsequenceEvent>().Any(e=>e.TargetSeat==2&&!e.Recover)&&s.E<ProgramSkillHpLostEvent>().Any(e=>e.SkillId=="mou-ol:miluo"&&e.TargetSeat==2&&e.Amount==1),"At the actual Round end, the original recipient's current Hand determines the one real HP-loss consequence.");
        SharedWindowRunsDeclaredClaimsWithoutACharacterLedger();
        RealFirstSuitReservationsSurviveNestedShowsSourceLossAndRoundReset();
        RealFirstSuitWindowSkipsAnUnstartedDeadOwnerClaim();
    }
    private static void SharedWindowRunsDeclaredClaimsWithoutACharacterLedger()
    {
        var s = Scene.Start(); s.Play();
        var match = (MatchRuntime)typeof(GameEngine).GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(s.G)!;
        var frames = match.Capabilities.Get<IProgramRuntimeFrames>();
        var windows = match.Capabilities.Get<IPublicHandRevealWindows>();
        var owner = match.Capabilities.Get<IMatchParticipantStateAccess>().At(0);
        var candidate = match.Capabilities.Get<IProgramWindowCandidateQueries>().Shard(owner)
            .GetTriggerCandidates(SkillProgramTriggerWindow.PublicHandRevealed).Single(source => source.SkillId == PlainReward);
        var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
        var materials = s.V().Hand.Take(2).ToArray();
        var handBefore = s.V().HandCount;
        const string privateFact = "fixture:private-reveal-capture";
        var capturedFacts = new SkillProgramTriggerFacts(0, s.V().Hp, false, CurrentHandCount: handBefore,
            BooleanStates: new System.Collections.ObjectModel.ReadOnlyDictionary<string, bool>(new Dictionary<string, bool> { [privateFact] = true }));
        s.Skill(Driver, "parent", []);
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "parent-start") == true);
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Driver);
        windows.Issue(parent.Id, new(parent.Id, source, candidate.GameplayHash, 1, materials[0].Suit, materials[0].Id), capturedFacts);
        windows.Issue(parent.Id, new(parent.Id, source, candidate.GameplayHash, 1, materials[1].Suit, materials[1].Id), capturedFacts);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "parent-start");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        var outer = s.G.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Single();
        var held = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Hold);
        s.Need(outer.Claims.All(claim => !claim.RetainOwnerDeath && !claim.UsageReserved) &&
            outer.InvocationAuthorizations is { } defaultAuthorizations && defaultAuthorizations.Count == outer.Claims.Count &&
            defaultAuthorizations.All(authorization => !authorization.RetainOwnerDeath && !authorization.UsageReserved),
            "Ordinary manual claims keep the default owner-death stop policy and do not claim an unreserved usage; source-independent permission does not grant death retention.");
        s.Need(outer.ParentFrameId == parent.Id && outer.Claims.Count == 2 && outer.ClaimIndex == 0 &&
            frames.Find(parent.Id)!.PublicHandRevealConsumedCount == 2 && windows.Suspends(parent.Id) &&
            outer.CapturedFacts.Count == 2 && outer.CapturedFacts.All(facts => facts?.CurrentHandCount == handBefore) &&
            frames.Find(parent.Id)!.PublicHandRevealCapturedFacts is { Count: 2 } parentFacts && parentFacts.All(facts => facts?.CurrentHandCount == handBefore) &&
            FirstSuitUsages(s).Length == 0 &&
            Enumerable.Range(0, 4).All(viewer => !SnapshotJson.Serialize(s.G.CreateSnapshot(viewer)).Contains(privateFact, StringComparison.Ordinal)),
            "A shared window accepts ordinary declared Draw1 claims, reserves their producer cursor once and suspends the original parent without a character proof ledger.");

        // These direct trusted Issue calls are not accepted commands. Test frame
        // serialization here; the real reveal scenarios above separately prove
        // command-prefix checkpoint replay.
        var restoredParent = JsonSerializer.Deserialize<ProgramSkillFrame>(JsonSerializer.Serialize((ProgramSkillFrame)frames.Find(parent.Id)!))!;
        var restoredWindow = JsonSerializer.Deserialize<PublicHandRevealWindowFrame>(JsonSerializer.Serialize(outer))!;
        frames.Replace(parent.Id, restoredParent);
        frames.Replace(outer.Id, restoredWindow);
        s.Need(restoredParent.PublicHandRevealConsumedCount == 2 && restoredWindow.Claims.SequenceEqual(outer.Claims) &&
            restoredWindow.ClaimIndex == 0 && restoredWindow.Claims is IList<PublicHandRevealClaim> { IsReadOnly: true } &&
            restoredWindow.CapturedFacts is IList<SkillProgramTriggerFacts?> { IsReadOnly: true } &&
            restoredWindow.CapturedFacts.Count == 2 && restoredWindow.CapturedFacts.All(facts => facts is { } saved && saved.CurrentHandCount == handBefore && saved.BooleanStates?.GetValueOrDefault(privateFact) == true) &&
            restoredParent.PublicHandRevealCapturedFacts is { Count: 2 } restoredFacts && restoredFacts.All(facts => facts?.CurrentHandCount == handBefore) &&
            windows.Suspends(parent.Id),
            "Frame serialization preserves the committed producer count, immutable claims, private captured facts and suspended child cursor.");

        windows.Issue(held.Id, new(held.Id, source, candidate.GameplayHash, 1, materials[0].Suit, materials[0].Id), capturedFacts);
        s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "parent-return") == true);
        var started = s.E<PublicHandRevealWindowStartedEvent>();
        var inner = started.Single(window => window.ParentFrameId == held.Id);
        var returned = s.G.Events.Select(item => item.Payload).ToList();
        var innerReturn = returned.FindIndex(fact => fact is PublicHandRevealWindowReturnedEvent done && done.WindowId == inner.WindowId);
        var outerReturn = returned.FindIndex(fact => fact is PublicHandRevealWindowReturnedEvent done && done.WindowId == outer.Id);
        s.Need(started.Count(window => window.ParentFrameId == parent.Id) == 1 && innerReturn >= 0 && outerReturn > innerReturn &&
            s.E<PublicHandRevealClaimReturnedEvent>().Count(fact => fact.WindowId == outer.Id) == 2 &&
            s.E<PublicHandRevealClaimReturnedEvent>().Count(fact => fact.WindowId == inner.WindowId) == 1 &&
            s.V().HandCount == handBefore + 3 && s.V().Skills?.Any(skill => skill.Id == PlainReward) != true,
            "Nested claims return before the outer cursor; already issued ordinary rewards finish after their source is lost, with each Draw1 paid once.");
        var capturedNumbers = s.E<ProgramExpressionNumberCapturedEvent>().Where(fact => fact.SkillId == PlainReward && fact.Name == "captured-hand-count").ToArray();
        s.Need(capturedNumbers.Length == 3 && capturedNumbers.All(fact => fact.Value == handBefore) &&
            s.E<PublicHandRevealClaimReturnedEvent>().All(fact => !JsonSerializer.Serialize(fact).Contains(privateFact, StringComparison.Ordinal) &&
                !JsonSerializer.Serialize(fact).Contains("CapturedFacts", StringComparison.Ordinal)),
            "All three ordinary rewards bind the original captured Hand count despite intervening draws; public claim-return facts omit the private source facts.");
        var issued = s.E<PublicHandRevealWindowStartedEvent>().Length;
        s.Need(!windows.Begin(parent.Id, PublicHandRevealReturnKind.Program) &&
            s.E<PublicHandRevealWindowStartedEvent>().Length == issued &&
            frames.Find(parent.Id)!.PublicHandRevealConsumedCount == 2,
            "A completed producer cannot open its consumed claims again without consulting returned-event history.");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "parent-return"); s.Play();
    }

    private static void PublicHandRevealSourcesRejectInvalidDeclarations()
    {
        const string skill = "fixture:public-reveal-contract";
        var valid = JsonNode.Parse(SkillProgramTestJson.CurrentSchema("""
            {"schemaVersion":__RULES_SCHEMA__,"skills":[{"id":"fixture:public-reveal-contract","revision":1,"triggers":[{
              "id":"capture","window":"publicHandRevealed","subject":"owner","optional":false,
              "invocation":{"source":"publicHandReveal","ownerDeath":"retain"},
              "effects":[{"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}]}]}]}
            """))!;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [skill] = new { name = "Public reveal issuance", description = "Shared loader contract" } } });
        _ = SkillProgramCatalog.Load(valid.ToJsonString(), presentation);
        foreach (var invalid in new[] { "optional", "eventUsage", "markerCost", "lateQualification" })
        {
            var copy = valid.DeepClone();
            var trigger = copy["skills"]![0]!["triggers"]![0]!;
            if (invalid == "optional") trigger["optional"] = true;
            else if (invalid == "eventUsage") { trigger["usageScope"] = "event"; trigger["usageLimit"] = 1; }
            else if (invalid == "markerCost") trigger["markerCost"] = JsonNode.Parse("""{"marker":"ren","amount":1}""");
            else trigger["evaluateConditionAtResolution"] = true;
            var rejected = false;
            try { _ = SkillProgramCatalog.Load(copy.ToJsonString(), presentation); }
            catch (InvalidOperationException) { rejected = true; }
            if (!rejected) throw new InvalidOperationException("The shared public Hand source admitted an invalid declaration: " + invalid);
        }
    }

    private static void RealFirstSuitReservationsSurviveNestedShowsSourceLossAndRoundReset()
    {
        var s = Scene.Start("quota"); s.Play();
        var hearts = s.V().Hand.Where(card => card.Suit == Suit.Heart).Take(2).Select(card => card.Id).ToArray();
        var club = s.V().Hand.First(card => card.Suit == Suit.Club).Id;
        s.Need(hearts.Length == 2, "The fixed real Hand supplies two same-suit entities and one different suit without a seed search.");
        var shown = new[] { hearts[0], hearts[1], club };
        var beforeHand = s.V().HandCount;
        s.Show(shown);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        var window = s.G.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Single();
        var reveal = s.E<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Driver && e.BindingId == "show");
        s.Need(reveal.Cards.Select(card => card.Id).Order().SequenceEqual(shown.Order()) &&
            window.ParentFrameId == reveal.FrameId && window.ClaimIndex == 0 && window.Claims.Count == 2 &&
            window.Claims.Select(claim => claim.Suit).Order().SequenceEqual(new[] { Suit.Heart, Suit.Club }.Order()) &&
            window.Claims.All(claim => claim.UsageReserved && claim.RetainOwnerDeath) &&
            window.InvocationAuthorizations is { Count: 2 } authorizations && authorizations.All(authorization => authorization.UsageReserved) &&
            FirstSuitUsages(s).Length == 2 && FirstSuitUsages(s).All(usage => usage.Count == 1) && FirstSuitStarts(s).Length == 1,
            "A real three-card public Hand batch reserves only its two distinct suit usages before starting the first reward.");
        var unstarted = window.Claims[1];
        s.Need(!FirstSuitStarts(s).Any(start => start.BindingId == unstarted.Source.BindingId),
            "The second suit is already reserved while its actual reward has not started.");
        s.Cold();
        var restored = s.G.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Single();
        s.Need(restored.Id == window.Id && restored.Claims.SequenceEqual(window.Claims) &&
            restored.InvocationAuthorizations is { Count: 2 } restoredAuthorizations && restoredAuthorizations.All(authorization => authorization.UsageReserved),
            "Real command-prefix recovery preserves both pre-reserved suit claims and their invocation permissions.");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        s.Answer(choice => choice.Cards.SequenceEqual([unstarted.CardId])); s.Play();
        s.Need(s.E<ProgramCardsRevealedEvent>().Any(e => e.SkillId == Hold && e.Cards.Select(card => card.Id).SequenceEqual([unstarted.CardId])) &&
            s.E<PublicHandRevealWindowStartedEvent>().Length == 1 && FirstSuitUsages(s).Length == 2 &&
            FirstSuitStarts(s).Length == 2 && FirstSuitDraws(s) is [{ RequestedCount: 1, ActualDrawCount: 1 }, { RequestedCount: 1, ActualDrawCount: 1 }] &&
            s.E<PublicHandRevealClaimReturnedEvent>().Count(e => e.WindowId == window.Id) == 2 &&
            s.V().HandCount == beforeHand + 2 && s.V().Skills?.Any(skill => skill.Id == Jueyan) != true,
            "A nested real show of the pre-reserved second suit cannot duplicate its reward; both original issued Draw1 children finish once after Jueyan is lost.");
        s.Skill(Driver, "regain", []); s.Play();
        s.Need(s.V().Skills?.Any(skill => skill.Id == Jueyan) == true,
            "The same living owner really regains Jueyan through the shared skill-grant operation.");
        s.Show(shown); s.Play();
        s.Need(FirstSuitUsages(s).Length == 2 && FirstSuitStarts(s).Length == 2 && FirstSuitDraws(s).Length == 2 &&
            s.E<PublicHandRevealWindowStartedEvent>().Length == 1,
            "Losing and regaining the source during the same actual Round does not replenish either spent suit usage.");
        s.Cold(); s.End();
        s.Reach(() => s.E<RoundStartedEvent>().Any(e => e.RoundNumber == 2)); s.Play();
        s.Need(shown.All(id => s.V().Hand.Any(card => card.Id == id)), "The small fixture keeps the original displayed entities through the real Round boundary.");
        var roundStart = s.G.Events.ToList().FindIndex(e => e.Payload is RoundStartedEvent { RoundNumber: 2 });
        var beforeSecondRoundShow = s.V().HandCount;
        s.Show(shown); s.Play();
        var secondRoundUsages = s.G.Events.Skip(roundStart + 1).Select(e => e.Payload).OfType<SkillUsageConsumedEvent>()
            .Where(e => e.SkillOwnerSeat == 0 && e.SkillId == Jueyan && e.Scope == SkillUsageScope.Round && FirstSuitBinding(e.UsageId)).ToArray();
        s.Need(secondRoundUsages.Select(e => e.UsageId).Order().SequenceEqual(new[] { "public-hand-first-heart", "public-hand-first-club" }.Order()) &&
            secondRoundUsages.All(e => e.Count == 1) && FirstSuitUsages(s).Length == 4 && FirstSuitDraws(s).Length == 4 &&
            FirstSuitDraws(s).All(e => e is { RequestedCount: 1, ActualDrawCount: 1 }) && s.V().HandCount == beforeSecondRoundShow + 2,
            "The next real Round replenishes both declared suit quotas, so the same three-card batch again produces exactly two Draw1 rewards.");
    }

    private static void RealFirstSuitWindowSkipsAnUnstartedDeadOwnerClaim()
    {
        var s = Scene.Start("death"); s.Play();
        var heart = s.V().Hand.First(card => card.Suit == Suit.Heart).Id;
        var club = s.V().Hand.First(card => card.Suit == Suit.Club).Id;
        s.Show([heart, club]);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
        var window = s.G.ResolutionStack.OfType<PublicHandRevealWindowFrame>().Single();
        s.Need(window.Claims.Count == 2 && window.ClaimIndex == 0 && window.Claims.All(claim => claim.UsageReserved) &&
            FirstSuitUsages(s).Length == 2 && FirstSuitStarts(s).Length == 1,
            "Both real suit claims are reserved before death can occur inside the first reward's actual draw child.");
        s.Cold(); s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => !s.V().IsAlive || s.P is { PlayerSeat: 0, Kind: DecisionKind.RescueDying });
        if (s.P is { PlayerSeat: 0, Kind: DecisionKind.RescueDying })
            s.Answer(choice => choice.Parameters.GetValueOrDefault("response") is "let-die" or "pass");
        s.Reach(() => !s.V().IsAlive);
        s.Reach(() => s.E<PublicHandRevealWindowReturnedEvent>().Any(e => e.WindowId == window.Id));
        s.Need(s.G.State.Winner == Winner.None && !s.V().IsAlive &&
            s.E<PlayerDyingEvent>().Count(e => e.VictimSeat == 0) == 1 && s.E<PlayerDiedEvent>().Count(e => e.VictimSeat == 0) == 1 &&
            s.E<DyingResolvedEvent>().Any(e => e.VictimSeat == 0 && !e.Survived) &&
            FirstSuitUsages(s).Length == 2 && FirstSuitStarts(s).Length == 1 &&
            FirstSuitDraws(s) is [{ RequestedCount: 1, ActualDrawCount: 1 }] &&
            s.E<PublicHandRevealClaimReturnedEvent>().Count(e => e.WindowId == window.Id) == 2 &&
            !FirstSuitStarts(s).Any(e => e.BindingId == window.Claims[1].Source.BindingId),
            "In the live Team game, the delivered first reward drains after its owner's real death while the cursor skips the second unstarted claim without issuing another Draw.");
    }

    private sealed class Scene(GameEngine game,ContentRegistry registry)
    {
        public GameEngine G=game;public PendingDecision? P=>Enumerable.Range(0,4).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V()=>G.CreateSnapshot(0).Players[0];public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public bool HasBranch(string branch)=>P is{PlayerSeat:0}p&&p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="mou-xiao-qiao-ol"&&c.Parameters.GetValueOrDefault("branch")==branch);
        public void Branch(string branch,int? target=null)=>Answer(c=>c.Parameters.GetValueOrDefault("branch")==branch&&(target is null||c.Targets.SequenceEqual([target.Value])));
        public void Skill(string skill,string id,IReadOnlyList<int> cards,IReadOnlyList<int>? targets=null)=>Accept(new UseProgramSkillCommand(0,skill,id,cards,targets??[],G.Revision,P!.PromptId));
        public void Show(IReadOnlyList<int> ids)
        {
            Skill(Driver, "show", []);
            foreach (var id in ids)
            {
                Reach(() => P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
                Answer(choice => choice.Cards.SequenceEqual([id]));
            }
        }
        public void Native(int id,int target){var a=G.GetHumanLegalActions().First(a=>a.CardId==id&&a.TargetSeats.SequenceEqual([target])&&a.ConversionSource is null);Accept(new PlayCardCommand(0,id,[target],G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId));}
        public void Play()=>Reach(()=>P is{PlayerSeat:0,Kind:DecisionKind.PlayCard});public void End()=>Accept(new EndPlayPhaseCommand(0,G.Revision,P!.PromptId));
        public void Answer(Func<PromptChoice,bool> match){var p=P!;var c=p.Choices.FirstOrDefault(match);Need(p.PlayerSeat==0&&c is not null,"The real human can answer only an actually published choice.");Accept(new AnswerPromptCommand(0,p.PromptId,c!.Id,G.Revision));}
        public void Accept(GameCommand c){try{var r=G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single());Need(r.Accepted,r.Error?.Message??"Native command rejected.");}catch(Exception e){throw new InvalidOperationException(e.Message+Diag(),e);}}
        public void Reach(Func<bool> done,[CallerLineNumber]int line=0)
        {
            for(var i=0;i<180;i++)
            {
                if(done())return;Need(G.State.Winner==Winner.None,"Unexpected native terminal cutoff at "+line);
                if(P is not{PlayerSeat:0}p)Accept(new AdvanceOneStepCommand(G.Revision));
                else if(p.Kind==DecisionKind.PlayCard)End();
                else if(p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip"))Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
                else throw new InvalidOperationException("Unexpected actual human boundary at "+line+Diag());
            }
            throw new InvalidOperationException("Bounded OL Xiao Qiao native flow not reached at "+line+Diag());
        }
        public void Cold(){var v=SnapshotJson.Serialize(G.CreateSnapshot(0));G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(v==SnapshotJson.Serialize(G.CreateSnapshot(0)),"The real public-reveal child and its original unspent physical gifts survive Cold unchanged.");}
        public void Need(bool ok,string why){if(!ok)throw new InvalidOperationException(why+Diag());}
        private string Diag()=>JsonSerializer.Serialize(new{G.State.TurnNumber,G.State.Phase,Prompt=P,Owner=V(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),Facts=G.Events.Where(e=>e.Payload.GetType().Name.StartsWith("MouXiaoQiao",StringComparison.Ordinal)||e.Payload.GetType().Name.StartsWith("PublicHandReveal",StringComparison.Ordinal)||e.Payload is CardUseDeclaredEvent or FireAttackCardRevealedEvent or FireAttackResolvedEvent).Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType()))});
        public static Scene Start(string scenario = "default")
        {
            var r=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scenario));
            ShanheStartingCard[] opening = [new(CardKind.DrawTwo,Suit.Heart,4),new(CardKind.Dodge,Suit.Heart,2),new(CardKind.FireAttack,Suit.Spade,5),new(CardKind.Slash,Suit.Diamond,6),new(CardKind.Slash,Suit.Spade,8)];
            if (scenario == "zero-counterspell") opening = [.. opening, new(CardKind.Nullification, Suit.Spade, 9)];
            var setup=new ShanheBattleSetup(7,1,"mouxq-real",Hero,3,0,0,0,0,[Driver,Hold,PlainReward],opening,[],
                [new("fixture:mouxq-enemy-a",Hp:12){StartingHand=[]},new("fixture:mouxq-enemy-b",Hp:12){StartingHand=[]}],[new("fixture:mouxq-ally",Hp:12){StartingHand=[new(CardKind.Dodge,Suit.Spade,6)]}],new string('f',64));
            var g=GameEngine.CreateStandard(new(){Seed=setup.BattleSeed,PlayerCount=4,HumanSeat=0,HumanRole=null,HumanTeamId=ShanheBattleSetup.PlayerTeamId,ModeId=ShanheBattleSetup.BattleModeId,ShanheBattle=setup,UseInteractiveSetup=false,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=8},r);var s=new Scene(g,r);s.Accept(new StartGameCommand());return s;
        }
    }
    private sealed class Fixture(string scenario):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-mou-xiao-qiao-ol",new(1,0,0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,Hero);
            foreach(var k in new[]{CardKind.Slash,CardKind.Dodge,CardKind.FireAttack,CardKind.DrawTwo}.Concat(scenario == "zero-counterspell" ? [CardKind.Nullification] : Array.Empty<CardKind>())){var c=CardCatalog.Get(k);b.AddCard(new("fixture:mouxq-"+k,c.DisplayName,c.CategoryName,c.Description,k));}
            var firstRewardHold = new List<object>
            {
                new{op="chooseOption",target="owner",resultBind="held",options=new[]{new{id="continue"}}}
            };
            if (scenario == "quota")
            {
                firstRewardHold.Add(new{op="selectOwnedCards",target="owner",zones=new[]{"hand"},minimumCards=1,maximumCards=1,resultBind="nested-show"});
                firstRewardHold.Add(new{op="revealBoundCards",target="owner",sourceBind="nested-show"});
                firstRewardHold.Add(new{op="skillState",target="owner",action="lose",match=new{skillIds=new[]{Jueyan}}});
            }
            if (scenario == "death") firstRewardHold.Add(new{op="loseHp",target="owner",amount=10});
            var defs=new object[]{
                new{id=Quiet,revision=1,triggers=new[]{new{id="quiet",window="afterNormalDraw",subject="owner",optional=false,effects=new[]{new{op="skipTurnPhases",target="owner",phases=new[]{"play"}}}}}},
                new{id=Hold,revision=1,triggers=new object[]{
                    new{id="hold",window="cardsGained",subject="owner",destinationZones=new[]{"hand"},movementOccurrence="perBatch",movementReasons=new[]{"skill-program.mou-ol:jueyan.Draw"},optional=false,usageScope="game",usageLimit=1,effects=firstRewardHold},
                    new{id="plain-hold",window="cardsGained",subject="owner",destinationZones=new[]{"hand"},movementOccurrence="perBatch",movementReasons=new[]{"skill-program."+PlainReward+".Draw"},optional=false,usageScope="game",usageLimit=1,effects=new object[]{
                        new{op="chooseOption",target="owner",resultBind="held",options=new[]{new{id="continue"}}},
                        new{op="skillState",target="owner",action="lose",match=new{skillIds=new[]{PlainReward}}}}}}},

                new{id=Driver,revision=1,modifiers=scenario == "quota" ? new object[]{new{id="keep-real-shown-hand",query="handLimit",operation="add",value=40,priority=0}} : [],activations=new object[]{
                    new{id="strip",minCards=0,maxCards=0,minTargets=1,maxTargets=1,targetKind="otherLiving",usesPerTurn=(int?)null,effects=new[]{new{op="selectAndMoveOwnedCard",target="owner",chooserRef=new{kind="owner"},cardOwnerRef=new{kind="selectedTarget"},zones=new[]{"hand"},count=1,destination="discardPile",awaitMovementTriggers=true}}},
                    new{id="show",minCards=0,maxCards=0,minTargets=0,maxTargets=0,targetKind="anyLiving",usesPerTurn=(int?)null,effects=new object[]{
                        new{op="selectOwnedCards",target="owner",zones=new[]{"hand"},minimumCards=scenario == "zero-counterspell" ? 1 : scenario == "death" ? 2 : 3,maximumCards=scenario == "zero-counterspell" ? 1 : scenario == "death" ? 2 : 3,resultBind="shown"},
                        new{op="revealBoundCards",target="owner",sourceBind="shown"}}},
                    new{id="regain",minCards=0,maxCards=0,minTargets=0,maxTargets=0,targetKind="anyLiving",usesPerTurn=(int?)null,effects=new[]{new{op="grantSkills",target="owner",skillIds=new[]{Jueyan}}}},
                    new{id="parent",minCards=0,maxCards=0,minTargets=0,maxTargets=0,targetKind="anyLiving",usesPerTurn=(int?)null,effects=new[]{
                        new{op="chooseOption",target="owner",resultBind="parent-start",options=new[]{new{id="parent-start"}}},
                        new{op="chooseOption",target="owner",resultBind="parent-return",options=new[]{new{id="parent-return"}}}}}}},
                new{id=PlainReward,revision=1,triggers=new[]{new{id="plain-reward",window="publicHandRevealed",subject="owner",optional=false,
                    effects=new object[]{new{op="captureNumber",target="owner",resultBind="captured-hand-count",valueExpr=new[]{"windowHandCount","owner"}},
                        new{op="draw",target="owner",amount=1,awaitMovementTriggers=true}}}}}

            };
            var labels=new Dictionary<string,object>{[Quiet]=new{name="原生安静同行",description="正常摸牌后跳过出牌阶段。"},[PlainReward]=new{name="普通展示受益",description="共享窗口签发的一次摸牌。"},[Driver]=new{name="实际弃牌夹具",description="执行原生指定角色手牌弃置。",optionLabels=new Dictionary<string,string>{{"parent-start","开始共享展示"},{"parent-return","结束共享展示"}}},[Hold]=new{name="实际展示奖励停点",description="只在第一次真实展示奖励摸牌后暂停。",optionLabels=new Dictionary<string,string>{{"continue","继续真实展示续接"}}}};
            var rules = JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.RulesSchemaVersion,skills=defs});
            if (scenario == "zero-counterspell")
            {
                var json = JsonNode.Parse(rules)!;
                json["skills"]![1]!["triggers"]!.AsArray().Add(JsonNode.Parse($$"""
                    {"id":"zero-counterspell-issued","window":"cardResponseAccepted","ownerRelation":"actor","cardKinds":["nullification"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"zero-counterspell-issued","options":[{"id":"continue"}]}]}
                    """));
                rules = json.ToJsonString();
            }
            var catalog=SkillProgramCatalog.Load(rules,JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=labels}));
            foreach(var(id,p)in catalog.Programs)b.AddSkill(new(id,catalog.Presentations[id].Name,catalog.Presentations[id].Description){Program=p,ProgramPresentation=catalog.Presentations[id]});
            b.AddGeneral(new("fixture:mouxq-ally","实际展示受益者","","standard:none","wu",12,[Quiet,"mou-ol:jueyan"]));
            foreach(var id in new[]{"fixture:mouxq-enemy-a","fixture:mouxq-enemy-b"})b.AddGeneral(new(id,"实际同行","","standard:none","qun",12,[Quiet]));
            b.AddDeck(new("fixture:mouxq-deck","固定真实手牌库存",0,2,[]){PhysicalCards=Enumerable.Range(0,70).Select(i=>new ContentDeckPhysicalCard("fixture:mouxq-"+CardKind.Slash,Suit.Club,7)).ToArray()});
            b.AddMode(new(ShanheBattleSetup.BattleModeId,"OL谋小乔真实主流程",4,4,new Dictionary<string,int>(),"fixture:mouxq-deck",GeneralPoolIds:[Hero,"fixture:mouxq-ally","fixture:mouxq-enemy-a","fixture:mouxq-enemy-b"],ModeKind:ContentModeKind.Team,TeamCounts:new Dictionary<string,int>{[ShanheBattleSetup.PlayerTeamId]=2,[ShanheBattleSetup.EnemyTeamId]=2}));
        }
    }
}
