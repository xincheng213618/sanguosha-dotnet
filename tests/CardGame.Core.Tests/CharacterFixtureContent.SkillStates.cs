using System.Text.Json;
using CardGame.Core;

internal static partial class CharacterFixtureContent
{
    internal static SkillProgram SuppressionProgram(string id, int hp, SkillProgram? original = null)
    {
        var rules = $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":{{JsonSerializer.Serialize(id)}},"revision":1,
          "skillStates":[{"target":"owner","action":"suppress",
            "match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.isSource"],0]]},
            "while":["==",["hp","owner"],{{hp}}]}]}]}
        """;
        var presentations = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = id, description = id } } });
        var declared = SkillProgramCatalog.Load(rules, presentations).Programs[id];
        if (original is null) return declared;
        var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(
            original.GameplayHash + "\n" + declared.GameplayHash))).ToLowerInvariant();
        return new(original.Id, original.Revision, hash, original.RuntimeVersion, original.MinimumRulesVersion,
            original.Modifiers, original.ViewAs, original.Activations, original.Triggers, original.Contributions,
            original.CardIdentities, original.BooleanStates, original.DamageModifiers, original.CardPolicies,
            original.LordSkillProjection, original.CannotChallengeDeclarations, original.HpSkillGrants,
            original.EquipmentSuitSkillGrants, original.SkillStates.Concat(declared.SkillStates).ToArray());
    }

}
