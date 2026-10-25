# S5 玩家龙魂3023

核对日期：2026-10-26。来源是社区[技能表](https://s.xiaochao.org/data/skills.json)与[研习树](https://s.xiaochao.org/data/tactic-tree.json)的本地已保存快照，沿用[本批玩家构筑技能来源](shanhe-build-skills-2026-10-25.md)。本次重新解析来源快照并核对当前源码，没有重新请求线上数据。

技能表3023：名称“龙魂”，`spellId=317`，`relatedIds=[317]`，品质4，原价1000铜钱。其规则为：至多两张同花色牌，红桃当桃、方片当火杀、梅花当闪、黑桃当无懈可击；两张红色牌增加此牌回复值或伤害值1，两张黑色牌弃置当前回合角色一张牌。原文双黑分支写“你弃置”，没有“可以”。

精确筛选`seasonId=5 && rewardId=3023`得到唯一节点107：第46层、第3列，研习消耗45，前置节点99，无附加条件，奖励类型“技能”，品质4。本项应绑定该节点，占一个额外技能槽，不列为无固定节点的默认开放库存。

`StandardShanheLonghunPackage`独立登记`shanhe:longhun-3023`。它从现有嵌入资源`src/Characters/zhao-yun/ol-ol-god/Skills/ol-shen-zhao-yun.rules.json`选择并深复制`ol:longhun`，重绑技能ID和双黑触发的来源ID，再将`two-club-discard`、`two-spade-discard`的`optional`改为`false`。八组view-as、手牌/装备付款、同花色限制、双红伤害/回复加成、原生使用和响应及弃牌操作均继承已有程序，不另复制一套规则。原始OL资源及其可选双黑弃牌保持原样。

运行时核对：`ol:longhun`没有独立Core技能ID硬编码；它的两项触发按`cardResponseAccepted`、`conversionSource`及`sourceViewAsId`筛选，沿绑定技能实例匹配已付款的转换链。选择`currentTurnPlayer`后由持有者从该角色手牌或装备区选择一张弃置；无可弃牌则沿既有`skipIfNoCards`结束。无双的`wushuang:skill-3023`另有无双用牌历史限定，不能直接复用为山河规则。

行为检查`ShanheLonghunChecks`以固定种子17、实体黑色牌和原生来牌构建合法对局。真实双梅花闪与双黑桃无懈必须直接进入强制选择，并使当前回合角色恰好弃一张实体牌；两张材料各支付一次并完成原生弃牌返回。单梅花闪没有该收益；普通OL双梅花仍可跳过。没有新增静态定义快照、存档/恢复检查、Core操作码、状态结构或规则格式版本。共享接线后的构建与验证由集成串行执行。

来源SHA-256：技能表`9FCAD12C69394382ED903986C7BFCC03FA13B0949173A3ED12B4AA008CB0B53E`；研习树`A754CA2F4AC25F0F140C8DF2D97693B5BB84EC5EDF55A52AC59F8CD511A18625`。
