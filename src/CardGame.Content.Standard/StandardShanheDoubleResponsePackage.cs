using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheDoubleResponsePackage : IGameContentPackage
{
    public const string HanzhanSkillId = "shanhe:current-hanzhan-2111";
    public const string XulijianSkillId = "shanhe:current-xulijian-2242";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-double-response", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{HanzhanSkillId}}}","revision":1,"cardPolicies":[
            {"id":"duel-two-slashes","kind":"minimumResponseCount","cardKinds":["duel"],"requiredCardKinds":["slash"],"value":2}]},
          {"id":"{{{XulijianSkillId}}}","revision":1,"cardPolicies":[
            {"id":"arrows-two-dodges","kind":"minimumResponseCount","cardKinds":["arrowBarrage"],"requiredCardKinds":["dodge"],"value":2}]}
        ]}
        """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [HanzhanSkillId] = new { name = "酣战", description = "与你【决斗】的角色每次需打出两张【杀】。" },
                [XulijianSkillId] = new { name = "蓄力箭", description = "你使用的【万箭齐发】其他角色需要打出2张【闪】来响应。" }
            } })));

    public void Register(IContentRegistryBuilder builder)
    {
        foreach (var (id, program) in Programs.Value.Programs)
        {
            var presentation = Programs.Value.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false) });
        }
    }
}
