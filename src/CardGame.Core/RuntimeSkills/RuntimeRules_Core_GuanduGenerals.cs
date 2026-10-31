
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_GuanduGenerals
{
    internal static IReadOnlyList<int> FreezeGuanduSeats(IEnumerable<int> seats) => Array.AsReadOnly(seats.ToArray());
}
