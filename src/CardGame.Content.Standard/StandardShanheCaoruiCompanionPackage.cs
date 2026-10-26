using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Screenshot-specific Cao Rui helpers: printed support skills plus the elite Luanxin.</summary>
public sealed class StandardShanheCaoruiCompanionPackage : IGameContentPackage
{
    public const string NormalId = "shanhe:companion-cao-rui-normal";
    public const string EliteId = "shanhe:companion-cao-rui-elite";
    public const string LuanxinSkillId = "shanhe:companion-luanxin";
    public PackageManifest Manifest { get; } = new("shanhe-caorui-companion", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{LuanxinSkillId}}}","revision":1,"triggers":[
            {"id":"foreign-hand-discard","window":"discardPileReceived","subject":"owner","sourceZones":["hand"],
             "discardOwnerScope":"own","movementDiscardOnly":true,"movementOccurrence":"perBatch","optional":false,
             "effects":[{"op":"adjustPersistentHandLimit","target":"owner","amount":1,"stateId":"discarded-hand"}]},
            {"id":"received-damage","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":false,
             "effects":[{"op":"changeMaximumHp","target":"owner","amount":1}]},
            {"id":"dealt-damage","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,
             "effects":[{"op":"recover","target":"owner","amount":1}]},
            {"id":"other-hand-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
             "movementOccurrence":"perBatch","optional":false,
             "effects":[{"op":"queueShanheLuanxinNextDraw","target":"owner"}]}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{LuanxinSkillId}}}":{"name":"乱心","description":"锁定技，你被弃置手牌后，你的手牌上限+1；你受到伤害后，你的体力上限+1；你造成伤害后，回复1点体力；你获得其他角色手牌后，下个摸牌阶段摸牌数+1。"}
        }}
        """));
    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[LuanxinSkillId];
        builder.AddSkill(new(LuanxinSkillId, presentation.Name, presentation.Description)
        { Program = catalog.Programs[LuanxinSkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked });
        foreach (var elite in new[] { false, true })
            builder.AddGeneral(new(elite ? EliteId : NormalId, "曹叡", "cao_rui", "classic:huituo", "wei", 3,
                elite ? ["classic:mingjian", LuanxinSkillId] : ["classic:mingjian"])
            { CharacterId = "character:cao-rui", VariantId = elite ? "shanhe-companion-elite" : "shanhe-companion-normal",
                RulesetId = "shanhe-2026", Gender = GeneralGender.Male });
    }
}
