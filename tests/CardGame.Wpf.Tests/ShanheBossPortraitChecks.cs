using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rectangle = System.Windows.Shapes.Rectangle;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheBossPortraitChecks
{
    private static readonly IReadOnlyDictionary<string, string> VerifiedSourcePortraitPaths = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["shanhe:source-buheng-2451-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000333.png",
        ["shanhe:source-buheng-2451-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000333.png",
        ["shanhe:source-buheng-2451-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000333.png",
        ["shanhe:source-chengzhan-2462-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000191.png",
        ["shanhe:source-chengzhan-2462-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000191.png",
        ["shanhe:source-chengzhan-2462-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000191.png",
        ["shanhe:source-wucong-2478-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000257.png",
        ["shanhe:source-wucong-2478-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000257.png",
        ["shanhe:source-liusandao-2479-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000096.png",
        ["shanhe:source-liusandao-2479-medium"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000096.png",
        ["shanhe:source-liusandao-2479-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000096.png",
        ["shanhe:source-liusandao-2479-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000096.png",
        ["shanhe:source-liusandao-2479-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000096.png",
        ["shanhe:source-xunyue-2480-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000195.png",
        ["shanhe:source-xunyue-2480-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000195.png",
        ["shanhe:source-xunyue-2480-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000195.png",
        ["shanhe:source-armorer-2488-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000204.png",
        ["shanhe:source-armorer-2488-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000204.png",
        ["shanhe:source-armorer-2488-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000204.png",
        ["shanhe:source-xinping-2489"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000261.png",
        ["shanhe:source-xusheng-2635-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_587.png",
        ["shanhe:source-xusheng-2635-pojun"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_587.png",
        ["shanhe:source-sunce-2655"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_452.png",
        ["shanhe:source-qiangwang-2669"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_415.png",
        ["shanhe:source-xunyu-2675"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_515.png",
        ["shanhe:source-mou-dongzhuo-2684"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_605.png",
        ["shanhe:mulu-2026"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000073.png",
        ["shanhe:mulu-rage-elephant-2026"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000001.png",
        ["shanhe:mulu-angry-elephant-2026"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000001.png",
        ["shanhe:source-shengui-264501-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_193.png",
        ["shanhe:source-shennu-268701"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/60501_1.png",
        ["shanhe:source-agriculture-officer-2500-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000213.png",
        ["shanhe:source-agriculture-officer-2500-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000213.png",
        ["shanhe:source-agriculture-officer-2500-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000213.png",
        ["shanhe:source-veteran-2474-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000129.png",
        ["shanhe:source-veteran-2474-medium"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000129.png",
        ["shanhe:source-veteran-2474-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000129.png",
        ["shanhe:source-veteran-2474-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000129.png",
        ["shanhe:source-veteran-2474-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000129.png",
        ["shanhe:source-wuhuan-rider-2467-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000107.png",
        ["shanhe:source-wuhuan-rider-2467-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000107.png",
        ["shanhe:source-wuhuan-rider-2467-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000107.png",
        ["shanhe:source-warrior-2487-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000230.png",
        ["shanhe:source-warrior-2487-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000230.png",
        ["shanhe:source-warrior-2487-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000230.png",
        ["shanhe:source-qiang-raider-2473-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000125.png",
        ["shanhe:source-qiang-raider-2473-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000125.png",
        ["shanhe:source-qiang-raider-2473-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000125.png",
        ["shanhe:source-jiangdong-veteran-2497-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000283.png",
        ["shanhe:source-jiangdong-veteran-2497-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000283.png",
        ["shanhe:source-jiangdong-veteran-2497-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000283.png",
        ["shanhe:source-local-magnate-2460-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000320.png",
        ["shanhe:source-local-magnate-2460-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000320.png",
        ["shanhe:source-local-magnate-2460-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000320.png",
        ["shanhe:source-wuque-assassin-2493-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000228.png",
        ["shanhe:source-wuque-assassin-2493-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000228.png",
        ["shanhe:source-wuque-assassin-2493-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000228.png",
        ["shanhe:source-executioner-2495-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000237.png",
        ["shanhe:source-executioner-2495-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000237.png",
        ["shanhe:source-executioner-2495-medium"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000237.png",
        ["shanhe:source-militia-trio-2475-first-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000208.png",
        ["shanhe:source-militia-trio-2475-first-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000208.png",
        ["shanhe:source-militia-trio-2475-first-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000208.png",
        ["shanhe:source-militia-trio-2475-second-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000249.png",
        ["shanhe:source-militia-trio-2475-second-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000249.png",
        ["shanhe:source-militia-trio-2475-third-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000309.png",
        ["shanhe:source-militia-trio-2475-third-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000309.png",
        ["shanhe:source-militia-trio-2475-third-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000309.png",
        ["shanhe:source-caosong-trio-2485-caosong-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_438.png",
        ["shanhe:source-caosong-trio-2485-caosong-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_438.png",
        ["shanhe:source-caosong-trio-2485-caosong-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_438.png",
        ["shanhe:source-guard-trio-2491-guard-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000222.png",
        ["shanhe:source-guard-trio-2491-guard-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000222.png",
        ["shanhe:source-guard-trio-2491-captain-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000233.png",
        ["shanhe:source-guard-trio-2491-captain-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000233.png",
        ["shanhe:source-liuyan-trio-2463-soldier-4861-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000181.png",
        ["shanhe:source-liuyan-trio-2463-soldier-4861-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000181.png",
        ["shanhe:source-liuyan-trio-2463-soldier-4862-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000181.png",
        ["shanhe:source-liuyan-trio-2463-soldier-4862-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000181.png",
        ["shanhe:source-liuyan-trio-2463-liuyan-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_634.png",
        ["shanhe:source-liuyan-trio-2463-liuyan-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_634.png",
        ["shanhe:source-liuyan-trio-2463-liuyan-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_634.png",
        ["shanhe:source-caosong-trio-2485-guard-4861-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000215.png",
        ["shanhe:source-caosong-trio-2485-guard-4861-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000215.png",
        ["shanhe:source-caosong-trio-2485-guard-4862-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000215.png",
        ["shanhe:source-caosong-trio-2485-guard-4862-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000215.png",
        ["shanhe:source-linzhonggu-2476-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000301.png",
        ["shanhe:source-linzhonggu-2476-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000301.png",
        ["shanhe:source-qianzhao-pair-2471-qianzhao-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_580.png",
        ["shanhe:source-qianzhao-pair-2471-qianzhao-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_580.png",
        ["shanhe:source-qianzhao-pair-2471-wuhuan-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000297.png",
        ["shanhe:source-qianzhao-pair-2471-wuhuan-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000297.png",
        ["shanhe:source-qianzhao-pair-2471-wuhuan-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000297.png",
        ["shanhe:source-elemental-trio-2472-water-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4067.png",
        ["shanhe:source-elemental-trio-2472-fire-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4030.png",
        ["shanhe:source-elemental-trio-2472-fire-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4030.png",
        ["shanhe:source-elemental-trio-2472-fire-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4030.png",
        ["shanhe:source-elemental-trio-2472-qiongqi-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4138.png",
        ["shanhe:source-elemental-trio-2472-qiongqi-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4138.png",
        ["shanhe:source-elemental-trio-2472-qiongqi-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4138.png",
        ["shanhe:source-beigong-duo-2450-statue"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_4410.png",
        ["shanhe:source-beigong-duo-2450-beigong-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000355.png",
        ["shanhe:source-beigong-duo-2450-beigong-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000355.png",
        ["shanhe:source-white-horse-2454-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000128.png",
        ["shanhe:source-white-horse-2454-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000128.png",
        ["shanhe:source-white-horse-2454-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000128.png",
        ["shanhe:source-war-elephant-2453-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000003.png",
        ["shanhe:source-war-elephant-2453-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000003.png",
        ["shanhe:source-war-elephant-2453-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000003.png",
        ["shanhe:source-vanguard-2494-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000236.png",
        ["shanhe:source-vanguard-2494-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000236.png",
        ["shanhe:source-vanguard-2494-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000236.png",
        ["shanhe:source-ranger-2481-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000251.png",
        ["shanhe:source-ranger-2481-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000251.png",
        ["shanhe:source-ranger-2481-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000251.png",
        ["shanhe:source-ranger-2481-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000251.png",
        ["shanhe:source-youyan-warrior-2466-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000170.png",
        ["shanhe:source-youyan-warrior-2466-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000170.png",
        ["shanhe:source-armory-commander-2486-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000282.png",
        ["shanhe:source-armory-commander-2486-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000282.png",
        ["shanhe:source-armory-commander-2486-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000282.png",
        ["shanhe:source-demon-assassin-2457-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000299.png",
        ["shanhe:source-demon-assassin-2457-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000299.png",
        ["shanhe:source-demon-assassin-2457-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000299.png",
        ["shanhe:source-demon-raider-2458-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000303.png",
        ["shanhe:source-demon-raider-2458-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000303.png",
        ["shanhe:source-demon-raider-2458-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000303.png",
        ["shanhe:source-demon-trio-2464-raider-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000303.png",
        ["shanhe:source-demon-trio-2464-raider-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000303.png",
        ["shanhe:source-demon-trio-2464-raider-hell"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000303.png",
        ["shanhe:source-demon-archer-2461-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000238.png",
        ["shanhe:source-demon-archer-2461-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000238.png",
        ["shanhe:source-demon-archer-2461-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000238.png",
        ["shanhe:source-demon-trio-2464-archer-base"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000238.png",
        ["shanhe:source-demon-trio-2464-archer-hard"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000238.png",
        ["shanhe:source-demon-trio-2464-archer-nightmare"] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000238.png",
        [StandardShanheSourceCoalitionScoutPackage.BaseId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000148.png",
        [StandardShanheSourceCoalitionScoutPackage.HardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000148.png",
        [StandardShanheSourceCoalitionScoutPackage.HellId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000148.png",
        [StandardShanheSourceCoalitionTrioPackage.ScoutBaseId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000148.png",
        [StandardShanheSourceCoalitionTrioPackage.ScoutHardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000148.png",
        [StandardShanheSourceCoalitionTrioPackage.ScoutHellId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000148.png",
        [StandardShanheSourceCoalitionSpyPackage.BaseId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000304.png",
        [StandardShanheSourceCoalitionSpyPackage.HardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000304.png",
        [StandardShanheSourceCoalitionSpyPackage.NightmareId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000304.png",
        [StandardShanheSourceCoalitionTrioPackage.SpyBaseId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000304.png",
        [StandardShanheSourceCoalitionTrioPackage.SpyHardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000304.png",
        [StandardShanheSourceCoalitionTrioPackage.SpyNightmareId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000304.png",
        [StandardShanheSourceCoalitionTrioPackage.QuartermasterBaseId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000175.png",
        [StandardShanheSourceCoalitionTrioPackage.QuartermasterHardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000175.png",
        [StandardShanheS5ZhangniujiaoPackage.ZhangniujiaoId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000367.png",
        [StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000367.png",
        [StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000367.png",
        [StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000367.png",
        [StandardShanheSourceZhangYanPackage.ZhangYanBaseId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000364.png",
        [StandardShanheSourceZhangYanPackage.ZhangYanMediumId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000364.png",
        [StandardShanheSourceZhangYanPackage.ZhangYanHardId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000364.png",
        [StandardShanheS5YuanshuPackage.YuanyaoId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000339.png",
        [StandardShanheS5YuanshuPackage.YuanshuId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_100.png",
        [StandardShanheS5YuanshuPackage.YuanyinId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000338.png",
        [StandardShanheS5GongsunZanPackage.GongsunZanId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_320.png",
        [StandardShanheS5GongsunZanPackage.GongsunYueId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000165.png",
        [StandardShanheS5XiliangDarkPackage.QinyiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000413.png",
        [StandardShanheS5XiliangDarkPackage.ChenweiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000121.png",
        [StandardShanheS5XiliangDarkPackage.LiheiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000414.png",
        [StandardShanheS5YuduPackage.YuduId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000098.png",
        [StandardShanheS5ZhangliangPackage.ZhangliangId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000367.png",
        [StandardShanheS5XuncanCaoshiPackage.XuncanId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_525.png",
        [StandardShanheS5XuncanCaoshiPackage.CaoshiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000051.png",
        [StandardShanheS5XujingXuqiuPackage.XuqiuId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000307.png",
        [StandardShanheS5BossPackage.DongZhuoFengyiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_470.png",
        [StandardShanheS5BossPackage.LvbuBajiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_319.png",
        [StandardShanheS5BossPackage.DiaoChanBiyueId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_445.png",
        [StandardShanheS5MaSiblingsPackage.MachaoId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_303.png",
        [StandardShanheS5MaSiblingsPackage.MayunluId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_346.png",
        [StandardShanheS5DianweiWeiziPackage.DianweiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_229.png",
        [StandardShanheS5DianweiWeiziPackage.WeiziId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_501.png",
        [StandardShanheS5XuyouJushouPackage.XuyouId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_406.png",
        [StandardShanheS5XuyouJushouPackage.JushouId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_330.png",
        [StandardShanheS5MoushiPackage.XunchenId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_460.png",
        [StandardShanheS5MoushiPackage.GuotuId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_624.png",
        [StandardShanheS5MoushiPackage.ShenpeiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_459.png",
        [StandardShanheS5CaoWeiPioneersPackage.CaozhenId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_321.png",
        [StandardShanheS5CaoWeiPioneersPackage.CaocaoId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_311.png",
        [StandardShanheS5CaoWeiPioneersPackage.CaohongId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_114.png",
        [StandardShanheS5TaoyuanPackage.ZhangfeiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_301.png",
        [StandardShanheS5TaoyuanPackage.LiubeiId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_299.png",
        [StandardShanheS5TaoyuanPackage.GuanyuId] = "src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_300.png"
    };

    private static readonly IReadOnlySet<int> VerifiedOrdinaryBattleIds = new HashSet<int>
        { 2455, 2459, 2456, 2465, 2453, 2454, 2494, 2481, 2466, 2486, 2458, 2461, 2457, 2464,
          2500, 2474, 2467, 2487, 2473, 2497, 2460, 2493, 2495, 2475, 2485, 2491, 2463, 2476, 2471, 2472, 2450,
          2451, 2462, 2478, 2479, 2480, 2488, 2489, 2635, 2655, 2669, 2675, 2684, 2397, 2645, 2687 };

    private static readonly IReadOnlySet<string> OrdinaryTroopPortraitIds = VerifiedSourcePortraitPaths.Keys
        .Where(id => id.StartsWith("shanhe:source-", StringComparison.Ordinal) &&
            !id.StartsWith("shanhe:source-zhang", StringComparison.Ordinal)).ToHashSet(StringComparer.Ordinal);

    internal static string? GetVerifiedSourcePortraitPath(string id) =>
        VerifiedSourcePortraitPaths.TryGetValue(id, out var path) ? path : null;

    public static void PublishedS5BossAliasesDecodeAndRenderOnRealSeats(string output)
    {
        var checkedIds = new HashSet<string>(StringComparer.Ordinal);
        var unverified = new HashSet<string>(StringComparer.Ordinal);
        var registeredOnly = new HashSet<string>(StringComparer.Ordinal)
            { StandardShanheS5ZhangniujiaoPackage.ZhangniujiaoId, StandardShanheSourceZhangYanPackage.ZhangYanBaseId };
        foreach (var tier in new[] { 1, 2, 3 })
        {
            var definition = StandardShanheCampaignCatalog.CreateDefinition(tier, StandardShanheCampaignCatalog.Returning2026EditionId);
            foreach (var node in definition.Nodes)
            foreach (var encounter in node.Encounters ?? [])
            {
                var ids = encounter.Enemies.Select(member => member.GeneralId)
                    .Concat(encounter.BossTransition is { } stage ? new[] { stage.GeneralId } : [])
                    .Where(id => id.StartsWith("shanhe:s5-", StringComparison.Ordinal) ||
                        id.StartsWith("shanhe:source-zhang", StringComparison.Ordinal) || OrdinaryTroopPortraitIds.Contains(id))
                    .Distinct(StringComparer.Ordinal).Where(id => !checkedIds.Contains(id)).ToArray();
                if (ids.Length == 0) continue;
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(Setup(definition, node, encounter) with { BossTransition = encounter.BossTransition });
                foreach (var id in ids) CheckPortrait(registry, id, checkedIds);
            }
        }
        foreach (var difficulty in new[] { 6, 11, 16, 25 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var node = source.Nodes.Single(candidate => candidate.Id == "source-city-5068");
            var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2653);
            var first = difficulty < 11 ? StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId : difficulty < 16
                ? StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId : StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId;
            var second = difficulty < 11 ? StandardShanheSourceZhangYanPackage.ZhangYanMediumId : StandardShanheSourceZhangYanPackage.ZhangYanHardId;
            Program.Assert(encounter.Enemies.Count == 1 && encounter.Enemies[0].GeneralId == first && encounter.BossTransition?.GeneralId == second,
                "Natural source2653 difficulty boundaries retain their actual separate first and second body identities.");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(Setup(source, node, encounter) with
                { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition });
            CheckPortrait(registry, first, checkedIds); CheckPortrait(registry, second, checkedIds);
        }
        foreach (var difficulty in new[] { 1, 6, 11, 16, 21 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var groups = source.Nodes.SelectMany(node => (node.Encounters ?? [])
                .Where(encounter => encounter.SourceBattleId is { } battleId && VerifiedOrdinaryBattleIds.Contains(battleId))
                .Select(encounter => (Node: node, Encounter: encounter)))
                .GroupBy(pair => pair.Encounter.SourceBattleId).Select(group => group.First()).ToArray();
            Program.Assert(new HashSet<int?> { 2455, 2459, 2456, 2465 }.IsSubsetOf(groups.Select(pair => pair.Encounter.SourceBattleId)),
                "Every represented source tier still naturally publishes all four verified coalition recipes; other troop families retain their own route availability.");
            foreach (var pair in groups)
            {
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(Setup(source, pair.Node, pair.Encounter) with
                    { SourceDifficultyId = difficulty, BossTransition = pair.Encounter.BossTransition });
                foreach (var id in pair.Encounter.Enemies.Select(enemy => enemy.GeneralId)
                    .Concat(pair.Encounter.BossTransition is { } stage ? new[] { stage.GeneralId } : []))
                    CheckPortrait(registry, id, checkedIds);
            }
        }
        // These two bodies are registered content, but no supported natural
        // source route selects them. Do not count registry presence as a draw.
        var registrationProbe = new ShanheBattleSetup(83, 1, "registered-only-zhangyan-portraits", "classic:guan-yu", 4,
            0, 0, 0, 0, [], [], [], [new(StandardShanheS5ZhangniujiaoPackage.ZhangniujiaoId)], [], "registered-only-portrait-probe")
            { BossTransition = new(0, StandardShanheSourceZhangYanPackage.ZhangYanBaseId, 4, 4) };
        var registrationRegistry = StandardShanheCampaignCatalog.CreateBattleRegistry(registrationProbe);
        var registeredIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in registeredOnly) CheckPortrait(registrationRegistry, id, registeredIds);
        Program.Assert(!registeredOnly.Overlaps(checkedIds) && registeredIds.SetEquals(registeredOnly),
            "The two unused base bodies have verified registered portraits and are explicitly separate from naturally selected first or transition forms.");
        Program.Assert(unverified.Count == 0 && new[]
        {
            StandardShanheS5MaSiblingsPackage.MachaoId, StandardShanheS5MaSiblingsPackage.MayunluId,
            StandardShanheS5DongwuVeteransPackage.ChengpuId, StandardShanheS5DongwuVeteransPackage.HandangId,
            StandardShanheS5MoushiPackage.XunchenId, StandardShanheS5MoushiPackage.GuotuId,
            StandardShanheS5MoushiPackage.ShenpeiId, StandardShanheS5HuaxiongBasePackage.HuaxiongId,
            StandardShanheS5ZhangxiuPackage.ZhangxiuId, StandardShanheS5CaoWeiPioneersPackage.CaozhenId,
            StandardShanheS5CaoWeiPioneersPackage.CaocaoId, StandardShanheS5CaoWeiPioneersPackage.CaohongId,
            StandardShanheS5ZuociHuatuoPackage.ZuociId, StandardShanheS5ZuociHuatuoPackage.HuatuoId
        }.All(checkedIds.Contains),
            "The actual natural S5 and ZhangYan source rosters have real verified portraits without an invented required placeholder: " + string.Join(",", unverified));
        Program.Assert(VerifiedSourcePortraitPaths.Keys.Except(registeredOnly).All(checkedIds.Contains),
            "Every naturally selected imported source portrait is checked on its actual enemy or transition registry ID; only the two explicitly registered-only base forms are excluded.");

        var ordinaryAliases = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            [StandardShanheSourceCaiWenjiPackage.BaseId] = "boundary-cai-wen-ji",
            [StandardShanheSourceCaiWenjiPackage.HardId] = "boundary-cai-wen-ji",
            [StandardShanheSourceCaiWenjiPackage.NightmareId] = "boundary-cai-wen-ji",
            [StandardShanheSourceMaRiDiPackage.BaseId] = "ol-ma-mi-di",
            [StandardShanheSourceMaRiDiPackage.HardId] = "ol-ma-mi-di",
            [StandardShanheSourceMaRiDiPackage.NightmareId] = "ol-ma-mi-di",
            [StandardShanheSourceMaRiDiPackage.HellId] = "ol-ma-mi-di",
            [StandardShanheSourceZhangHePackage.BaseId] = "ol-sp-zhang-he",
            [StandardShanheSourceZhangHePackage.WeijingId] = "ol-sp-zhang-he",
            [StandardShanheSourceGaoGanPackage.BaseId] = "ol-gao-gan",
            [StandardShanheSourceGaoGanPackage.HardId] = "ol-gao-gan",
            [StandardShanheSourceGaoGanPackage.HellId] = "ol-gao-gan",
            ["shanhe:source-zuo-ci-2470-base"] = "zuo-ci",
            ["shanhe:source-zuo-ci-2470-hard"] = "zuo-ci",
            ["shanhe:source-zuo-ci-2470-nightmare"] = "zuo-ci",
            ["shanhe:source-jiangdong-trio-2492-chengpu-base"] = "cheng-pu",
            ["shanhe:source-jiangdong-trio-2492-chengpu-hard"] = "cheng-pu",
            ["shanhe:source-jiangdong-trio-2492-chengpu-hell"] = "cheng-pu",
            ["shanhe:source-jiangdong-trio-2492-handang-base"] = "han-dang",
            ["shanhe:source-jiangdong-trio-2492-handang-hard"] = "han-dang",
            ["shanhe:source-jiangdong-trio-2492-handang-nightmare"] = "han-dang",
            ["shanhe:source-jiangdong-trio-2492-huanggai-base"] = "boundary-huang-gai",
            ["shanhe:source-jiangdong-trio-2492-huanggai-hard"] = "boundary-huang-gai",
            ["shanhe:source-wolong-trio-2482-wolong-base"] = "wolong-zhuge-liang",
            ["shanhe:source-wolong-trio-2482-wolong-hard"] = "wolong-zhuge-liang",
            ["shanhe:source-wolong-trio-2482-wolong-nightmare"] = "wolong-zhuge-liang",
            ["shanhe:source-wolong-trio-2482-wolong-hell"] = "wolong-zhuge-liang",
            ["shanhe:source-wolong-trio-2482-simahui-base"] = "ol-sima-hui",
            ["shanhe:source-wolong-trio-2482-simahui-hard"] = "ol-sima-hui",
            ["shanhe:source-wolong-trio-2482-simahui-nightmare"] = "ol-sima-hui",
            ["shanhe:source-wolong-trio-2482-pangtong-base"] = "boundary-pang-tong",
            ["shanhe:source-wolong-trio-2482-pangtong-nightmare"] = "boundary-pang-tong",
            ["shanhe:source-scholars-2490-caiyong-base"] = "cai-yong",
            ["shanhe:source-scholars-2490-caiyong-hard"] = "cai-yong",
            ["shanhe:source-scholars-2490-caiyong-hell"] = "cai-yong",
            ["shanhe:source-scholars-2490-yufan-base"] = "boundary-yu-fan",
            ["shanhe:source-scholars-2490-yufan-hard"] = "boundary-yu-fan",
            ["shanhe:source-scholars-2490-kongrong-base"] = "mou-kong-rong-ol",
            ["shanhe:source-scholars-2490-kongrong-nightmare"] = "mou-kong-rong-ol",
            ["shanhe:source-scholars-2490-maridi-base"] = "ol-ma-mi-di",
            ["shanhe:source-scholars-2490-maridi-hard"] = "ol-ma-mi-di",
            ["shanhe:source-scholars-2490-wangcan-base"] = "ol-wang-can",
            ["shanhe:source-scholars-2490-wangcan-hard"] = "ol-wang-can",
            ["shanhe:source-scholars-2490-wangcan-hell"] = "ol-wang-can"
        };
        foreach (var (id, expectedKey) in ordinaryAliases)
        {
            var expected = GeneralArt.GetSkin(expectedKey);
            Program.Assert(expected is not null && GeneralArt.GetSkin(id)?.LocalPath == expected.LocalPath &&
                GeneralArt.GetPortrait(id, decodeWidth: 96) is ImageBrush
                    { ImageSource: BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } } brush && HasPaintedPixels(brush),
                "Every published ordinary source form must decode and paint its exact existing local portrait: " + id);
        }

        RenderActualMaSiblingsTable(output);
        RenderActualCaiWenjiTable(output);
        RenderActualLocalGarrisonTable(output);
        RenderActualVerifiedOrdinaryTable(output, 2465,
            [StandardShanheSourceCoalitionTrioPackage.SpyNightmareId, StandardShanheSourceCoalitionTrioPackage.ScoutHellId,
             StandardShanheSourceCoalitionTrioPackage.QuartermasterHardId],
            "shanhe-coalition-trio-2465-source25-portraits.png", "source-city-5101");
        RenderActualVerifiedOrdinaryTable(output, 2464,
            [StandardShanheSourceDemonAssassinPackage.HellId, StandardShanheSourceDemonArcherPackage.TrioNightmareId,
             StandardShanheSourceDemonRaiderPackage.TrioHellId], "shanhe-demon-trio-2464-source25-portraits.png");
        RenderActualVerifiedOrdinaryTable(output, 2453, [StandardShanheSourceWarElephantPackage.HellId],
            "shanhe-war-elephant-2453-source25-portrait.png");
        RenderActualVerifiedOrdinaryTable(output, 2494, [StandardShanheSourceVanguardPackage.NightmareId],
            "shanhe-vanguard-2494-source25-portrait.png");
        RenderActualVerifiedOrdinaryTable(output, 2491, StandardShanheSourceGuardTrioPackage.GetRosterGeneralIds(25),
            "shanhe-guard-trio-2491-source25-portraits.png");
        RenderActualVerifiedOrdinaryTable(output, 2472, StandardShanheSourceElementalTrioPackage.GetRosterGeneralIds(25),
            "shanhe-elemental-trio-2472-source25-portraits.png");
        RenderActualVerifiedOrdinaryTable(output, 2450, StandardShanheSourceBeigongDuoPackage.GetRosterGeneralIds(25),
            "shanhe-beigong-duo-2450-source25-portraits.png");
        RenderActualVerifiedOrdinaryTable(output, 2397,
            [StandardShanheMuluPackage.MuluId, StandardShanheMuluPackage.RageElephantId,
             StandardShanheMuluPackage.AngryElephantId, StandardShanheMuluPackage.RageElephantId,
             StandardShanheMuluPackage.AngryElephantId], "shanhe-mulu-source25-portraits.png", "source-city-5101");
        RenderRestoredSourcePortraitTables(output);
        RenderActualZhangYanStageSwitch(output);

        void CheckPortrait(ContentRegistry registry, string id, ISet<string> observed)
        {
            var general = registry.Generals[id];
            if (general.PortraitKey == "supporter" && GetVerifiedSourcePortraitPath(id) is null)
            { unverified.Add(id); return; }
            if (!observed.Add(id)) return;
            var expectedKey = general.PortraitKey == "dong_bai" ? "ol-dong-bai" : general.PortraitKey.Replace('_', '-');
            var expectedPath = GetVerifiedSourcePortraitPath(id) ?? GeneralArt.GetSkin(expectedKey)?.LocalPath;
            Program.Assert(expectedPath is not null && GeneralArt.GetSkin(id)?.LocalPath == expectedPath &&
                GeneralArt.GetPortrait(id, decodeWidth: 96) is ImageBrush
                    { ImageSource: BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } } brush && HasPaintedPixels(brush),
                "The actual published registry body must decode and paint its own verified source portrait: " + id);
        }
    }

    private static ShanheBattleSetup Setup(ShanheCampaignDefinition definition, ShanheNodeDefinition node, ShanheEncounterDefinition encounter) =>
        new(83, 1, node.Id, "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [], encounter.Enemies, encounter.Allies ?? [],
            ShanheCampaignContent.GetContentHash(definition))
        { DefaultGodFactionId = definition.DefaultGodFactionId, StartingResourcesExpireOnUse = definition.StartingResourcesExpireOnUse };

    private static bool HasPaintedPixels(Brush brush)
    {
        var drawing = new DrawingVisual();
        using (var context = drawing.RenderOpen()) context.DrawRectangle(brush, null, new Rect(0, 0, 32, 48));
        var bitmap = new RenderTargetBitmap(32, 48, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(drawing);
        return HasPaintedPixels(bitmap);
    }

    private static bool HasPaintedPixels(BitmapSource bitmap)
    {
        var pixels = new byte[bitmap.PixelWidth * bitmap.PixelHeight * 4];
        bitmap.CopyPixels(pixels, bitmap.PixelWidth * 4, 0);
        for (var i = 3; i < pixels.Length; i += 4) if (pixels[i] > 0) return true;
        return false;
    }

    private static void RenderActualMaSiblingsTable(string output)
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var node = definition.Nodes.Single(n => n.Id == "dagu");
        var encounter = node.Encounters!.Single(e => e.Id == "dagu:ma-siblings-s5");
        RenderActualEncounterTable(output, definition, node, encounter,
            [StandardShanheS5MaSiblingsPackage.MachaoId, StandardShanheS5MaSiblingsPackage.MayunluId],
            "shanhe-s5-ma-siblings-portraits.png",
            "The published Ma Chao/Ma Yunlu recipe must start its actual three-seat battle.",
            StandardShanheS5MaSiblingsPackage.MachaoId, "shanhe-s5-ma-chao-portrait.png");
    }

    private static void RenderActualCaiWenjiTable(string output)
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(3, StandardShanheCampaignCatalog.Returning2026EditionId);
        var node = definition.Nodes.Single(n => n.Id == "dagu");
        var encounter = node.Encounters!.Single(e => e.Id == "dagu:caiwenji-2469-local");
        Program.Assert(encounter.Enemies.Count == 1 && encounter.Enemies[0].GeneralId == StandardShanheSourceCaiWenjiPackage.NightmareId,
            "The published high-tier Cai Wenji recipe must use its actual cumulative Beige/Moshi/Tiandu source form.");
        RenderActualEncounterTable(output, definition, node, encounter,
            [StandardShanheSourceCaiWenjiPackage.NightmareId], "shanhe-source-caiwenji-2469-portraits.png",
            "The published Cai Wenji recipe must start its actual two-seat battle.");
    }

    private static void RenderActualLocalGarrisonTable(string output)
    {
        var definition = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(1);
        var pair = definition.Nodes.Where(node => node.Kind == ShanheNodeKind.EliteBattle && node.ChapterNumber == 3)
            .SelectMany(node => (node.Encounters ?? []).Where(encounter =>
                    encounter.Enemies.Any(enemy => enemy.GeneralId == StandardShanheLocalTroopPackage.MedicId) &&
                    encounter.Enemies.Any(enemy => enemy.GeneralId == StandardShanheLocalTroopPackage.RedBladeId) &&
                    encounter.Enemies.Any(enemy => enemy.GeneralId == StandardShanheLocalTroopPackage.CaptainId))
                .Select(encounter => (Node: node, Encounter: encounter))).First();
        RenderActualEncounterTable(output, definition, pair.Node, pair.Encounter,
            pair.Encounter.Enemies.Select(enemy => enemy.GeneralId).ToArray(), "shanhe-local-garrison-portraits.png",
            "The actual published elite garrison starts every local captain, guard, medic and red-blade seat with its newly supplied card art.");
    }

    private static void RenderActualVerifiedOrdinaryTable(string output, int battleId, IReadOnlyList<string> expectedIds,
        string imageFile, string? sourceNodeId = null)
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(25);
        var authored = sourceNodeId is not null ? source.Nodes.Single(node => node.Id == sourceNodeId)
            : source.Nodes.First(node => node.Encounters?.Any(item => item.SourceBattleId == battleId) == true);
        var encounter = authored.Encounters!.Single(item => item.SourceBattleId == battleId);
        if (battleId == 2397)
            Program.Assert(encounter.SourceVisibleEnemyCount == 1 && encounter.Enemies.Count == 5 && encounter.BossTransition is null,
                "The existing local five-body Mulu recipe retains its complete roster and the independent source-visible count; artwork adds no opening body or role profile.");
        var node = authored with
        {
            NextNodeIds = [], EliteUpgradeNodeId = null, Encounters = [encounter], ChapterRouteId = null,
            ChapterRole = ShanheChapterNodeRole.None, ChapterProgressStep = 0, RequiredChapterProgress = 0
        };
        var definition = source with { StartingNodeIds = [node.Id], RandomStartingNode = false, Nodes = [node] };
        var session = ShanheCampaignSession.Create(definition,
            new(83, "classic:guan-yu") { SourceDifficultyId = 25, UseCodexRewardPool = true }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var prepared = session.PreviewEncounter(node.Id);
        Program.Assert(prepared.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(expectedIds) && prepared.Enemies.Count == expectedIds.Count,
            "The actual source recipe retains its complete independently verified troop bodies in source order: " + battleId);
        Program.Assert(session.Submit(new MoveShanheNodeCommand(node.Id, session.Revision)).Accepted && session.PendingBattleSetup is not null,
            "The real source campaign entry constructs its native battle setup without a simulated victory or a seed search: " + battleId);
        var setup = session.PendingBattleSetup!;
        var preparedJson = System.Text.Json.JsonSerializer.Serialize(prepared.Enemies);
        Program.Assert(setup.SourceDifficultyId == 25 && System.Text.Json.JsonSerializer.Serialize(setup.EnemyRoster) == preparedJson,
            "The native table receives the exact frozen tier-25 grown preview roster, rather than the unprepared authored source template.");
        var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        RenderActualEncounterTable(output, definition, node, prepared, expectedIds.ToArray(),
            imageFile, "The real tier-25 ordinary recipe starts every actual source troop seat: " + battleId,
            setupOverride: setup, afterInitialRender: (game, model, _) =>
            {
                var enemies = model.Seats.Where(seat => !seat.IsHuman).OrderBy(seat => seat.Seat).ToArray();
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
                Program.Assert(game.PlayerCount == expectedIds.Count + 1 && model.Seats.Count == expectedIds.Count + 1 && enemies.Length == expectedIds.Count &&
                        enemies.Select(seat => seat.GeneralId).SequenceEqual(expectedIds) &&
                        System.Text.Json.JsonSerializer.Serialize(game.CreateCheckpoint().Options.ShanheBattle!.EnemyRoster) == preparedJson,
                    "Every actual source troop retains its distinct original picture, source order and complete grown native battle configuration: " + battleId);
                for (var index = 0; index < enemies.Length; index++)
                {
                    var member = prepared.Enemies[index];
                    var general = registry.Generals[member.GeneralId];
                    var maximum = general.BaseHp + member.MaxHpBonus;
                    Program.Assert(enemies[index].Hp == (member.Hp ?? general.InitialHp ?? maximum) &&
                            enemies[index].MaxHp == maximum && enemies[index].Armor == member.InitialArmor,
                        "The visible source troop seat keeps the exact current/max HP and initial armor from the frozen grown source recipe: " + member.GeneralId);
                }
                Program.Assert(ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal,
                    "Painting the native source troop seats leaves the campaign entry journal untouched.");
            });
    }

    private static void RenderRestoredSourcePortraitTables(string output)
    {
        var definition = StandardShanheCampaignCatalog.CreateDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        (string Node, string Encounter, string File, string[] Generals)[] recipes =
        [
            ("dagu", "dagu:yudu-s5", "shanhe-yudu-source-portrait-table.png", [StandardShanheS5YuduPackage.YuduId]),
            ("guangcheng", "guangcheng:zhangliang-s5", "shanhe-zhangliang-source-portrait-table.png", [StandardShanheS5ZhangliangPackage.ZhangliangId]),
            ("huanyuan", "huanyuan:xuncan-caoshi-s5", "shanhe-xuncan-caoshi-source-portrait-table.png",
                [StandardShanheS5XuncanCaoshiPackage.XuncanId, StandardShanheS5XuncanCaoshiPackage.CaoshiId]),
            ("dagu", "guangcheng:yuanshu-s5", "shanhe-yuanshu-source-portrait-table.png",
                [StandardShanheS5YuanshuPackage.YuanyaoId, StandardShanheS5YuanshuPackage.YuanshuId, StandardShanheS5YuanshuPackage.YuanyinId]),
            ("dagu", "dagu:gongsun-zan-yue-s5", "shanhe-gongsun-zan-yue-source-portrait-table.png",
                [StandardShanheS5GongsunZanPackage.GongsunZanId, StandardShanheS5GongsunZanPackage.GongsunYueId]),
            ("huanyuan", "huanyuan:xiliang-dark-s5", "shanhe-xiliang-dark-source-portrait-table.png",
                [StandardShanheS5XiliangDarkPackage.QinyiId, StandardShanheS5XiliangDarkPackage.ChenweiId, StandardShanheS5XiliangDarkPackage.LiheiId])
        ];
        var nodesById = definition.Nodes.ToDictionary(node => node.Id, StringComparer.Ordinal);
        foreach (var recipe in recipes)
        {
            if (!nodesById.TryGetValue(recipe.Node, out var node))
                throw new InvalidOperationException("The exact restored-art fixture node is missing: " + recipe.Node +
                    "; available nodes: " + string.Join(", ", nodesById.Keys));
            var encountersById = (node.Encounters ?? []).ToDictionary(encounter => encounter.Id, StringComparer.Ordinal);
            if (!encountersById.TryGetValue(recipe.Encounter, out var encounter))
                throw new InvalidOperationException("The exact restored-art fixture encounter is missing: node=" + recipe.Node +
                    ", encounter=" + recipe.Encounter + "; available encounters: " + string.Join(", ", encountersById.Keys) +
                    "; actual owners of that exact ID: " + string.Join(", ", definition.Nodes.Where(candidate =>
                        candidate.Encounters?.Any(item => item.Id == recipe.Encounter) == true).Select(candidate => candidate.Id)));
            Program.Assert(encounter.Enemies.Select(member => member.GeneralId).SequenceEqual(recipe.Generals),
                "The restored-art table fixture uses the actual complete published local recipe in its original order: " + recipe.Encounter);
            RenderActualEncounterTable(output, definition, node, encounter, recipe.Generals, recipe.File,
                "The actual published restored-art recipe starts every required original enemy seat: " + recipe.Encounter);
        }
    }

    private static void RenderActualZhangYanStageSwitch(string output)
    {
        RenderSourceStageSwitch(output, 6, 2653, "source-city-5068",
            StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId, StandardShanheSourceZhangYanPackage.ZhangYanMediumId,
            "shanhe-zhangniujiao-first-stage-portrait.png", "shanhe-zhangyan-second-stage-portrait.png");
        RenderSourceStageSwitch(output, 16, 2645, "source-city-5048", "shanhe:s5-lv-bu-2645",
            StandardShanheBossPackage.SourceShenguiNightmareId,
            "shanhe-shengui-first-stage-portrait.png", "shanhe-shengui-second-stage-portrait.png");
        RenderSourceStageSwitch(output, 16, 2687, "source-city-5148", StandardShanheS5GaichaoPackage.DongZhuoGaichaoId,
            StandardShanheSourceShenNuStagePackage.GeneralId,
            "shanhe-shennu-first-stage-portrait.png", "shanhe-shennu-second-stage-portrait.png");
    }

    private static void RenderSourceStageSwitch(string output, int difficulty, int battleId, string nodeId,
        string firstId, string secondId, string firstImage, string secondImage)
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
        var node = source.Nodes.Single(candidate => candidate.Id == nodeId);
        var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == battleId);
        Program.Assert(encounter.Enemies is [{ GeneralId: var first }] && first == firstId && encounter.BossTransition?.GeneralId == secondId,
            "The image-switch fixture begins with the actual first body and its genuine reserved source transition: " + battleId);
        var setup = new ShanheBattleSetup(71, 1, node.Id, "classic:guan-yu", 4, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 10)], [],
            [encounter.Enemies[0] with { Hp = 1, StartingHand = [], StartingCards = [], StartingEquipment = [],
                InitialHandBonus = 0, InitialArmor = 0, ArmorRecoveryPerRound = 0 }], [], ShanheCampaignContent.GetContentHash(source))
        { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition, DefaultGodFactionId = source.DefaultGodFactionId,
            StartingResourcesExpireOnUse = source.StartingResourcesExpireOnUse };
        RenderActualEncounterTable(output, source, node, encounter, [firstId], firstImage,
            "The real source battle starts its one-HP unarmored first body before the physical-card stage regression: " + battleId,
            setupOverride: setup, afterInitialRender: (game, model, surface) =>
            {
                var originalSeat = model.Seats.Single(seat => seat.GeneralId == firstId).Seat;
                var firstName = game.CreateSnapshot(0).Players.Single(player => player.Seat == originalSeat).GeneralName;
                Reach(() => game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0);
                var fixedSlash = game.Events.Select(entry => entry.Payload).OfType<ShanheStartingResourceCreatedEvent>()
                    .Single(entry => !entry.EquipmentResource && entry.RecipeIndex == 0).CardId;
                var action = game.GetHumanLegalActions().First(candidate => candidate.CardId == fixedSlash && candidate.TargetSeats.SequenceEqual(new[] { 1 }));
                Accept(new PlayCardCommand(0, fixedSlash, action.TargetSeats, game.Revision, game.CreateSnapshot(0).PendingDecision!.PromptId,
                    action.PlayedCardKind, action.TargetCardId));
                Reach(() => game.Events.Any(entry => entry.Payload is ShanheBossStageStartedEvent));
                var transition = game.Events.Select(entry => entry.Payload).OfType<ShanheBossStageStartedEvent>().Single();
                Program.Assert(originalSeat == 1 && transition.BossSeat == originalSeat && transition.PreviousGeneralId == firstId && transition.GeneralId == secondId &&
                    game.CreateSnapshot(0).Players.Single(player => player.Seat == originalSeat).GeneralId == secondId && game.PlayerCount == 2 &&
                    game.Events.Any(entry => entry.Payload is DamageAppliedEvent { SourceSeat: 0, TargetSeat: 1, Amount: 1 }),
                    "The real fixed physical Slash causes one native damage and the genuine second form replaces the same enemy seat without adding a follower.");
                ShiFifthBatchUiChecks.Refresh(model);
                surface.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.ContextIdle); surface.UpdateLayout();
                Program.Render(surface, 1440, 880, Path.Combine(output, secondImage));
                Program.Assert(ReferenceEquals(Program.Engine(model), game) && model.Seats.Single(seat => seat.Seat == originalSeat).GeneralId == secondId,
                    "The same live WPF model updates the same enemy seat to its exact second body after the actual source stage event: " + battleId);
                AssertActualSeatPortrait(surface, model, secondId);
                AssertStageHistory(game, model);
                if (battleId == 2687)
                {
                    var checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
                    var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), StandardShanheCampaignCatalog.CreateBattleRegistry(setup));
                    Program.Assert(JsonSerializer.Serialize(game.Log) == JsonSerializer.Serialize(restored.Log) &&
                        JsonSerializer.Serialize(game.Events) == JsonSerializer.Serialize(restored.Events),
                        "Cold command replay reconstructs the exact public stage boundary and original raw history.");
                    using var coldModel = new MainViewModel(false, 83, true, new MemorySaveStore(), useExpandedContent: true,
                        shanheSaveStore: new MemoryShanheCampaignSaveStore())
                    { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false, IsAutoAdvance = false };
                    typeof(MainViewModel).GetField("_shanheTableGame", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(coldModel, restored);
                    typeof(MainViewModel).GetField("_shanheTableSetup", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(coldModel, setup);
                    typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(coldModel, [restored]);
                    coldModel.IsNewGameSetupOpen = false;
                    AssertStageHistory(restored, coldModel);
                    var coldWindow = new MainWindow(coldModel); coldWindow.ApplyTemplate();
                    try
                    {
                        var coldSurface = (FrameworkElement)coldWindow.Content;
                        Program.Render(coldSurface, 1440, 880, Path.Combine(output, "shanhe-shennu-second-stage-cold-history.png"));
                        AssertActualSeatPortrait(coldSurface, coldModel, secondId);
                        Program.Assert(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint,
                            "Rebuilding and rendering the historical names and actual portrait does not change the restored battle checkpoint.");
                    }
                    finally { coldWindow.Content = null; coldWindow.DataContext = null; coldWindow.Close(); }
                }

                void AssertStageHistory(GameEngine engine, MainViewModel view)
                {
                    var rawLogs = JsonSerializer.Serialize(engine.Log);
                    var rawEvents = JsonSerializer.Serialize(engine.Events);
                    var checkpoint = GameCheckpointJson.Serialize(engine.CreateCheckpoint());
                    var stage = engine.Events.Select(entry => entry.Payload).OfType<ShanheBossStageStartedEvent>().Single();
                    var cut = stage.LastPreviousFormLogSequence;
                    Program.Assert(cut is >= 0, "A real source transformation carries its pre-replacement native log boundary.");
                    var attack = engine.Log.Last(entry => entry.Type == "CardUsed" && entry.ActorSeat == 0 && entry.TargetSeat == originalSeat && entry.Sequence <= cut);
                    var hit = engine.Log.Last(entry => entry.Type == "Damage" && entry.ActorSeat == 0 && entry.TargetSeat == originalSeat && entry.Sequence <= cut);
                    var draw = engine.Log.First(entry => entry.Type == "CardsDrawn" && entry.ActorSeat == originalSeat && entry.Sequence > cut);
                    var secondName = engine.CreateSnapshot(0).Players.Single(player => player.Seat == originalSeat).GeneralName;
                    foreach (var entry in new[] { attack, hit, draw })
                    {
                        var expected = entry == draw ? secondName : firstName;
                        var other = entry == draw ? firstName : secondName;
                        var projected = view.FilteredBattleLog.Single(row => row.Sequence == entry.Sequence);
                        Program.Assert(firstName != secondName && projected.Message.Contains(expected, StringComparison.Ordinal) &&
                            projected.ParticipantNames.Contains(expected) &&
                            !projected.ParticipantNames.Contains(other) && view.GameLog.Any(row => row.EndsWith(projected.Message, StringComparison.Ordinal)),
                            "The actual same-seat stage preserves the old target and lethal hit name, then projects the new body's draw in both battle-log views: " + battleId + "/" + entry.Type);
                    }
                    ShiFifthBatchUiChecks.Refresh(view);
                    Program.Assert(rawLogs == JsonSerializer.Serialize(engine.Log) && rawEvents == JsonSerializer.Serialize(engine.Events) &&
                        checkpoint == GameCheckpointJson.Serialize(engine.CreateCheckpoint()),
                        "Historical-name projection and refresh leave original public logs, facts and command checkpoint unchanged.");
                }

                void Accept(GameCommand command)
                { var result = game.Submit(command); Program.Assert(result.Accepted, result.Error?.Message ?? "The actual stage portrait command was rejected."); }
                void Reach(Func<bool> done)
                {
                    for (var step = 0; step < 120 && !done(); step++)
                    {
                        Program.Assert(game.State.Winner == Winner.None, "The minimal portrait fixture must reach its stage boundary before settlement.");
                        var prompt = game.CreateSnapshot(0).PendingDecision;
                        if (prompt is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge or DecisionKind.RescueDying })
                        {
                            var pass = prompt.Choices.First(choice => choice.Cards.Count == 0);
                            Accept(new AnswerPromptCommand(0, prompt.PromptId, pass.Id, game.Revision));
                        }
                        else Accept(new AdvanceOneStepCommand(game.Revision));
                    }
                    Program.Assert(done(), "The bounded actual-card fixture must stop at the first real source stage event: " + battleId);
                }
            });
    }

    private static void RenderActualEncounterTable(string output, ShanheCampaignDefinition definition,
        ShanheNodeDefinition node, ShanheEncounterDefinition encounter, IReadOnlyList<string> generalIds,
        string renderFile, string startMessage, string? cropGeneralId = null, string? cropFile = null,
        ShanheBattleSetup? setupOverride = null, Action<GameEngine, MainViewModel, FrameworkElement>? afterInitialRender = null)
    {
        var setup = setupOverride ?? Setup(definition, node, encounter);
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        Program.Assert(game.Submit(new StartGameCommand()).Accepted, startMessage);
        using var model = new MainViewModel(false, 83, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore())
        { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false, IsAutoAdvance = false };
        typeof(MainViewModel).GetField("_shanheTableGame", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, game);
        typeof(MainViewModel).GetField("_shanheTableSetup", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, setup);
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, [game]);
        model.IsNewGameSetupOpen = false;
        var window = new MainWindow(model); window.ApplyTemplate();
        try
        {
            var surface = (FrameworkElement)window.Content;
            Program.Render(surface, 1440, 880, Path.Combine(output, renderFile));
            foreach (var id in generalIds.Distinct(StringComparer.Ordinal))
            {
                var seats = model.Seats.Where(seat => seat.GeneralId == id).OrderBy(seat => seat.Seat).ToArray();
                Program.Assert(seats.Length == generalIds.Count(generalId => generalId == id),
                    "Every requested native body, including repeated source guards, occupies its own actual seat: " + id);
                foreach (var seat in seats)
                {
                    var crop = AssertActualSeatPortrait(surface, model, id, seat.Seat);
                    if (id == cropGeneralId)
                    {
                        var encoder = new PngBitmapEncoder(); encoder.Frames.Add(BitmapFrame.Create(crop));
                        using var file = File.Create(Path.Combine(output, cropFile!)); encoder.Save(file);
                    }
                }
            }
            afterInitialRender?.Invoke(game, model, surface);
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }
    }

    private static RenderTargetBitmap AssertActualSeatPortrait(FrameworkElement surface, MainViewModel model, string id, int? actualSeat = null)
    {
        var seat = model.Seats.Single(s => s.GeneralId == id && (actualSeat is null || s.Seat == actualSeat));
        var portrait = Program.Find<Rectangle>(surface).Single(r => r.Name == "SinglePortrait" && ReferenceEquals(r.DataContext, seat));
        Program.Assert(seat.HasPortrait && seat.Portrait.Image is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
            portrait is { ActualWidth: > 0, ActualHeight: > 0, Fill: ImageBrush { ImageSource: not null } } && portrait.Visibility == Visibility.Visible,
            "The actual public enemy seat template must bind its source form's decoded portrait rather than transparent fallback: " + id);
        if (GetVerifiedSourcePortraitPath(id) is { } sourcePath)
        {
            var actual = (BitmapSource)seat.Portrait.Image!;
            var expected = CardGame.Wpf.Resources.AssetImages.Load(sourcePath, decodeWidth: actual.PixelWidth);
            Program.Assert(expected is not null && actual.PixelWidth == expected.PixelWidth && actual.PixelHeight == expected.PixelHeight &&
                GeneralArt.GetSkin(id)?.LocalPath == sourcePath,
                "The actual enemy seat decodes its exact verified original source card at the displayed image's resolution: " + id);
            var stride = actual.PixelWidth * 4;
            var actualPixels = new byte[stride * actual.PixelHeight]; var expectedPixels = new byte[actualPixels.Length];
            new FormatConvertedBitmap(actual, PixelFormats.Bgra32, null, 0).CopyPixels(actualPixels, stride, 0);
            new FormatConvertedBitmap(expected!, PixelFormats.Bgra32, null, 0).CopyPixels(expectedPixels, stride, 0);
            Program.Assert(actualPixels.AsSpan().SequenceEqual(expectedPixels),
                "The actual SinglePortrait enemy seat retains the verified source card's complete original pixels: " + id);
            var paintedImage = (portrait.Fill as ImageBrush)?.ImageSource as BitmapSource;
            Program.Assert(paintedImage is not null && paintedImage.PixelWidth == expected!.PixelWidth && paintedImage.PixelHeight == expected.PixelHeight,
                "The rendered enemy Rectangle binds the same complete source image as its live seat view model: " + id);
            var paintedPixels = new byte[expectedPixels.Length];
            new FormatConvertedBitmap(paintedImage!, PixelFormats.Bgra32, null, 0).CopyPixels(paintedPixels, stride, 0);
            Program.Assert(paintedPixels.AsSpan().SequenceEqual(expectedPixels),
                "The visible enemy Rectangle paints the exact current-stage source image, including after a same-seat transformation: " + id);
        }
        var crop = new RenderTargetBitmap((int)Math.Ceiling(portrait.ActualWidth), (int)Math.Ceiling(portrait.ActualHeight), 96, 96, PixelFormats.Pbgra32);
        crop.Render(portrait);
        Program.Assert(HasPaintedPixels(crop), "The real laid-out enemy portrait must contain painted pixels: " + id);
        return crop;
    }
}
