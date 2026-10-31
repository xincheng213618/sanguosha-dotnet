using System.Globalization;

namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_wang_yun_ol_WangYunPrograms
{
    internal static bool WangYunMaterial(Card card)=>RuntimeRules_Core_CommonRules.IsSlashCard(card.Kind) || EquipmentCatalog.IsEquipment(card.Kind);

    internal static Dictionary<string,string> WangYunArgs(ProgramSkillFrame f,string branch)=>new(){["program-action"]="wang-yun-plan",["frame-id"]=f.Id.ToString(CultureInfo.InvariantCulture),["branch"]=branch};
}
