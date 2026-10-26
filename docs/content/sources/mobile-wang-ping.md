# 王平·移动版

> 此版本与 `classic:wang-ping` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。普通移动身份，蜀、男、4体力，神话再临阴；完整引用已注册的 `classic:feijun` 与 `classic:binglue`，没有复制技能、目标账本或运行时。

来源：[移动官网250](https://www.sanguosha.cn/pc/hero-detail-250.html)、[移动BWIKI王平固定版本39178](https://wiki.biligame.com/msgs/index.php?title=王平&oldid=39178)（2026-06-18）、[飞军固定版本20784](https://wiki.biligame.com/msgs/index.php?title=飞军&oldid=20784)、[兵略固定版本20785](https://wiki.biligame.com/msgs/index.php?title=兵略&oldid=20785)。普通经典人物栏列出4000将魂合成/招募获得；其国战栏仍标注未开放权限，本次不接国战军令技能。

飞军每个出牌阶段一次，先弃置自己一张牌，再按当前手牌或装备数量选择相应效果和其他角色。手牌更多的目标交给王平一张牌；装备更多的目标弃置自己一张装备。当前移动问答明确：自身可以弃装备支付，手牌分支目标可交手牌或装备牌，判定牌不能交付；两种数量资格不能混用。

现有 `classic-wang-ping.rules.json` 的飞军先用真实HE实体支付并等移牌子结算，再经 `selectRelativeZoneDemandTarget` 按付款后的数量冻结目标与模式；两分支分别由目标自己选HE交付或选择装备弃置。原 `GameEngine.ProgramTargetCommit.cs` 在原帧发布 `ProgramTargetCommittedEvent`，兵略只奖励每名角色首次遭遇，按整个游戏历史记账，不因再次获得技能而清空。兵略摸牌改变王平手牌数后，飞军继续使用已冻结的目标资格。

官网兵略较长措辞与当前移动专页“首次对每名其他角色发动”在此是同一目标历史规则。复用两个完整ID可保留原技能之间精确的 `sourceProgramId` 关联；不把兵略替换为逐回合或逐阶段一次奖励。已有 `FengLinWangPingChecks.PaidHandDemandAndFrozenQualification` 覆盖付款后比较、兵略真实摸2、资格反转后的目标交付及代表重放，纯引用不新增重复人物夹具。

经典立绘来自[官网41700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/41700.jpg)，对应当前官网250经典首图；本地目标名 `official-mobile-wang-ping.jpg`，`portraitKey` 为 `mobile-wang-ping`，图鉴分组 `myth-yin`。
