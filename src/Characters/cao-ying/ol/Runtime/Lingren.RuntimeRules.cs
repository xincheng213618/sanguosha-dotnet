
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_cao_ying_ol_Lingren
{
    internal static bool CaoYingDamageKind(CardKind kind) => RuntimeRules_Core_CommonRules.IsSlashCard(kind) || kind is CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.UnexpectedAssault;
}
