using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The fixed native entry for a validated, already-paid live expedition purchase.</summary>
public sealed class StandardShanheMarketPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-live-market", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheMarketPrograms.ExecutionSkillId}}}","revision":1,"triggers":[
            {"id":"{{{ShanheMarketPrograms.PurchaseTriggerId}}}","window":"skillsChanged","subject":"owner","optional":false,
             "isSkillInvocation":false,"effects":[{"op":"applyShanheMarketPurchase","target":"owner"}]}]}
        ]}
        """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [ShanheMarketPrograms.ExecutionSkillId] =
                new { name = "行军集市", description = "结算已经支付的行军集市购买。" } } })));
    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value; var id = ShanheMarketPrograms.ExecutionSkillId;
        var p = catalog.Presentations[id];
        builder.AddSkill(new(id, p.Name, p.Description)
            { Program = catalog.Programs[id], ProgramPresentation = p, Tags = SkillTag.Locked });
    }
}
