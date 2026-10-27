# 贪得无厌2455与联军斥候

来源为官方 [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的山河图战斗配置与技能表。固定缓存外层 SHA256 为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`，解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`，解码 `cha_spell.sgs` 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

`Fight.fightID=2455`、`Ggroup=2455`，`Text.ID=2455` 的战斗标题为贪得无厌。完整阵容只有一行 `General.a=2455`：`b=4861`、`c=联军斥候`、`d=1000148`、`f=0`、`h=100`。角色4861为士兵，统一使用 `character:shanhe-source-soldier-4861`，外观暂以 `supporter` 作为本地缺图回退，不借用廖化、卧龙诸葛亮或黄权的角色身份与附带技能。

原始体力与上限4/4、通常摸牌2、随机初始手牌4、护甲0、护甲回复0、护甲上限5、额外出杀0、额外伤害0。原行没有固定装备、随身牌或 `att_*` 属性，阵容没有隐藏成员。`NewbieGroup=22455` 是独立战斗组，不追加为2455的随从；`pvpspell=3189` 是另一模式字段，不加入本山河图技能配方。

三个形态覆盖完整源难度1—25，按来源阈值累计技能：

| 源难度 | 来源字段 | 本地技能 |
| --- | --- | --- |
| 1—10 | `v=426` | `classic:dangxian` 当先 |
| 11—20 | 以上加 `getspell_KN=98` | 以上加 `classic:kanpo` 看破 |
| 21—25 | 以上加 `getspell_LY=859` | 以上加 `ol:dianhu` 点虎 |

426（`DangXian`）：锁定技，回合开始时执行一个额外的出牌阶段。复用 `classic-lifecycle-skills` 的原生 `turnStartBeforeNormalFlow` → `insertPhase(play, beforeNormalPreparation)`，保留后续正常阶段；不附带界当先的弃牌堆取杀或付体力流程。

98（`KanPo`）：一张黑色手牌当无懈可击使用。复用 `conversion-cutover` 的手牌、黑色、响应转换。这里使用普通看破，不加入界看破的装备材料与不可被响应效果。

859（`DianHuSP`）：锁定技，游戏开始时指定一名其他角色；自己对其造成伤害或其回复体力后，摸一张牌。复用 `ol-huang-quan` 的原生开局选人、真实伤害与实际恢复事件，每次伤害事件摸一张，不按伤害点数增加；不附带黄权的谏计，也不改用龙舟点虎的队友伤害、逐点摸牌版本。

`UniversalGroup` 的普通池15、17均引用本组，分别对应S5第一章广成关5021—5027、虎牢关5041—5047，共14个普通城市。池15的引用还带 `premisegroup=1270`，池17带 `premisegroup=1485`；这里只确证来源池成员关系，不据此宣称每次征程必定出现或推断服务端抽取权重。身体值由来源成长投影按真实难度、天数、章节与可见人数一次计算，包内保留原始4/4。

内容包为 `StandardShanheSourceCoalitionScoutPackage`，三个身份为 `shanhe:source-coalition-scout-2455-base`、`shanhe:source-coalition-scout-2455-hard`、`shanhe:source-coalition-scout-2455-hell`，使用 `GetGeneralId(1..25)` 选择。包依赖 `standard-classic-generals` 已登记的三项技能，不重复注册规则束。

本项纯复用现有能力，不新增Core机制或逐角色重复检查。共享机制已有当先额外阶段、黑色手牌无懈转换与 `HuangQuanChecks.ActualRecordedDamageAndForeignRecoveryCold` 等覆盖入口；本次仅静态核对来源与程序，没有执行这些方法或其辅助流程。地图、工厂与综合前向验证由本批统一集成。
