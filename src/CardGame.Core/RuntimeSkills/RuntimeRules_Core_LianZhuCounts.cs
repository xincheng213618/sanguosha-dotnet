namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_LianZhuCounts
{
    internal static int LianZhuChoiceAmount(LianZhuReceipt receipt) => receipt.CountPolicy?.Amount ?? 2;
}
