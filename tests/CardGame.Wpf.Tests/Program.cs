using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Core;
using CardGame.Content.Standard;
using CardGame.Wpf;
using CardGame.Wpf.Controls;
using CardGame.Wpf.ViewModels;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.Audio;
using CardGame.Wpf.Presentation;

internal static class Program
{
    private static int _passed;
    private static int _failed;
    private static string[] _nameFilters = [];
    private static readonly HashSet<string> MatchedFilters = new(StringComparer.OrdinalIgnoreCase);
    private static bool _verbose;
    internal static bool RecordMotion { get; private set; }
    private static string? _startAfterName;
    private static bool _startAfterReached;
    private static readonly BindingListener BindingErrors = new();

    [STAThread]
    private static int Main(string[] args)
    {
        Console.OutputEncoding = new System.Text.UTF8Encoding(false);
        if (args.Contains("--filter", StringComparer.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("Use --filter=<name>; an incomplete filter must not start the full suite.");
            return 2;
        }

        if (args.Any(argument => argument.StartsWith("--", StringComparison.Ordinal) &&
            argument != "--record-motion" && argument != "--verify-native-audio" && argument != "--verbose" &&
            !argument.StartsWith("--filter=", StringComparison.OrdinalIgnoreCase) &&
            !argument.StartsWith("--start-after=", StringComparison.OrdinalIgnoreCase)))
        {
            Console.Error.WriteLine("Unknown WPF check option. Use --filter=<name>.");
            return 2;
        }
        var app = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
        app.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/CardGame.Wpf;component/Themes/TableTheme.xaml")
        });
        PresentationTraceSources.DataBindingSource.Listeners.Add(BindingErrors);
        PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
        try
        {
            _verbose = args.Contains("--verbose", StringComparer.OrdinalIgnoreCase);
            RecordMotion = args.Contains("--record-motion", StringComparer.OrdinalIgnoreCase);
            _nameFilters = args.Where(argument => argument.StartsWith("--filter=", StringComparison.OrdinalIgnoreCase))
                .Select(argument => argument["--filter=".Length..].Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            if (_nameFilters.Any(value => value.Length == 0))
            {
                Console.Error.WriteLine("A WPF check filter cannot be empty.");
                return 2;
            }
            _startAfterName = args.FirstOrDefault(argument =>
                    argument.StartsWith("--start-after=", StringComparison.OrdinalIgnoreCase))?
                ["--start-after=".Length..].Trim();
            _startAfterReached = _startAfterName is null;
            var output = args.FirstOrDefault(argument =>
                !argument.StartsWith("--", StringComparison.Ordinal)) ??
                Path.Combine(Path.GetTempPath(), "card-ui-check");
            Directory.CreateDirectory(output);
            Check("local assets: external artwork deployment decoding and recovery", () => LocalAssetChecks.DeploymentDecodingAndRecovery(output));
            Check("LAN room UI: seats readiness private prompts and stable selection", () => LanRoomUiChecks.SeatsPrivacyAndSelection(output));
            Check("Mode rooms UI: selected modes seats AI readiness and quick start", () => LanRoomUiChecks.SelectedModesEnterRooms(output));
            Check("active effect badges restore actual duration and clear consumed facts", () => ActiveEffectUiChecks.LiveEffectsRestoreAndExpire(output));
            Check("Equipment slots UI: abolished slots remain visible and restore", () => EquipmentSlotUiChecks.AbolishedSlotsRemainVisibleAndRestore(output));
            Check("table shortcuts execute once per key press and preserve modal drafts", TableShortcutChecks.SinglePressActions);
            Check("table modal focus follows startup and current visible panels", () => ModalFocusChecks.StartupAndModalTransitions(output));
            Check("table overlays cover elevated badges and restore pointer input", () => TableOverlayNavigationChecks.ModalsCoverElevatedBadges(output));
            Check("manual discard UI: shared action row partial confirmation and automatic completion", () => DiscardUiChecks.PartialPaymentsShareActionRowAndFinishAutomatically(output));
            Check("Card distribution UI: Tuoyu batch regions confirm cancel and cold restore", () => CardDistributionUiChecks.TuoyuMultiSelectRegionsConfirmAndRestore(output));
            Check("Card distribution UI: public pile faces and interrupted gain child", () => CardDistributionUiChecks.PublicPileCardsRemainVisibleAndStopAtGainChild(output));
            Check("Card distribution UI: consecutive bound gifts respect native budget", () => CardDistributionUiChecks.ConsecutiveBoundGiftsShareBatchAndRespectLimit(output));
            Check("stale prompt choices cannot answer reloaded or consecutive decisions", () => StalePromptChoiceChecks.ReloadedChoices(output));
            Check("skill quantity activation and effect choices share the lower action dock", () => DockSkillChoiceUiChecks.QuantityActivationAndEffectsUseSameDock(output));
            Check("Selection area: Gongxin Zhiheng repeatable skills and Slash native drafts", () => SelectionAreaChecks.NativeDraftsAndUsageLimits(output));
            Check("Zhiheng UI: classic and boundary select all hand remain editable until confirmation", () => MultiCardDraftUiChecks.ZhihengSelectsAllHandBeforeConfirmation(output));
            Check("Shi Zhao Yun UI: cardless rail draft cancel target and round limit", () => ShiZhaoYunUiChecks.CardlessSkillRailDraftCancelTargetsAndRoundLimit(output));
            Check("Shi batch UI: public progress and committed preparation choices", () => ShiBatchUiChecks.PublicProgressAndCommittedPreparationChoices(output));
            Check("Shi second batch UI: skill progress native preparation and gift drafts", () => ShiSecondBatchUiChecks.SkillProgressAndNativePreparationAndGiftDrafts(output));
            Check("Shi third batch UI: gallery progress original cards and multi-card cost", () => ShiThirdBatchUiChecks.GalleryProgressOriginalCardsAndMultiCardCost(output));
            Check("Shi fourth batch UI: gallery progress and shared active entry", () => ShiFourthBatchUiChecks.GalleryProgressAndSharedActiveEntry(output));
            Check("Shi fifth batch UI: gallery progress and real shared hand use response", () => ShiFifthBatchUiChecks.GalleryProgressAndForeignHandUseResponse(output));
            Check("Shi fifth batch UI: Dimeng two cards remain unpaid until confirmation", () => ShiLuSuDiscardUiChecks.DimengTwoHandCardsRemainUnpaidUntilSingleConfirmation(output));
            Check("Shi sixth batch UI: gallery portraits skill counts and live progress", () => ShiSixthBatchUiChecks.GalleryPortraitSkillCountsAndProgress(output));
            Check("Shi sixth batch UI: Nigu different suits remain unpaid until confirmation", () => ShiSixthBatchUiChecks.NiguTwoDifferentSuitsRemainUnpaidUntilConfirmation(output));
            Check("Shi sixth batch UI: Xiongtu four cards remain unpaid until confirmation", () => ShiSunJunDiscardUiChecks.XiongtuFourHandCardsRemainUnpaidUntilSingleConfirmation(output));
            Check("Shi seventh batch UI: gallery and live progress", () => ShiSixthBatchUiChecks.GalleryPortraitSkillCountsAndProgress(output, ["mobile:shi-xiao-qiao", "mobile:shi-zhang-yan", "mobile:shi-deng-ai"]));
            Check("Dinghan UI: many trick names wrap scroll and commit add remove skip", () => DockSkillChoiceUiChecks.DinghanNamesWrapAndCommit(output));
            Check("Qizheng UI: native designation response and gain buttons survive restore", () => QizhengUiChecks.NativeChoicesRemainReachable(output));
            Check("skill rail counts preserve order and align the unpaired last skill beside the portrait", () => SkillRailLayoutChecks.CountsKeepOrderAndAlignLastSkill(output));
            Check("recorded general selection saves load without replacing their source", RecordedSelectionSaveChecks.LoadHistoricalFingerprintWithoutReplacingSource);
            Check("custom general roster opens searches and previews with bounded presentation cost", () => GeneralRosterPerformanceChecks.OpenSearchAndPreview(output));
            Check("custom general roster categories search within eligible series and reset across modes", () => GeneralRosterCategoryChecks.SeriesSearchAndModeBoundaries(output));
            Check("god rules deduplication merges skins and labels gameplay differences", () => GeneralRosterCategoryChecks.GodRulesMergeSkinsAndDistinguishVersions(output));
            Check("custom general editor: visual rules save reopen and visible errors", () => CustomGeneralEditorChecks.VisualRulesSaveReopenAndErrorsKeepDraft(output));
            Check("custom general sessions: editor next-game isolation and strict saved snapshot", CustomGeneralSessionChecks.EditorChangesAffectNextGameWhileSavedCustomMatchRestoresExactly);
            Check("custom general persistence: atomic library and validation", () => CustomGeneralPersistenceChecks.LibraryRoundTripsAndFailedWritesPreserveOriginal(output));
            Check("custom general persistence: saved content snapshot and actual cold restore", () => CustomGeneralPersistenceChecks.SaveSnapshotRestoresAfterCurrentLibraryChanges(output));
            Check("ordinary rules deduplication removes duplicate entries and preserves skins", () => GeneralRosterCategoryChecks.OrdinaryRulesHaveSingleEntriesAndRetainSkins(output));
            Check("Bagua response UI: armor choice judgment then hand Dodge", () => BaguaResponseUiChecks.ArmorChoicePrecedesJudgmentAndHandDodge(output));
            Check("filtered presentation batches log and guide results without losing entries", FilteredPresentationChecks.BatchFiltersPreserveEntries);
            Check("Battle log UI: compact seat filters named colored history and styled scrolling", () => BattleLogUiChecks.SeatsFilterNamedHistoryAndKeepStyledScrolling(output));
            Check("general gallery realizes visible rows and opens with bounded portrait cost", () => GeneralGalleryPerformanceChecks.VisibleRowsAndPortraitCost(output));
            Check("Wumiao Flood UI: ordered portraits undo aim and native commit", () => WumiaoFloodUiChecks.OrderedPortraitTargetsUndoAimAndCommit(output));
            Check("Wu Guan Yu Wuyou UI: foreign contribution issuer and private return", () => WuGuanYuWuyouUiChecks.ForeignContributionUsesIssuerAndKeepsPrivateReturn(output));
            Check("Mo Diao Chan UI: forced hand selection waits for confirmation", () => MoDiaoChanUiChecks.LockedHandStillRequiresConfirmation(output));
            Check("Mo Cao Cao UI: Bachao restored portraits confirmation and decline", () => MoCaoCaoUiChecks.BachaoTargetsAndDeclineRemainUsable(output));
            Check("Mobile boundary Yi Ji UI: private bottom and actual recipient button", () => BoundaryYiJiMobileUiChecks.JijiePrivateBottomAndRecipientButton(output));
            Check("Mobile boundary Cao Xiu UI: actual target hand batch and damage choice", () => BoundaryCaoXiuMobileUiChecks.ActualTargetHandBatchAndDamageChoice(output));
            Check("Mobile boundary Quan Cong UI: charge rail choices and damage menu cold", () => BoundaryQuanCongUiChecks.ChargedYaomingRailOptionsAndDamageMenuCold(output));
            Check("Mobile boundary gallery: registered portraits and actual details", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:yi-ji-mobile", "boundary:cao-xiu-mobile", "boundary:quan-cong-mobile" }));
            Check("Mobile boundary batch 44 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:man-chong-mobile", "boundary:wu-yi-mobile", "boundary:zhu-zhi-mobile" }));
            Check("Mobile boundary Zhu Zhi UI: initial Anguo target marker and progress", () => BoundaryZhuZhiUiChecks.InitialAnguoTargetMarkerAndProgress(output));
            Check("Mobile boundary batch 45 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:zhou-cang-mobile", "boundary:cao-zhen-mobile", "boundary:sun-xiu-mobile" }));
            Check("Mobile boundary Cao Zhen UI: secret targets and owner-only forecast progress", () => BoundaryCaoZhenUiChecks.SecretForecastActualTargetsAndOwnerOnlyProgress(output));
            Check("Mobile boundary batch 46 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:chen-qun-mobile", "boundary:gu-yong-mobile", "boundary:zhu-huan-mobile" }));
            Check("Mobile boundary Gu Yong UI: mixed-color reveal and native self recipient draft", () => BoundaryGuYongUiChecks.MixedColorSameTypeRevealAndNativeSelfRecipientDraft(output));
            Check("Mobile boundary batch 48 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:zhang-ni-mobile", "boundary:yan-yan-mobile", "boundary:wang-ji-mobile" }));
            Check("Mobile boundary batch 49 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:cao-pi-mobile", "boundary:liu-shan-mobile", "boundary:zhou-fei-mobile" }));
            Check("Mobile boundary batch 50 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:yu-ji-mobile", "boundary:zuo-ci-mobile", "boundary:zhong-hui-mobile" }));
            Check("Boundary batch 51 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:yuan-shu-mobile", "boundary:yu-fan-mobile", "boundary:guan-ping" }));
            Check("Mobile boundary batch 52 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:cao-zhang-mobile", "boundary:liao-hua-mobile", "boundary:li-ru-mobile" }));
            Check("Mobile boundary batch 53 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:guan-xing-zhang-bao-mobile", "boundary:bu-lian-shi-mobile", "boundary:liu-biao-mobile" }));
            Check("Mobile boundary batch 54 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:cai-wen-ji-mobile", "boundary:cheng-pu-mobile", "boundary:fu-huang-hou-mobile" }));
            Check("Mobile boundary batch 55 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:guo-huai-mobile", "boundary:jian-yong-mobile", "boundary:cai-fu-ren-mobile" }));
            Check("Mobile boundary batch 56 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:cao-zhi-mobile", "boundary:ling-tong-mobile", "boundary:han-dang-mobile" }));
            Check("Mobile boundary batch 57 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:xu-sheng-mobile", "boundary:wu-guo-tai-mobile", "boundary:ma-dai-mobile" }));
            Check("Mobile boundary batch 58 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:gao-shun-mobile", "boundary:wang-yi-mobile", "boundary:fa-zheng-mobile" }));
            Check("Mobile boundary batch 59 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:yu-jin-mobile", "boundary:gongsun-zan-mobile", "boundary:ju-shou-mobile" }));
            Check("Mobile boundary batch 60 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:hua-xiong-mobile", "boundary:zhou-tai-mobile", "boundary:huang-zhong-mobile", "boundary:wei-yan-mobile" }));
            Check("Mobile boundary batch 61 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:lu-bu-mobile", "boundary:zhuge-liang-mobile", "boundary:zhen-ji-mobile", "boundary:zhang-fei-mobile", "boundary:cao-cao-mobile", "boundary:sima-yi-mobile" }));
            Check("Mobile boundary batch 62 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:huang-gai-mobile", "boundary:gan-ning-mobile", "boundary:diao-chan-mobile", "boundary:sun-quan-mobile", "boundary:huang-yueying-mobile", "boundary:guo-jia-mobile" }));
            Check("Mobile boundary batch 63 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:da-qiao-mobile", "boundary:lu-xun-mobile", "boundary:sun-shangxiang-mobile", "boundary:zhang-liao-mobile", "boundary:xu-chu-mobile", "boundary:xiahou-dun-mobile", "boundary:xiahou-yuan-mobile", "boundary:cao-ren-mobile" }));
            Check("Mobile boundary batch 64 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:liu-bei-mobile", "boundary:guan-yu-mobile", "boundary:zhao-yun-mobile", "boundary:ma-chao-mobile", "boundary:lu-meng-mobile", "boundary:zhou-yu-mobile", "boundary:hua-tuo-mobile", "boundary:zhang-jiao-mobile" }));
            Check("Mobile boundary batch 65 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:sun-jian-mobile", "boundary:xun-yu-mobile", "boundary:dian-wei-mobile", "boundary:pang-tong-mobile", "boundary:pang-de-mobile", "boundary:xu-huang-mobile", "boundary:yan-liang-wen-chou-mobile" }));
            Check("Mobile boundary batch 66 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "boundary:wolong-zhuge-liang-mobile", "boundary:dong-zhuo-mobile", "boundary:yuan-shao-mobile", "boundary:meng-huo-mobile", "boundary:zhu-rong-mobile", "boundary:sun-ce-mobile", "boundary:deng-ai-mobile", "boundary:jiang-wei-mobile", "boundary:zhang-zhao-zhang-hong-mobile", "boundary:xiao-qiao-mobile" }));
            Check("Mobile standard batch 67 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "mobile:li-dian", "mobile:taishi-ci", "mobile:cao-chong", "mobile:chen-gong", "mobile:liu-feng", "mobile:lu-su", "mobile:xiahou-shi", "mobile:sp-xu-shu", "mobile:jia-xu", "mobile:zhang-song" }));
            Check("Mobile Yuan Shao UI: effective-suit pair confirms native Arrow", MobileYuanShaoUiChecks.EffectiveSuitPairEnablesNativeConfirmation);
            Check("Mobile standard batch 86 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:cao-pi", "mobile:yuan-shao", "mobile:meng-huo", "mobile:zhu-rong" }));
            Check("Mobile standard batch 87 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:xu-huang", "mobile:dong-zhuo", "mobile:sun-ce", "mobile:zhong-hui", "mobile:pang-tong", "mobile:xun-yu", "mobile:liu-shan" }));
            Check("Mobile standard batch 85 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:guo-jia", "mobile:zhou-yu", "mobile:huang-zhong", "mobile:pang-de", "mobile:sun-jian" }));
            Check("Mobile standard batch 84 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:da-qiao", "mobile:sun-shangxiang", "mobile:diao-chan", "mobile:xiahou-dun", "mobile:liu-bei", "mobile:zhuge-liang" }));
            Check("Mobile standard batch 83 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:lu-meng", "mobile:zhang-liao", "mobile:xu-chu", "mobile:cao-ren" }));
            Check("Mobile standard batch 82 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:zhen-ji", "mobile:sun-quan", "mobile:sima-yi", "mobile:hua-tuo" }));
            Check("Mobile standard batch 81 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:cao-ying", "mobile:ma-chao", "mobile:bei-mi-hu", "mobile:huang-yueying", "mobile:lu-xun", "mobile:cao-cao", "mobile:gan-ning" }));
            Check("Mobile standard batch 80 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:dian-wei", "mobile:deng-ai", "mobile:wei-yan", "mobile:guan-yu", "mobile:zhang-fei", "mobile:lu-bu", "mobile:zhao-yun", "mobile:huang-gai" }));
            Check("Mobile standard batch 79 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:zhang-xing-cai", "mobile:zhou-fang", "mobile:cai-wen-ji", "mobile:xiahou-yuan", "mobile:jiang-wei", "mobile:zhuge-zhan" }));
            Check("Mobile standard batch 78 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:guo-huanghou", "mobile:zhuge-guo", "mobile:yan-yan", "mobile:zhang-xiu", "mobile:wang-ping" }));
            Check("Mobile standard batch 77 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:xu-shi", "mobile:lu-zhi", "mobile:wang-can", "mobile:zhang-rang" }));
            Check("Mobile standard batch 76 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:xue-zong", "mobile:ji-kang", "mobile:wu-xian", "mobile:cai-yong", "mobile:liu-qi" }));
            Check("Mobile standard batch 75 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:sun-hao", "mobile:lu-ji", "mobile:xu-you", "mobile:dong-yun" }));
            Check("Mobile standard batch 74 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:yue-jin", "mobile:fu-wan", "mobile:xiahou-ba", "mobile:qin-mi", "mobile:gongsun-yuan", "mobile:sha-mo-ke", "mobile:hao-zhao" }));
            Check("Mobile standard batch 73 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:wang-ji", "mobile:mi-zhu", "mobile:sima-lang", "mobile:ma-zhong", "mobile:shi-xie", "mobile:ta-dun", "mobile:dong-bai", "mobile:qu-yi", "mobile:xi-zhi-cai", "mobile:liu-yu", "mobile:cao-jie" }));
            Check("Mobile standard batch 72 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:cao-xiu", "mobile:zhong-yao", "mobile:liu-chen", "mobile:li-yan", "mobile:huang-hao", "mobile:sun-xiu", "mobile:cen-hun", "mobile:sun-zi-liu-fang", "mobile:zhang-lu" }));
            Check("Mobile standard batch 71 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:yu-jin", "mobile:cao-hong", "mobile:he-tai-hou", "mobile:wen-pin", "mobile:zhuge-dan", "mobile:cao-zhi", "mobile:ling-tong", "mobile:wu-guo-tai", "mobile:cao-rui", "mobile:chen-qun", "mobile:gu-yong", "mobile:bu-zhi", "mobile:li-tong" }));
            Check("Mobile standard batch 70 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:guo-huai", "mobile:guan-xing-zhang-bao", "mobile:li-ru", "mobile:zu-mao", "mobile:zhang-bao", "mobile:ding-feng", "mobile:sun-qian", "mobile:kan-ze", "mobile:sun-deng", "mobile:guo-tu-feng-ji", "mobile:zhu-zhi", "mobile:quan-cong" }));
            Check("Mobile standard batch 69 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "mobile:guan-ping", "mobile:yu-fan", "mobile:liu-biao", "mobile:man-chong", "mobile:jian-yong", "mobile:pan-zhang-ma-zhong", "mobile:fu-huang-hou", "mobile:cao-zhen", "mobile:wu-yi", "mobile:zhu-ran", "mobile:sun-lu-ban", "mobile:zhuge-ke", "mobile:ling-cao" }));
            Check("Mobile standard batch 68 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                  new HashSet<string>(StringComparer.Ordinal) { "mobile:cao-ang", "mobile:chen-lin", "mobile:zhuge-jin", "mobile:bu-lian-shi", "mobile:zhu-huan", "mobile:han-dang", "mobile:cao-zhang", "mobile:liao-hua", "mobile:ma-dai", "mobile:wang-yi", "mobile:cheng-pu", "mobile:xun-you" }));
            Check("Mobile boundary Cai Fu Ren UI: private two-card view actual paid choice cold", () => BoundaryCaiFuRenMobileUiChecks.ActualPrivateTwoCardViewAndPaidChoiceCold(output));
            Check("Mobile boundary Zuo Ci UI: private exchange actual Back selected draft cold", () => BoundaryZuoCiUiChecks.ActualPrivateExchangeBackAndSelectedDraftCold(output));
            Check("Mobile boundary Zhou Fei UI: opaque storage original ending use and remainder cold", () => BoundaryZhouFeiUiChecks.ForeignOpaqueStoreOriginalEndingUseAndRemainderCold(output));
            Check("Mobile boundary Yan Yan UI: structured state counter and persistent quota cold", () => BoundaryYanYanUiChecks.StructuredPolarityOccurrenceAndPersistentQuotaRailCold(output));
            Check("Mobile boundary Wang Ji UI: actual inspected foreign hand and card button cold", () => BoundaryWangJiUiChecks.ActualInspectedForeignHandAndPaidCardButtonCold(output));
            Check("Mobile boundary batch 47 gallery: official portraits and printed rules", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output,
                new HashSet<string>(StringComparer.Ordinal) { "boundary:zhu-ran-mobile", "boundary:pan-zhang-ma-zhong-mobile", "boundary:sun-lu-ban-mobile" }));
            Check("Mobile boundary Sun Lu Ban UI: owner branch and actual foreign hidden hand", () => BoundarySunLuBanUiChecks.OwnerBranchAndActualForeignHiddenHandPicker(output));
            Check("Edition general gallery: namespaced portraits and real details", () => EditionGeneralGalleryChecks.NamespacedPortraitsAndDetails(output));
            Check("God editions UI: formal portraits complete details and actual edition labels", () => MobileGodGalleryChecks.GodSeriesPortraitsAndDetails(output));
            Check("Shouli field mount UI: published foreign conversion and native payment", () => ShouliFieldMountUiChecks.ForeignMountUsesPublishedConversion(output));
            Check("Benevolence public pile UI: shared faces and read-only inspection", () => BenevolencePileUiChecks.SharedPublicCardsOpenReadOnly(output));
            Check("Table order UI: public moved positions change panels and seat numbers", () => TableOrderUiChecks.MovedPositionsUpdatePanelsAndNumbers(output));
            Check("Le Diao Chan UI: original marks and whole Hand exchange", () => LeDiaoChanUiChecks.OriginalMarksAndWholeHandSwapUseNativeChoices(output));
            Check("Le Da Qiao Qin UI: original judgment subslots and native choices", () => LeDaQiaoJudgmentUiChecks.QinSubslotsKeepOriginalCardsAndNativeChoices(output));
            Check("Native public pile UI: original grain play rail and response faces", NativePublicPileUiChecks.OriginalGrainPlayRailAndResponseFaces);
            Check("Mou Cao Pi UI: portrait skills and timed Fangzhu status", () => MouCaoPiUiChecks.PortraitSkillsAndFangzhuState(output));
            Check("Identity concealment UI: ActualProjectedConcealmentAndAppearanceBridge", () => IdentityConcealmentViewChecks.ActualProjectedConcealmentAndAppearanceBridge(output));
            Check("Named private pile UI: owner pairs public counts and clear", () => NamedPrivatePileUiChecks.OwnerPairsPublicCountsAndClearingUseSharedReadOnlyFaces(output));
            Check("overlapping hand cards keep stable pointer targets", () => HandPointerChecks.OverlapAndPointer(output));
            Check("hand motion preserves surviving positions across equal-count replacements", () => HandReflowChecks.EqualCountReplacement(output));
            Check("general selection input respects overlays and consecutive national prompts", () => GeneralSelectionChecks.InputBoundaries(output));
            Check("Opening customization UI: identities roster and hand confirmation", () => OpeningCustomizationUiChecks.RolesRosterAndHandConfirmation(output));
            Check("general selection switches related editions without consuming candidate slots", () => GeneralSelectionChecks.RelatedVariants(output));
            Check("mode lobby filters real identity, team and national entries without changing the match", () => CheckModeLobby(output));
            Check("Wushuang UI: entry shop and independent cold restore", () => WushuangUiChecks.EntryShopAndIndependentColdRestore(output));
            Check("Wushuang UI: tactic counts wrap without clipping", () => WushuangUiChecks.TacticCountsWrapWithoutClipping(output));
            Check("Wushuang UI: Mo Lu Bu opening and normal initiative are visible", () => WushuangUiChecks.MoLuBuOpeningAndNormalInitiativeAreVisible(output));
            Check("Wushuang UI: mixed shop equipment shows its native purchase", () => WushuangUiChecks.MixedShopEquipmentShowsItsNativePurchase(output));
            Check("Wushuang UI: full skill slots require visible replacement", () => WushuangUiChecks.FullSkillSlotsRequireVisibleReplacement(output));
            Check("Wushuang UI: free refresh and expanded slots follow native snapshot", () => WushuangUiChecks.FreeRefreshAndExpandedSlotsFollowNativeSnapshot(output));
            Check("Wushuang UI: active tactic keeps an executable entry", () => WushuangUiChecks.ActiveTacticKeepsAnExecutableEntry(output));
            Check("Wushuang UI: Sanchen multi-selection survives release and clock refresh", () => WushuangUiChecks.SanchenSelectionSurvivesClockRefresh(output));
            Check("Wushuang UI: Jinzhi selects materials before basic card and cancels unpaid", () => WushuangUiChecks.JinzhiSelectsMaterialsBeforeBasicCard(output));
            Check("Jiange activity UI: faction tier rules AI teammate and real setup", () => CheckJiangeActivity(output));
            Check("Shanhe campaign UI: story map choices and independent cold restore", () => ShanheCampaignUiChecks.StoryMapChoicesAndIndependentColdRestore(output));
            Check("Shanhe campaign UI: actual encounter save routing and stale map choices", () => ShanheCampaignUiChecks.ActualEncounterSaveRoutingAndStaleMapChoices(output));
            Check("Shanhe campaign UI: edition selection exclusive boss and cold restore", () => ShanheCampaignUiChecks.EditionSelectionExclusiveBossAndColdRestore(output));
            Check("Shanhe campaign UI: launcher selection map and detached window", () => ShanheCampaignUiChecks.LauncherSelectionMapAndDetachedWindow(output));
            Check("Shanhe campaign UI: full roster search pages and selection", () => ShanheCampaignUiChecks.FullRosterSearchPagesAndSelection(output));
            Check("Shanhe campaign UI: preparation locks and encounter preview", () => ShanheCampaignUiChecks.PreparationLocksAndEncounterPreview(output));
            Check("Shanhe campaign UI: public fixed NPC cards read-only preview", () => ShanheCampaignUiChecks.PublishedFixedNpcCardsAreReadOnlyInEncounterPreview(output));
            Check("Shanhe campaign UI: eight-seat boss stages and final settlement preview", () => ShanheCampaignUiChecks.EightSeatBossStagesAndFinalSettlementPreview(output));
            Check("Shanhe campaign UI: published boss portraits render on actual seats", () => ShanheBossPortraitChecks.PublishedS5BossAliasesDecodeAndRenderOnRealSeats(output));
            Check("Shanhe campaign UI: inheritance before map and cold restore", () => ShanheCampaignUiChecks.InheritanceOffersBeforeFirstMapAndSurvivesColdRestore(output));
            Check("Shanhe campaign UI: shop purchase states and codex navigation", () => ShanheModulesUiChecks.ShopPurchaseAndCodexNavigation(output));
            Check("Shanhe codex UI: unlock equip persist and market", () => ShanheCodexUiChecks.UnlockEquipPersistAndMarket(output));
            Check("Shanhe market UI: free refresh clicks paid allowance and zero wallet", () => ShanheCodexUiChecks.FreeRefreshClicksKeepPaidAllowancePricesAndZeroWalletState(output));
            Check("Shanhe market UI: unlimited paid refresh wallet and free priority", () => ShanheCodexUiChecks.UnlimitedPaidRefreshLabelAndButtonFollowTheActualWallet(output));
            Check("Shanhe market UI: restock purchase same slot prices and refresh allowances", () => ShanheCodexUiChecks.RestockPurchaseClicksReplaceOneSlotAndKeepPricesAndRefreshAllowances(output));
            Check("Shanhe codex UI: failed profile write preserves progress", ShanheCodexUiChecks.FailedProfileWritePreservesProgress);
            Check("Shanhe loadout plans: legacy JSON and detached operations", ShanheCodexLoadoutPersistenceChecks.LegacyJsonAndLoadoutOperationsStayDetached);
            Check("Shanhe loadout plans: invalid metadata and operations rejected", ShanheCodexLoadoutPersistenceChecks.DamagedLoadoutsAndInvalidOperationsAreRejected);
            Check("Shanhe loadout plans: real controls independent copies and frozen expedition", () => ShanheCodexLoadoutUiChecks.PlanControlsPersistIndependentCopiesAndFrozenExpedition(output));
            Check("Shanhe loadout plans: failed reads and writes preserve current plan and books", ShanheCodexLoadoutUiChecks.FailedReadsAndWritesKeepTheCurrentPlanAndBooks);
            Check("Shanhe Dong Ji UI: dynamic exact card confirmation and locked gift", () => ShanheDongJiUiChecks.DynamicRecipientCountConfirmsExactCardsAndExcludesLockedGift(output));
            Check("Shanhe codex settlement: actual victory one-time books and rejected outcomes", () => ShanheCodexSettlementChecks.ActualCompletedVictoryCreditsOnceAndRejectsOtherOutcomes(output));
            Check("Shanhe run records: real history tab keeps completed runs and credited books", () => ShanheRunRecordUiChecks.CompletedRunsRemainVisibleInTheRealHistoryTab(output));
            Check("Shanhe reward UI: real victory cards confirmation skip skill and replacement", () => ShanheRewardUiChecks.RealVictoryRewardCardsConfirmSkipSkillAndReplacement(output));
            Check("Shanhe reward UI: boss bundles advance through three chapters", () => ShanheRewardUiChecks.BossBundlesRequireEachGroupThenAdvanceThreeChapters(output));
            Check("Shanhe reward UI: categories four choices bonus-only and three boss groups", () => ShanheRewardUiChecks.CategoriesFourChoicesBonusOnlyAndThreeBossGroups(output));
            Check("Shanhe reward UI: shop skill replacement confirms before payment", () => ShanheRewardUiChecks.ShopSkillsUseReplacementModalAndChargeOnlyOnConfirmation(output));
            Check("Boss activity UI: real coalition setup stage and cross-mode restore", () => BossActivityUiChecks.RealSetupAndCrossModeRestore(output));
            Check("Hulao activity UI: forms shuffle counters and cross-mode restore", () => HulaoActivityUiChecks.SetupFormCounterAndCrossModeRestore(output));
            Check("Shenwu activity UI: shared god draft six seats and restore", () => ShenwuActivityUiChecks.SharedGodDraftSixSeatsAndRestore(output));
            Check("Shenwu expanded UI: versions five enemy tiers artwork and restore", () => ShenwuActivityUiChecks.ExpandedVersionsTiersAndRestore(output));
            Check("Nian activity UI: four seats clock and cross-configuration restore", () => NianActivityUiChecks.SetupFourSeatsFormPortraitsAndCrossConfigurationRestore(output));
            Check("Hell activity UI: five tiers real five seats dormant auxiliary and restore", () => HellActivityUiChecks.FiveTierLobbyRealFiveSeatsDormantAuxiliaryAndRestore(output));
            Check("Wenhe activity UI: private goal eight seats and cross-configuration restore", () => WenheActivityUiChecks.SurvivalLobbyPrivateGoalEightSeatsAndCrossConfigurationRestore(output));
            Check("Guandu activity UI: two armies roles event and cold restore", () => GuanduActivityUiChecks.TwoArmiesCommanderGeneralEventAndCrossConfigurationRestore(output));
            Check("Guandu activity UI: physical card target sets recast and native finish", () => GuanduActivityUiChecks.PhysicalCardsSelectBoundedRecipientsWholeArmyAndRecast(output));
            Check("Dragon boat activity UI: four teams private draft and cross configuration", () => DragonBoatActivityUiChecks.FourTeamLobbyPrivateDraftAndCrossConfigurationRestore(output));
            Check("Dragon boat activity UI: arrival order and actual two round team report", () => DragonBoatActivityUiChecks.NativeTreasureArrivalOrderAndTwoRoundTeamReport(output));
            Check("Wenhe activity UI: native ranks summary history and winner sound", () => WenheActivityUiChecks.NativeEliminationRanksDriveSummaryHistoryAndWinnerSound(output));
            Check("Hell activity UI: click physical soul restores automatic public targets", () => HellActivityUiChecks.ClickPhysicalSoulRestoresAutomaticPublicTargets(output));
            Check("Hell activity UI: single card conversion rail restores all targets", () => HellActivityUiChecks.SingleCardConversionSoulAppearsOnRailAndUsesAllTargets(output));
            Check("Nian activity UI: click dead ally cancel confirm and save shared revival", () => NianActivityUiChecks.ClickDeadAllyAndConfirmSharedRevival(output));
            Check("identity dual generals show consecutive choices portraits and both skill sources", () => IdentityDualGeneralUiChecks.SelectionAndTable(output));
            Check("Interaction motion: native lifecycle reversal and preference cancellation", InteractionMotionChecks.NativeLifecyclePreservesLayoutAndRetiresAnimations);
            Check("selection, target toggle, cancel and play use legal actions", CheckSelections);
            Check("Automatic targets UI: unique card and conversion drafts", AutomaticTargetSelectionChecks.CardAndConversionDraftsSelectOnlyUniqueTargets);
            Check("Locked card identity UI: direct Longnu Wushen play and Jieying hand badge", LockedCardIdentityUiChecks.DirectPlayAndJieyingHandBadge);
            Check("Automatic targets UI: active skill material legality and prompt defaults", AutomaticTargetSelectionChecks.ActiveSkillDraftsFollowMaterialLegality);
            Check("Automatic rescue UI: unavailable rescue passes and real choices remain", () => AutomaticRescuePassChecks.UnavailableRescuePass(output));
            Check("own public pile conversion: real Bing rail and response source", OwnPublicPileConversionUiChecks.RealBingPlayRailAndPublishedResponseSource);
            Check("Liu Ba UI: private coins and bottom exchange", LiuBaCoinUiChecks.RealMintPrivateBottomExchangeAndReversibleChoice);
            Check("conversion choices retain exact provenance without leaking trusted action details", CardConversionUiChecks.Run);
            Check("Le Cao Zhi UI: private Fu alias draft and exact native payment", () => LeCaoZhiUiChecks.PrivateFuAliasDraftAndExactNativePayment(output));
            Check("Huoji selects its rail skill before red cards and commits the exact conversion", () => CardConversionTargetUiChecks.HuojiSelectsSkillThenRedCard(output));
            Check("multi-target Slash portraits: Tianyi Lihuo Fangtian conversions and exact confirmation", () => MultiTargetSlashUiChecks.PortraitDraftsUseExactNativeTargets(output));
            Check("conversion skill rail selects published materials including chains and delayed cards", () => ConversionSkillSelectionChecks.SingleCardRules(output));
            Check("conversion skill rail chooses output before materials and retires old mode buttons", () => ConversionSkillSelectionChecks.MultipleOutputs(output));
            Check("conversion response rail: material driven exact hand equipment payment", () => ResponseConversionSelectionChecks.ResponsesRequireSkillAndExactMaterials(output));
            Check("Zhangba response UI: native Slash Longdan and weapon pair drafts", () => ResponseConversionSelectionChecks.ZhangbaUsesWeaponSlotAndExactPair(output));
            Check("conversion skill rail: material driven one and two card Longhun", () => ResponseConversionSelectionChecks.PlayGroupsSingleAndMultipleCardConversions(output));
            Check("conversion skill rail: material determines Boundary Longdan output", () => ConversionSkillSelectionChecks.MaterialsDetermineOutput(output));
            Check("Zhuque Fan UI: confirm Slash before choosing normal or fire with Longdan", () => ConversionSkillSelectionChecks.ZhuqueFanAfterSlashConfirmation(output));
            Check("composed skills share the generic WPF draft and submit their stable program identity", SkillProgramUiChecks.ActiveSelectionAndSubmission);
            Check("Program paid choices preserve actor control and opaque slots", () => ProgramPaidChoiceUiChecks.SharedChoiceSurfaces(output));
            Check("Fuchao UI: incremental discard exposes pick undo and finish cold", () => KongShuUiChecks.FuchaoIncrementalDiscardKeepsPickUndoAndFinishVisible(output));
            Check("Quanmou Give UI: cancel reselect and confirm exact physical card", () => MouSimaYiGiveUiChecks.CancelReselectAndConfirm(output));
            Check("Concurrent participant UI: private human input Step Auto and reload", ParticipantPromptUiChecks.PrivateHumanInputStepAutoAndReload);
            Check("declaration and general library views protect private faces", () => RuleInformationUiChecks.PrivateFacesAndPublicDeclarations(output));
            Check("Program owned-card sets render a private shared draft and commit once", () => ProgramOwnedCardsUiChecks.PrivateSetUsesSharedChoiceSurface(output));
            Check("Chenglue UI: batch and incremental discards share confirmation cold", () => ChenglueUiChecks.BatchAndIncrementalDiscards(output));
            Check("Native discard UI: variable cost multi-selection and round count restrictions", () => NativeDiscardBatchUiChecks.NativeVariableCostKeepsMultipleCardsAndCountRestrictions(output));
            Check("Native discard UI: Cao Xian adjustment and target batch draft", () => NativeDiscardBatchUiChecks.CaoXianAdjustmentAndTargetDiscardUseOneEditableDraft(output));
            Check("Native additional discard UI: Jianzhuan hand equipment batch pays once", () => NativeAdditionalDiscardUiChecks.JianzhuanSelectsHandAndEquipmentBeforeSinglePayment(output));
            Check("Native additional discard UI: Shimou reset clears recovered and local draft", () => NativeAdditionalDiscardUiChecks.ShimouNativeResetClearsRecoveredAndLocalDraft(output));
            Check("Multi-card drafts UI: active discard survives release and committed clock refresh", () => MultiCardDraftUiChecks.ActiveDiscardSurvivesReleaseAndClock(output));
            Check("Multi-card drafts UI: owned and payment require explicit confirmation", () => MultiCardDraftUiChecks.MultiCardPromptsRequireExplicitConfirmation(output));
            Check("Revealed subsets UI: Chengxiang rank draft confirmation and empty cancel", () => RevealedSubsetUiChecks.RankDraftConfirmationAndEmptyCancel(output));
            Check("Revealed subsets UI: shared private count and public suit drafts", () => RevealedSubsetUiChecks.SharedPrivateCountAndPublicSuitDrafts(output));
            Check("Revealed subsets UI: Xunxun Guanxing top and bottom rows", () => RevealedSubsetUiChecks.TopAndBottomRowsExactAndFlexible(output));
            Check("Public pile UI: multi-card storage lower dock and portrait inspection", () => PublicPileInteractionUiChecks.StoreSetsAndInspectPortraitPiles(output));
            Check("hand responses label actual decline and retain native press feedback", () => NativeActionFeedbackChecks.ResponseSemantics(output));
            Check("Tieqi UI: cancel declines suit discard without paying selected cards", () => TieqiUiChecks.CancelDeclinesSuitDiscard(output));
            Check("Cold Slash UI: native physical response incoming name and Slash effect", () => ColdSlashUiChecks.PhysicalResponseIncomingAndSlashEffects(output));
            Check("device preferences persist before play and survive legacy loads and tutorials", () => PreferencesChecks.StartupAndMigration(output));
            Check("invalid preferences remain recoverable and slider writes coalesce safely", () => PreferencesChecks.FailuresAndDebounce(output));
            Check("playback batches internal steps while preserving human boundaries and replay", PlaybackChecks.AutomaticStepsPreserveCommittedBoundaries);
            Check("opaque target-card slots render privately and commit through WPF commands", () => TargetCardChecks.Controls(output));
            Check("Harvest UI: public pool persists through AI picks and card flights", () => BasicCardTransferChecks.Harvest(output));
            Check("Target card UI: concrete hand slots resolve without second picker", () => TargetCardChecks.OnlyHandCard(output));
            Check("Target card UI: multiple taken cards require one confirmation", () => TargetCardChecks.MultiTakeConfirmsOnce(output));
            Check("dual portraits protect hidden slots and retain legacy skill semantics", () => NationalSeatChecks.PrivacyAndReveal(output));
            Check("dual health and bones UI: explicit pools and assigned slots", () => DualHealthAndBoneUiChecks.ExplicitPoolsAndOccupiedBoneSlots(output));
            Check("saved UI boundaries restore and continue through actual files", () => PersistenceChecks.RoundTrips(output));
            Check("failed writes preserve saves and failed reads initialize a new game", () => PersistenceChecks.FailedWritesPreserveSavesAndFailedLoadsInitializeGame(output));
            Check("faulted sessions preserve saves and explain recovery", PersistenceChecks.FaultedSessionPreservesSavesAndExplainsRecovery);
            Check("exception reports preserve failed commands and survive new matches", () => ExceptionReportChecks.FaultReportsSurviveNewMatches(output));
            Check("exception reports keep recovery when log writes fail", () => ExceptionReportChecks.DiagnosticWriteFailurePreservesFaultHandling(output));
            Check("exception reports retain stacks when recovery capture fails", () => ExceptionReportChecks.MissingRecoveryPrefixStillPreservesStack(output));
            Check("autosave coalesces commands and resumes after closing", PersistenceChecks.AutomaticAndExit);
            Check("battle feedback exposes only committed public actions", FeedbackChecks.PublicProjection);
            Check("skill draw feedback follows native Zhiheng Jiefan and cold recovery", () => SkillDrawFeedbackChecks.NativeSkillsAndColdRestore(output));
            Check("battle feedback shares frame geometry and refreshes moving readouts", () => BattleFrameGeometryChecks.FrameGeometry(output));
            Check("battle feedback health tracks follow committed damage recovery and loaded boundaries", () => HealthFeedbackChecks.CommittedHealth(output));
            Check("Armor seats: real opening stacks payments accessible digits and zero hiding", () => ArmorSeatUiChecks.RealOpeningStacksAndDamagePaymentsRefreshEverySeat(output));
            Check("card flights keep local hand origins private and retire cached layout", () => HandCardFlightChecks.OriginBoundaries(output));
            Check("card flights hand off public faces once and restore interrupted arrivals", () => PublicCardHandoffChecks.ArrivalAndBoundaries(output));
            Check("card flights preserve public suit rank and converted effective faces", () => PublicCardIdentityChecks.PublicFaces(output));
            Check("multi material responses display one virtual card", PublicCardIdentityChecks.MultiMaterialResponses);
            Check("Guanxu UI: fixed exchange separate discard and cancellation", () => CardExchangeUiChecks.FixedExchangeDiscardAndCancellation(output));
            Check("audio follows committed actions and survives mute, background and device failure", AudioChecks.CommandRouting);
            if (args.Contains("--verify-native-audio")) Check("official native audio plays MP3 and converted WAV at zero volume", OfficialAudioChecks.NativeSilentPlayback);
            if (args.Contains("--verify-native-audio")) Check("native WPF audio opens and completes every effect at zero volume", AudioChecks.NativeSilentPlayback);
            if (!_startAfterReached)
                throw new InvalidOperationException($"No WPF check matched start-after '{_startAfterName}'.");
            foreach (var filter in _nameFilters)
                if (!MatchedFilters.Contains(filter))
                    throw new InvalidOperationException($"No WPF checks matched filter '{filter}'.");
            Assert(BindingErrors.Errors.Count == 0, string.Join(Environment.NewLine, BindingErrors.Errors.Take(15)));
            Console.WriteLine($"{_passed} WPF checks passed, {_failed} failed. Renders: {output}");
            return _failed == 0 ? 0 : 1;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
        finally { app.Shutdown(); }
    }

    private static void Check(string name, Action action)
    {
        if (!_startAfterReached)
        {
            if (string.Equals(name, _startAfterName, StringComparison.Ordinal))
                _startAfterReached = true;
            return;
        }
        var matchingFilters = _nameFilters.Where(filter => name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (_nameFilters.Length > 0 && matchingFilters.Length == 0)
            return;
        foreach (var filter in matchingFilters) MatchedFilters.Add(filter);
        var started = Stopwatch.GetTimestamp();
        try
        {
            action();
            _passed++;
            Console.WriteLine($"[PASS] {name}");
        }
        catch (Exception exception)
        {
            _failed++;
            Console.Error.WriteLine($"[FAIL] {name}: {exception}");
        }
        finally
        {
            if (_verbose) Console.WriteLine($"[TIME] {name}: {Stopwatch.GetElapsedTime(started).TotalMilliseconds.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)} ms");
        }
    }

    private static MainViewModel NewViewModel(int seed = 721019) => new(false, seed, showSetup: false, saveStore: new MemorySaveStore()) { IsMotionEnabled = false };


    private static void CheckModeLobby(string output)
    {
        using var vm = new MainViewModel(false, 721019, showSetup: true, saveStore: new MemorySaveStore(), useExpandedContent: true, wushuangSaveStore: new MemorySaveStore())
        {
            IsMotionEnabled = false
        };
        var revision = Engine(vm).Revision;
        var window = new MainWindow(vm);
        window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        Assert(vm.VisibleTableModes.Count == vm.TableModes.Count && vm.SelectedModeCategory.Id == "all" &&
               vm.VisibleTableModes.Any(mode => mode.ModeId == "activity:shenwu-2017") &&
               vm.VisibleTableModes.Any(mode => mode.ModeId == "activity:nian-2016") &&
               vm.VisibleTableModes.Any(mode => mode.ModeId == "activity:dragon-boat-2018") &&
               vm.VisibleTableModes.Any(mode => mode.ModeId == "activity:guandu-2023") &&
               vm.VisibleTableModes.Any(mode => mode.ModeId == WushuangMode.ModeId && mode.PlayerCount == 2) &&
               vm.VisibleTableModes.Select(mode => mode.ModeBadge).ToHashSet().SetEquals(
                   ["经典身份", "双将身份", "公开阵营", "双将试验", "五阶 PVE", "五阶生存", "五阶竞舟", "五阶军争", "限时单挑"]) &&
               vm.StartingRoles.Any(role => role.Role is null) &&
               vm.StartingRoles.Where(role => role.Role.HasValue).Select(role => role.Role!.Value).ToHashSet()
                   .SetEquals([Role.Lord, Role.Loyalist, Role.Rebel, Role.Renegade]) && vm.SelectedStartingRole.Role is null,
            "The expanded mode lobby did not expose all registered entry families.");
        Render(root, 1440, 880, Path.Combine(output, "134-home-lobby.png"));
        Render(root, 1120, 740, Path.Combine(output, "134-home-lobby-compact.png"));
        Assert(!vm.IsLobbyConfigurationOpen && ((HomeLobby)window.FindName("LobbyHome")).IsEnabled,
            "Startup must show the home lobby before the detailed setup.");
        var home = (HomeLobby)window.FindName("LobbyHome");
        foreach (var (buttonName, categoryId) in new[]
        {
            ("ClassicBattleButton", "identity"), ("TeamBattleButton", "team"),
            ("NationalBattleButton", "national"), ("EnterBattleButton", "all")
        })
        {
            var entry = (Button)home.FindName(buttonName);
            Assert(entry.ActualWidth > 150 && entry.ActualHeight > 100 && entry.Command!.CanExecute(entry.CommandParameter),
                $"Home entry {buttonName} is not accessible at the minimum layout size.");
            entry.Command!.Execute(entry.CommandParameter);
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
            Assert(vm.IsLobbyConfigurationOpen && vm.SelectedModeCategory.Id == categoryId && !home.IsEnabled,
                "A home entry did not open its real category or isolate the modal input.");
            vm.BackToLobbyCommand.Execute(null);
        }
        vm.OpenLobbyCategoryCommand.Execute("all");
        Render(root, 1120, 740, Path.Combine(output, "134-mode-lobby.png"));
        var groups = (ItemsControl)window.FindName("ModeGroupChoices");
        var lobby = (ListBox)window.FindName("TableModeChoices");
        Assert(groups.ActualHeight > 0 && groups.Items.Count == vm.VisibleModeGroups.Count &&
               vm.VisibleModeGroups.Sum(group => group.Variants.Count) == vm.VisibleTableModes.Count &&
               vm.VisibleModeGroups.SelectMany(group => group.Variants).ToHashSet().SetEquals(vm.VisibleTableModes) &&
               (ListBox)window.FindName("ModeCategoryChoices") is { Items.Count: 5 },
            "Mode cards or category controls are inaccessible.");
        ModeSelectionChecks.RefreshBoundSelection(vm, lobby);
        ModeSelectionChecks.TwoStepSetup(vm, window, root, output);

        vm.SelectedModeCategory = vm.ModeCategories.Single(category => category.Id == "national");
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
        Assert(vm.VisibleTableModes.Count == 1 && vm.VisibleTableModes.Single().ModeId == "national:ambitious-6" &&
               vm.SelectedTableMode.ModeId.StartsWith("national:") && vm.IsNationalModeSelection,
            "National category leaked another mode or retained an invalid selection.");
        vm.SelectedModeCategory = vm.ModeCategories.Single(category => category.Id == "team");
        Assert(vm.VisibleTableModes.Count == 1 && vm.SelectedTableMode.ModeId == "team:standard-2v2" && vm.IsTeamModeSelection,
            "Team category did not select its real registered entry.");
        vm.SelectedModeCategory = vm.ModeCategories.Single(category => category.Id == "identity");
        Assert(vm.VisibleTableModes.Select(mode => mode.ModeId).SequenceEqual(["identity:classic-8", "identity:classic-dual-8", "identity:classic-5"]) && vm.IsIdentityModeSelection,
            "Identity category must offer only five- and eight-player identity tables.");
        Assert(Engine(vm).Revision == revision && vm.IsNewGameSetupOpen,
            "Browsing the mode lobby changed or replaced the suspended match.");
        var shortcut = typeof(MainWindow).GetMethod("HandleShortcut", BindingFlags.NonPublic | BindingFlags.Instance)!;
        shortcut.Invoke(window, [Key.Escape, ModifierKeys.None]);
        Assert(!vm.IsLobbyConfigurationOpen && vm.IsNewGameSetupOpen,
            "Escape from setup must return to the lobby without entering the suspended game.");
        vm.OpenSettingsCommand.Execute(null);
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
        Assert(!((Border)window.FindName("NewGameSetupPanel")).IsEnabled,
            "Settings must isolate home lobby input.");
        shortcut.Invoke(window, [Key.Escape, ModifierKeys.None]);
        Assert(!vm.IsSettingsOpen && vm.IsNewGameSetupOpen, "Closing settings must preserve the lobby.");
        vm.CancelNewGameSetupCommand.Execute(null);
        vm.NewGameCommand.Execute(null);
        Assert(!vm.IsLobbyConfigurationOpen, "Reopening the lobby must not retain a stale configuration overlay.");
        Assert(BindingErrors.Errors.Count == 0, string.Join(Environment.NewLine, BindingErrors.Errors.Take(10)));
        window.Content = null;
        window.Close();
    }


    internal static GameEngine Engine(MainViewModel vm) =>
        (GameEngine)typeof(MainViewModel).GetField("_game", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(vm)!;

    internal static void StartLordFixture(MainViewModel vm)
    {
        // Mechanism fixtures do not depend on the lobby's random identity draw.
        vm.SelectedStartingRole = new StartingRoleOption("fixture", Role.Lord, string.Empty);
        vm.StartNewGameCommand.Execute(null);
        vm.ContinueFromIdentityRevealCommand.Execute(null);
    }

    internal static void AssertNoBindingErrors() =>
        Assert(BindingErrors.Errors.Count == 0, string.Join(Environment.NewLine, BindingErrors.Errors.Take(10)));

    internal static void Assert(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    internal static IEnumerable<T> Find<T>(DependencyObject root) where T : DependencyObject
    {
        if (root is T item) yield return item;
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            foreach (var child in Find<T>(VisualTreeHelper.GetChild(root, i))) yield return child;
    }

    private static void CheckJiangeActivity(string output)
    {
        using var vm = new MainViewModel(false, 17, showSetup: true, saveStore: new MemorySaveStore(), useExpandedContent: true)
        { IsMotionEnabled = false };
        var window = new MainWindow(vm);
        window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        var home = (HomeLobby)window.FindName("LobbyHome");
        Render(root, 1440, 880, null);
        ((Button)home.FindName("ActivityBattleButton")).Command.Execute("activity");
        Assert(vm.VisibleTableModes.Count(mode => mode.ModeId.StartsWith("activity:jiange-", StringComparison.Ordinal)) == 3 && vm.IsActivityModeSelection && vm.IsTeamModeSelection &&
            !vm.IsIdentityModeSelection && vm.SelectedStartingTeam.TeamId == "team:shu" &&
            vm.StartingTeams.Select(t => t.TeamId).ToHashSet().SetEquals(["team:shu", "team:wei"]),
            "Activity entry must offer the three real difficulties and Wei/Shu faction choices.");
        Assert(vm.TeamModeSetupText.Contains("五阶") && vm.TeamModeSetupText.Contains("人机搭档") && vm.TeamModeSetupText.Contains("万夫莫开"),
            "Setup must explain the actual player tier, teammate and event.");
        Render(root, 1440, 880, Path.Combine(output, "150-jiange-setup.png"));
        vm.StartNewGameFromLobbyCommand.Execute(null);
        vm.ContinueFromIdentityRevealCommand.Execute(null);
        Assert(vm.IsGeneralSelectionPending && vm.GeneralSelectionSubtitle.Contains("五阶") &&
            vm.WindowTitle.Contains("剑阁决战") && vm.IdentityObjective.Contains("英灵"),
            "A real activity game must reach the same-faction general prompt with activity-specific labels.");
        Assert(Engine(vm).CreateSnapshot(0).Players.Count(p => p.IsHuman) == 1,
            "The teammate and all six NPC seats must remain AI controlled.");
        Render(root, 1440, 880, Path.Combine(output, "151-jiange-selection.png"));
        vm.ConfirmGeneralChoiceCommand.Execute(null);
        if (vm.RunToHumanCommand.CanExecute(null)) vm.RunToHumanCommand.Execute(null);
        Assert(Engine(vm).CreateSnapshot(0).Players.All(p => p.GeneralId.Length > 0) &&
            vm.Seats.All(p => p.RoleLabel is "蜀军" or "魏军"),
            "NPC setup must complete through the real UI commands and show the actual factions.");
        Render(root, 1440, 880, Path.Combine(output, "152-jiange-table.png"));
        vm.SaveGameCommand.Execute(null);
        var savedMode = Engine(vm).ModeId;
        vm.LoadManualGameCommand.Execute(null);
        Assert(!vm.HasSaveError && Engine(vm).ModeId == savedMode && vm.WindowTitle.Contains("剑阁决战"),
            "The desktop must restore a real activity save with the optional content package.");
        window.Close();
    }

    internal static void Render(FrameworkElement root, int width, int height, string? path)
    {
        root.Measure(new Size(width, height));
        root.Arrange(new Rect(0, 0, width, height));
        root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
        var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(root);
        if (path is null) return;
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using var file = File.Create(path);
        encoder.Save(file);
    }

    private static void CheckSelections()
    {
        using var vm = NewViewModel();
        StartLordFixture(vm);
        vm.SelectGeneralChoiceCommand.Execute(vm.GeneralChoices[0]);
        AdvanceToDecision(vm);
        var engine = Engine(vm);
        var action = engine.GetHumanLegalActions().First(a => a.CardId.HasValue && a.TargetSeats.Count == 1 && a.TargetCardId is null);
        var card = vm.Hand.Single(c => c.Id == action.CardId);
        vm.SelectCardCommand.Execute(card);
        var target = vm.Seats.Single(seat => seat.Seat == action.TargetSeat);
        Assert(target.IsLegalTarget, "Legal target is not highlighted.");
        if (target.IsSelectedTarget) vm.SelectTargetCommand.Execute(target);
        vm.SelectTargetCommand.Execute(target);
        Assert(vm.CanPlaySelected && target.IsSelectedTarget, "Selecting the target must enable confirmation.");
        vm.SelectTargetCommand.Execute(target);
        Assert(!target.IsSelectedTarget, "Clicking the target again should deselect it.");
        vm.ClearSelectionCommand.Execute(null);
        Assert(!vm.HasSelection && vm.Hand.All(c => !c.IsSelected) && vm.Seats.All(s => !s.IsSelectedTarget), "Cancel left stale highlights.");
        vm.SelectCardCommand.Execute(card);
        if (!target.IsSelectedTarget) vm.SelectTargetCommand.Execute(target);
        var revision = engine.Revision;
        vm.PlaySelectedCardCommand.Execute(null);
        Assert(engine.Revision > revision, "Confirmed card was not played.");
        Assert(!vm.HasSelection, "Selection did not clear after play.");
        Assert(!vm.PromptText.StartsWith("操作未执行"), vm.PromptText);
    }


    internal static ICommand? SetupResponseDeclineCommand(MainViewModel vm) =>
        Engine(vm).PendingDecision is { PlayerSeat: 0 } response
            ? response.Kind switch
            {
                DecisionKind.Nullification => vm.CancelActionCommand,
                DecisionKind.RespondDodge or DecisionKind.RespondSlash => vm.DeclineResponseCommand,
                _ => null
            }
            : null;

    internal static void AdvanceToDecision(MainViewModel vm)
    {
        for (var i = 0; i < 10000 && vm.CanStepAi; i++) vm.StepAiCommand.Execute(null);
    }

    internal static void ResolveDiscard(MainViewModel vm)
    {
        vm.RecommendDiscardCommand.Execute(null);
        Assert(vm.CanConfirmSelected, "Recommended discard has the wrong size.");
        vm.ConfirmSelectedCommand.Execute(null);
        Assert(!vm.IsDiscardSelectionPending && vm.SelectedDiscardCount == 0, "Discard did not finish.");
    }


    private sealed class BindingListener : TraceListener
    {
        public List<string> Errors { get; } = [];
        public override void Write(string? message) { }
        public override void WriteLine(string? message)
        {
            if (message is not null && (message.Contains("Error:") || message.Contains("Warning:"))) Errors.Add(message);
        }
    }
}
