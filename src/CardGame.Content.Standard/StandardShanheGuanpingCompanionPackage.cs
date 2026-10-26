using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The recruitment panel's Guan Ping keeps native Longyin; the elite adds compulsory Moyan.</summary>
public sealed class StandardShanheGuanpingCompanionPackage : IGameContentPackage
{
    public const string NormalId = "classic:guan-ping";
    public const string EliteId = "shanhe:companion-guan-ping-elite";
    public const string MoyanSkillId = "shanhe:companion-moyan";
    public PackageManifest Manifest { get; } = new("shanhe-guanping-companion", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{MoyanSkillId}}}","revision":1,"triggers":[
            {"id":"outside-loss-judgment","window":"cardsMoved","subject":"owner","optional":false,
             "sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch",
             "condition":{"kind":"not","children":[{"kind":"ownerIsTurnPlayer"}]},
             "effects":[
               {"op":"startJudgment","target":"owner","judgmentReason":"shanhe-guanping-moyan",
                "resultBind":"judgment","visibility":"public"},
               {"op":"filterBoundCards","target":"owner","sourceBind":"judgment","resultBind":"red-judgment","suits":["heart","diamond"]},
               {"op":"selectRedJudgmentFireTarget","target":"owner","sourceBind":"red-judgment"},
               {"op":"damage","target":"selectedTarget","amount":1,"nature":"fire","skipIfNoTarget":true},
               {"op":"moveBoundCards","target":"owner","sourceBind":"judgment","destination":"discardPile"}
             ]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{MoyanSkillId}}}":{"name":"魔炎","description":"锁定技，当你回合外失去牌时，你判定，若结果为红色，你对一名其他角色造成1点火焰伤害。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[MoyanSkillId];
        builder.AddSkill(new(MoyanSkillId, presentation.Name, presentation.Description)
        { Program = catalog.Programs[MoyanSkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked });
        builder.AddGeneral(new(EliteId, "关平", "guan_ping", "classic:longyin", "shu", 4, [MoyanSkillId])
        {
            CharacterId = "character:guan-ping", VariantId = "shanhe-companion-elite", RulesetId = "shanhe-2026",
            Gender = GeneralGender.Male
        });
    }
}
