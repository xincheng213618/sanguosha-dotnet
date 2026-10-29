using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static partial class ShanheCampaignChecks
{
    public static void FinalDefinitionsReuseFrozenKeysAndKeepExternalValidation()
    {
        // Source recipes request a different cached Returning2026 template key.
        // Starting here also exercises that nested factory path before local keys.
        var source = StandardShanheCampaignCatalog.CreateDefinition(25, StandardShanheCampaignCatalog.Source2026EditionId);
        var sourceCodex = StandardShanheCampaignCatalog.CreateCodexDefinition(25, StandardShanheCampaignCatalog.Source2026EditionId);
        Require(source.SourceDifficultyId == 25 && sourceCodex.SourceDifficultyId == 25 &&
            ReferenceEquals(source, StandardShanheCampaignCatalog.CreateDefinition(25, StandardShanheCampaignCatalog.Source2026EditionId)) &&
            ReferenceEquals(sourceCodex, StandardShanheCampaignCatalog.CreateCodexDefinition(25, StandardShanheCampaignCatalog.Source2026EditionId)) &&
            !ReferenceEquals(source, sourceCodex),
            "Source twenty-five builds its ordinary and codex templates without a same-key Lazy cycle and reuses each final snapshot independently.");

        var ordinary = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var codex = StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        Require(ReferenceEquals(ordinary, StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId)) &&
            ReferenceEquals(codex, StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId)) &&
            !ReferenceEquals(ordinary, codex) &&
            !ReferenceEquals(ordinary, StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId)) &&
            !ReferenceEquals(ordinary, StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.LocalEditionId)) &&
            !ReferenceEquals(ordinary, source),
            "The entry kind, edition and exact difficulty are independent final-definition cache keys.");
        RejectMutation(ordinary.Nodes);
        RejectMutation(ordinary.Rewards);

        var index = Enumerable.Range(0, ordinary.Nodes.Count).First(i => ordinary.Nodes[i].NextNodeIds.Count > 0);
        var originalNode = ordinary.Nodes[index];
        RejectMutation(originalNode.NextNodeIds);
        var mutableNext = originalNode.NextNodeIds.ToList();
        var mutableNodes = ordinary.Nodes.ToArray();
        const string changedName = "外部定义独立修改";
        mutableNodes[index] = originalNode with { Name = changedName, NextNodeIds = mutableNext };
        var external = ordinary with { Nodes = mutableNodes };
        var detached = ShanheCampaignContent.FreezeAndValidate(external);
        mutableNext.Add("fixture:unknown-cache-route");
        Require(detached.Nodes[index].Name == changedName && detached.Nodes[index].NextNodeIds.SequenceEqual(originalNode.NextNodeIds) &&
            ordinary.Nodes[index].Name == originalNode.Name && ReferenceEquals(ordinary.Nodes[index], originalNode),
            "An external with-variant is detached by the ordinary deep-freeze boundary and cannot mutate its cached source.");
        MustThrow(() => ShanheCampaignContent.FreezeAndValidate(external));
        mutableNodes[index] = originalNode;
        Require(detached.Nodes[index].Name == changedName &&
            ReferenceEquals(ordinary, StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId)),
            "Invalid external routes still fail validation; later caller-array changes leave both the detached variant and the original cache intact.");
    }
}
