using System.Globalization;

namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules__shared_OlMoRumo
{
    internal static string OlMoRumoInstance(long entryId) => "ol:mo-rumo@" + entryId.ToString(CultureInfo.InvariantCulture);
}
