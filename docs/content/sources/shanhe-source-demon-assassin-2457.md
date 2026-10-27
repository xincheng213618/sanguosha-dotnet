# 擒拿刺客2457与魔军刺客

来源为官方 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的S5山河图配置。固定外层 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`，解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`，解码 `cha_spell.sgs` 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

`Fight.fightID=2457`、`Ggroup=2457`，`Text.ID=2457` 的战斗标题为擒拿刺客。完整阵容只有一行 `General.a=2457`：`b=4861`、`c=魔军刺客`、`d=1000299`、`f=0`、`h=100`。原始体力与上限3/3，通常摸牌2、随机起手4、护甲0、护甲回复0、护甲上限5、基础额外出杀0、额外伤害0。没有固定装备、随身牌、特殊 `att_*` 属性或隐藏成员。`NewbieGroup=22457` 为另一完整战斗组，`pvpspell=3189` 为另一模式字段，均不加入本配方。

4861为士兵身份，角色统一使用 `character:shanhe-source-soldier-4861`。来源外观1000299暂未对应到已核本地肖像，使用 `supporter` 作为本地缺图回退，不借用马岱、周鲂或郭槐的身体、肖像及附带技能。

完整源难度1—25按以下阈值累计技能：

| 源难度 | 来源字段 | 精确本地技能 |
| --- | --- | --- |
| 1—10 | `v=428` | `wushuang:skill-3180` 潜袭 |
| 11—20 | 以上加 `getspell_KN=953` | 以上加 `mobile:youdi` 诱敌 |
| 21—25 | 以上加 `getspell_LY=3113` | 以上加 `shanhe:source-yidu-3113` 遗毒 |

428（`QianXi`）为准备阶段判定，然后选择距离为1的一名其他角色，令其本回合不能使用或打出与判定结果颜色相同的手牌。既有 `wushuang-skills-a` 的3180是该判定版本：真实公开判定、选择距离1角色、按最终判定颜色授予本回合手牌颜色限制，并清理原判定牌；其原生能力没有无双模式前提。内容包只导入这一项定义，不导入整个无双活动。普通马岱的摸牌弃牌版本、杀伤前减上限版本及界马岱的展示牌版本均不等价。

本次同时修正该既有能力的距离校验：触发资格及最终选人校验改用 `GetCombatDistance`，与原 `otherLivingAtDistanceOne` 选人器一致。真实坐骑、技能距离修正及死亡座次处理参与距离计算，原始座次环距离仅保留为诊断查询。判定结果、原技能身份与授予限制的生命周期没有改变。

953（`YouDiSP`）为结束阶段令其他角色弃置自己一张手牌；若不是杀，取得对方一张牌；若不是黑色牌，摸一张牌。复用 `mobile:youdi`，其原生流程冻结弃牌花色，支持非黑色摸牌及非杀取得手牌或装备牌两条条件分支，真实弃牌和摸牌、取得的派生子流程返回原拥有者帧。不采用 `ol:youdi` 的黑色摸牌条件，也不采用488旧诱敌的手牌与装备混合弃牌范围。

3113（`YiDu`）复用上一批已实现的独立来源遗毒：仅指定唯一目标的杀或伤害锦囊牌完成后，若没有对该目标造成实际伤害，可展示其至多三张手牌；同色时由目标本人弃置这些牌。该项保持山河图模式限制、同一实际用牌身份、目标自弃资格与原实体付款/返回，不使用允许多目标选人的OL遗毒。

内容包 `StandardShanheSourceDemonAssassinPackage` 提供 `BaseId`、`HardId`、`HellId` 和 `GetGeneralId(1..25)`，身份分别为 `shanhe:source-demon-assassin-2457-base`、`shanhe:source-demon-assassin-2457-hard`、`shanhe:source-demon-assassin-2457-hell`。包依赖 `standard-classic-generals` 的移动诱敌，以及既有 `shanhe-source-wuque-assassin-2493` 包的一次来源遗毒登记；本包不重复登记3113规则束。

来源普通池15（前提组1272）、17（前提组1487）引用本组，覆盖第一章广成关5021—5027、虎牢关5041—5047，共14个普通城市，两地区来源最低难度均为1。这里确证成员关系，不据此推断服务端抽取权重或每次征程必然遇到本组。来源身体值仍由统一来源成长投影一次处理。

新增行为回归入口为 `ShanheSourceQianxiChecks.RealHorseDistanceJudgmentRestrictsTheChosenTargetsBlackHand`：四席真实开局中安装进攻马，使目标原始座次距离2、战斗距离1；完成原判定及封黑色手牌，再用实体杀验证目标不能支付黑色手牌闪。诱敌复用 `MobileZhouFangChecks.YoudiNonblackDrawPrecedesChosenHeTakeAndBlackElementalBranchesCold`，来源遗毒复用 `ShanheSourceYiduChecks.SoleTargetCompletesNativeRevealWhileOlRetainsMultipleTargets` 的已有覆盖；此处仅记录现有覆盖入口，本项未执行含恢复流程的方法。最终运行结果由本批统一验证确认。
