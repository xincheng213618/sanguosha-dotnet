using System.IO;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class EditionGeneralGalleryChecks
{
    public static void NamespacedPortraitsAndDetails(string output, IReadOnlySet<string>? detailGeneralIds = null)
    {
        detailGeneralIds = detailGeneralIds?.Select(GeneralRulesCatalog.CanonicalId).ToHashSet(StringComparer.Ordinal);
        var registry = StandardContentRegistry.CreateWithClassicGenerals();
        using var model = new MainViewModel(false, 17, true, new MemorySaveStore(),
            historyStore: new MemoryMatchHistoryStore(), preferencesStore: new MemoryPlayerPreferencesStore(),
            contentRegistry: registry) { IsMotionEnabled = false, IsSoundEnabled = false };
        model.OpenGeneralGalleryCommand.Execute(null);
        var revision = Program.Engine(model).Revision;
        Program.Assert(new[] { "anniversary:kan-ze", "anniversary:hu-zhao", "anniversary:xu-jing",
                "anniversary:du-kui", "anniversary:liu-li", "anniversary:liu-yong",
                "anniversary:zhang-fen", "anniversary:bai-ling-yun", "anniversary:yang-biao",
                "anniversary:le-jiu", "anniversary:he-yan", "anniversary:guo-zhao",
                "anniversary:mang-ya-chang", "anniversary:feng-xi", "anniversary:zhao-tong-zhao-guang",
                "anniversary:lv-dai", "anniversary:fu-rong-fu-qian", "anniversary:gao-xiang",
                "anniversary:xiahou-en", "anniversary:zhang-wen", "anniversary:zhang-xun",
                "anniversary:mu-shun", "anniversary:sun-huan", "anniversary:xiahou-jie",
                "anniversary:cai-yang", "anniversary:xing-dao-rong", "anniversary:zhu-zhi",
                "anniversary:han-song", "anniversary:xiahou-hui", "anniversary:xun-xu",
                "anniversary:zhang-huan", "anniversary:che-zhou", "anniversary:wang-chang",
                "anniversary:wu-fu", "anniversary:ma-tie", "anniversary:yan-xiang",
                "anniversary:cui-lie", "anniversary:hou-cheng", "anniversary:ma-miao",
                "anniversary:yuan-yin", "anniversary:qiao-rui", "anniversary:hu-che-er",
                "anniversary:zhang-heng", "anniversary:hao-meng", "anniversary:lei-bo",
                "anniversary:liu-you", "mobile:he-qi", "boundary:liu-feng",
                "mobile:guan-ping", "mobile:yu-fan", "mobile:liu-biao", "mobile:man-chong", "mobile:jian-yong", "mobile:pan-zhang-ma-zhong", "mobile:fu-huang-hou", "mobile:cao-zhen", "mobile:wu-yi", "mobile:zhu-ran", "mobile:sun-lu-ban", "mobile:zhuge-ke", "mobile:ling-cao",
                "mobile:cao-ang", "mobile:chen-lin", "mobile:zhuge-jin", "mobile:bu-lian-shi", "mobile:zhu-huan", "mobile:han-dang", "mobile:cao-zhang", "mobile:liao-hua", "mobile:ma-dai", "mobile:wang-yi", "mobile:cheng-pu", "mobile:xun-you",
                "mobile:li-dian", "mobile:taishi-ci", "mobile:cao-chong", "mobile:chen-gong", "mobile:liu-feng", "mobile:lu-su", "mobile:xiahou-shi", "mobile:sp-xu-shu", "mobile:jia-xu", "mobile:zhang-song",
                "boundary:yi-ji-mobile", "boundary:cao-xiu-mobile", "boundary:quan-cong-mobile",
                "boundary:man-chong-mobile", "boundary:wu-yi-mobile", "boundary:zhu-zhi-mobile",
                "boundary:zhou-cang-mobile", "boundary:cao-zhen-mobile", "boundary:sun-xiu-mobile",
                "boundary:chen-qun-mobile", "boundary:gu-yong-mobile", "boundary:zhu-huan-mobile",
                "boundary:zhu-ran-mobile", "boundary:pan-zhang-ma-zhong-mobile", "boundary:sun-lu-ban-mobile",
                "boundary:zhang-ni-mobile", "boundary:yan-yan-mobile", "boundary:wang-ji-mobile",
                "boundary:cao-pi-mobile", "boundary:liu-shan-mobile", "boundary:zhou-fei-mobile",
                "boundary:yu-ji-mobile", "boundary:zuo-ci-mobile", "boundary:zhong-hui-mobile",
                "boundary:yuan-shu-mobile", "boundary:yu-fan-mobile", "boundary:guan-ping",
                "boundary:cao-zhang-mobile", "boundary:liao-hua-mobile", "boundary:li-ru-mobile",
                "boundary:guan-xing-zhang-bao-mobile", "boundary:bu-lian-shi-mobile", "boundary:liu-biao-mobile",
                "boundary:cai-wen-ji-mobile", "boundary:cheng-pu-mobile", "boundary:fu-huang-hou-mobile",
                "boundary:guo-huai-mobile", "boundary:jian-yong-mobile", "boundary:cai-fu-ren-mobile",
                "boundary:cao-zhi-mobile", "boundary:ling-tong-mobile", "boundary:han-dang-mobile",
                "boundary:xu-sheng-mobile", "boundary:wu-guo-tai-mobile", "boundary:ma-dai-mobile",
                "boundary:gao-shun-mobile", "boundary:wang-yi-mobile", "boundary:fa-zheng-mobile",
                "boundary:yu-jin-mobile", "boundary:gongsun-zan-mobile", "boundary:ju-shou-mobile",
                "boundary:hua-xiong-mobile", "boundary:zhou-tai-mobile", "boundary:huang-zhong-mobile", "boundary:wei-yan-mobile",
                "boundary:lu-bu-mobile", "boundary:zhuge-liang-mobile", "boundary:zhen-ji-mobile", "boundary:zhang-fei-mobile", "boundary:cao-cao-mobile", "boundary:sima-yi-mobile",
                "boundary:huang-gai-mobile", "boundary:gan-ning-mobile", "boundary:diao-chan-mobile", "boundary:sun-quan-mobile", "boundary:huang-yueying-mobile", "boundary:guo-jia-mobile",
                "boundary:da-qiao-mobile", "boundary:lu-xun-mobile", "boundary:sun-shangxiang-mobile", "boundary:zhang-liao-mobile", "boundary:xu-chu-mobile", "boundary:xiahou-dun-mobile", "boundary:xiahou-yuan-mobile", "boundary:cao-ren-mobile",
                "boundary:liu-bei-mobile", "boundary:guan-yu-mobile", "boundary:zhao-yun-mobile", "boundary:ma-chao-mobile", "boundary:lu-meng-mobile", "boundary:zhou-yu-mobile", "boundary:hua-tuo-mobile", "boundary:zhang-jiao-mobile" }.All(id =>
                model.GeneralGalleryEntries.Any(entry => entry.GeneralId == GeneralRulesCatalog.CanonicalId(id))),
            "The added distinct editions are present in the gallery.");
        if (detailGeneralIds is not null)
            Program.Assert(detailGeneralIds.All(id => model.GeneralGalleryEntries.Any(entry => entry.GeneralId == GeneralRulesCatalog.CanonicalId(id))),
                "Every requested edition must be registered before rendering its details.");
        var browseIds = new[]
        {
            "anniversary:kan-ze", "anniversary:hu-zhao", "anniversary:xu-jing", "anniversary:du-kui", "anniversary:liu-li", "anniversary:liu-yong",
            "anniversary:zhang-fen", "anniversary:bai-ling-yun", "anniversary:yang-biao", "anniversary:le-jiu", "anniversary:he-yan", "anniversary:guo-zhao",
            "anniversary:mang-ya-chang", "anniversary:feng-xi", "anniversary:zhao-tong-zhao-guang", "anniversary:lv-dai", "anniversary:fu-rong-fu-qian", "anniversary:gao-xiang",
            "anniversary:xiahou-en", "anniversary:zhang-wen", "anniversary:zhang-xun", "anniversary:mu-shun", "anniversary:sun-huan", "anniversary:xiahou-jie",
            "anniversary:cai-yang", "anniversary:xing-dao-rong", "anniversary:zhu-zhi", "anniversary:han-song", "anniversary:xiahou-hui", "anniversary:xun-xu",
            "anniversary:zhang-huan", "anniversary:che-zhou", "anniversary:wang-chang", "anniversary:wu-fu", "anniversary:ma-tie", "anniversary:yan-xiang",
            "anniversary:cui-lie", "anniversary:hou-cheng", "anniversary:ma-miao", "anniversary:yuan-yin", "anniversary:qiao-rui", "anniversary:hu-che-er",
            "anniversary:zhang-heng", "anniversary:hao-meng", "anniversary:lei-bo", "anniversary:liu-you", "mobile:he-qi", "boundary:liu-feng",
            "boundary:yi-ji-mobile", "boundary:cao-xiu-mobile", "boundary:quan-cong-mobile", "boundary:man-chong-mobile", "boundary:wu-yi-mobile", "boundary:zhu-zhi-mobile",
            "boundary:zhou-cang-mobile", "boundary:cao-zhen-mobile", "boundary:sun-xiu-mobile", "boundary:chen-qun-mobile", "boundary:gu-yong-mobile", "boundary:zhu-huan-mobile",
            "boundary:zhu-ran-mobile", "boundary:pan-zhang-ma-zhong-mobile", "boundary:sun-lu-ban-mobile", "boundary:zhang-ni-mobile", "boundary:yan-yan-mobile", "boundary:wang-ji-mobile",
            "boundary:cao-pi-mobile", "boundary:liu-shan-mobile", "boundary:zhou-fei-mobile", "boundary:yu-ji-mobile", "boundary:zuo-ci-mobile", "boundary:zhong-hui-mobile",
            "boundary:yuan-shu-mobile", "boundary:yu-fan-mobile", "boundary:guan-ping", "boundary:cao-zhang-mobile", "boundary:liao-hua-mobile", "boundary:li-ru-mobile",
            "boundary:guan-xing-zhang-bao-mobile", "boundary:bu-lian-shi-mobile", "boundary:liu-biao-mobile", "boundary:cai-wen-ji-mobile", "boundary:cheng-pu-mobile", "boundary:fu-huang-hou-mobile",
            "boundary:guo-huai-mobile", "boundary:jian-yong-mobile", "boundary:cai-fu-ren-mobile", "boundary:cao-zhi-mobile", "boundary:ling-tong-mobile", "boundary:han-dang-mobile",
            "boundary:xu-sheng-mobile", "boundary:wu-guo-tai-mobile", "boundary:ma-dai-mobile", "boundary:gao-shun-mobile", "boundary:wang-yi-mobile", "boundary:fa-zheng-mobile",
            "boundary:yu-jin-mobile", "boundary:gongsun-zan-mobile", "boundary:ju-shou-mobile", "boundary:hua-xiong-mobile", "boundary:zhou-tai-mobile", "boundary:huang-zhong-mobile",
            "boundary:wei-yan-mobile", "boundary:lu-bu-mobile", "boundary:zhuge-liang-mobile", "boundary:zhen-ji-mobile", "boundary:zhang-fei-mobile", "boundary:cao-cao-mobile",
            "boundary:sima-yi-mobile", "boundary:huang-gai-mobile", "boundary:gan-ning-mobile", "boundary:diao-chan-mobile", "boundary:sun-quan-mobile", "boundary:huang-yueying-mobile",
            "boundary:guo-jia-mobile", "boundary:da-qiao-mobile", "boundary:lu-xun-mobile", "boundary:sun-shangxiang-mobile", "boundary:zhang-liao-mobile", "boundary:xu-chu-mobile",
            "boundary:xiahou-dun-mobile", "boundary:xiahou-yuan-mobile", "boundary:cao-ren-mobile", "boundary:liu-bei-mobile", "boundary:guan-yu-mobile", "boundary:zhao-yun-mobile",
            "boundary:ma-chao-mobile", "boundary:lu-meng-mobile", "boundary:zhou-yu-mobile", "boundary:hua-tuo-mobile", "boundary:zhang-jiao-mobile", "boundary:sun-jian-mobile",
            "boundary:xun-yu-mobile", "boundary:dian-wei-mobile", "boundary:pang-tong-mobile", "boundary:pang-de-mobile", "boundary:xu-huang-mobile", "boundary:yan-liang-wen-chou-mobile",
            "boundary:wolong-zhuge-liang-mobile", "boundary:dong-zhuo-mobile", "boundary:yuan-shao-mobile", "boundary:meng-huo-mobile", "boundary:zhu-rong-mobile", "boundary:sun-ce-mobile",
            "boundary:deng-ai-mobile", "boundary:jiang-wei-mobile", "boundary:zhang-zhao-zhang-hong-mobile", "boundary:xiao-qiao-mobile", "mou:huang-yueying-ol", "mou:zhou-yu-ol",
            "mou:huang-gai-mobile", "mou:hua-xiong-mobile", "mou:lu-meng-mobile", "mou:yu-jin-mobile", "mou:liu-cheng-mobile", "mou:xu-huang-mobile",
            "mou:chen-gong-mobile", "mou:xiahou-shi-mobile", "mou:zhen-ji-mobile", "mou:yuan-shao-mobile", "mou:zhu-rong-mobile", "mou:pang-tong-mobile",
            "mou:tian-yu-mobile", "mou:han-dang-mobile", "mou:meng-huo-mobile", "mou:lu-zhi-mobile", "mou:zhou-yu-mobile", "mou:xiahou-dun-mobile",
            "mou:da-qiao-mobile", "mou:huang-yueying-mobile", "mou:lu-bu-mobile", "mou:diao-chan-mobile", "mou:fa-zheng-mobile", "mou:gan-ning-mobile",
            "mou:guo-huai-mobile", "ol:cheng-ji-cheng-cui", "ol:sp-sun-shangxiang", "mou:guo-you-zhi-mobile", "mou:sun-jian-ol", "mou:taishi-ci-ol",
            "mou:gongsun-zan-ol", "mou:lu-xun-mobile", "mou:lu-zhi-ol", "mou:zhang-xiu-ol", "mou:wen-chou-ol", "mou:jiang-wei-mobile",
            "mou:hua-xiong-ol", "mou:deng-ai-ol", "mou:dong-zhao-ol", "mou:gongsun-zan-mobile", "mou:zhang-he-mobile", "mou:xiao-qiao-ol",
            "mou:zhu-ran-mobile", "mou:kong-rong-ol", "mou:dong-zhuo-ol", "mou:liu-xie", "mou:xu-sheng", "mou:guan-ping",
            "mou:sun-shangxiang-mobile", "mou:jiang-wei", "mou:dian-wei", "mou:huang-gai", "mou:cao-shuang", "mou:zhuge-jin",
            "mou:hu-che-er", "mou:huang-quan", "mou:cao-hong", "mou:zhou-yu", "mou:lu-su", "mou:sima-yi",
            "mou:jiang-ji", "mou:sima-shi", "mou:jia-xu", "mou:cao-ang", "mou:zhang-xiu", "mou:cheng-yu",
            "mou:guo-jia", "mou:chen-lin", "mou:xun-yu", "mou:ju-shou", "mou:hu-lie", "mou:dong-cheng",
            "mou:wang-ling", "mou:cheng-yu-ol", "ol:han-sui", "ol:xiahou-ba", "classic:zhuge-dan", "ol:he-tai-hou",
            "ol:wen-pin", "ol:dong-yun", "ol:shen-pei", "ol:sun-lu-yu", "ol:cheng-yu", "ol:zhang-lu",
            "ol:wang-rong", "ol:shi-xie", "ol:ma-yun-lu", "ol:ta-dun", "ol:yan-bai-hu", "ol:guan-suo",
            "ol:wang-lang", "ol:bao-san-niang", "ol:deng-zhi", "ol:cao-ying", "ol:dong-zhao", "ol:wu-yan",
            "ol:sp-zhang-he", "ol:xuan-gong-zhu", "ol:huang-yueying-sp", "ol:bu-zhi", "ol:dong-bai", "ol:zhao-xiang",
            "ol:he-qi", "ol:kan-ze", "ol:wang-yun", "ol:zhuge-guo", "ol:xi-zhi-cai", "ol:bei-mi-hu",
            "ol:liu-qi", "ol:lu-zhi-wei", "ol:sima-hui", "ol:xu-rong", "ol:li-jue", "ol:zhang-ji",
            "ol:su-fei", "ol:huang-quan", "ol:tang-zi", "ol:fan-chou", "ol:guo-si", "ol:zhang-qi-ying",
            "ol:cao-chun", "ol:wang-can", "ol:pang-tong-sp", "ol:yuan-tan-yuan-shang", "ol:lv-qian", "ol:lv-kuang-lv-xiang",
            "ol:xin-pi", "ol:hua-man", "ol:niu-jin", "ol:duan-wei", "mobile:liu-zan", "ol:ding-yuan",
            "ol:cao-song", "ol:bian-fu-ren", "ol:zhang-ling", "ol:cao-shuang", "ol:sp-zhang-liao", "ol:wo-long-feng-chu",
            "ol:sun-ru", "ol:liu-pi", "ol:gao-gan", "ol:huo-jun", "ol:qiao-zhou", "ol:du-xi",
            "ol:fu-rong", "ol:zhang-yi", "ol:yan-pu", "ol:luo-xian", "ol:hao-pu", "ol:ma-wan",
            "ol:du-fu-ren", "ol:fei-yi", "ol:zhang-shi-ping", "ol:qian-zhao", "ol:ding-shang-wan", "ol:he-jin",
            "ol:li-wan", "ol:yan-rou", "ol:cao-xi", "ol:hu-jin-ding", "ol:lu-yu-sheng", "ol:wang-kuang",
            "ol:yuan-huan", "ol:zhang-yan", "ol:wu-an-guo", "ol:han-fu", "ol:zhu-jun", "ol:ke-bi-neng",
            "ol:niu-fu", "ol:duan-jiong", "ol:ma-mi-di", "ol:ruan-hui", "ol:tian-chou", "ol:cheng-gong-ying",
            "ol:ma-xiu-ma-tie", "ol:pei-yuan-shao", "ol:wu-jing", "ol:xiang-chong", "ol:yang-feng", "ol:qin-lang",
            "ol:a-hui-nan", "ol:wang-guan", "ol:liu-yan", "ol:meng-da", "ol:cai-mao", "ol:ya-dan",
            "ol:cao-teng", "ol:yang-fu", "ol:wu-tu-gu", "ol:sun-hao", "ol:da-qiao-xiao-qiao", "ol:fan-jiang-zhang-da",
            "ol:wen-yang", "ol:ma-yuan-yi", "ol:hu-ban", "ol:liu-ba", "ol:liu-pan", "ol:qu-huang",
            "ol:ruan-yu", "ol:cao-yu", "ol:bao-xin", "ol:guo-tu", "ol:cui-yan", "ol:li-yi",
            "ol:sun-yuan", "ol:liu-zhang", "ol:tang-tang", "ol:yuan-ji", "ol:kong-shu", "ol:ma-cheng",
            "ol:zhou-qun", "ol:guan-hai", "ol:xie-ling-yu", "ol:li-feng", "ol:cai-fu-ren", "ol:gu-yong",
            "ol:huan-shu-jun", "ol:zhao-zhong", "ol:tao-qian", "ol:pang-ji", "ol:cao-xian", "ol:lu-shi",
            "anniversary:ruan-ji"
        }.Select(GeneralRulesCatalog.CanonicalId).Concat(GeneralRulesCatalog.Aliases
            .Where(pair => pair.Key.StartsWith("mobile:", StringComparison.Ordinal)).Select(pair => pair.Value))
            .ToHashSet(StringComparer.Ordinal);
        var entries = model.GeneralGalleryEntries.Where(entry =>
            detailGeneralIds?.Contains(entry.GeneralId) == true || browseIds.Contains(entry.GeneralId) ||
            entry.GeneralId.StartsWith("ol:mo-", StringComparison.Ordinal) ||
            entry.GeneralId.StartsWith("mobile:", StringComparison.Ordinal)).ToArray();
        bool HasEntry(string id) => entries.Any(entry => entry.GeneralId == GeneralRulesCatalog.CanonicalId(id));
        Program.Assert(entries.Where(entry => entry.GeneralId.StartsWith("le:", StringComparison.Ordinal)).All(entry =>
                CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify(entry.GeneralId) is { Id: "le", SeriesId: "le" }),
            "Every registered Le edition belongs to its selectable Le series with its distinct edition identity.");
        Program.Assert(entries.Where(entry => entry.GeneralId.StartsWith("wumiao:", StringComparison.Ordinal)).All(entry =>
                CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify(entry.GeneralId) is { Id: "wumiao", SeriesId: "wumiao" }),
            "Every registered Wumiao edition belongs to its selectable Wumiao series.");
        Program.Assert(CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify("wumiao:guan-yu") is { Id: "wumiao", SeriesId: "wumiao" } &&
            model.GeneralGallerySeries.Any(series => series.Id == "wumiao"),
            "Wu Guan Yu has a selectable Wumiao series and retains his distinct edition.");
        var olMagicEntries = entries.Where(entry => entry.GeneralId.StartsWith("ol:mo-", StringComparison.Ordinal)).ToArray();
        Program.Assert(olMagicEntries.Any(entry => entry.GeneralId == "ol:mo-sun-quan") &&
            model.GeneralGallerySeries.Any(series => series.Id == "magic") &&
            olMagicEntries.All(entry => CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify(entry.GeneralId)
                is { Id: "magic", SeriesId: "magic" }),
            "Official OL magic editions must expose their distinct selectable Magic series.");
        Program.Assert(HasEntry("wumiao:guan-yu") &&
            HasEntry("wumiao:lu-xun") &&
            HasEntry("wumiao:zhang-fei") &&
            HasEntry("le:cai-wen-ji") &&
            HasEntry("le:cai-yong") &&
            HasEntry("le:cao-zhi") &&
            HasEntry("le:zhou-fei") &&
            HasEntry("le:mi-heng") &&
            HasEntry("le:zou-shi") &&
            HasEntry("le:diao-chan") &&
            HasEntry("mou:zhou-yu-ol") &&
            HasEntry("mou:huang-gai-mobile") &&
            HasEntry("mou:hua-xiong-mobile") &&
            HasEntry("mou:lu-meng-mobile") &&
            HasEntry("mou:yu-jin-mobile") &&
            HasEntry("mou:liu-cheng-mobile") &&
            HasEntry("mou:xu-huang-mobile") &&
            HasEntry("mou:chen-gong-mobile") &&
            HasEntry("mou:xiahou-shi-mobile") &&
            HasEntry("mou:huang-yueying-ol") &&
            entries.Any(entry => entry.GeneralId.StartsWith("mobile:", StringComparison.Ordinal)) &&
            HasEntry("ol:han-sui") &&
            HasEntry("ol:xiahou-ba") &&
            HasEntry("classic:zhuge-dan") &&
            HasEntry("ol:he-tai-hou") &&
            HasEntry("ol:wen-pin") &&
            HasEntry("ol:dong-yun") &&
            HasEntry("ol:shen-pei") &&
            HasEntry("ol:sun-lu-yu") &&
            HasEntry("ol:cheng-yu") &&
            HasEntry("ol:zhang-lu") &&
            HasEntry("ol:wang-rong") &&
            HasEntry("ol:shi-xie") &&
            HasEntry("ol:ma-yun-lu") &&
            HasEntry("ol:ta-dun") &&
            HasEntry("ol:yan-bai-hu") &&
            HasEntry("ol:guan-suo") &&
            HasEntry("ol:wang-lang") &&
            HasEntry("ol:bao-san-niang") &&
            HasEntry("ol:deng-zhi") &&
            HasEntry("ol:cao-ying") &&
            HasEntry("ol:dong-zhao") &&
            HasEntry("ol:wu-yan") &&
            HasEntry("ol:sp-zhang-he") &&
            HasEntry("ol:xuan-gong-zhu") &&
            HasEntry("ol:huang-yueying-sp") &&
            HasEntry("ol:bu-zhi") &&
            HasEntry("ol:dong-bai") &&
            HasEntry("ol:zhao-xiang") &&
            HasEntry("ol:he-qi") &&
            HasEntry("ol:kan-ze") &&
            HasEntry("ol:wang-yun") &&
            HasEntry("ol:zhuge-guo") &&
            HasEntry("ol:xi-zhi-cai") &&
            HasEntry("ol:bei-mi-hu") &&
            HasEntry("ol:liu-qi") &&
            HasEntry("ol:lu-zhi-wei") &&
            HasEntry("ol:sima-hui") &&
            HasEntry("ol:xu-rong") &&
            HasEntry("ol:li-jue") &&
            HasEntry("ol:zhang-ji") &&
            HasEntry("ol:su-fei") &&
            HasEntry("ol:huang-quan") &&
            HasEntry("ol:tang-zi") &&
            HasEntry("ol:fan-chou") &&
            HasEntry("ol:guo-si") &&
            HasEntry("ol:zhang-qi-ying") &&
            HasEntry("ol:cao-chun") &&
            HasEntry("ol:wang-can") &&
            HasEntry("ol:pang-tong-sp") &&
            HasEntry("ol:yuan-tan-yuan-shang") &&
            HasEntry("ol:lv-qian") &&
            HasEntry("ol:lv-kuang-lv-xiang") &&
            HasEntry("ol:xin-pi") &&
            HasEntry("ol:hua-man") &&
            HasEntry("ol:niu-jin") &&
            HasEntry("ol:duan-wei") &&
            HasEntry("mobile:liu-zan") &&
            HasEntry("ol:ding-yuan") &&
            HasEntry("ol:cao-song") &&
            HasEntry("ol:bian-fu-ren") &&
            HasEntry("ol:zhang-ling") &&
            HasEntry("ol:cao-shuang") &&
            HasEntry("ol:sp-zhang-liao") &&
            HasEntry("ol:wo-long-feng-chu") &&
            HasEntry("ol:sun-ru") &&
            HasEntry("ol:liu-pi") &&
            HasEntry("ol:gao-gan") &&
            HasEntry("ol:huo-jun") &&
            HasEntry("ol:qiao-zhou") &&
            HasEntry("ol:du-xi") &&
            HasEntry("ol:fu-rong") &&
            HasEntry("mobile:wang-fu-zhao-lei") &&
            HasEntry("ol:zhang-yi") &&
            HasEntry("ol:yan-pu") &&
            HasEntry("ol:luo-xian") &&
            HasEntry("ol:hao-pu") &&
            HasEntry("ol:ma-wan") &&
            HasEntry("ol:du-fu-ren") &&
            HasEntry("ol:fei-yi") &&
            HasEntry("ol:zhang-shi-ping") &&
            HasEntry("ol:qian-zhao") &&
            HasEntry("ol:ding-shang-wan") &&
            HasEntry("ol:he-jin") &&
            HasEntry("ol:yan-rou") &&
            HasEntry("ol:cao-xi") &&
            HasEntry("ol:hu-jin-ding") &&
            HasEntry("ol:lu-yu-sheng") &&
            HasEntry("ol:wang-kuang") &&
            HasEntry("ol:yuan-huan") &&
            HasEntry("ol:zhang-yan") &&
            HasEntry("ol:wu-an-guo") &&
            HasEntry("ol:han-fu") &&
            HasEntry("ol:zhu-jun") &&
            HasEntry("ol:ke-bi-neng") &&
            HasEntry("ol:niu-fu") &&
            HasEntry("ol:duan-jiong") &&
            HasEntry("ol:ma-mi-di") &&
            HasEntry("ol:ruan-hui") &&
            HasEntry("ol:tian-chou") &&
            HasEntry("ol:cheng-gong-ying") &&
            HasEntry("ol:ma-xiu-ma-tie") &&
            HasEntry("ol:pei-yuan-shao") &&
            HasEntry("ol:wu-jing") &&
            HasEntry("ol:xiang-chong") &&
            HasEntry("ol:yang-feng") &&
            HasEntry("ol:qin-lang") &&
            HasEntry("ol:a-hui-nan") &&
            HasEntry("ol:wang-guan") &&
            HasEntry("ol:liu-yan") &&
            HasEntry("ol:meng-da") &&
            HasEntry("ol:cai-mao") &&
            HasEntry("ol:ya-dan") &&
            HasEntry("ol:cao-teng") &&
            HasEntry("ol:yang-fu") &&
            HasEntry("ol:wu-tu-gu") &&
            HasEntry("ol:da-qiao-xiao-qiao") &&
            HasEntry("ol:wen-yang") &&
            HasEntry("ol:ma-yuan-yi") &&
            HasEntry("ol:hu-ban") &&
            HasEntry("ol:liu-ba") &&
            HasEntry("ol:liu-pan") &&
            HasEntry("ol:qu-huang") &&
            HasEntry("ol:ruan-yu") &&
            HasEntry("ol:cao-yu") &&
            HasEntry("ol:bao-xin") &&
            HasEntry("ol:guo-tu") &&
            HasEntry("ol:cui-yan") &&
            HasEntry("ol:li-yi") &&
            HasEntry("ol:sun-yuan") &&
            HasEntry("ol:liu-zhang") &&
            HasEntry("ol:tang-tang") &&
            HasEntry("ol:yuan-ji") &&
            HasEntry("ol:kong-shu") &&
            HasEntry("ol:ma-cheng") &&
            HasEntry("ol:zhou-qun") &&
            HasEntry("ol:guan-hai") &&
            HasEntry("ol:xie-ling-yu") &&
            HasEntry("ol:li-feng") &&
            HasEntry("ol:cai-fu-ren") &&
            HasEntry("ol:gu-yong") &&
            HasEntry("ol:huan-shu-jun") &&
            HasEntry("ol:zhao-zhong") &&
            HasEntry("ol:tao-qian") &&
            HasEntry("ol:pang-ji") &&
            HasEntry("ol:fan-jiang-zhang-da") &&
            HasEntry("ol:sun-hao") &&
            HasEntry("ol:li-wan"),
            "The real catalogue must expose the added mobile and OL editions.");
        var panel = new GeneralGalleryPanel { DataContext = model };
        try
        {
            foreach (var entry in entries.Where(entry => detailGeneralIds is null || detailGeneralIds.Contains(entry.GeneralId)))
            {
                model.GeneralGallerySearchText = entry.Name;
                Program.Assert(model.GeneralGalleryEntries.Contains(entry) && entry.IsPlayable && entry.HasPortrait,
                    $"Searching {entry.GeneralId} must retain its playable entry and registered artwork.");
                model.SelectGeneralGalleryEntryCommand.Execute(entry);
                if (entry.GeneralId.StartsWith("anniversary:", StringComparison.Ordinal))
                    Program.Assert(entry.GroupName.Contains("十周年", StringComparison.Ordinal),
                        "Anniversary details must visibly identify their rules edition alongside existing OL variants.");
                Program.Assert(model.SelectedGeneralGalleryEntry == entry &&
                    entry.Portrait.Brush is ImageBrush { ImageSource: not null } &&
                    entry.Skills.Count > 0 && entry.Skills.All(skill => !string.IsNullOrWhiteSpace(skill.Name)),
                    "Opening edition details must decode its own local portrait and show the registered skills.");
                if (entry.GeneralId == "wumiao:lu-xun")
                {
                    Program.Assert(entry.Name == "武陆逊" && entry.Skills.Count == 3 &&
                        new[] { "wumiao:xiongmu", "wumiao:zhangcai", "wumiao:ruxian" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name &&
                                skill.Description == registered.Description);
                        }), "Wu Lu Xun must show all three complete printed skills with his distinct artwork.");
                    Program.Assert(CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify(entry.GeneralId)
                        is { Id: "wumiao", SeriesId: "wumiao" }, "Wu Lu Xun must belong to the selectable Wumiao series.");
                }
                if (entry.GeneralId == "mou:zhou-yu-ol")
                {
                    Program.Assert(entry.Name == "谋周瑜·OL" && entry.Skills.Count == 2 &&
                        new[] { "mou-ol:zuifeng", "mou-ol:guqu" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name &&
                                skill.Description == registered.Description);
                        }), "OL Mou Zhou Yu must show both complete current-client rules with his distinct artwork.");
                    Program.Assert(CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify(entry.GeneralId)
                        is { Id: "mou", SeriesId: "mou" }, "OL Mou Zhou Yu must belong to the selectable Mou series.");
                }
                if (entry.GeneralId == "mou:huang-yueying-ol")
                {
                    Program.Assert(entry.Name == "谋黄月英·OL" && entry.Skills.Count == 2 &&
                        new[] { "mou-ol:lixian", "mou-ol:bingcai" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name &&
                                skill.Description == registered.Description);
                        }), "OL Mou Huang Yueying must show both complete printed skills with her distinct artwork.");
                    Program.Assert(CardGame.Wpf.Presentation.GeneralGalleryCatalog.Classify(entry.GeneralId)
                        is { Id: "mou", SeriesId: "mou" }, "The distinct OL edition must be selectable in the Mou series.");
                }
                if (entry.GeneralId == "le:diao-chan")
                {
                    Program.Assert(entry.Name == "乐貂蝉" && entry.Skills.Count == 2 &&
                        new[] { "le:tanban", "le:diou" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                        }), "Le Diao Chan must show both complete primary rules and her distinct official portrait.");
                }
                if (entry.GeneralId == "le:zou-shi")
                {
                    Program.Assert(entry.Name == "乐邹氏" && entry.Skills.Count == 2 &&
                        new[] { "le:yunzheng", "le:huoxin" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name &&
                                skill.Description == registered.Description);
                        }), "Le Zou Shi must show both complete registered skills with her own edition portrait.");
                }
                if (entry.GeneralId == "ol:mo-sun-quan")
                {
                    Program.Assert(entry.Name == "魔孙权" && entry.Skills.Count == 3 &&
                        new[] { "ol:mo-quanyu", "ol:mo-tianen", "ol:mo-qiangang" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name &&
                                skill.Description == registered.Description);
                        }), "Mo Sun Quan's actual details show exactly the complete three printed skills.");
                    Program.Assert(entry.Skills.Single(skill => skill.Name == "乾纲").Description.Contains(
                        registry.GetSkill("ol:mo-rumo").Description, StringComparison.Ordinal),
                        "Qiangang must visibly explain the complete shared Rumo rule without a fourth printed skill.");
                }
                if (entry.GeneralId == "ol:li-feng")
                    Program.Assert(entry.Skills.Count == 2 && new[] { "ol:jiyun", "ol:shuliang" }.All(id =>
                    {
                        var registered = registry.GetSkill(id);
                        return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                    }), "Li Feng's actual gallery details must retain both complete registered skill descriptions.");
                if (entry.GeneralId == "ol:cai-fu-ren")
                    Program.Assert(entry.Skills.Count == 2 && new[] { "ol:xianzhou", "ol:qieting" }.All(id =>
                    {
                        var registered = registry.GetSkill(id);
                        return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                    }), "Cai Fu Ren's actual gallery details must retain both complete registered skill descriptions.");
                if (entry.GeneralId == "ol:gu-yong")
                    Program.Assert(entry.Skills.Count == 2 && new[] { "ol:shenxing", "ol:bingyi" }.All(id =>
                    {
                        var registered = registry.GetSkill(id);
                        return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                    }), "Ordinary Gu Yong's actual gallery details must retain both complete registered skill descriptions.");
                if (entry.GeneralId == "ol:huan-shu-jun")
                    Program.Assert(entry.Name == "环淑君" && entry.Skills.Count == 3 &&
                        new[] { "ol:huanpei", "ol:wenyi", "ol:miaoxin" }.All(id =>
                    {
                        var registered = registry.GetSkill(id);
                        return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                    }), "Huan Shu Jun's actual gallery details must retain her name and all three complete registered skill descriptions.");
                if (entry.GeneralId == "ol:pang-ji")
                    Program.Assert(entry.Name == "逢纪" && entry.Skills.Count == 2 &&
                        new[] { "ol:biguo", "ol:douyu" }.All(id =>
                    {
                        var registered = registry.GetSkill(id);
                        return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                    }), "Pang Ji's actual gallery retains both complete registered skill descriptions.");
                if (entry.GeneralId == "ol:tao-qian")
                    Program.Assert(entry.Name == "陶谦" && entry.Skills.Count == 3 &&
                        new[] { "ol:zongluan", "ol:zhaohuo", "ol:wenren" }.All(id =>
                    {
                        var registered = registry.GetSkill(id);
                        return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                    }), "Tao Qian's actual gallery details must retain his name and all three complete registered skill descriptions.");
                if (entry.GeneralId == "ol:zhao-zhong")
                {
                        Program.Assert(entry.Name == "赵忠" && entry.Skills.Count == 2 &&
                            new[] { "ol:pengbi", "ol:dici" }.All(id =>
                        {
                            var registered = registry.GetSkill(id);
                            return entry.Skills.Any(skill => skill.Name == registered.Name && skill.Description == registered.Description);
                        }), "Zhao Zhong's actual gallery details retain his name and both complete registered skill descriptions, including linked derived skill text.");
                    var pengbi = entry.Skills.Single(skill => skill.Name == registry.GetSkill("ol:pengbi").Name);
                    Program.Assert(new[] { "ol:yintian", "ol:biri" }.All(id =>
                        pengbi.Description.Contains(registry.GetSkill(id).Name, StringComparison.Ordinal) &&
                        pengbi.Description.Contains(registry.GetSkill(id).Description, StringComparison.Ordinal)),
                        "Pengbi's visible details must include both full derived skill rules.");
                }
                Program.Render(panel, 1120, 820, Path.Combine(output,
                    GeneralArt.NormalizeKey(entry.GeneralId) + "-gallery.png"));
                if (entry.HealthImages.Count > 10)
                {
                    var health = Program.Find<System.Windows.Controls.ItemsControl>(panel)
                        .Single(control => control.Name == "SelectedGeneralHealthIcons");
                    var icons = Program.Find<System.Windows.Controls.Image>(health).ToArray();
                    Program.Assert(icons.Length == entry.HealthImages.Count && health.ActualHeight > 48 &&
                        icons.All(icon =>
                        {
                            var bounds = icon.TransformToAncestor(health).TransformBounds(
                                new System.Windows.Rect(0, 0, icon.ActualWidth, icon.ActualHeight));
                            return bounds.Left >= 0 && bounds.Top >= 0 &&
                                bounds.Right <= health.ActualWidth + 0.5 && bounds.Bottom <= health.ActualHeight + 0.5;
                        }), "High-HP portraits must wrap every health icon inside the actual visible strip.");
                }
                var skillsScroller = Program.Find<System.Windows.Controls.ScrollViewer>(panel)
                    .Single(view => view.Name == "GeneralSkillsScroller");
                if (skillsScroller.ScrollableHeight > 0)
                {
                    skillsScroller.ScrollToEnd();
                    Program.Render(panel, 1120, 820, Path.Combine(output,
                        GeneralArt.NormalizeKey(entry.GeneralId) + "-gallery-skills-bottom.png"));
                    Program.Assert(Math.Abs(skillsScroller.VerticalOffset - skillsScroller.ScrollableHeight) < 1,
                        "Long registered skill descriptions must remain reachable through the actual details scroller.");
                    skillsScroller.ScrollToTop();
                    panel.UpdateLayout();
                }
                model.CloseGeneralGalleryDetailsCommand.Execute(null);
            }
            Program.Assert(Program.Engine(model).Revision == revision,
                "Browsing and closing edition details must not advance the match.");
        }
        finally { panel.DataContext = null; }
    }
}
