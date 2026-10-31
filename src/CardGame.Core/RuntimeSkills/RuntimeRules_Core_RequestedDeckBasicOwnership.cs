namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_RequestedDeckBasicOwnership
{
    internal static PendingDecision RequestedDeckBasicNativeDecision(PendingDecision p) => RuntimeRules_liu_zhang_ol_ZonghuNeeds.ZonghuNativeDecision(p.Choices.Any(c=>c.Parameters.ContainsKey("deck-basic-source"))
        ? p with {Choices=Array.AsReadOnly(p.Choices.Where(c=>!c.Parameters.ContainsKey("deck-basic-source")).ToArray())} : p);
}
