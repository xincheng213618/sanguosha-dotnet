namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_qian_zhao_ol_Kuansai
{
    internal static IReadOnlyList<int> KuansaiActualTargets(CardUseFrame use) => Array.AsReadOnly(use.Action!.EffectiveDesignatedTargetSeats.Distinct().ToArray());
}
