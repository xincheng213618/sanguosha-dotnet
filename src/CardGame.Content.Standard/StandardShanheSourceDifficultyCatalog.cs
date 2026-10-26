using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

/// <summary>Exact S5 source rows, independent of the three locally authored campaign difficulties.</summary>
public static class StandardShanheSourceDifficultyCatalog
{
    public const string SourceUrl = "https://s.xiaochao.org/data/manifest.json";
    public const string GeneratedAt = "2026-10-21T11:39:31.85Z";
    public const string ModifierLabelSourceUrl = "https://s.xiaochao.org/app.js?v=23421f2ef401";
    public const string OfficialConfigUrl = "https://web.sanguosha.com/220/h5_2/res/config/Config.sgs";
    public const string OfficialClientBundleUrl = "https://web.sanguosha.com/220/h5_2/sgsGame_a.sgs";
    public const string OfficialConfigSha256 = "2ac666930803aed834a2f48fea400737d8f8572b98bec90f129dd09b5ee8fe22";
    public const string OfficialClientBundleSha256 = "96b7131953f758a30da44c9bdbbcd8a429174050ef7c16961216b90cd923ec31";

    public static ShanheSourceDifficultyCatalog Season5 { get; } = new(5, 25, SourceUrl, GeneratedAt,
        "通过大难度下的任一小难度可解锁下一大难度。",
        [
            new(1, 1, "普通", "普通1", "", "100011,200;8100201,1",
                new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null))
            { SourceTierInGroup = 1, Availability = new(null, 1, 1, null, null, null), RecSlotCtn = null },
            new(2, 1, "普通", "普通2", "敌人体力+1，初始手牌+1", "100011,220",
                new(1, 0, 1, 0, 0, 0, 0, 0, 3, 3, null))
            { SourceTierInGroup = 2, Availability = new(1, 1, 1, null, "通过普通1后解锁", "通过普通1难度后解锁"), RecSlotCtn = null },
            new(3, 1, "普通", "普通3", "敌人初始手牌+1，出【杀】次数+1", "100011,240",
                new(1, 0, 2, 1, 0, 0, 0, 0, 6, 6, null))
            { SourceTierInGroup = 3, Availability = new(2, 1, 1, null, "通过普通2后解锁", "通过普通2难度后解锁"), RecSlotCtn = null },
            new(4, 1, "普通", "普通4", "敌人摸牌+1", "100011,260",
                new(1, 1, 2, 1, 0, 0, 0, 0, 9, 9, null))
            { SourceTierInGroup = 4, Availability = new(3, 1, 1, null, "通过普通3后解锁", "通过普通3难度后解锁"), RecSlotCtn = null },
            new(5, 1, "普通", "普通5", "敌人体力+1，摸牌+1，初始手牌+1", "100011,280;112301,3",
                new(2, 2, 3, 1, 0, 0, 0, 0, 12, 12, null))
            { SourceTierInGroup = 5, Availability = new(4, 1, 1, null, "通过普通4后解锁", "通过普通4难度后解锁"), RecSlotCtn = null },
            new(6, 2, "中等", "中等1", "战斗结束后玩家不再自动回复体力", "100011,300;8100201,1",
                new(0, 1, 1, 0, 0, 0, 0, 0, 30, 30, 1))
            { SourceTierInGroup = 1, Availability = new(1, 45, 1, null, "通过普通1\n等级达到45级", "通过普通1且等级达到45级后解锁"), RecSlotCtn = null },
            new(7, 2, "中等", "中等2", "敌人摸牌+1，初始手牌+1", "100011,320",
                new(0, 2, 2, 0, 0, 0, 0, 0, 38, 38, 1))
            { SourceTierInGroup = 2, Availability = new(6, 45, 1, null, "通过中等1后解锁", "通过中等1难度后解锁"), RecSlotCtn = null },
            new(8, 2, "中等", "中等3", "敌人出【杀】次数+1", "100011,340",
                new(0, 2, 2, 1, 0, 0, 0, 0, 46, 46, 1))
            { SourceTierInGroup = 3, Availability = new(7, 45, 1, null, "通过中等2后解锁", "通过中等2难度后解锁"), RecSlotCtn = null },
            new(9, 2, "中等", "中等4", "敌人摸牌+1，初始手牌+1", "100011,360",
                new(0, 3, 3, 1, 0, 0, 0, 0, 54, 54, 1))
            { SourceTierInGroup = 4, Availability = new(8, 45, 1, null, "通过中等3后解锁", "通过中等3难度后解锁"), RecSlotCtn = null },
            new(10, 2, "中等", "中等5", "敌人体力+1，初始手牌+1", "100011,380;112301,4",
                new(1, 3, 4, 1, 0, 0, 0, 0, 62, 62, 1))
            { SourceTierInGroup = 5, Availability = new(9, 45, 1, null, "通过中等4后解锁", "通过中等4难度后解锁"), RecSlotCtn = null },
            new(11, 3, "困难", "困难1", "敌人拥有护甲回复", "100011,400;6139309,1",
                new(0, 1, 1, 0, 0, 0, 0, 0, 70, 70, 1))
            { SourceTierInGroup = 1, Availability = new(6, 80, 1, null, "通过中等1\n等级达到80级后解锁", "通过中等1且等级达到80级后解锁"), RecSlotCtn = 3 },
            new(12, 3, "困难", "困难2", "敌人摸牌+1，初始手牌+1", "100011,420",
                new(0, 2, 2, 0, 0, 0, 0, 0, 78, 78, 1))
            { SourceTierInGroup = 2, Availability = new(11, 80, 1, null, "通过困难1后解锁", "通过困难1难度后解锁"), RecSlotCtn = 3 },
            new(13, 3, "困难", "困难3", "敌人出【杀】次数+1", "100011,440",
                new(0, 2, 2, 1, 0, 0, 0, 0, 86, 86, 1))
            { SourceTierInGroup = 3, Availability = new(12, 80, 1, null, "通过困难2后解锁", "通过困难2难度后解锁"), RecSlotCtn = 3 },
            new(14, 3, "困难", "困难4", "敌人摸牌+1，初始手牌+1，护甲+1", "100011,460",
                new(0, 3, 3, 1, 1, 0, 0, 0, 94, 94, 1))
            { SourceTierInGroup = 4, Availability = new(13, 80, 1, null, "通过困难3后解锁", "通过困难3难度后解锁"), RecSlotCtn = 3 },
            new(15, 3, "困难", "困难5", "敌人体力+1，初始手牌+1，护甲+1", "100011,480;112301,10",
                new(1, 3, 4, 1, 2, 0, 0, 0, 102, 102, 1))
            { SourceTierInGroup = 5, Availability = new(14, 80, 1, null, "通过困难4后解锁", "通过困难4难度后解锁"), RecSlotCtn = 3 },
            new(16, 4, "噩梦", "噩梦1", "出现难度突变", "100011,500;8100201,2",
                new(0, 1, 1, 0, 0, 0, 0, 0, 110, 110, 1))
            { SourceTierInGroup = 1, Availability = new(11, 100, 0, "20261015T040000", "通过困难1\n等级达到100级后解锁", "通过困难1且等级达到100级后解锁"), RecSlotCtn = 4 },
            new(17, 4, "噩梦", "噩梦2", "敌人摸牌+1，初始手牌+1", "100011,520",
                new(0, 2, 2, 0, 0, 0, 0, 0, 115, 115, 1))
            { SourceTierInGroup = 2, Availability = new(16, 100, 0, "20261015T040000", "通过噩梦1后解锁", "通过噩梦1难度后解锁"), RecSlotCtn = 4 },
            new(18, 4, "噩梦", "噩梦3", "敌人出【杀】次数+1，护甲+1", "100011,540",
                new(0, 2, 2, 1, 1, 0, 0, 0, 120, 120, 1))
            { SourceTierInGroup = 3, Availability = new(17, 100, 0, "20261015T040000", "通过噩梦2后解锁", "通过噩梦2难度后解锁"), RecSlotCtn = 4 },
            new(19, 4, "噩梦", "噩梦4", "敌人摸牌+1，初始手牌+1，护甲+1", "100011,560",
                new(0, 3, 3, 1, 2, 0, 0, 0, 125, 125, 1))
            { SourceTierInGroup = 4, Availability = new(18, 100, 0, "20261015T040000", "通过噩梦3后解锁", "通过噩梦3难度后解锁"), RecSlotCtn = 4 },
            new(20, 4, "噩梦", "噩梦5", "敌人体力+1，初始手牌+1，护甲+2", "100011,580;112301,15",
                new(1, 3, 4, 1, 4, 0, 0, 0, 130, 130, 1))
            { SourceTierInGroup = 5, Availability = new(19, 100, 0, "20261015T040000", "通过噩梦4后解锁", "通过噩梦4难度后解锁"), RecSlotCtn = 4 },
            new(21, 5, "炼狱", "炼狱1", "第二章起，敌人造成伤害+1", "100011,600;6169209,1",
                new(0, 1, 1, 0, 0, 0, 0, 0, 140, 140, 1))
            { SourceTierInGroup = 1, Availability = new(16, 110, 0, "20261105T040000", "通过噩梦1\n等级达到110级后解锁", "通过噩梦1且等级达到110级后解锁"), RecSlotCtn = 5 },
            new(22, 5, "炼狱", "炼狱2", "敌人摸牌+1，初始手牌+1，护甲+1", "100011,620",
                new(0, 2, 2, 0, 1, 0, 0, 0, 145, 145, 1))
            { SourceTierInGroup = 2, Availability = new(21, 110, 0, "20261105T040000", "通过炼狱1后解锁", "通过炼狱1难度后解锁"), RecSlotCtn = 5 },
            new(23, 5, "炼狱", "炼狱3", "敌人体力+1，出【杀】次数+1，护甲+1", "100011,640",
                new(1, 2, 2, 1, 2, 0, 0, 0, 150, 150, 1))
            { SourceTierInGroup = 3, Availability = new(22, 110, 0, "20261105T040000", "通过炼狱2后解锁", "通过炼狱2难度后解锁"), RecSlotCtn = 5 },
            new(24, 5, "炼狱", "炼狱4", "敌人摸牌+1，初始手牌+1，护甲+2，造成伤害+1", "100011,660",
                new(1, 3, 3, 1, 4, 0, 1, 0, 155, 155, 1))
            { SourceTierInGroup = 4, Availability = new(23, 110, 0, "20261105T040000", "通过炼狱3后解锁", "通过炼狱3难度后解锁"), RecSlotCtn = 5 },
            new(25, 5, "炼狱", "炼狱5", "敌人体力+1，初始手牌+1，护甲+2", "100011,680;21272501,6",
                new(2, 3, 4, 1, 6, 0, 1, 0, 160, 160, 1))
            { SourceTierInGroup = 5, Availability = new(24, 110, 0, "20261105T040000", "通过炼狱4后解锁", "通过炼狱4难度后解锁"), RecSlotCtn = 5 },
        ])
    {
        OfficialEvidence = new(OfficialConfigUrl, OfficialConfigSha256, "hd_roguelike.sgs.Root.DifficultySelection",
            OfficialClientBundleUrl, OfficialClientBundleSha256, "2026-10-27")
    };
}
