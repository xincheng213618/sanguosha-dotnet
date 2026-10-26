using System.Collections.ObjectModel;

namespace CardGame.Content.Standard;

/// <summary>Removed equivalent editions and their surviving artwork owners. No gameplay or save redirects.</summary>
public static class GeneralRulesCatalog
{
    // Explicit review, not a name or artwork heuristic. See docs/content/GENERAL_RULES_EQUIVALENCE.md.
    public static IReadOnlyDictionary<string, string> Aliases { get; } = CreateAliases();

    public static string CanonicalId(string id) => Aliases.GetValueOrDefault(id, id);
    public static bool IsCanonical(string id) => !Aliases.ContainsKey(id);

    private static IReadOnlyDictionary<string, string> CreateAliases()
    {
        var aliases = new Dictionary<string, string>(GodGeneralRulesCatalog.Aliases, StringComparer.Ordinal)
        {
            ["boundary:da-qiao-mobile"] = "boundary:da-qiao",
            ["boundary:gan-ning-mobile"] = "boundary:gan-ning",
            ["boundary:guan-yu-mobile"] = "boundary:guan-yu",
            ["boundary:guo-jia-mobile"] = "boundary:guo-jia",
            ["boundary:huang-gai-mobile"] = "boundary:huang-gai",
            ["boundary:lu-bu-mobile"] = "boundary:lu-bu",
            ["boundary:lu-xun-mobile"] = "boundary:lu-xun",
            ["boundary:sima-yi-mobile"] = "boundary:sima-yi",
            ["boundary:sun-shangxiang-mobile"] = "boundary:sun-shangxiang",
            ["boundary:xu-sheng-mobile"] = "boundary:xu-sheng",
            ["boundary:zhen-ji-mobile"] = "boundary:zhen-ji",
            ["boundary:zhou-yu-mobile"] = "boundary:zhou-yu",
            ["boundary:zhuge-liang-mobile"] = "boundary:zhuge-liang",
            ["mobile:bei-mi-hu"] = "ol:bei-mi-hu",
            ["mobile:bu-zhi"] = "ol:bu-zhi",
            ["mobile:cao-cao"] = "classic:cao-cao",
            ["mobile:cao-chong"] = "classic:cao-chong",
            ["mobile:cao-hong"] = "ol:cao-hong",
            ["mobile:cao-rui"] = "classic:cao-rui",
            ["mobile:cao-xiu"] = "classic:cao-xiu",
            ["mobile:cao-zhang"] = "classic:cao-zhang",
            ["mobile:cao-zhi"] = "classic:cao-zhi",
            ["mobile:chen-gong"] = "classic:chen-gong",
            ["mobile:chen-lin"] = "ol:chen-lin",
            ["mobile:dong-zhuo"] = "classic:dong-zhuo",
            ["mobile:fu-wan"] = "ol:fu-wan",
            ["mobile:gan-ning"] = "classic:gan-ning",
            ["mobile:guan-ping"] = "classic:guan-ping",
            ["mobile:guan-yu"] = "classic:guan-yu",
            ["mobile:guo-tu-feng-ji"] = "classic:guo-tu-feng-ji",
            ["mobile:han-dang"] = "classic:han-dang",
            ["mobile:hao-zhao"] = "classic:hao-zhao",
            ["mobile:hua-tuo"] = "classic:hua-tuo",
            ["mobile:huang-gai"] = "classic:huang-gai",
            ["mobile:huang-hao"] = "classic:huang-hao",
            ["mobile:huang-yueying"] = "classic:huang-yueying",
            ["mobile:jia-xu"] = "classic:jia-xu",
            ["mobile:jian-yong"] = "classic:jian-yong",
            ["mobile:jiang-wei"] = "classic:jiang-wei",
            ["mobile:li-tong"] = "ol:li-tong",
            ["mobile:liao-hua"] = "classic:liao-hua",
            ["mobile:ling-tong"] = "classic:ling-tong",
            ["mobile:liu-biao"] = "classic:liu-biao",
            ["mobile:liu-chen"] = "classic:liu-chen",
            ["mobile:liu-feng"] = "classic:liu-feng",
            ["mobile:liu-qi"] = "ol:liu-qi",
            ["mobile:liu-yu"] = "classic:liu-yu",
            ["mobile:lu-bu"] = "classic:lu-bu",
            ["mobile:lu-xun"] = "classic:lu-xun",
            ["mobile:man-chong"] = "classic:man-chong",
            ["mobile:mi-zhu"] = "ol:mi-zhu",
            ["mobile:pan-zhang-ma-zhong"] = "classic:pan-zhang-ma-zhong",
            ["mobile:pang-de"] = "classic:pang-de",
            ["mobile:qin-mi"] = "classic:qin-mi",
            ["mobile:quan-cong"] = "classic:quan-cong",
            ["mobile:sha-mo-ke"] = "classic:sha-mo-ke",
            ["mobile:sima-lang"] = "ol:sima-lang",
            ["mobile:sun-deng"] = "classic:sun-deng",
            ["mobile:sun-jian"] = "classic:sun-jian",
            ["mobile:sun-lu-ban"] = "classic:sun-lu-ban",
            ["mobile:sun-qian"] = "ol:sun-qian",
            ["mobile:wang-ji"] = "classic:wang-ji",
            ["mobile:wang-ping"] = "classic:wang-ping",
            ["mobile:wei-yan"] = "classic:wei-yan",
            ["mobile:wen-pin"] = "ol:wen-pin",
            ["mobile:wu-guo-tai"] = "classic:wu-guo-tai",
            ["mobile:xi-zhi-cai"] = "ol:xi-zhi-cai",
            ["mobile:xiahou-ba"] = "ol:xiahou-ba",
            ["mobile:xiahou-dun"] = "classic:xiahou-dun",
            ["mobile:xiahou-yuan"] = "classic:xiahou-yuan",
            ["mobile:xu-huang"] = "classic:xu-huang",
            ["mobile:xu-shi"] = "classic:xu-shi",
            ["mobile:xun-you"] = "classic:xun-you",
            ["mobile:yan-yan"] = "classic:yan-yan",
            ["mobile:yu-fan"] = "classic:yu-fan",
            ["mobile:yue-jin"] = "ol:yue-jin",
            ["mobile:zhang-fei"] = "classic:zhang-fei",
            ["mobile:zhang-lu"] = "ol:zhang-lu",
            ["mobile:zhang-xiu"] = "classic:zhang-xiu",
            ["mobile:zhao-yun"] = "classic:zhao-yun",
            ["mobile:zhu-ran"] = "classic:zhu-ran",
            ["mobile:zhu-rong"] = "classic:zhu-rong",
            ["mobile:zhu-zhi"] = "classic:zhu-zhi",
            ["mobile:zhuge-dan"] = "classic:zhuge-dan",
            ["mobile:zhuge-liang"] = "classic:zhuge-liang",
            ["mobile:zhuge-zhan"] = "classic:zhuge-zhan",
            ["mobile:zu-mao"] = "ol:zu-mao",
        };
        return new ReadOnlyDictionary<string, string>(aliases);
    }
}
