using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The elite companion's extra skill, transcribed from the supplied recruitment panel.</summary>
public sealed class StandardShanheRecruitmentPackage : IGameContentPackage
{
    public const string HuihuoSkillId = "shanhe:companion-huihuo";
    public const string XunyuNormalId = "shanhe:companion-xun-yu-normal";
    public const string XunyuEliteId = "shanhe:companion-xun-yu-elite";
    public PackageManifest Manifest { get; } = new("shanhe-recruitment", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{HuihuoSkillId}}}","revision":1,
           "modifiers":[{"id":"extra-slash","query":"slashLimit","operation":"add","value":1,"priority":0}],
           "triggers":[{"id":"death-fire","window":"ownerDied","subject":"owner","optional":false,
             "effects":[{"op":"selectAllPublicTeamTargets","target":"owner","targetKind":"publicTeamEnemies","allowEmpty":true},
                        {"op":"damageParticipants","target":"selectedTargets","amount":3,"nature":"fire"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{HuihuoSkillId}}}":{"name":"回火","description":"锁定技，出牌阶段，你使用【杀】的限制次数+1。当你死亡后，你对所有敌方角色造成3点火焰伤害。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[HuihuoSkillId];
        builder.AddSkill(new(HuihuoSkillId, presentation.Name, presentation.Description)
        { Program = catalog.Programs[HuihuoSkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked });
        foreach (var elite in new[] { false, true })
            builder.AddGeneral(new(elite ? XunyuEliteId : XunyuNormalId, "荀彧", "xun_yu", "boundary:quhu-current", "wei", 3,
                [elite ? "boundary:jieming-current" : "classic:jieming"])
            { CharacterId = "character:xun-yu", VariantId = elite ? "shanhe-companion-elite" : "shanhe-companion-normal",
                RulesetId = "shanhe-2026", Gender = GeneralGender.Male });
    }
}
