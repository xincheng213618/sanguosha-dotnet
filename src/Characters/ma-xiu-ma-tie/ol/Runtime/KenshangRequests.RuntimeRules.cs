
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_ma_xiu_ma_tie_ol_KenshangRequests
{
    internal static string KenshangOriginalDecisionHash(PendingDecision decision)=>Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(decision))));
}
