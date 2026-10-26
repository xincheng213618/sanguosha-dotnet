namespace CardGame.Wpf.Presentation;

// Gallery organization is presentation metadata, independent of rule-package IDs and save fingerprints.
// Classification sources and version choices: docs/GENERAL_GALLERY.md.
public static class GeneralGalleryCatalog
{
    public static IReadOnlyList<GallerySeries> Series { get; } = Array.AsReadOnly<GallerySeries>(
    [
        new("all", "全部", "浏览全部武将"),
        new("custom", "自定义", "自己创建的武将与技能"),
        new("standard", "标准", "标准版武将"),
        new("myth", "神话再临", "风 · 火 · 林 · 山 · 阴 · 雷"),
        new("fame", "一将成名", "一将成名 · 一至七"),
        new("boundary", "界限突破", "标准与神话再临的界限突破武将"),
        new("boundary-fame", "界一将", "一将成名的界限突破武将"),
        new("god", "神武将", "神将专属图鉴"),
        new("sp", "SP", "SP 武将"),
        new("mou", "谋", "谋系列武将"),
        new("friend", "友", "友系列武将"),
        new("shi", "势", "兵势篇武将"),
        new("le", "乐", "正音雅乐系列武将"),
        new("wumiao", "武庙", "武庙系列武将"),
        new("magic", "魔", "魔系列武将"),
        new("other", "其他扩展", "其他扩展武将")
    ]);

    public static IReadOnlyList<GalleryGroup> Groups { get; } =
    [
        new("custom", "custom", "自定义", "自定义武将"),
        new("standard", "standard", "标准", "标准版"),
        new("myth-wind", "myth", "风", "神话再临 · 风"),
        new("myth-fire", "myth", "火", "神话再临 · 火"),
        new("myth-forest", "myth", "林", "神话再临 · 林"),
        new("myth-mountain", "myth", "山", "神话再临 · 山"),
        new("myth-yin", "myth", "阴", "神话再临 · 阴"),
        new("myth-thunder", "myth", "雷", "神话再临 · 雷"),
        new("fame-1", "fame", "一将 · 一", "一将成名 · 一"),
        new("fame-2", "fame", "一将 · 二", "一将成名 · 二"),
        new("fame-3", "fame", "一将 · 三", "一将成名 · 三"),
        new("fame-4", "fame", "一将 · 四", "一将成名 · 四"),
        new("fame-5", "fame", "一将 · 五", "一将成名 · 五"),
        new("fame-6", "fame", "一将 · 六", "一将成名 · 六"),
        new("fame-7", "fame", "一将 · 七", "一将成名 · 七"),
        new("boundary", "boundary", "界限突破", "界限突破"),
        new("boundary-fame", "boundary-fame", "界一将", "界一将"),
        new("god", "god", "神武将", "神武将"),
        new("sp", "sp", "SP", "SP 武将"),
        new("mou", "mou", "谋", "谋系列"),
        new("friend", "friend", "友", "友系列"),
        new("shi", "shi", "势", "兵势篇"),
        new("le", "le", "乐", "正音雅乐"),
        new("wumiao", "wumiao", "武庙", "武庙"),
        new("magic", "magic", "魔", "魔系列"),
        new("other", "other", "其他", "其他扩展")
    ];

    private static readonly IReadOnlyDictionary<string, string> GeneralGroups = BuildGeneralGroups();

    public static bool IsVisible(string generalId) =>
        CardGame.Content.Standard.GeneralRulesCatalog.IsCanonical(generalId) &&
        !generalId.StartsWith("standard:", StringComparison.Ordinal) &&
        !generalId.StartsWith("composed:", StringComparison.Ordinal) &&
        !generalId.StartsWith("national:", StringComparison.Ordinal);

    public static GalleryGroup Classify(string generalId)
    {
        if (!GeneralGroups.TryGetValue(generalId, out var groupId))
        {
            groupId = generalId.Split(':', 2)[0] switch
            {
                "custom" => "custom",
                "sp" => "sp",
                "mou" => "mou",
                "friend" => "friend",
                "wumiao" => "wumiao",
                "le" => "le",
                // New formal variants need an explicit assignment; never guess from a shared name.
                _ => "other"
            };
        }
        return Groups.Single(group => group.Id == groupId);
    }

    private static IReadOnlyDictionary<string, string> BuildGeneralGroups()
    {
        var groups = new Dictionary<string, string>(StringComparer.Ordinal);
        groups.Add("mobile:cao-pi", "myth-forest");
        groups.Add("mobile:xu-huang", "myth-forest");
        groups.Add("mobile:dong-zhuo", "myth-forest");
        groups.Add("mobile:sun-ce", "myth-mountain");
        groups.Add("mobile:liu-shan", "myth-mountain");
        groups.Add("mobile:zhong-hui", "fame-2");
        groups.Add("mobile:pang-tong", "myth-fire");
        groups.Add("mobile:xun-yu", "myth-fire");
        groups.Add("mobile:yuan-shao", "myth-fire");
        groups.Add("mobile:meng-huo", "myth-forest");
        groups.Add("mobile:zhu-rong", "myth-forest");
        groups.Add("mobile:sun-jian", "myth-forest");
        groups.Add("mobile:guo-jia", "standard");
        groups.Add("mobile:zhou-yu", "standard");
        groups.Add("mobile:huang-zhong", "myth-wind");
        groups.Add("mobile:pang-de", "myth-fire");
        groups.Add("mobile:da-qiao", "standard");
        groups.Add("mobile:sun-shangxiang", "standard");
        groups.Add("mobile:diao-chan", "standard");
        groups.Add("mobile:xiahou-dun", "standard");
        groups.Add("mobile:liu-bei", "standard");
        groups.Add("mobile:zhuge-liang", "standard");
        groups.Add("mobile:lu-meng", "standard");
        groups.Add("mobile:zhang-liao", "standard");
        groups.Add("mobile:xu-chu", "standard");
        groups.Add("mobile:cao-ren", "myth-wind");
        groups.Add("mobile:hua-tuo", "standard");
        groups.Add("mobile:zhen-ji", "standard");
        groups.Add("mobile:sun-quan", "standard");
        groups.Add("mobile:sima-yi", "standard");
        groups.Add("mobile:cao-ying", "sp");
        groups.Add("mobile:ma-chao", "standard");
        groups.Add("mobile:bei-mi-hu", "sp");
        groups.Add("mobile:huang-yueying", "standard");
        groups.Add("mobile:lu-xun", "standard");
        groups.Add("mobile:cao-cao", "standard");
        groups.Add("mobile:gan-ning", "standard");
        groups.Add("mobile:zhao-yun", "standard");
        groups.Add("mobile:huang-gai", "standard");
        groups.Add("mobile:dian-wei", "myth-fire");
        groups.Add("mobile:deng-ai", "myth-mountain");
        groups.Add("mobile:wei-yan", "myth-wind");
        groups.Add("mobile:guan-yu", "standard");
        groups.Add("mobile:zhang-fei", "standard");
        groups.Add("mobile:lu-bu", "standard");
        groups.Add("mobile:jiang-wei", "myth-mountain");
        groups.Add("mobile:zhuge-zhan", "myth-thunder");
        groups.Add("mobile:zhang-xing-cai", "sp");
        groups.Add("mobile:zhou-fang", "sp");
        groups.Add("mobile:cai-wen-ji", "myth-mountain");
        groups.Add("mobile:xiahou-yuan", "myth-wind");
        groups.Add("mobile:guo-huanghou", "fame-6");
        groups.Add("mobile:zhuge-guo", "sp");
        groups.Add("mobile:yan-yan", "myth-yin");
        groups.Add("mobile:zhang-xiu", "myth-thunder");
        groups.Add("mobile:wang-ping", "myth-yin");
        groups.Add("mobile:xu-shi", "fame-7");
        groups.Add("mobile:lu-zhi", "myth-yin");
        groups.Add("mobile:wang-can", "other");
        groups.Add("mobile:zhang-rang", "fame-6");
        groups.Add("mobile:xue-zong", "fame-7");
        groups.Add("mobile:ji-kang", "fame-7");
        groups.Add("mobile:wu-xian", "fame-7");
        groups.Add("mobile:cai-yong", "fame-7");
        groups.Add("mobile:liu-qi", "sp");
        groups.Add("mobile:sun-hao", "sp");
        groups.Add("mobile:lu-ji", "myth-yin");
        groups.Add("mobile:xu-you", "myth-yin");
        groups.Add("mobile:dong-yun", "sp");
        groups.Add("mobile:yue-jin", "sp");
        groups.Add("mobile:fu-wan", "sp");
        groups.Add("mobile:xiahou-ba", "sp");
        groups.Add("mobile:qin-mi", "fame-7");
        groups.Add("mobile:gongsun-yuan", "fame-5");
        groups.Add("mobile:sha-mo-ke", "sp");
        groups.Add("mobile:hao-zhao", "myth-thunder");
        groups.Add("mobile:wang-ji", "sp");
        groups.Add("mobile:mi-zhu", "sp");
        groups.Add("mobile:sima-lang", "sp");
        groups.Add("mobile:ma-zhong", "sp");
        groups.Add("mobile:shi-xie", "sp");
        groups.Add("mobile:ta-dun", "sp");
        groups.Add("mobile:dong-bai", "sp");
        groups.Add("mobile:qu-yi", "sp");
        groups.Add("mobile:xi-zhi-cai", "sp");
        groups.Add("mobile:liu-yu", "fame-6");
        groups.Add("mobile:cao-jie", "fame-7");
        groups.Add("mobile:zhang-lu", "sp");
        groups.Add("mobile:cao-xiu", "fame-5");
        groups.Add("mobile:zhong-yao", "fame-5");
        groups.Add("mobile:liu-chen", "fame-5");
        groups.Add("mobile:li-yan", "fame-6");
        groups.Add("mobile:huang-hao", "fame-6");
        groups.Add("mobile:sun-xiu", "fame-5");
        groups.Add("mobile:cen-hun", "fame-6");
        groups.Add("mobile:sun-zi-liu-fang", "fame-6");
        groups.Add("mobile:yu-jin", "fame-1");
        groups.Add("mobile:cao-hong", "sp");
        groups.Add("mobile:he-tai-hou", "sp");
        groups.Add("mobile:wen-pin", "sp");
        groups.Add("mobile:zhuge-dan", "sp");
        groups.Add("mobile:cao-zhi", "fame-1");
        groups.Add("mobile:ling-tong", "fame-1");
        groups.Add("mobile:wu-guo-tai", "fame-1");
        groups.Add("mobile:cao-rui", "fame-5");
        groups.Add("mobile:chen-qun", "fame-4");
        groups.Add("mobile:gu-yong", "fame-4");
        groups.Add("mobile:bu-zhi", "sp");
        groups.Add("mobile:li-tong", "sp");
        groups.Add("mobile:guo-huai", "fame-3");
        groups.Add("mobile:guan-xing-zhang-bao", "fame-2");
        groups.Add("mobile:li-ru", "fame-3");
        groups.Add("mobile:zu-mao", "sp");
        groups.Add("mobile:zhang-bao", "sp");
        groups.Add("mobile:ding-feng", "sp");
        groups.Add("mobile:sun-qian", "sp");
        groups.Add("mobile:kan-ze", "sp");
        groups.Add("mobile:sun-deng", "fame-6");
        groups.Add("mobile:guo-tu-feng-ji", "fame-5");
        groups.Add("mobile:zhu-zhi", "fame-5");
        groups.Add("mobile:quan-cong", "fame-5");
        groups.Add("boundary:yu-ji-mobile", "boundary");
        groups.Add("boundary:yuan-shu-mobile", "boundary");
        groups.Add("boundary:cao-zhang-mobile", "boundary-fame");
        groups.Add("boundary:guan-xing-zhang-bao-mobile", "boundary-fame");
        groups.Add("boundary:cai-wen-ji-mobile", "boundary");
        groups.Add("boundary:cheng-pu-mobile", "boundary-fame");
        groups.Add("boundary:fu-huang-hou-mobile", "boundary-fame");
        groups.Add("boundary:guo-huai-mobile", "boundary-fame");
        groups.Add("boundary:jian-yong-mobile", "boundary-fame");
        groups.Add("boundary:cai-fu-ren-mobile", "boundary-fame");
        groups.Add("boundary:cao-zhi-mobile", "boundary-fame");
        groups.Add("boundary:ling-tong-mobile", "boundary-fame");
        groups.Add("boundary:han-dang-mobile", "boundary-fame");
        groups.Add("boundary:xu-sheng-mobile", "boundary-fame");
        groups.Add("boundary:wu-guo-tai-mobile", "boundary-fame");
        groups.Add("boundary:ma-dai-mobile", "boundary-fame");
        groups.Add("boundary:gao-shun-mobile", "boundary-fame");
        groups.Add("boundary:wang-yi-mobile", "boundary-fame");
        groups.Add("boundary:fa-zheng-mobile", "boundary-fame");
        groups.Add("boundary:yu-jin-mobile", "boundary-fame");
        groups.Add("boundary:gongsun-zan-mobile", "boundary");
        groups.Add("boundary:ju-shou-mobile", "boundary-fame");
        groups.Add("boundary:hua-xiong-mobile", "boundary");
        groups.Add("boundary:zhou-tai-mobile", "boundary");
        groups.Add("boundary:huang-zhong-mobile", "boundary");
        groups.Add("boundary:wei-yan-mobile", "boundary");
        groups.Add("boundary:lu-bu-mobile", "boundary");
        groups.Add("boundary:zhuge-liang-mobile", "boundary");
        groups.Add("boundary:zhen-ji-mobile", "boundary");
        groups.Add("boundary:zhang-fei-mobile", "boundary");
        groups.Add("boundary:cao-cao-mobile", "boundary");
        groups.Add("boundary:sima-yi-mobile", "boundary");
        groups.Add("boundary:huang-gai-mobile", "boundary");
        groups.Add("boundary:gan-ning-mobile", "boundary");
        groups.Add("boundary:diao-chan-mobile", "boundary");
        groups.Add("boundary:sun-quan-mobile", "boundary");
        groups.Add("boundary:huang-yueying-mobile", "boundary");
        groups.Add("boundary:guo-jia-mobile", "boundary");
        groups.Add("boundary:da-qiao-mobile", "boundary");
        groups.Add("boundary:lu-xun-mobile", "boundary");
        groups.Add("boundary:sun-shangxiang-mobile", "boundary");
        groups.Add("boundary:zhang-liao-mobile", "boundary");
        groups.Add("boundary:xu-chu-mobile", "boundary");
        groups.Add("boundary:xiahou-dun-mobile", "boundary");
        groups.Add("boundary:xiahou-yuan-mobile", "boundary");
        groups.Add("boundary:cao-ren-mobile", "boundary");
        groups.Add("boundary:liu-bei-mobile", "boundary");
        groups.Add("boundary:guan-yu-mobile", "boundary");
        groups.Add("boundary:zhao-yun-mobile", "boundary");
        groups.Add("boundary:ma-chao-mobile", "boundary");
        groups.Add("boundary:lu-meng-mobile", "boundary");
        groups.Add("boundary:zhou-yu-mobile", "boundary");
        groups.Add("boundary:hua-tuo-mobile", "boundary");
        groups.Add("boundary:zhang-jiao-mobile", "boundary");
        groups.Add("boundary:sun-jian-mobile", "boundary");
        groups.Add("boundary:xun-yu-mobile", "boundary");
        groups.Add("boundary:dian-wei-mobile", "boundary");
        groups.Add("boundary:pang-tong-mobile", "boundary");
        groups.Add("boundary:pang-de-mobile", "boundary");
        groups.Add("boundary:xu-huang-mobile", "boundary");
        groups.Add("boundary:yan-liang-wen-chou-mobile", "boundary");
        groups.Add("boundary:wolong-zhuge-liang-mobile", "boundary");
        groups.Add("boundary:dong-zhuo-mobile", "boundary");
        groups.Add("boundary:yuan-shao-mobile", "boundary");
        groups.Add("boundary:meng-huo-mobile", "boundary");
        groups.Add("boundary:zhu-rong-mobile", "boundary");
        groups.Add("boundary:sun-ce-mobile", "boundary");
        groups.Add("boundary:deng-ai-mobile", "boundary");
        groups.Add("boundary:jiang-wei-mobile", "boundary");
        groups.Add("boundary:zhang-zhao-zhang-hong-mobile", "boundary");
        groups.Add("boundary:xiao-qiao-mobile", "boundary");
        groups.Add("boundary:bu-lian-shi-mobile", "boundary-fame");
        groups.Add("boundary:liu-biao-mobile", "boundary-fame");
        groups.Add("boundary:liao-hua-mobile", "boundary-fame");
        groups.Add("boundary:li-ru-mobile", "boundary-fame");
        groups.Add("boundary:yu-fan-mobile", "boundary-fame");
        groups.Add("boundary:guan-ping", "boundary-fame");
        groups.Add("boundary:zuo-ci-mobile", "boundary");
        groups.Add("boundary:zhong-hui-mobile", "boundary-fame");
        groups.Add("boundary:cao-pi-mobile", "boundary-fame");
        groups.Add("boundary:liu-shan-mobile", "boundary-fame");
        groups.Add("boundary:zhou-fei-mobile", "boundary-fame");
        groups.Add("boundary:zhang-ni-mobile", "boundary-fame");
        groups.Add("boundary:yan-yan-mobile", "boundary-fame");
        groups.Add("boundary:wang-ji-mobile", "boundary-fame");
        groups.Add("boundary:zhu-ran-mobile", "boundary-fame");
        groups.Add("boundary:pan-zhang-ma-zhong-mobile", "boundary-fame");
        groups.Add("boundary:sun-lu-ban-mobile", "boundary-fame");
        groups.Add("boundary:chen-qun-mobile", "boundary-fame");
        groups.Add("boundary:gu-yong-mobile", "boundary-fame");
        groups.Add("boundary:zhu-huan-mobile", "boundary-fame");
        groups.Add("boundary:zhou-cang-mobile", "boundary-fame");
        groups.Add("boundary:cao-zhen-mobile", "boundary-fame");
        groups.Add("boundary:sun-xiu-mobile", "boundary-fame");
        groups.Add("boundary:man-chong-mobile", "boundary-fame");
        groups.Add("boundary:wu-yi-mobile", "boundary-fame");
        groups.Add("boundary:zhu-zhi-mobile", "boundary-fame");
        groups.Add("boundary:yi-ji-mobile", "boundary");
        groups.Add("boundary:cao-xiu-mobile", "boundary-fame");
        groups.Add("boundary:quan-cong-mobile", "boundary-fame");
        groups.Add("mobile:shi-zhao-yun", "shi");
        groups.Add("mobile:shi-wei-yan", "shi");
        groups.Add("mobile:shi-chen-qun", "shi");
        groups.Add("mobile:shi-xiahou-shang", "shi");
        groups.Add("mobile:shi-wang-chang", "shi");
        groups.Add("mobile:shi-tian-feng", "shi");
        groups.Add("mobile:shi-chen-zhi", "shi");
        groups.Add("mobile:shi-yu-ji", "shi");
        groups.Add("mobile:shi-cheng-pu", "shi");
        groups.Add("mobile:shi-dong-zhao", "shi");
        groups.Add("mobile:shi-chen-dao", "shi");
        groups.Add("mobile:shi-huang-zu", "shi");
        groups.Add("mobile:shi-guo-yuan", "shi");
        groups.Add("mobile:shi-xin-xian-ying", "shi");
        groups.Add("mobile:shi-huan-jie", "shi");
        groups.Add("mobile:shi-lu-yu-sheng", "shi");
        groups.Add("mobile:shi-lu-su", "shi");
        groups.Add("mobile:shi-he-qi", "shi");
        groups.Add("mobile:shi-sun-chen", "shi");
        groups.Add("mobile:shi-sun-jun", "shi");
        groups.Add("mobile:shi-cao-zhen", "shi");
        groups.Add("mobile:shi-xiao-qiao", "shi");
        groups.Add("mobile:shi-zhang-yan", "shi");
        groups.Add("mobile:shi-deng-ai", "shi");
        groups.Add("le:mi-heng", "le");
        groups.Add("le:zhou-fei", "le");
        groups.Add("le:cai-wen-ji", "le");
        groups.Add("le:cai-yong", "le");
        groups.Add("le:cao-zhi", "le");
        groups.Add("le:xiao-qiao", "le");
        groups.Add("le:da-qiao", "le");
        groups.Add("le:zhou-yu", "le");
        groups.Add("le:zou-shi", "le");
        groups.Add("le:diao-chan", "le");
        groups.Add("wumiao:guan-yu", "wumiao");
        groups.Add("wumiao:lu-xun", "wumiao");
        groups.Add("wumiao:zhang-fei", "wumiao");
        groups.Add("ol:mo-sun-quan", "magic");
        groups.Add("ol:mo-lu-bu", "magic");
        groups.Add("ol:mo-diao-chan", "magic");
        groups.Add("ol:mo-zhang-fei", "magic");
        groups.Add("ol:mo-cao-cao", "magic");
        groups.Add("ol:mo-sima-yi", "magic");
        Add("standard", "liu-bei guan-yu zhang-fei zhuge-liang zhao-yun ma-chao huang-yueying sun-quan gan-ning lu-meng huang-gai zhou-yu da-qiao lu-xun sun-shangxiang cao-cao sima-yi xiahou-dun zhang-liao xu-chu guo-jia zhen-ji hua-tuo lu-bu diao-chan hua-xiong");
        Add("myth-wind", "xiahou-yuan cao-ren huang-zhong wei-yan xiao-qiao zhou-tai zhang-jiao yu-ji");
        Add("myth-fire", "dian-wei xun-yu pang-tong wolong-zhuge-liang taishi-ci yuan-shao yan-liang-wen-chou pang-de");
        Add("myth-forest", "cao-pi xu-huang sun-jian meng-huo zhu-rong lu-su dong-zhuo jia-xu");
        Add("myth-mountain", "sun-ce cai-wen-ji deng-ai jiang-wei zhang-he liu-shan zhang-zhao-zhang-hong zuo-ci");
        Add("fame-1", "yu-jin xu-shu gao-shun cao-zhi zhang-chun-hua ling-tong chen-gong wu-guo-tai fa-zheng ma-su");
        Add("fame-2", "cao-zhang wang-yi xun-you zhong-hui ma-dai liao-hua guan-xing-zhang-bao bu-lian-shi cheng-pu han-dang liu-biao");
        Add("fame-3", "cao-chong guo-huai man-chong guan-ping pan-zhang-ma-zhong xu-sheng li-ru liu-feng jian-yong yu-fan zhu-ran fu-huanghou");
        Add("fame-4", "gu-yong zhu-huan zhang-song ju-shou cao-zhen han-hao-shi-huan chen-qun wu-yi zhou-cang sun-lu-ban");
        Add("fame-5", "zhu-zhi cao-rui cao-xiu zhong-yao liu-chen xiahou-shi zhang-ni sun-xiu quan-cong gongsun-yuan guo-tu-feng-ji");
        Add("fame-5", "sha-mo-ke");
        Add("fame-6", "guo-huanghou li-yan sun-deng liu-yu cen-hun sun-zi-liu-fang huang-hao zhang-rang");
        Add("fame-7", "xin-xianying wu-xian xu-shi cao-jie ji-kang qin-mi xue-zong cai-yong");
        Add("god", "shen-guan-yu shen-sima-yi shen-lu-meng shen-cao-cao shen-zhao-yun shen-zhou-yu shen-lu-bu");
        foreach (var id in new[] { "guo-jia", "sun-ce", "taishi-ci", "xun-yu", "lu-su", "hua-tuo", "lu-xun", "gan-ning", "liu-bei", "zhao-yun", "zhuge-liang", "zhang-liao", "cao-cao", "lu-bu", "zhou-yu", "guan-yu", "lu-meng", "sima-yi", "jiang-wei", "ma-chao" })
            groups.Add("mobile:shen-" + id, "god");
        foreach (var id in new[] { "jiang-wei", "deng-ai", "zhang-fei", "hua-tuo", "xu-chu", "ma-chao", "pang-tong", "zhong-hui", "zhang-jiao", "dian-wei", "huang-zhong", "cao-pi", "sun-quan", "zhang-liao", "guan-yu", "liu-bei", "lu-xun" })
            groups.Add("anniversary:shen-" + id, "god");
        foreach (var id in new[] { "guo-jia", "sun-quan", "zhang-liao", "lu-bu", "zhen-ji", "lu-su", "cao-cao", "zhou-yu", "zhuge-liang", "lu-meng", "zhao-yun", "gan-ning", "zhang-jiao", "taishi-ci", "hua-tuo", "liu-bei", "sima-yi", "guan-yu", "lu-xun", "xu-chu", "dian-wei", "zhong-hui", "deng-ai", "pang-tong", "diao-chan", "wei-yan", "da-qiao-xiao-qiao", "zhang-fei", "zuo-ci" })
            groups.Add("happy:shen-" + id, "god");
        foreach (var id in new[] { "guan-yu", "zhou-yu", "zhuge-liang", "lu-bu", "zhao-yun", "sima-yi",
                     "liu-bei", "lu-xun", "gan-ning", "zhang-liao", "sun-quan", "zhang-jiao", "dian-wei", "huang-zhong", "cao-pi", "zhen-ji", "zhao-yun-2016", "sima-yi-2016", "cao-cao" })
            groups.Add("ol:shen-" + id, "god");
        groups.Add("ol:sp-pang-de", "other");
        groups.Add("ol:sp-diao-chan", "other");
        groups.Add("ol:sp-jia-xu", "other");
        groups.Add("ol:zhang-bao", "other");
        groups.Add("ol:zhuge-jin", "other");
        groups.Add("ol:zhang-xing-cai", "other");
        groups.Add("ol:zu-mao", "other");
        groups.Add("ol:ding-feng", "other");
        groups.Add("ol:pan-feng", "other");
        groups.Add("ol:ma-liang", "other");
        groups.Add("ol:ling-cao", "other");
        groups.Add("ol:yue-jin", "other");
        groups.Add("ol:xu-jing", "other");
        groups.Add("ol:sun-qian", "other");
        groups.Add("ol:sp-jiang-wei", "other");
        groups.Add("ol:mi-zhu", "other");
        groups.Add("ol:sima-lang", "other");
        groups.Add("ol:li-tong", "other");
        groups.Add("ol:ma-zhong", "other");
        groups.Add("ol:sun-shao", "other");
        groups.Add("ol:zhao-yan", "other");
        groups.Add("ol:tang-ji", "other");
        groups.Add("ol:liu-hong", "other");
        groups.Add("ol:wang-shuang", "other");
        groups.Add("ol:wei-zi", "other");
        groups.Add("ol:tian-yu", "other");
        groups.Add("ol:zhang-hu-yue-chen", "other");
        groups.Add("ol:wei-guan", "other");
        groups.Add("ol:wang-xiang", "other");
        groups.Add("ol:du-yu", "other");
        groups.Add("ol:jia-chong", "other");
        groups.Add("ol:yang-hu", "other");
        groups.Add("ol:guo-huai", "other");
        groups.Add("ol:zhou-chu", "other");
        groups.Add("ol:zuo-fen", "other");
        groups.Add("ol:zhang-hua", "other");
        groups.Add("ol:sima-zhou", "other");
        groups.Add("ol:xin-chang", "other");
        groups.Add("ol:shi-bao", "other");
        groups.Add("ol:huang-cheng-yan", "other");
        groups.Add("ol:chen-lin", "other");
        groups.Add("ol:sp-cai-wen-ji", "other");
        groups.Add("ol:sp-cao-ren", "other");
        groups.Add("ol:guan-yin-ping", "other");
        groups.Add("ol:zhuge-ke", "other");
        groups.Add("ol:cao-hong", "other");
        groups.Add("ol:sp-ma-chao", "other");
        groups.Add("ol:fu-wan", "other");
        groups.Add("ol:liu-xie", "other");
        groups.Add("ol:ling-ju", "other");
        groups.Add("ol:yang-xiu", "other");
        groups.Add("classic:qu-yi", "other");
        Add("sp", "gongsun-zan");
        groups.Add("sp:le-jin", "sp");
        Add("myth-yin", "yan-yan wang-ping lu-ji sun-liang xu-you wang-ji kuai-yue-kuai-liang lu-zhi");
        Add("myth-thunder", "hao-zhao zhuge-zhan chen-dao guanqiu-jian lu-kang yuan-shu zhou-fei zhang-xiu");
        groups.Add("boundary:zhang-jiao", "boundary");
        groups.Add("boundary:sima-yi", "boundary");
        groups.Add("boundary:diao-chan", "boundary");
        groups.Add("classic:li-dian", "boundary");
        groups.Add("boundary:guo-jia", "boundary");
        groups.Add("boundary:cao-cao", "boundary");
        groups.Add("boundary:zhang-liao", "boundary");
        groups.Add("boundary:gan-ning", "boundary");
        groups.Add("boundary:xu-chu", "boundary");
        groups.Add("boundary:zhou-yu", "boundary");
        groups.Add("boundary:lu-meng", "boundary");
        groups.Add("boundary:huang-gai", "boundary");
        groups.Add("boundary:lu-xun", "boundary");
        groups.Add("boundary:liu-bei", "boundary");
        groups.Add("boundary:da-qiao", "boundary");
        groups.Add("boundary:hua-tuo", "boundary");
        groups.Add("boundary:guan-yu", "boundary");
        groups.Add("boundary:zhang-fei", "boundary");
        groups.Add("boundary:ma-chao", "boundary");
        groups.Add("boundary:xiahou-dun", "boundary");
        groups.Add("boundary:li-dian", "boundary");
        groups.Add("boundary:zhen-ji", "boundary");
        groups.Add("boundary:huang-yueying", "boundary");
        groups.Add("boundary:zhuge-liang", "boundary");
        groups.Add("boundary:sun-shangxiang", "boundary");
        groups.Add("boundary:huang-zhong", "boundary");
        groups.Add("boundary:wei-yan", "boundary");
        groups.Add("boundary:deng-ai", "boundary");
        groups.Add("boundary:jiang-wei", "boundary");
        groups.Add("boundary:sun-ce", "boundary");
        groups.Add("boundary:zhu-rong", "boundary");
        groups.Add("boundary:meng-huo", "boundary");
        groups.Add("boundary:gongsun-zan", "boundary");
        groups.Add("boundary:hua-xiong", "boundary");
        groups.Add("boundary:lu-bu", "boundary");
        groups.Add("boundary:yuan-shao", "boundary");
        groups.Add("boundary:sun-quan", "boundary");
        groups.Add("boundary:pang-tong", "boundary");
        groups.Add("boundary:xu-shu", "boundary-fame");
        groups.Add("boundary:xun-yu", "boundary");
        groups.Add("boundary:sun-jian", "boundary");
        groups.Add("boundary:xiahou-yuan", "boundary");
        groups.Add("boundary:taishi-ci", "boundary");
        groups.Add("boundary:cao-zhang", "boundary-fame");
        groups.Add("boundary:pang-de", "boundary");
        groups.Add("boundary:fa-zheng", "boundary-fame");
        groups.Add("boundary:han-dang", "boundary-fame");
        groups.Add("boundary:zhang-he", "boundary");
        groups.Add("boundary:cao-chong", "boundary-fame");
        groups.Add("boundary:xu-huang", "boundary");
        groups.Add("boundary:dong-zhuo", "boundary");
        groups.Add("boundary:jia-xu", "boundary");
        groups.Add("boundary:dian-wei", "boundary");
        groups.Add("boundary:cai-wen-ji", "boundary");
        groups.Add("boundary:zhang-chun-hua", "boundary");
        groups.Add("boundary:xu-sheng", "boundary-fame");
        groups.Add("boundary:zhang-song", "boundary-fame");
        groups.Add("boundary:ju-shou", "boundary-fame");
        groups.Add("boundary:guan-xing-zhang-bao", "boundary");
        groups.Add("boundary:zhou-tai", "boundary");
        groups.Add("boundary:cao-ren", "boundary");
        groups.Add("ol:yi-ji", "other");
        groups.Add("ol:huang-zu", "other");
        groups.Add("ol:cao-xing", "other");
        groups.Add("ol:xun-chen", "other");
        groups.Add("ol:deng-zhong", "other");
        groups.Add("ol:liang-xing", "other");
        groups.Add("ol:gao-lan", "other");
        groups.Add("ol:zhang-chang-pu", "other");
        groups.Add("ol:lv-kai", "other");
        groups.Add("ol:zhou-fang", "other");
        groups.Add("ol:yan-jun", "other");
        groups.Add("ol:jiang-gan", "other");
        groups.Add("ol:pan-jun", "other");
        groups.Add("ol:pan-shu", "other");
        groups.Add("ol:yang-yi", "other");
        groups.Add("ol:zhu-ling", "other");
        groups.Add("ol:liu-bian", "other");
        groups.Add("ol:chen-deng", "other");
        groups.Add("ol:qinghe-gongzhu", "other");
        groups.Add("ol:yang-wan", "other");
        groups.Add("ol:rui-ji", "other");
        groups.Add("ol:teng-fang-lan", "other");
        groups.Add("mobile:sun-ru", "other");
        groups.Add("mobile:chen-zhen", "other");
        groups.Add("mobile:liu-zan", "other");
        groups.Add("ol:duan-wei", "other");
        groups.Add("ol:niu-jin", "other");
        groups.Add("ol:ding-yuan", "other");
        groups.Add("ol:cao-song", "other");
        groups.Add("ol:bian-fu-ren", "other");
        groups.Add("ol:zhang-ling", "other");
        groups.Add("ol:cao-shuang", "other");
        groups.Add("ol:sp-zhang-liao", "other");
        groups.Add("ol:wo-long-feng-chu", "other");
        groups.Add("ol:sun-ru", "other");
        groups.Add("ol:liu-pi", "other");
        groups.Add("ol:gao-gan", "other");
        groups.Add("ol:huo-jun", "other");
        groups.Add("ol:qiao-zhou", "other");
        groups.Add("ol:du-xi", "other");
        groups.Add("ol:fu-rong", "other");
        groups.Add("mobile:wang-fu-zhao-lei", "other");
        groups.Add("ol:zhang-yi", "other");
        groups.Add("ol:yan-pu", "other");
        groups.Add("ol:luo-xian", "other");
        groups.Add("ol:hao-pu", "other");
        groups.Add("ol:ma-wan", "other");
        groups.Add("ol:du-fu-ren", "other");
        groups.Add("ol:fei-yi", "other");
        groups.Add("ol:zhang-shi-ping", "other");
        groups.Add("ol:qian-zhao", "other");
        groups.Add("ol:ding-shang-wan", "other");
        groups.Add("ol:he-jin", "other");
        groups.Add("ol:li-wan", "other");
        groups.Add("ol:yan-rou", "other");
        groups.Add("ol:cao-xi", "other");
        groups.Add("ol:hu-jin-ding", "other");
        groups.Add("ol:lu-yu-sheng", "other");
        groups.Add("ol:wang-kuang", "other");
        groups.Add("ol:yuan-huan", "other");
        groups.Add("ol:zhang-yan", "other");
        groups.Add("ol:wu-an-guo", "other");
        groups.Add("ol:han-fu", "other");
        groups.Add("ol:zhu-jun", "other");
        groups.Add("ol:ke-bi-neng", "other");
        groups.Add("ol:niu-fu", "other");
        groups.Add("ol:duan-jiong", "other");
        groups.Add("ol:ma-mi-di", "other");
        groups.Add("ol:ruan-hui", "other");
        groups.Add("ol:tian-chou", "other");
        groups.Add("ol:cheng-gong-ying", "other");
        groups.Add("ol:ma-xiu-ma-tie", "other");
        groups.Add("ol:pei-yuan-shao", "other");
        groups.Add("ol:wu-jing", "other");
        groups.Add("ol:xiang-chong", "other");
        groups.Add("ol:yang-feng", "other");
        groups.Add("ol:qin-lang", "other");
        groups.Add("ol:a-hui-nan", "other");
        groups.Add("ol:wang-guan", "other");
        groups.Add("ol:liu-yan", "other");
        groups.Add("ol:meng-da", "other");
        groups.Add("ol:cai-mao", "other");
        groups.Add("ol:ya-dan", "other");
        groups.Add("ol:cao-teng", "other");
        groups.Add("ol:yang-fu", "other");
        groups.Add("ol:wu-tu-gu", "other");
        groups.Add("ol:sun-hao", "other");
        groups.Add("ol:da-qiao-xiao-qiao", "other");
        groups.Add("ol:fan-jiang-zhang-da", "other");
        groups.Add("ol:wen-yang", "other");
        groups.Add("ol:ma-yuan-yi", "other");
        groups.Add("ol:hu-ban", "other");
        groups.Add("ol:liu-ba", "other");
        groups.Add("ol:liu-pan", "other");
        groups.Add("ol:qu-huang", "other");
        groups.Add("ol:ruan-yu", "other");
        groups.Add("ol:cao-yu", "other");
        groups.Add("ol:bao-xin", "other");
        groups.Add("ol:guo-tu", "other");
        groups.Add("ol:cui-yan", "other");
        groups.Add("ol:li-yi", "other");
        groups.Add("ol:sun-yuan", "other");
        groups.Add("ol:liu-zhang", "other");
        groups.Add("ol:tang-tang", "other");
        groups.Add("ol:yuan-ji", "other");
        groups.Add("ol:kong-shu", "other");
        groups.Add("ol:ma-cheng", "other");
        groups.Add("ol:zhou-qun", "other");
        groups.Add("ol:lu-shi", "other");
        groups.Add("anniversary:ruan-ji", "other");
        groups.Add("anniversary:kan-ze", "other");
        groups.Add("anniversary:hu-zhao", "other");
        groups.Add("anniversary:xu-jing", "other");
        groups.Add("anniversary:du-kui", "other");
        groups.Add("anniversary:liu-li", "other");
        groups.Add("anniversary:liu-yong", "other");
        groups.Add("anniversary:zhang-fen", "other");
        groups.Add("anniversary:bai-ling-yun", "other");
        groups.Add("anniversary:yang-biao", "other");
        groups.Add("anniversary:le-jiu", "other");
        groups.Add("anniversary:he-yan", "other");
        groups.Add("anniversary:guo-zhao", "other");
        groups.Add("anniversary:mang-ya-chang", "other");
        groups.Add("anniversary:feng-xi", "other");
        groups.Add("anniversary:zhao-tong-zhao-guang", "other");
        groups.Add("anniversary:lv-dai", "other");
        groups.Add("anniversary:fu-rong-fu-qian", "other");
        groups.Add("anniversary:gao-xiang", "other");
        groups.Add("anniversary:xiahou-en", "other");
        groups.Add("anniversary:zhang-wen", "other");
        groups.Add("anniversary:zhang-xun", "other");
        groups.Add("anniversary:mu-shun", "other");
        groups.Add("anniversary:sun-huan", "other");
        groups.Add("anniversary:xiahou-jie", "other");
        groups.Add("anniversary:cai-yang", "other");
        groups.Add("anniversary:xing-dao-rong", "other");
        groups.Add("anniversary:zhu-zhi", "other");
        groups.Add("anniversary:han-song", "other");
        groups.Add("anniversary:xiahou-hui", "other");
        groups.Add("anniversary:xun-xu", "other");
        groups.Add("anniversary:zhang-huan", "other");
        groups.Add("anniversary:wu-fu", "other");
        groups.Add("anniversary:cui-lie", "other");
        groups.Add("anniversary:hou-cheng", "other");
        groups.Add("anniversary:ma-miao", "other");
        groups.Add("anniversary:yuan-yin", "other");
        groups.Add("anniversary:qiao-rui", "other");
        groups.Add("anniversary:hu-che-er", "other");
        groups.Add("anniversary:zhang-heng", "other");
        groups.Add("anniversary:hao-meng", "other");
        groups.Add("anniversary:lei-bo", "other");
        groups.Add("anniversary:liu-you", "other");
        groups.Add("mobile:he-qi", "other");
        groups.Add("mobile:guan-ping", "fame-3");
        groups.Add("mobile:yu-fan", "fame-3");
        groups.Add("mobile:liu-biao", "fame-3");
        groups.Add("mobile:man-chong", "fame-3");
        groups.Add("mobile:jian-yong", "fame-3");
        groups.Add("mobile:pan-zhang-ma-zhong", "fame-3");
        groups.Add("mobile:fu-huang-hou", "fame-3");
        groups.Add("mobile:cao-zhen", "fame-4");
        groups.Add("mobile:wu-yi", "fame-4");
        groups.Add("mobile:zhu-ran", "fame-3");
        groups.Add("mobile:sun-lu-ban", "fame-4");
        groups.Add("mobile:zhuge-ke", "sp");
        groups.Add("mobile:ling-cao", "sp");
        groups.Add("mobile:cao-ang", "sp");
        groups.Add("mobile:chen-lin", "sp");
        groups.Add("mobile:zhuge-jin", "sp");
        groups.Add("mobile:bu-lian-shi", "fame-2");
        groups.Add("mobile:zhu-huan", "fame-4");
        groups.Add("mobile:han-dang", "fame-2");
        groups.Add("mobile:cao-zhang", "fame-2");
        groups.Add("mobile:liao-hua", "fame-2");
        groups.Add("mobile:ma-dai", "fame-2");
        groups.Add("mobile:wang-yi", "fame-2");
        groups.Add("mobile:cheng-pu", "fame-2");
        groups.Add("mobile:xun-you", "fame-2");
        groups.Add("mobile:li-dian", "sp");
        groups.Add("mobile:taishi-ci", "myth-fire");
        groups.Add("mobile:cao-chong", "fame-3");
        groups.Add("mobile:chen-gong", "fame-1");
        groups.Add("mobile:liu-feng", "fame-3");
        groups.Add("mobile:lu-su", "myth-forest");
        groups.Add("mobile:xiahou-shi", "fame-5");
        groups.Add("mobile:sp-xu-shu", "sp");
        groups.Add("mobile:jia-xu", "myth-forest");
        groups.Add("mobile:zhang-song", "fame-4");
        groups.Add("boundary:liu-feng", "boundary-fame");
        groups.Add("anniversary:ma-tie", "other");
        groups.Add("anniversary:yan-xiang", "other");
        groups.Add("anniversary:che-zhou", "other");
        groups.Add("anniversary:wang-chang", "other");
        groups.Add("ol:guan-hai", "other");
        groups.Add("ol:xie-ling-yu", "other");
        groups.Add("ol:li-feng", "other");
        groups.Add("ol:cai-fu-ren", "other");
        groups.Add("ol:gu-yong", "other");
        groups.Add("ol:huan-shu-jun", "other");
        groups.Add("ol:zhao-zhong", "other");
        groups.Add("ol:tao-qian", "other");
        groups.Add("ol:pang-ji", "other");
        groups.Add("ol:zhong-yan", "other");
        groups.Add("ol:han-sui", "other");
        groups.Add("ol:xiahou-ba", "other");
        groups.Add("classic:zhuge-dan", "other");
        groups.Add("ol:he-tai-hou", "other");
        groups.Add("ol:wen-pin", "other");
        groups.Add("ol:dong-yun", "other");
        groups.Add("ol:shen-pei", "other");
        groups.Add("ol:sun-lu-yu", "other");
        groups.Add("ol:cheng-yu", "other");
        groups.Add("ol:zhang-lu", "other");
        groups.Add("ol:wang-rong", "other");
        groups.Add("ol:shi-xie", "other");
        groups.Add("ol:ma-yun-lu", "other");
        groups.Add("ol:ta-dun", "other");
        groups.Add("ol:yan-bai-hu", "other");
        groups.Add("ol:guan-suo", "other");
        groups.Add("ol:wang-lang", "other");
        groups.Add("ol:bao-san-niang", "other");
        groups.Add("ol:deng-zhi", "other");
        groups.Add("ol:cao-ying", "other");
        groups.Add("ol:dong-zhao", "other");
        groups.Add("ol:huang-yueying-sp", "other");
        groups.Add("ol:bu-zhi", "other");
        groups.Add("ol:dong-bai", "other");
        groups.Add("ol:zhao-xiang", "other");
        groups.Add("ol:he-qi", "other");
        groups.Add("ol:kan-ze", "other");
        return groups;

        void Add(string group, string ids)
        {
            foreach (var id in ids.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                groups.Add("classic:" + id, group);
        }
    }
}

public sealed record GallerySeries(string Id, string Name, string Description);
public sealed record GalleryGroup(string Id, string SeriesId, string Name, string Title);
