using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheAvatarPoolChecks
{
    public static void PublishedZuociDrawsOrdinaryAvatarsWithoutChangingFixedSeats()
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var encounter = definition.Nodes.Single(node => node.Id == "dagu").Encounters!
            .Single(item => item.Id == "dagu:zuoci-huatuo-s5");
        var setup = new ShanheBattleSetup(83, 1, "dagu", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [],
            encounter.Enemies, encounter.Allies ?? [], ShanheCampaignContent.GetContentHash(definition))
        {
            // The future form is a real playable ordinary general, held out until deployment.
            BossTransition = new(0, "classic:zhang-liao", 10, 10)
        };
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        var expected = new[] { setup.PlayerGeneralId }.Concat(setup.AllyRoster.Select(item => item.GeneralId))
            .Concat(setup.EnemyRoster.Select(item => item.GeneralId)).ToArray();
        Need(game.CreateSnapshot(0).Players.Select(player => player.GeneralId).SequenceEqual(expected),
            "The full ordinary avatar library never adds a seat or randomizes the published fixed encounter.");
        Need(game.Submit(new StartGameCommand()).Accepted, "The actual source Zuo Ci encounter starts.");
        var owner = 1 + setup.AllyRoster.Count;
        PrivateGeneralLibrarySnapshot? library = null;
        for (var step = 0; step < 100; step++)
        {
            library = game.CreateSnapshot(owner).Players[owner].PrivateGeneralLibraries?.SingleOrDefault();
            if (library?.RevealedGeneralId is not null) break;
            game.DriveAdvanceOneStep();
        }
        Need(library is { Count: 3, GeneralIds.Count: 3, RevealedGeneralId: not null, DeclaredSkillId: not null } &&
            library.GeneralIds.Distinct().Count() == 3 && library.GeneralIds.All(id =>
                StandardShanheCampaignCatalog.GetPlayerGeneralIds().Contains(id) && !expected.Contains(id) &&
                id != setup.BossTransition.GeneralId),
            "Native S5 Huashen obtains and declares three real ordinary avatars while excluding living participants and the reserved second form.");
        Need(registry.GetSkill(library!.DeclaredSkillId!).ImplementationStatus == SkillImplementationStatus.Complete &&
            game.CreateSnapshot(owner).Players[owner].Skills!.Any(skill => skill.ContentId == library.DeclaredSkillId) &&
            game.CreateSnapshot(0).Players[owner].PrivateGeneralLibraries is [{ Count: 3, GeneralIds: null }],
            "The actual borrowed skill is granted, and another player sees the public count without learning the hidden library.");
        Need(game.CreateSnapshot(0).Players.Select(player => player.GeneralId).SequenceEqual(expected) &&
            game.Events.All(item => item.Payload is not ShanheBossStageStartedEvent),
            "Avatars remain private skill resources; neither their initialization nor declaration deploys the reserved boss phase.");
    }

    private static void Need(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }
}
