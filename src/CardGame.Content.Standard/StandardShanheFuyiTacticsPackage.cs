using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-specific card supply from the real deck at each owner's turn start.</summary>
public sealed class StandardShanheFuyiTacticsPackage : IGameContentPackage
{
    public const string FuyiSlashSkillId = "shanhe:current-fuyiqu-2093";
    public const string FuyiDodgeSkillId = "shanhe:current-fuyiqu-2144";
    public const string FuyiPeachSkillId = "shanhe:current-fuyiqu-2094";
    public const string FuyiDismantleSkillId = "shanhe:current-fuyiqu-2095";
    public const string FuyiStealSkillId = "shanhe:current-fuyiqu-2096";
    public const string FuyiDuelSkillId = "shanhe:current-fuyiqu-2097";
    public const string FuyiIronChainSkillId = "shanhe:current-fuyiqu-2098";
    public const string FuyiFireAttackSkillId = "shanhe:current-fuyiqu-2099";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-fuyi-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{FuyiSlashSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["slash","fireSlash","thunderSlash","coldSlash"]}]}]},
          {"id":"{{{FuyiDodgeSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["dodge"]}]}]},
          {"id":"{{{FuyiPeachSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":2,"cardKinds":["peach"]}]}]},
          {"id":"{{{FuyiDismantleSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["dismantlement"]}]}]},
          {"id":"{{{FuyiStealSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["snatch"]}]}]},
          {"id":"{{{FuyiDuelSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["duel"]}]}]},
          {"id":"{{{FuyiIronChainSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["ironChain"]}]}]},
          {"id":"{{{FuyiFireAttackSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-deck-supply","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["fireAttack"]}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{FuyiSlashSkillId}}}":{"name":"拂衣去·杀","description":"回合开始时，你获得1张【杀】。"},
          "{{{FuyiDodgeSkillId}}}":{"name":"拂衣去·闪","description":"回合开始时，你获得1张【闪】。"},
          "{{{FuyiPeachSkillId}}}":{"name":"拂衣去·桃","description":"回合开始时，你获得2张【桃】。"},
          "{{{FuyiDismantleSkillId}}}":{"name":"拂衣去·拆","description":"回合开始时，你获得1张【过河拆桥】。"},
          "{{{FuyiStealSkillId}}}":{"name":"拂衣去·顺","description":"回合开始时，你获得1张【顺手牵羊】。"},
          "{{{FuyiDuelSkillId}}}":{"name":"拂衣去·决","description":"回合开始时，你获得1张【决斗】。"},
          "{{{FuyiIronChainSkillId}}}":{"name":"拂衣去·锁","description":"回合开始时，你获得1张【铁索连环】。"},
          "{{{FuyiFireAttackSkillId}}}":{"name":"拂衣去·火","description":"回合开始时，你获得1张【火攻】。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        foreach (var (id, program) in catalog.Programs)
        {
            var presentation = catalog.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            {
                Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
