
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_HellSoulCards
{
    internal static string HellSoulGrant(long id) => $"card-use:hell-return-soul:{id}";
}
