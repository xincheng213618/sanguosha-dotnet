using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The four-HP companion and exact skill editions shown in the supplied opening-team panel.</summary>
public sealed class StandardShanheChilianCompanionPackage : IGameContentPackage
{
    public const string ChilianId = "shanhe:companion-chilian";
    public const string ChunlaoId = "shanhe:chilian-chunlao";
    public const string LiangyinId = "shanhe:chilian-liangyin";
    public PackageManifest Manifest { get; } = new("shanhe-chilian-companion", new(1, 0, 0),
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ChunlaoId}}}","revision":1,"triggers":[
            {"id":"store-own-or-neighbor-discard-up-to-nine","window":"discardPileReceived","subject":"owner","optional":false,
             "evaluateConditionAtResolution":true,"condition":{"kind":"compare","left":{"kind":"currentOwnedZoneCount","zone":"chunlao"},
                "operator":"lessThan","right":{"kind":"integerConstant","value":9}},
             "effects":[{"op":"storeAdjacentDiscardedSlash","target":"owner","destinationZone":"chunlao"}]},
            {"id":"round-priced-pile-alcohol","window":"dyingResponse","subject":"owner","optional":true,
             "effects":[{"op":"useRoundPricedPileDyingAlcohol","target":"owner","zones":["chunlao"],"stateId":"round-wine-uses"}]}]},
          {"id":"{{{LiangyinId}}}","revision":1,"triggers":[
            {"id":"game-out-draw","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","optional":true,
             "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLivingWithMoreHandCards"},
                        {"op":"draw","target":"selectedTarget","amount":1}]},
            {"id":"game-in-discard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","optional":true,
             "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLivingWithFewerHandCardsAndOwnedCards"},
                        {"op":"selectOwnedCards","target":"selectedTarget","zones":["hand","equipment"],"amount":1,"resultBind":"liangyin-discard"},
                        {"op":"moveBoundCards","target":"owner","sourceBind":"liangyin-discard","destination":"discardPile","awaitMovementTriggers":true}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ChunlaoId}}}":{"name":"醇醪","description":"你或你相邻角色的【杀】因弃置置入弃牌堆后，将之置于你的武将牌上，称为“醇”（至多为9）。当一名角色处于濒死状态时，你可以移去X张“醇”，视为其使用一张【酒】（X为本轮以此法使用【酒】的次数）。"},
          "{{{LiangyinId}}}":{"name":"良姻","description":"当有牌移出游戏时，你可以令手牌数大于你的一名角色摸一张牌。当有牌从游戏外加入任意角色的手牌时，你可以令手牌数小于你的一名角色弃置一张牌。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        foreach (var (id, program) in catalog.Programs)
        {
            var presentation = catalog.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation });
        }
        builder.AddGeneral(new(ChilianId, "赤炼", "", ChunlaoId, "qun", 4, [LiangyinId])
        {
            CharacterId = "character:shanhe-chilian", VariantId = "shanhe-companion-2026",
            RulesetId = "shanhe-2026", Gender = GeneralGender.Female
        });
    }
}
