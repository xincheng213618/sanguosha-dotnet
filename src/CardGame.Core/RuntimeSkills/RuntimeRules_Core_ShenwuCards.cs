
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ShenwuCards
{
    internal static string ShenwuTransplantGrant(long id) => $"card-use:shenwu-transplant:{id}";
}
