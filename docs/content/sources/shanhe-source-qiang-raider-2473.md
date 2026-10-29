# 趁人之危2473与羌人劫匪

来源为官方 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的山河图战斗配置和技能表。固定缓存外层 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`，解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`。

`fightID=2473`、`Ggroup=2473`，战斗标题为趁人之危。其完整阵容只有一行 `General.a=2473`：`b=4861`、`c=羌人劫匪`、`d=1000125`、`h=100`。角色表4861为士兵（`ShiBing61`）；运行时保留 `supporter` 回退键，桌面可按下列精确来源卡面及运行时ID绑定显示，不借用严白虎、郭汜或张飞的身份与肖像。

原始体力与上限4/4、通常摸牌2、随机初始手牌4、护甲0、护甲回复0、护甲上限5、额外出杀0、额外伤害0；没有固定装备、随身牌、隐藏成员或特殊 `att_*` 属性。`NewbieGroup=22473` 是独立战斗配置，不追加为2473的随从。普通遭遇池15与17引用本组，来源配置覆盖14个普通城市节点；战场成长继续按真实来源难度、天数、章节与可见人数1投影，包内保留原始身体值。

三个形态覆盖完整源难度1—25，技能按阈值累计：

| 源难度 | 原始字段 | 本地技能 |
| --- | --- | --- |
| 1—10 | `v=276` | `ol:zhidao` 雉盗 |
| 11—15 | 以上加 `getspell_KN=939` | 以上加 `boundary:paoxiao` 咆哮 |
| 16—25 | 以上加 `getspell_EM=864` | 以上加 `ol:sidao` 伺盗 |

276（`ZhiDao`）为锁定技：每回合限一次，自己出牌阶段对其他角色造成实际伤害后，取得其每个区域各一张牌，随后本回合使用牌不能指定其他角色为目标。现有雉盗按真实伤害帧逐区取得，并施加本回合仅能指定自己的限制，与来源主文一致。这里只授予雉盗，不附带严白虎的寄篱。

939（`PaoXiaoPo`）为无限制使用杀，且杀被抵消后，本回合下一次杀造成的伤害+1。复用的界咆哮包含这两部分；不使用移动版“使用过杀后本阶段无距离限制”的另一版本。

864（`SiDao`）为出牌阶段限一次：连续对同一名其他角色使用两张牌后，可将一张手牌当顺手牵羊对其使用，须满足合法使用条件。现有伺盗跟踪实际使用及完成身份，按原出牌阶段限制并以真实手牌支付原生顺牵，符合来源主文。这里只授予伺盗，不附带郭汜的贪狭；高档仍保留雉盗，雉盗已经生效的本回合目标限制不会因追加伺盗而被绕过。

内容包为 `StandardShanheSourceQiangRaiderPackage`，三个身份为 `shanhe:source-qiang-raider-2473-base`、`shanhe:source-qiang-raider-2473-hard`、`shanhe:source-qiang-raider-2473-nightmare`；通过 `GetGeneralId(1..25)` 选择，角色统一为 `character:shanhe-source-soldier-4861`。

## 原生卡面

2026-10-29核对了普通组2473完整单人配方的 `rawOrdinal=0 → b=4861 → d=1000125`。载体4861的角色行无 `au/av` 头像重定向；本图来自本组明确的PVE皮肤覆盖，不能按士兵载体或雉盗、咆哮、伺盗所属武将借图。`cha_gs_dbs_fs_skininfo.sgs/root.pveskin` 的1000125行指定 `seatpic=generalface_1000125`、`headpic=1000125`、`diagrampic=100012500`；该解码皮肤表SHA-256为 `00ee5f994401976563e241571d3e5618f573ad2a4b85336ff132afb262f1e8d4`。客户端 `q6t.getSeatSkinUrl → hA.SkinSeatUrl` 经 `N2t.SeatImgUrl=general/seat/static/` 及桌面 `res/runtime/pc/` 生成[官方卡面URL](https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/seat/static/generalface_1000125.png)。

官方响应HTTP200、`image/png`，目视为红巾挥刃兵卒；原生146×172、RGBA、51577字节。原字节保存为 `src/CardGame.Wpf/Assets/Shanhe/Npc/generalface_1000125.png`，文件SHA-256为 `528920968d09ed81903c4f282935f0704e481c01e8c3dd753be61a53f6edeffb`，解码RGBA像素SHA-256为 `6637e49c909bc39627475b1f1c8542c5456572640fc2c4aa9d7ce9b02b0769d5`。未裁剪、缩放、换色或重编码。三个精确运行时ID分别覆盖1–10、11–15、16–25，并共享同一源皮肤；不泛绑其他4861载体NPC。原行无 `g/hide/generalShow`，按源默认分别为false/false/0，`startChapter=1`、`f=0`，没有主帅身份。

本批纯复用既有程序，不新增Core能力、规则束或逐角色快照检查。既有机制检查包括 `DamageZoneAndAdjacentTargetChecks.ActualEachZonePrivateSelectionOnceAndTurnExpiry`、`BoundaryZhangFeiChecks.CancellationAndUseFreeze`、`GuoSiChecks.ActualCompletedParentRejectsForeignWindowProofCold`；此处仅记录已有覆盖入口，本批未执行这些检查或其辅助流程。
