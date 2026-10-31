using CardGame.Content.Standard;
using CardGame.Core;

internal static class ShortPlanningAiChecks
{
    public static void RemovesDefenseBeforeAvailableSlashAndKeepsLegacyPriority()
    {
        foreach (var removalKind in new[] { LegalActionKind.Dismantlement, LegalActionKind.Snatch })
        {
            var scene = Scene(removalKind, CardKind.Slash, CardKind.RenwangShield);
            var weapon = PublicCard(203, CardKind.QilinBow);
            var view = Change(scene.View, 1, player => player with { Equipment = [scene.Armor, weapon] });
            var removeWeapon = scene.Remove with { TargetCardId = weapon.Id, Description = "remove weapon" };
            var actions = new[] { removeWeapon, scene.Remove, scene.Slash, End };
            var old = Brain(AiDecisionPolicy.CooperativeCategoryGiftVersion).ChoosePlay(view, actions, 1);
            var current = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(view, actions, 1);
            Need(old.Action == removeWeapon && current.Action == scene.Remove,
                "One-step planning must prioritize removing the existing Slash's defense rather than the higher-priced weapon; the historical policy keeps its former priority.");
            Need(Score(current.Thought, scene.Remove) > Score(old.Thought, scene.Remove) + 19 &&
                current.Thought.Candidates.Single(candidate => candidate.Action == scene.Remove).Reason.Contains("一步规划"),
                "The prepared Slash must contribute a visible, discounted continuation value.");
            var noSlash = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(view, [removeWeapon, scene.Remove, End], 1);
            Need(noSlash.Action == removeWeapon,
                "Without a published follow-up Slash the AI must not invent an attack or award an armor-combo bonus.");
        }
        var bagua = Scene(LegalActionKind.Dismantlement, CardKind.Slash, CardKind.BaguaFormation);
        var baguaActions = new[] { bagua.Remove, bagua.Slash, End };
        var baguaOld = Brain(AiDecisionPolicy.CooperativeCategoryGiftVersion).ChoosePlay(bagua.View, baguaActions, 1);
        var baguaCurrent = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(bagua.View, baguaActions, 1);
        Need(Score(baguaCurrent.Thought, bagua.Remove) > Score(baguaOld.Thought, bagua.Remove) + 19,
            "Removing an uncertain Bagua response contributes a smaller discounted continuation value than a deterministic blocker.");
    }

    public static void SelectsFireAndRedSlashAgainstPublicArmor()
    {
        var vine = Scene(LegalActionKind.Dismantlement, CardKind.FireSlash, CardKind.Tengjia);
        var normalMaterial = PublicCard(103, CardKind.Slash);
        var normal = new LegalAction(LegalActionKind.Slash, normalMaterial.Id, 1, "normal slash")
            { SlashTraits = new(IsRed: false, IgnoresArmor: false, Nature: DamageNature.Normal) };
        var vineView = Change(Change(vine.View, 0, player => player with
            { Hand = player.Hand.Append(normalMaterial).ToArray(), HandCount = 3 }),
            1, player => player with { Hp = 2 });
        var fire = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(vineView,
            [vine.Remove, normal, vine.Slash, End], 1);
        Need(fire.Action == vine.Slash && Score(fire.Thought, normal) < 0 &&
            fire.Thought.Candidates.Single(candidate => candidate.Action == vine.Remove).Reason.Contains("火焰弱点"),
            "Compare the best attack on both sides: an available normal Slash cannot justify removing Vine when another fire Slash already exploits it.");
        var fireNatureNormal = normal with { SlashTraits = new(IsRed: false, IgnoresArmor: false, Nature: DamageNature.Fire) };
        var crimsonView = Change(vineView, 0, player => player with { Equipment = [PublicCard(301, CardKind.CrimsonSoulZither)] });
        var crimson = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(crimsonView,
            [fireNatureNormal, vine.Slash, End], 1);
        Need(crimson.Action == vine.Slash && Score(crimson.Thought, fireNatureNormal) < 0,
            "Vine blocks an ordinary Slash card before Crimson Soul Zither changes damage nature; an actual Fire Slash still exploits its vulnerability.");

        var robe = Scene(LegalActionKind.Dismantlement, CardKind.Slash, CardKind.RedCottonRobe);
        var robeFire = robe.Slash with { SlashTraits = new(IsRed: false, IgnoresArmor: false, Nature: DamageNature.Fire) };
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(robe.View, [robeFire, End], 1).Action == End,
            "Red Cotton Robe instead checks final fire damage nature, including an ordinary Slash converted to fire damage.");
        var cold = Scene(LegalActionKind.Dismantlement, CardKind.ColdSlash, CardKind.RedCottonRobe);
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(cold.View, [cold.Slash, End], 1).Action == End,
            "The registered robe prevents Cold damage too, so retain the blocked Cold Slash and end play.");

        var shield = Scene(LegalActionKind.Dismantlement, CardKind.Slash, CardKind.RenwangShield);
        var redMaterial = PublicCard(103, CardKind.Slash) with { Suit = Suit.Heart };
        var red = new LegalAction(LegalActionKind.Slash, redMaterial.Id, 1, "red slash")
            { SlashTraits = new(IsRed: true, IgnoresArmor: false, Nature: DamageNature.Normal) };
        var shieldView = Change(shield.View, 0, player => player with
            { Hand = player.Hand.Append(redMaterial).ToArray(), HandCount = 3 });
        var selected = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(shieldView,
            [shield.Slash, red, End], 1);
        Need(selected.Action == red && Score(selected.Thought, shield.Slash) < 0,
            "Prefer the published red Slash that penetrates Renwang Shield instead of wasting a black Slash.");
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(shield.View, [shield.Slash, End], 1).Action == End,
            "When only a publicly blocked black Slash remains, retain it and end play.");
        var boostedView = Change(Change(shield.View, 0, player => player with
            { HasAlcoholEffect = true, Equipment = [PublicCard(301, CardKind.GudingBlade)] }),
            1, player => player with { Hp = 1, MaxHp = 10 });
        var boosted = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(boostedView, [shield.Slash, End], 1);
        var boostedOld = Brain(AiDecisionPolicy.CooperativeCategoryGiftVersion).ChoosePlay(boostedView, [shield.Slash, End], 1);
        Need(Score(boostedOld.Thought, shield.Slash) > 150 && boosted.Action == End && Score(boosted.Thought, shield.Slash) < 0,
            "A fully blocked Slash remains unusable even when alcohol, empty-hand weapon and low-HP finishing bonuses inflate the old score above a fixed penalty.");
        var bypassView = Change(shield.View, 0, player => player with { Equipment = [PublicCard(301, CardKind.QinggangSword)] });
        var bypassSlash = shield.Slash with { SlashTraits = new(IsRed: false, IgnoresArmor: true, Nature: DamageNature.Normal) };
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(bypassView, [bypassSlash, End], 1).Action == bypassSlash,
            "A public armor-ignoring weapon removes the modeled obstacle, so the same black Slash remains useful.");
        var uncertain = shield.Slash with { SlashTraits = null };
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(shield.View, [uncertain, End], 1).Action == uncertain,
            "Without declared-color traits, printed black material cannot establish a fully blocked use.");
    }

    public static void OnlyPlansIndependentPublishedAttacksAndNeverMutatesInput()
    {
        var scene = Scene(LegalActionKind.Dismantlement, CardKind.Slash, CardKind.RenwangShield);
        var sharedMaterial = scene.Remove with { CardId = scene.Slash.CardId, PlayedCardKind = CardKind.Dismantlement };
        var actions = new[] { sharedMaterial, scene.Slash, End };
        var before = SnapshotJson.Serialize(scene.View);
        var current = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(scene.View, actions, 1);
        Need(!current.Thought.Candidates.Single(candidate => candidate.Action == sharedMaterial).Reason.Contains("一步规划"),
            "One physical card may be used as Qixi or Slash, never both steps of the plan.");
        var callbackCount = 0;
        var independent = new[] { scene.Remove, scene.Slash, End };
        var result = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(scene.View, independent, 1, (action, target) =>
        {
            Need(independent.Contains(action) && target == 1,
                "Planning may query current published attacks only, never a forged projected action.");
            callbackCount++;
            return PublicDamagePrediction.Calculate(4, 0, 1, PublicDamagePaymentKind.HpLoss, 4);
        });
        var legacy = Brain(AiDecisionPolicy.CooperativeCategoryGiftVersion).ChoosePlay(scene.View, independent, 1);
        Need(Math.Abs(Score(result.Thought, scene.Remove) - Score(legacy.Thought, scene.Remove)) < .01 &&
            callbackCount > 0 && SnapshotJson.Serialize(scene.View) == before,
            "Published direct HP loss bypasses armor preparation; scoring leaves the source snapshot unchanged.");
    }

    private static readonly LegalAction End = new(LegalActionKind.EndPlay, null, null, "end");
    private static SimpleAiBrain Brain(int policy) => new(0, 83, policy);
    private static double Score(AiThoughtRecord thought, LegalAction action) =>
        thought.Candidates.Single(candidate => candidate.Action == action).Score;
    private static CardSnapshot PublicCard(int id, CardKind kind) =>
        new(id, kind, Suit.Spade, 7, CardCatalog.Get(kind).DisplayName, "7");
    private static GameSnapshot Change(GameSnapshot view, int seat, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == seat ? change(player) : player).ToArray() };
    private static (GameSnapshot View, CardSnapshot Armor, LegalAction Remove, LegalAction Slash) Scene(
        LegalActionKind removal, CardKind slashKind, CardKind armorKind)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 721019, HumanSeat = 0, HumanRole = Role.Rebel }, StandardContentRegistry.Create());
        var view = game.CreateSnapshot(0) with { Status = EngineStatus.AwaitingHumanPlay, Phase = TurnPhase.Play, CurrentSeat = 0, TurnNumber = 1,
            ModeKind = ContentModeKind.Team };
        var removalCard = PublicCard(101, removal == LegalActionKind.Dismantlement ? CardKind.Dismantlement : CardKind.Snatch);
        var slashCard = PublicCard(102, slashKind);
        var armor = PublicCard(201, armorKind);
        view = view with { Players = view.Players.Select(player => player with
        {
            Hp = 4, MaxHp = 4, Armor = 0, IsAlive = player.Seat is 0 or 1,
            TeamId = player.Seat == 0 ? "ally" : "enemy", Skills = [], SecondarySkills = [],
            Hand = player.Seat == 0 ? [removalCard, slashCard] : [], HandCount = player.Seat == 0 ? 2 : 0,
            Equipment = player.Seat == 1 ? [armor] : [], Judgment = []
        }).ToArray() };
        return (view, armor, new(removal, removalCard.Id, 1, "remove armor", TargetCardId: armor.Id),
            new LegalAction(LegalActionKind.Slash, slashCard.Id, 1, "slash")
            { SlashTraits = new(IsRed: false, IgnoresArmor: false, Nature: slashKind switch
                { CardKind.FireSlash => DamageNature.Fire, CardKind.ThunderSlash => DamageNature.Thunder,
                  CardKind.ColdSlash => DamageNature.Cold, _ => DamageNature.Normal }) });
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
