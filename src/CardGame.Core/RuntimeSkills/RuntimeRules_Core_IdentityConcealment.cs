
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_IdentityConcealment
{
    internal const string IdentityConcealedGeneralId = "concealment:ol";

    internal static GeneralDefinition IdentityConcealedGeneral()=>new(IdentityConcealedGeneralId,"隐匿","",[]);
}
