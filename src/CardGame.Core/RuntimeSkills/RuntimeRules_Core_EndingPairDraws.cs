namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_EndingPairDraws
{
    internal static string EndingPairRoundUsage(string state) => "ending-hand-divergence:" + state;
}
