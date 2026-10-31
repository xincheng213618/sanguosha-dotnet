
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_mi_heng_le_SiruiQueries
{
    internal static int SiruiNameLength(CardKind kind) => CardCatalog.Get(kind).DisplayName.EnumerateRunes().Count();
}
