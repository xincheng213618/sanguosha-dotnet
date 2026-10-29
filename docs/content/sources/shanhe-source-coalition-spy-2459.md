# 暗中破坏：联军细作（来源2459）

来源为 [三国杀OL官方客户端配置](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs) 的 `hd_roguelike.sgs.Root.Fight/General/UniversalGroup` 和 `cha_spell.sgs`，按 `tools/sync_wushuang_reference.py` 的既有方式解码。

- 配置归档 SHA-256：`d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`。
- 解码 `hd_roguelike.sgs` SHA-256：`97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`。
- 解码 `cha_spell.sgs` SHA-256：`e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

## 完整单成员身体

`Fight2459` 的名称文本为“暗中破坏”，`Ggroup=2459`；原组只有一名“联军细作”。`NewbieGroup=22459` 是独立的新手替代配方，不是额外成员。

```json
{"a":2459,"startChapter":1,"b":4861,"c":"联军细作","d":"1000304","f":0,"h":100,"k":3,"l":3,"m":0,"n":0,"o":5,"p":2,"q":4,"r":0,"otherad":0,"v":"423","getspell_KN":"702","getspell_EM":"954","pvpspell":"3189"}
```

`b=4861` 对应泛用“士兵”，基础体力/上限3/3、摸牌2、起手4、护甲0、护甲恢复0、护甲上限5、额外杀及额外伤害0。没有固定装备、固定手牌、`att_*` 数值档位、隐藏随从或二阶段。来源实战成长由现有难度、日期、章节和单可见敌人计算器处理；内容包保持原始身体。

桌面肖像按三个精确形态ID绑定已核对的PVE皮肤1000304原卡，来源与图片SHA见[美术记录](../shanhe-npc-art-sources.json)。三种来源身份保留 `character:shanhe-source-soldier-4861`，仅持有下面的源技能组合。

| 来源难度 | 身份后缀 | 累积源技能 | 复用程序 |
| --- | --- | --- | --- |
| 1–10 | `base` | 423奇策 | `classic:qice` |
| 11–15 | `hard` | 423奇策、702替身 | 增加 `boundary:tishen` |
| 16–25 | `nightmare` | 423奇策、702替身、954断发 | 增加 `mobile:duanfa` |

## 精确复用及版本差异

**423奇策（QiCe）**：出牌阶段限一次，将所有手牌当任意普通锦囊牌使用。现有 `classic:qice` 的 `useAllHandCardsAsOrdinaryTrick` 使用真实全手牌材料，保留原生锦囊目标、付款、响应及返回流程。该技能没有同包智愚的 `classicIdentityMode` 条件，也没有荀攸身体限定；本组不授予智愚。

**702替身（TiShenPo）**：限定技，准备阶段回复所有体力，每因此回复1点摸一张牌。`boundary:tishen` 在真实准备窗口通过 `recoverToMaximum` 和 `drawRecoveryReceipt` 执行，按实际回复回执摸牌，限整局一次。它不限定张飞身体，不采用其他版本的替身效果，也不附带咆哮。

**954断发（DuanFa）**：出牌阶段弃置任意张黑色牌，然后摸等量牌，本阶段以此法弃置总数不能超过体力上限。这里“牌”包括手牌与装备区牌，复用现有 `mobile:duanfa` 的真实黑色HE付款、实际出牌阶段预算和原帧孩子返回。`ol:duanfa` 的现有定义只允许手牌，不足以复现来源954，不能因为同名而使用它。移动版程序校验精确技能身份 `mobile:duanfa`、当前实际出牌阶段及付款，没有周鲂身体限定；本组只授断发，不附带同包诱敌。

技能定义与原生能力均复用现有注册，内容包不重新注册或修改这些技能，也不引入新挂起状态或逐将重复定义检查。

## 城市与奖励边界

Season5普通池15（`premisegroup=1483`）及17引用2459，实际关联第一章广成关5021–5027、虎牢关5041–5047，共14座普通城市。原条件和服务器选择权重未在本地执行，因此具体投放仍需注明本地候选策略，不能把引用次数解释为概率。

原Fight奖励为 `reward=101`、`reward1=401`，不将本地铜币奖励冒称该来源的官方现金表。
