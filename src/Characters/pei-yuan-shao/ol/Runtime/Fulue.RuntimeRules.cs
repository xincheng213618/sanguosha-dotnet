namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_pei_yuan_shao_ol_Fulue
{
    internal static FulueOption OtherFulueOption(FulueOption option)=>option==FulueOption.Damage?FulueOption.Obtain:FulueOption.Damage;

    internal static bool FulueCardKind(CardKind kind)=>RuntimeRules_Core_CompletedUndamagedTargetReveal.IsCompletedUndamagedDamageUse(kind);
}
