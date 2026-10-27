using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHuxinMirrorChecks
{
    private const string Skill = StandardShanheHuxinMirrorPackage.SkillId;
    private const string Owner = "fixture:huxin-owner", Peer = "fixture:huxin-peer", Driver = "fixture:huxin-driver",
        Watch = "fixture:huxin-movement", Ban = "fixture:huxin-ban-hand", Deck = "fixture:huxin-deck";

    public static void ActualArmorPaymentPreventsLargeOrLethalDamageAndHonoursNativeArmorBypass()
    {
        foreach (var (hp, amount) in new[] { (4, 2), (1, 1) })
        {
            var s = Scene.Start(hp);
            var armor = s.V(0).Equipment.Single(c => c.Kind == CardKind.HuxinMirror).Id;
            Need(s.Has(0), "The physical source20746 armor installs its own active equipment grant.");
            s.Use("hurt-" + amount);
            s.Until(() => s.P?.SkillPrompt?.SkillId == Skill);
            Need(s.G.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single().Amount == amount &&
                s.G.CardMovements.All(m => m.CardId != armor || m.From != CardLocation.Equipment(0)),
                "The optional offer belongs to the original finalized damage and does not pay before acceptance.");
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
            var payment = s.Events<HuxinMirrorPaidEvent>().Single();
            Need(payment.Receipt is { Source.OwnerSeat: 0, Source.SkillId: Skill } && payment.Receipt.CardId == armor &&
                payment.Receipt.Amount == amount && payment.Receipt.Source.SkillInstanceId == "equipment:shanhe-huxin-mirror:" + armor &&
                s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == payment.ProgramFrameId && f.HuxinMirrorPayment == payment.Receipt) &&
                s.Events<ProgramDamagePreventedEvent>().Count(e => e.SkillId == Skill && e.Amount == amount) == 1 &&
                s.Events<DamageAppliedEvent>().Length == 0 && s.V(0).Hp == hp && !s.Has(0),
                "Real armor payment removes its grant once, retains the original prevention frame through the paid movement child and prevents the entire segment.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue" || c.Parameters.GetValueOrDefault("option") == "continue" || c.Id.Value.EndsWith("continue", StringComparison.Ordinal));
            s.Play();
            Need(s.V(0).Hp == hp && s.V(0).HandCount == 1 && s.Events<HuxinMirrorPaidEvent>().Length == 1 &&
                s.G.CardMovements.Count(m => m.CardId == armor && m.From == CardLocation.Equipment(0) &&
                    m.To == CardLocation.DiscardPile && m.Reason.Value == "equipment.huxin-mirror.prevent") == 1 &&
                s.Events<DamageAppliedEvent>().Length == 0 && s.G.State.Winner == Winner.None,
                "The real paid movement observer draws once and returns to the same damage frame without repaying or applying damage.");
        }
        var ordinary = Scene.Start(); ordinary.Use("hurt-1"); ordinary.Play();
        Need(ordinary.V(0).Hp == 3 && ordinary.Has(0) && ordinary.Events<HuxinMirrorPaidEvent>().Length == 0 &&
            ordinary.Events<DamageAppliedEvent>().Single().Amount == 1,
            "An ordinary nonlethal point applies normally and never offers or pays the armor.");
        var protectedHp = Scene.Start(1); protectedHp.Use("armor-one"); protectedHp.Play();
        protectedHp.Use("hurt-1"); protectedHp.Play();
        Need(protectedHp.V(0).Hp == 1 && protectedHp.V(0).Armor == 0 && protectedHp.Has(0) &&
            protectedHp.Events<HuxinMirrorPaidEvent>().Length == 0,
            "A point absorbed by real native armor is not falsely treated as lethal to the remaining one HP.");
        var declined = Scene.Start(); declined.Use("hurt-2"); declined.Until(() => declined.P?.SkillPrompt?.SkillId == Skill);
        declined.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); declined.Play();
        Need(declined.V(0).Hp == 2 && declined.Has(0) && declined.Events<HuxinMirrorPaidEvent>().Length == 0,
            "Declining the original optional offer retains the equipped entity and the real two-point damage.");
        var sealedArmor = Scene.Start(seal: true);
        sealedArmor.Submit(new UseProgramSkillCommand(0, Driver, "hurt-peer", [], [1], sealedArmor.G.Revision, sealedArmor.P!.PromptId));
        sealedArmor.Play();
        sealedArmor.Use("hurt-2"); sealedArmor.Play();
        Need(sealedArmor.V(0).Hp == 2 && sealedArmor.Has(0) && sealedArmor.Events<HuxinMirrorPaidEvent>().Length == 0 &&
            sealedArmor.Events<CardTargetRestrictionGrantedEvent>().Any(e => e.Restriction.TargetSeat == 0 &&
                e.Restriction.Source.OwnerSeat == 1 && e.Restriction.Restriction == SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn),
            "A genuine current-turn armor seal suppresses the equipment's optional prevention without removing its physical card.");
        var bypass = Scene.Start(bypass: true);
        bypass.Card(CardKind.QinggangSword); bypass.Play(); bypass.Card(CardKind.Alcohol); bypass.Play();
        var slash = bypass.Card(CardKind.Slash, 1); bypass.Play();
        Need(bypass.V(1).Hp == 4 && bypass.Has(1) && bypass.Events<HuxinMirrorPaidEvent>().Length == 0 &&
            bypass.Events<DamageAppliedEvent>().Single(e => e.TargetSeat == 1).Amount == 2 &&
            bypass.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == bypass.Events<CardUseDeclaredEvent>().Single(d => d.CardId == slash).ResolutionId) == 1,
            "A real paid alcohol Slash carrying Qinggang's native armor bypass applies two damage and never lets the bypassed armor pay.");
        CheckQinggangDoesNotSuppressAnotherChainedMirror();
    }

    private static void CheckQinggangDoesNotSuppressAnotherChainedMirror()
    {
        var s = Scene.Start(chain: true);
        s.Card(CardKind.QinggangSword); s.Play(); s.Card(CardKind.Alcohol); s.Play();
        var iron = s.V(0).Hand.Single(c => c.Kind == CardKind.IronChain).Id;
        var link = s.G.GetHumanLegalActions().Single(a => a.CardId == iron && a.TargetSeats.SequenceEqual(new[] { 1, 2 }));
        s.Submit(new PlayCardCommand(0, iron, link.TargetSeats, s.G.Revision, s.P!.PromptId, link.PlayedCardKind)); s.Play();
        Need(s.V(1).IsChained && s.V(2).IsChained, "A real paid Iron Chain links both actual mirror holders.");
        var mirror = s.V(2).Equipment.Single(c => c.Kind == CardKind.HuxinMirror).Id;
        var fire = s.Card(CardKind.FireSlash, 1); s.Play();
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == fire);
        var payment = s.Events<HuxinMirrorPaidEvent>().Single();
        Need(s.V(1).Hp == 4 && s.Has(1) && s.V(2).Hp == 6 && !s.Has(2) &&
            payment.Receipt is { Source.OwnerSeat: 2, Amount: 2 } && payment.Receipt.CardId == mirror &&
            payment.Receipt.AttackFrameId == use.ResolutionId &&
            s.Events<DamageAppliedEvent>().Count(e => e.TargetSeat == 1 && e.Amount == 2) == 1 &&
            s.Events<DamageAppliedEvent>().All(e => e.TargetSeat != 2) &&
            s.G.CardMovements.Count(m => m.CardId == mirror && m.From == CardLocation.Equipment(2) &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == "equipment.huxin-mirror.prevent") == 1 &&
            s.G.CardMovements.Count(m => m.CardId == fire && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1,
            "Qinggang bypasses only the directly attacked mirror; the actual later chained holder pays its own armor once and prevents the same frozen Fire Slash damage.");
    }

    public static void HandMirrorPlacesOnAnotherNativeSlotAndReturnsReplacementRecoveryAndMovementChildren()
    {
        var s = Scene.Start(placement: true);
        var incoming = s.V(0).Hand.First(c => c.Kind == CardKind.HuxinMirror).Id;
        var spare = s.V(0).Hand.Last(c => c.Kind == CardKind.HuxinMirror).Id;
        var oldArmor = s.V(1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        Need(!s.Has(0) && !s.Has(1) && s.V(1).Hp == 4, "Only a real equipped mirror grants prevention; a hand mirror does not.");
        var action = s.G.GetHumanLegalActions().Single(a => a.CardId == incoming && a.Kind == LegalActionKind.Equip && a.TargetSeat == 1);
        s.Submit(new PlayCardCommand(0, incoming, action.TargetSeats, s.G.Revision, s.P!.PromptId, action.PlayedCardKind));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var parent = s.G.ResolutionStack.OfType<HuxinMirrorPlacementFrame>().Single();
        Need(parent.ActorSeat == 0 && parent.TargetSeat == 1 && parent.CardId == incoming && parent.From == CardLocation.Hand(0) &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.ResumeHuxinMirrorPlacementFrameId == parent.Id &&
                w.Batch.ParentFrameId == parent.Id) && s.V(1).Equipment.Single(c => c.Kind == CardKind.HuxinMirror).Id == incoming &&
            s.Has(1) && !s.Has(0) && s.V(1).Hp == 5 &&
            s.Events<SilverLionRemovedRecoveryEvent>().Count(e => e.PlayerSeat == 1 && e.RecoveredAmount == 1) == 1 &&
            s.Events<CardUseDeclaredEvent>().All(e => e.CardId != incoming) && s.Events<CardUseFinishedEvent>().Length == 0,
            "The original hand entity directly replaces the other person's armor, returns real Silver Lion recovery and pauses on its own movement child without inventing a card use.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue" || c.Parameters.GetValueOrDefault("option") == "continue" || c.Id.Value.EndsWith("continue", StringComparison.Ordinal));
        s.Play();
        Need(s.G.CardMovements.Count(m => m.CardId == incoming && m.From == CardLocation.Hand(0) &&
                m.To == CardLocation.Equipment(1) && m.Reason == CardMoveReasons.EquipmentEnter) == 1 &&
            s.G.CardMovements.All(m => m.CardId != incoming || m.To != CardLocation.Processing) &&
            s.G.CardMovements.Count(m => m.CardId == oldArmor && m.From == CardLocation.Equipment(1) &&
                m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.EquipmentReplace) == 1 &&
            s.Events<EquipmentChangedEvent>().Count(e => e.ResolutionId == parent.Id && e.CardId == incoming && e.ReplacedCardId == oldArmor) == 1 &&
            s.V(0).HandCount == 2 && s.Events<HuxinMirrorPaidEvent>().Length == 0,
            "Placement commits each original move once and resumes the original human Play after its real movement benefit.");
        var self = s.G.GetHumanLegalActions().Single(a => a.CardId == spare && a.Kind == LegalActionKind.Equip && a.TargetSeat is null);
        s.Submit(new PlayCardCommand(0, spare, self.TargetSeats, s.G.Revision, s.P!.PromptId, self.PlayedCardKind)); s.Play();
        Need(s.Has(0) && s.V(0).Equipment.Single(c => c.Kind == CardKind.HuxinMirror).Id == spare &&
            s.Events<CardUseDeclaredEvent>().Count(e => e.CardId == spare && e.CardKind == CardKind.HuxinMirror) == 1 &&
            s.Events<CardUseFinishedEvent>().Length == 1,
            "The separate ordinary self-equip path remains a real card use; only the source rule's other-person placement bypasses Use.");
        s.Submit(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        CheckPlacementDoesNotInheritARealHandUseProhibition();
    }

    private static void CheckPlacementDoesNotInheritARealHandUseProhibition()
    {
        var s = Scene.Start(placement: true, ban: true);
        var mirror = s.V(0).Hand.First(c => c.Kind == CardKind.HuxinMirror).Id;
        s.Submit(new UseProgramSkillCommand(0, Driver, "hurt-peer", [], [1], s.G.Revision, s.P!.PromptId)); s.Play();
        Need(s.Events<HandCardColorRestrictionGrantedEvent>().Count(e => e.Restriction.AffectedSeat == 0 &&
                e.Restriction.Source.OwnerSeat == 1) == 2 &&
            s.G.GetHumanLegalActions().All(a => a.CardId != mirror || a.Kind != LegalActionKind.Equip || a.TargetSeat is not null),
            "A genuine opponent damage-triggered two-colour hand ban removes ordinary hand-mirror use through the native use restriction.");
        var place = s.G.GetHumanLegalActions().Single(a => a.CardId == mirror && a.Kind == LegalActionKind.Equip && a.TargetSeat == 1);
        s.Submit(new PlayCardCommand(0, mirror, place.TargetSeats, s.G.Revision, s.P!.PromptId, place.PlayedCardKind));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.V(1).Equipment.Single(c => c.Kind == CardKind.HuxinMirror).Id == mirror && s.Has(1) &&
            s.G.CardMovements.Count(m => m.CardId == mirror && m.From == CardLocation.Hand(0) &&
                m.To == CardLocation.Equipment(1)) == 1 &&
            s.Events<CardUseDeclaredEvent>().All(e => e.CardId != mirror),
            "The very same prohibited hand entity still follows the source rule's legal direct placement and original movement-child return, without using the card.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public bool Has(int seat) => V(seat).Skills!.Any(s => s.ContentId == Skill);
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mirror command."); }
        public void Use(string activation) => Submit(new UseProgramSkillCommand(0, Driver, activation, [], [], G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> condition)
        { var p = P!; var c = p.Choices.Single(condition); Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision)); }
        public int Card(CardKind kind, int? target = null)
        {
            var card = V(0).Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().Single(a => a.CardId == card.Id && a.Kind != LegalActionKind.Recast && a.TargetSeat == target);
            Submit(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind)); return card.Id;
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> ready)
        {
            for (var step = 0; step < 240; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "Mirror fixture must remain a short live native flow.");
                if (P is { } prompt && !(prompt.Kind == DecisionKind.PlayCard && G.ResolutionStack.Count != 0))
                    throw new InvalidOperationException("Unexpected mirror boundary: " + JsonSerializer.Serialize(new { prompt.Kind, prompt.PlayerSeat, prompt.SkillPrompt?.SkillId }));
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Mirror fixture exceeded its bounded native flow.");
        }
        public static Scene Start(int hp = 4, bool placement = false, bool bypass = false, bool chain = false, bool ban = false, bool seal = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheHuxinMirrorPackage(), new Fixture(chain ? 3 : 2, ban, seal));
            IReadOnlyList<ShanheStartingCard> cards = placement ? [new(CardKind.HuxinMirror, Suit.Diamond, 2), new(CardKind.HuxinMirror, Suit.Club, 1)] :
                chain ? [new(CardKind.QinggangSword, Suit.Spade, 6), new(CardKind.FireSlash, Suit.Heart, 7),
                    new(CardKind.Alcohol, Suit.Spade, 9), new(CardKind.IronChain, Suit.Club, 12)] :
                bypass ? [new(CardKind.QinggangSword, Suit.Spade, 6), new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Alcohol, Suit.Spade, 9)] : [];
            var setup = new ShanheBattleSetup(63, 1, "huxin-native", Owner, hp, 0, 0, 0, 0, [], cards,
                placement || bypass || chain ? [] : [new(CardKind.HuxinMirror, Suit.Diamond, 2)],
                Enumerable.Range(1, chain ? 2 : 1).Select(_ => new ShanheCombatantDefinition(Peer, Hp: placement ? 4 : 6)
                { StartingEquipment = placement ? [new(CardKind.SilverLion, Suit.Club, 2)] :
                    bypass || chain ? [new(CardKind.HuxinMirror, Suit.Diamond, 2)] : [] }).ToArray(), [], new string('a', 64));
            var s = new Scene(GameEngine.CreateStandard(new()
            { Seed = setup.BattleSeed, PlayerCount = chain ? 3 : 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2 }, registry));
            s.Submit(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(int count, bool ban, bool seal) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:huxin-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            object A(string id, object effect) => new { id, minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { effect } };
            var skills = new object[]
            {
                new { id = Driver, revision = 1, activations = new object[]
                {
                    A("hurt-1", new { op = "damage", target = "owner", amount = 1 }),
                    A("hurt-2", new { op = "damage", target = "owner", amount = 2 }),
                    new { id = "hurt-peer", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving",
                        usesPerTurn = (int?)null, effects = new[] { new { op = "damage", target = "selectedTarget", amount = 1 } } },
                    A("armor-one", new { op = "grantArmor", target = "owner", amount = 1 })
                } },
                new { id = Ban, revision = 1, triggers = new[] { new { id = "ban-actual-damage-source", window = "afterDamageApplied", subject = "owner",
                    damageOccurrence = "perDamage", optional = false,
                    effects = new object[] { new { op = "selectTarget", target = "owner", targetKind = "eventSource" },
                        seal ? new { op = "grantTurnCardTargetRestriction", target = "selectedTarget", targetRestriction = "armorIneffectiveForTurn" }
                            : (object)new { op = "grantTurnHandCardProhibition", target = "selectedTarget" } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-movement", window = "cardsMoved", subject = "owner",
                    sourceZones = new[] { "hand", "equipment" }, movementOccurrence = "perOwnerBatch",
                    movementReasons = new[] { "equipment.huxin-mirror.prevent", CardMoveReasons.EquipmentEnter.Value }, optional = false,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } },
                        new { op = "draw", target = "owner", amount = 1 } } } } }
            };
            var labels = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实伤害驱动", description = "原生伤害、护甲与防具限制。" },
                [Ban] = new { name = "实际受伤后限制", description = "受伤后对真实伤害来源施加本回合限制。" },
                [Watch] = new { name = "真实镜子移动孩子", description = "原实体付款或放置后摸一张牌。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs)
                b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                { Program = program, ProgramPresentation = programs.Presentations[id] });
            b.AddCard(new("fixture:huxin-fire-slash", "火杀", "基本牌", CardCatalog.Get(CardKind.FireSlash).Description, CardKind.FireSlash));
            b.AddCard(new("fixture:huxin-silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            b.AddGeneral(new(Owner, "护心镜持有者", "supporter", Driver, "shu", 4, [Watch]));
            b.AddGeneral(new(Peer, "真实受赠者", "supporter", ban || seal ? Ban : "standard:none", "wei", 6));
            b.AddDeck(new(Deck, "无自然起手的真实牌堆", 0, 0, [new("standard:dodge", 32)]));
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "护心镜真实牌流", count, count, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = count - 1 }));
        }
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
